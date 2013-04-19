using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.Wms.Bll.Service
{
    public class RegionService : ServiceBase<Region>, IRegionService
    {
        [Dependency]
        public IRegionRepository RegionRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, Region reg)
        {
            IQueryable<Region> regionQuery = RegionRepository.GetQueryable();
            var regDetail = regionQuery.Where(s =>
               s.RegionName.Contains(reg.RegionName)
               && s.State.Contains(reg.State)).OrderBy(ul => ul.RegionName);
            int total = regDetail.Count();
            var regDetails = regDetail.Skip((page - 1) * rows).Take(rows);
            var reg_Detail = regDetails.ToArray().Select(s => new
            {
                s.ID,
                s.RegionName,
                s.Description,
                State = s.State == "01" ? "可用" : "不可用"
            });
            return new { total, rows = reg_Detail.ToArray() };
        }

        public bool Add(Region region)
        {
            var reg = new Region();
            reg.ID = region.ID;
            reg.RegionName = region.RegionName;
            reg.State = region.State;
            reg.Description = region.Description;
            RegionRepository.Add(reg);
            RegionRepository.SaveChanges();
            return true;
        }

        public bool Save(Region region)
        {
            var reg = RegionRepository.GetQueryable().FirstOrDefault(s => s.ID == region.ID);
            reg.ID = region.ID;
            reg.RegionName = region.RegionName;
            reg.State = region.State;
            reg.Description = region.Description;
            RegionRepository.SaveChanges();
            return true;
        }

        public bool Delete(int regionId)
        {
            var reg = RegionRepository.GetQueryable().FirstOrDefault(s => s.ID == regionId );
            if (reg != null)
            {
                RegionRepository.Delete(reg);
                RegionRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public object GetRegion(int page, int rows, string queryString, string value)
        {

            string id = "", regionName = "";

            if (queryString == "id")
            {
                id = value;

            //string regionName = "";
            //int id=-1;
            //if (queryString == "ID" && value!="")
            //{
            //    try { id = Convert.ToInt32(value); }
            //    catch { id =0; }

            }
            else
            {
                regionName = value;
            }

            IQueryable<Region> employeeQuery = RegionRepository.GetQueryable();
            var region = employeeQuery.Where(r =>r.RegionName.Contains(regionName) && r.State == "01")
                .OrderBy(r => r.ID).AsEnumerable()
                .Select(r => new
                {
                    r.ID,
                    r.Description,
                    r.RegionName,
                    State = r.State == "01" ? "可用" : "不可用",

            //IQueryable<Region> regionQuery = RegionRepository.GetQueryable();
            //var region = regionQuery.Where(r=> r.State == "01")
            //    .OrderBy(r => r.ID).AsEnumerable().
            //    Select(r => new
            //    {
            //        r.ID,
            //        r.RegionName,
            //        State = r.State == "01" ? "可用" : "不可用"
            //    });
            //if (id >=0)
            //{
            //    region = region.Where(r => r.ID == id);
            //}
            //else 
            //{
            //    region = region.Where(r => r.RegionName.Contains(regionName));
            //}
            //region = region.AsEnumerable().
            //    Select(r => new
            //    {
            //        r.ID,
            //        r.RegionName,
            //        r.State

            });
            int total = region.Count();
            region = region.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = region.ToArray() };
           
        }

        public System.Data.DataTable GetRegion(int page, int rows, string regionName, string state, string t)
        {
            IQueryable<Region> regionQuery = RegionRepository.GetQueryable();
            var srm = regionQuery.Where(r => r.RegionName.Contains(regionName))
                .OrderBy(r => r.ID).AsEnumerable()
                .Select(r => new
                {
                    r.ID,
                    r.RegionName,
                    r.Description,
                    State = r.State == "01" ? "可用" : "不可用"
                });
            if (!state.Equals(""))
            {
                srm = regionQuery.Where(r =>r.RegionName.Contains(regionName) && r.State.Contains(state))
                    .OrderBy(r => r.ID).AsEnumerable()
                    .Select(r => new
                    {
                        r.ID,
                        r.RegionName,
                        r.Description,
                        State = r.State == "01" ? "可用" : "不可用"
                    });
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("区域编码", typeof(string));
            dt.Columns.Add("区域名称", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            dt.Columns.Add("是否可用", typeof(string));
            foreach (var item in srm)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.RegionName,
                        item.Description,
                        item.State
                    );
            }
            return dt;
        }

    }
}
