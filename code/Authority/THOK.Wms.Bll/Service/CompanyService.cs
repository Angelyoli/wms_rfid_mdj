using System;
using System.Linq;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class CompanyService : ServiceBase<Company>, ICompanyService
    {
        [Dependency]
        public ICompanyRepository CompanyRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICompanyService 增，删，改，查等方法

        public object GetDetails(int page, int rows, string CompanyCode, string CompanyName, string CompanyType, string IsActive)
        {
            IQueryable<Company> companyQuery = CompanyRepository.GetQueryable();
            var company = companyQuery.Where(c => c.CompanyCode.Contains(CompanyCode) && c.CompanyName.Contains(CompanyName) && c.CompanyType.Contains(CompanyType))
                .OrderByDescending(c => c.UpdateTime).AsEnumerable()
                .Select(c => new
                {
                    c.ID,
                    c.CompanyCode,
                    c.CompanyName,
                    c.Description,
                    CompanyType = c.CompanyType == "1" ? "配送中心" : c.CompanyType == "2" ? "市公司" : "县公司",
                    c.WarehouseCapacity,c.WarehouseCount,c.WarehouseSpace,c.SortingCount,ParentCompanyName=c.ParentCompany.CompanyName,c.ParentCompanyID,
                    IsActive = c.IsActive == "1" ? "可用" : "不可用",
                    UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
            if (!IsActive.Equals(""))
            {
                string bStatus = IsActive == "可用" ? "1" : "0";
                company = companyQuery.Where(c => c.CompanyCode.Contains(CompanyCode) && c.CompanyName.Contains(CompanyName) && c.CompanyType.Contains(CompanyType) && c.IsActive.Contains(bStatus))
                .OrderByDescending(c => c.UpdateTime).AsEnumerable()
                .Select(c => new
                {
                    c.ID,
                    c.CompanyCode,
                    c.CompanyName,
                    c.Description,
                    c.CompanyType,
                    c.WarehouseCapacity,
                    c.WarehouseCount,
                    c.WarehouseSpace,
                    c.SortingCount,
                    ParentCompanyName = c.ParentCompany.CompanyName,
                    c.ParentCompanyID,
                    IsActive = c.IsActive == "1" ? "可用" : "不可用",
                    UpdateTime=c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            int total = company.Count();
            company = company.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = company.ToArray() };
        }

        public bool Add(Company company)
        {
            var comp = new  Company();
            var parent = CompanyRepository.GetQueryable().FirstOrDefault(p => p.ID == company.ParentCompanyID);
            comp.ID = Guid.NewGuid();
            comp.CompanyCode = company.CompanyCode;
            comp.CompanyName = company.CompanyName;
            comp.CompanyType = company.CompanyType;
            comp.Description = company.Description;
            comp.ParentCompany = parent ?? comp;
            comp.UniformCode = company.UniformCode;
            comp.WarehouseCapacity = company.WarehouseCapacity;
            comp.WarehouseCount = company.WarehouseCount;
            comp.WarehouseSpace = company.WarehouseSpace;
            comp.SortingCount = company.SortingCount;            
            comp.IsActive = company.IsActive;
            comp.UpdateTime = DateTime.Now;

            CompanyRepository.Add(comp);
            CompanyRepository.SaveChanges();
            return true;
        }

        public bool Delete(string  companyID)
        {
            Guid cid = new Guid(companyID);
            var com = CompanyRepository.GetQueryable()
                .FirstOrDefault(c => c.ID == cid);
            if (com != null)
            {
                Del(CompanyRepository, com.Companies);
                CompanyRepository.Delete(com);
                CompanyRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Company company)
        {
            var comp = CompanyRepository.GetQueryable().FirstOrDefault(c => c.ID == company.ID);
            var par = CompanyRepository.GetQueryable().FirstOrDefault(c => c.ID == company.ParentCompanyID);
            comp.CompanyCode = company.CompanyCode;
            comp.CompanyName = company.CompanyName;
            comp.CompanyType = company.CompanyType;
            comp.Description = company.Description;
            comp.ParentCompany = par;
            comp.SortingCount = company.SortingCount;
            comp.UniformCode = company.UniformCode;
            comp.UpdateTime = DateTime.Now;
            comp.WarehouseCapacity = company.WarehouseCapacity;
            comp.WarehouseCount = company.WarehouseCount;
            comp.WarehouseSpace = company.WarehouseSpace;
            comp.IsActive = company.IsActive;           
            CompanyRepository.SaveChanges();
            return true;
        }

        #endregion

        /// <summary>
        /// 查找上级名称
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="CompanyCode"></param>
        /// <param name="CompanyName"></param>
        /// <returns></returns>
        public object GetParentName(int page, int rows, string queryString, string value)
        {
            string companyCode = "", companyName = "";

            if (queryString == "CompanyCode")
            {
                companyCode = value;
            }
            else
            {
                companyName = value;
            }
            IQueryable<Company> companyQuery = CompanyRepository.GetQueryable();
            var company = companyQuery.Where(c => c.CompanyCode.Contains(companyCode) && c.CompanyName.Contains(companyName))
                .OrderBy(c => c.CompanyCode).AsEnumerable()
                .Select(c => new
                {
                    c.ID,
                    c.CompanyCode,
                    c.CompanyName,
                    ParentCompanyID = c.ParentCompany.ParentCompanyID,
                    ParentCompanyName = c.ParentCompany.CompanyName,                    
                    IsActive = c.IsActive == "1" ? "可用" : "不可用",
                    UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
            int total = company.Count();
            company = company.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = company.ToArray() };
        }
    }
}
