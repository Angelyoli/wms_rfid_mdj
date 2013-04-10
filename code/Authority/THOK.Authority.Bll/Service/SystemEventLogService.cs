using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class SystemEventLogService : ServiceBase<SystemEventLog>, ISystemEventLogService
    {
        [Dependency]
        public ISystemEventLogRepository SystemEventLogRepository { get; set; }
        [Dependency]
        public ILoginLogRepository LoginLogRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository{get;set;}
        [Dependency]
        public ISystemRepository SystemRepository{get;set;}

        public object GetDetails(int page, int rows, string eventlogtime, string eventtype, string eventname, string frompc, string operateuser, string targetsystem)
        {
            IQueryable<THOK.Authority.DbModel.SystemEventLog> query = SystemEventLogRepository.GetQueryable();
            var systemeventlogs = query.Where(i => i.EventLogTime.Contains(eventlogtime)
                    && i.EventType.Contains(eventtype) && i.EventName.Contains(eventname) && i.FromPC.Contains(frompc) && i.OperateUser.Contains(operateuser) && i.TargetSystem.Contains(targetsystem))
                    .OrderBy(i => i.EventLogID)
                    .Select(i => new { i.EventLogID, i.EventName, i.EventType, i.FromPC, i.EventLogTime, i.OperateUser, i.EventDescription, i.TargetSystem });


            int total = systemeventlogs.Count();
            systemeventlogs = systemeventlogs.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = systemeventlogs.ToArray() };
        }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool CreateLoginLog(string login_time, string logout_time, string user_name, Guid system_ID)
        {
            string ipaddress = System.Net.Dns.Resolve(System.Net.Dns.GetHostName()).AddressList[0].ToString();
            var userid = UserRepository.GetQueryable().Where(s => s.UserName == user_name).Select(s => new {userId= s.UserID });
            var LoginLog = new LoginLog()
            {
                LogID = Guid.NewGuid(),
                LoginPC = ipaddress,
                LoginTime = login_time,
                LogoutTime = logout_time,
                User_UserID = userid.ToArray()[0].userId,
                System_SystemID = system_ID
            };
            LoginLogRepository.Add(LoginLog);
            LoginLogRepository.SaveChanges();
            return true;
        }
        public bool UpdateLoginLog(string user_name,string logout_time)
        {
           var LoginLog=LoginLogRepository.GetQueryable().Where(s=>s.User.UserName==user_name).Select(s=>s).OrderByDescending(s=>s.LoginTime).ToArray()[0];
           if (LoginLog != null)
            {
                LoginLog.LogoutTime = logout_time;
                LoginLogRepository.SaveChanges();
                return true;
            }
            else { return false; }
        }
    }
}
