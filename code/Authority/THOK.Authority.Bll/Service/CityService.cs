using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class CityService : ServiceBase<City>, ICityService
    {
        [Dependency]
        public ICityRepository CityRepository { get; set; }
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string cityName, string description, string isActive)
        {
            IQueryable<THOK.Authority.DbModel.City> query = CityRepository.GetQueryable();
            bool isactive;
                    var citys = query.OrderBy(i => i.CityID).Select(i => new { i.CityID, i.CityName, i.Description, IsActive = i.IsActive ? "启用" : "禁用" });
            if (cityName != "" || description != "" || isActive != "")
            {
                if (isActive == "true") isactive = true;
                else isactive = false;
                citys = query.Where(i => i.CityName.Contains(cityName)
                    && i.Description.Contains(description) && i.IsActive == isactive)
                    .OrderBy(i => i.CityID)
                    .Select(i => new { i.CityID, i.CityName, i.Description, IsActive = i.IsActive ? "启用" : "禁用" });
            }

            int total = citys.Count();
            citys = citys.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = citys.ToArray() };
        }

        public bool Add(string cityName, string description, bool isActive)
        {
            var city = new THOK.Authority.DbModel.City()
            {
                CityID = Guid.NewGuid(),
                CityName = cityName,
                Description = description,
                IsActive = isActive
            };
            CityRepository.Add(city);
            CityRepository.SaveChanges();
            return true;
        }

        public bool Delete(string cityID)
        {
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable()
                .FirstOrDefault(i => i.CityID == gCityID);
            if (city != null)
            {
                CityRepository.Delete(city);                
                CityRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string cityID, string cityName, string description, bool isActive)
        {
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable()
                .FirstOrDefault(i => i.CityID == gCityID);
            city.CityName = cityName;
            city.Description = description;
            city.IsActive = isActive;
            CityRepository.SaveChanges();
            return true;
        }
        
        public object GetCityByCityID(string cityID)
        {
            Guid cid=new Guid(cityID);
            var city = CityRepository.GetQueryable().FirstOrDefault(c => c.CityID == cid);
            return city.CityName;
        }

        public object GetDetails(string userName, string cityID, string systemID)
        {
            Guid cityid = new Guid(cityID);
            Guid systemid = new Guid(systemID);
            var user = UserRepository.GetQueryable().FirstOrDefault(u => u.UserName == userName);            
            var userSystem = UserSystemRepository.GetQueryable().Where(us => us.User_UserID == user.UserID && us.System_SystemID == systemid && us.City_CityID == cityid).Select(us => us.UserSystemID);
            var usersystems = UserSystemRepository.GetQueryable().Where(us => !userSystem.Any(uid => uid == us.UserSystemID) && us.User_UserID == user.UserID && us.System_SystemID == systemid).Select(us => new { us.City.CityID, us.City.CityName, us.City.Description, Status = us.City.IsActive ? "启用" : "禁用" });
            return usersystems.ToArray();
        }

        public System.Data.DataTable GetCity(int page, int rows, City city, bool activeIsNull)
        {
            IQueryable<City> cityQuery = CityRepository.GetQueryable();

            var cityDetail = cityQuery.Where(c=>c.CityName.Contains(city.CityName)&& c.Description.Contains(city.Description));
            if (activeIsNull == false)
            {
                cityDetail = cityDetail.Where(c => c.IsActive == city.IsActive);
            }
            cityDetail = cityDetail.OrderBy(c => c.CityName);
            var city_Detail = cityDetail.ToArray().Select(c => new
            {
                c.CityName,
                c.Description,
                IsActive = c.IsActive == true ? "启用" : "禁用"
            });

            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("地市名称", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            foreach (var c in city_Detail)
            {
                dt.Rows.Add
                    (
                        c.CityName,
                        c.Description,
                        c.IsActive
                    );
            }
            return dt;
        }
    }
}
