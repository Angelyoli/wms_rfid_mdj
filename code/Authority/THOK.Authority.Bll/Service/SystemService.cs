﻿using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;

namespace THOK.Authority.Bll.Service
{
    public class SystemService : ServiceBase<THOK.Authority.DbModel.System>, ISystemService
    {
        [Dependency]
        public ISystemRepository SystemRepository { get; set; }
        [Dependency]
        public IModuleRepository ModuleRepository { get; set; }
        [Dependency]
        public IRoleSystemRepository RoleSystemRepository { get; set; }
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public IUserModuleRepository UserModuleRepository { get; set; }
        [Dependency]
        public IUserFunctionRepository UserFunctionRepository { get; set; }
        [Dependency]
        public ILoginLogRepository LoginLogRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string systemName, string description, string status)
        {
            IQueryable<THOK.Authority.DbModel.System> query = SystemRepository.GetQueryable();
            var systems = query.Where(i => i.SystemName.Contains(systemName) && i.Description.Contains(description))
                .OrderBy(i => i.SystemID)
                .Select(i => new { i.SystemID, i.SystemName, i.Description, Status = i.Status ? "启用" : "禁用" });
            if (status != "")
            {
                bool bStatus = Convert.ToBoolean(status);
                systems = query.Where(i => i.SystemName.Contains(systemName) && i.Description.Contains(description) && i.Status == bStatus)
                    .OrderBy(i => i.SystemID)
                    .Select(i => new { i.SystemID, i.SystemName, i.Description, Status = i.Status ? "启用" : "禁用" });
            }
            int total = systems.Count();
            systems = systems.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = systems.ToArray() };
        }

        public bool Add(string systemName, string description, bool status)
        {
            var system = new THOK.Authority.DbModel.System()
            {
                SystemID = Guid.NewGuid(),
                SystemName = systemName,
                Description = description,
                Status = status
            };
            SystemRepository.Add(system);
            SystemRepository.SaveChanges();
            return true;
        }

        public bool Delete(string systemId)
        {
            Guid gsystemId = new Guid(systemId);
            var system = SystemRepository.GetQueryable()
                .FirstOrDefault(i => i.SystemID == gsystemId);
            if (system != null)
            {
                Del(ModuleRepository, system.Modules);
                Del(RoleSystemRepository, system.RoleSystems);
                Del(UserSystemRepository, system.UserSystems);
                Del(LoginLogRepository, system.LoginLogs);
                SystemRepository.Delete(system);
                SystemRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string systemId, string systemName, string description, bool status)
        {
            Guid sid = new Guid(systemId);
            var system = SystemRepository.GetQueryable()
                .FirstOrDefault(i => i.SystemID == sid);
            system.SystemName = systemName;
            system.Description = description;
            system.Status = status;
            SystemRepository.SaveChanges();
            return true;
        }

        public object GetSystemById(string systemID)
        {
            Guid sid = new Guid(systemID);
            var sysytem = SystemRepository.GetQueryable().FirstOrDefault(s => s.SystemID == sid);
            return sysytem.SystemName;
        }

        public object GetDetails(string userName, string systemID, string cityID)
        {
            Guid cityid = new Guid(cityID);
            Guid systemid = new Guid(systemID);
            var user = UserRepository.GetQueryable().FirstOrDefault(u => u.UserName == userName);
            var userSystemId = UserSystemRepository.GetQueryable().Where(us => us.User_UserID == user.UserID 
                && us.System.SystemID == systemid && us.City_CityID == cityid).Select(us => us.UserSystemID);
            var userSystems = UserSystemRepository.GetQueryable().Where(us => !userSystemId.Contains(us.UserSystemID)
                && us.User_UserID == user.UserID && us.City.CityID == cityid);
            var userSystem = userSystems.Where(us => us.UserModules.Any(um => um.UserFunctions.Any(uf => uf.IsActive == true)))
                .Select(us => new {us.System.SystemID, us.System.SystemName, us.System.Description, Status = us.City.IsActive ? "启用" : "禁用" });
            return userSystem.ToArray();
        }

        public System.Data.DataTable GetSystem(int page, int rows, DbModel.System system, bool isactiveIsNull)
        {
            IQueryable<THOK.Authority.DbModel.System> systemQuery = SystemRepository.GetQueryable();

            var systemDetail = systemQuery.Where(s =>
                s.SystemName.Contains(system.SystemName)
                && s.Description.Contains(system.Description));
            if (isactiveIsNull == false)
            {
                systemDetail = systemDetail.Where(s => s.Status == system.Status);
            }
            systemDetail = systemDetail.OrderBy(s => s.SystemName);
            var system_Detail = systemDetail.ToArray().Select(s => new
            {
                s.SystemName,
                s.Description,
                Status = s.Status == true ? "启用" : "禁用"
            });

            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("系统名称", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            foreach (var s in system_Detail)
            {
                dt.Rows.Add
                    (
                        s.SystemName,
                        s.Description,
                        s.Status
                    );
            }
            return dt;
        }

    }
}
