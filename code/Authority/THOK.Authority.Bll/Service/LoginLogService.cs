using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class LoginLogService : ServiceBase<LoginLog>, ILoginLogService
    {
        [Dependency]
        public ILoginLogRepository LoginLogRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        [Dependency]
        public ISystemRepository SystemRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string userName, string systemName)
        {
            IQueryable<LoginLog> loginLogquery = LoginLogRepository.GetQueryable();
            IQueryable<User> userLogquery = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.System> systemLogquery = SystemRepository.GetQueryable();
            var loginlog = loginLogquery.Join(userLogquery,
                    lo => lo.User_UserID,
                    u => u.UserID,
                    (lo, u) => new { lo.LogID, lo.LoginPC, lo.LoginTime, lo.LogoutTime, lo.User_UserID, lo.System_SystemID, u.UserName })
                    .Join(systemLogquery,
                    lo => lo.System_SystemID,
                    s => s.SystemID,
                    (lo, s) => new { lo.LogID,lo.LoginPC,lo.LoginTime,lo.LogoutTime,lo.User_UserID,lo.System_SystemID,lo.UserName,s.SystemName})
                    .Where(lo=>lo.UserName.Contains(userName) && lo.SystemName.Contains(systemName))
                    .OrderByDescending(lo=>lo.LoginTime)
                    .Select(lo => new { lo.LogID, lo.LoginPC, lo.LoginTime, lo.LogoutTime, lo.User_UserID, lo.System_SystemID,lo.UserName,lo.SystemName });
            int total = loginlog.Count();
            loginlog = loginlog.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = loginlog.ToArray() };
        }

        public bool Delete(string loginLogId, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            Guid loid = new Guid(loginLogId);
            var log = LoginLogRepository.GetQueryable().FirstOrDefault(lo => lo.LogID == loid);
            if (log != null)
            {
                LoginLogRepository.Delete(log);
                LoginLogRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因：未找到当前需要删除的数据！";
            }
            return result;
        }

        public bool Emptys(out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var log = LoginLogRepository.GetQueryable();
            if (log != null)
            {
                foreach (var lo in log)
                {
                    LoginLogRepository.Delete(lo);
                }
                LoginLogRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因：未找到当前需要删除的数据！";
            }
            return result;
        }

        public System.Data.DataTable GetLoginLog(int page, int rows, string loginPC, string loginTime, string logoutTime)
        {
            IQueryable<LoginLog> loginLogquery = LoginLogRepository.GetQueryable();
            IQueryable<User> userLogquery = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.System> systemLogquery = SystemRepository.GetQueryable();
            var loginlog = loginLogquery.Join(userLogquery,
                    lo => lo.User_UserID,
                    u => u.UserID,
                    (lo, u) => new { lo.LoginPC, lo.LoginTime, lo.LogoutTime,lo.System_SystemID, u.UserName })
                    .Join(systemLogquery,
                    lo => lo.System_SystemID,
                    s => s.SystemID,
                    (lo, s) => new { lo.LoginPC, lo.LoginTime, lo.LogoutTime,lo.UserName, s.SystemName })
                    .Where(lo => lo.LoginPC.Contains(loginPC) && lo.LoginTime.Contains(loginTime) && lo.LogoutTime.Contains(logoutTime))
                    .OrderByDescending(lo => lo.LoginTime)
                    .Select(lo => new {  lo.LoginPC, lo.LoginTime, lo.LogoutTime, lo.UserName, lo.SystemName });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("登陆电脑", typeof(string));
            dt.Columns.Add("登录时间", typeof(string));
            dt.Columns.Add("退出时间", typeof(string));
            dt.Columns.Add("登录的用户名", typeof(string));
            dt.Columns.Add("登录的系统", typeof(string));
            foreach (var item in loginlog)
            {
                dt.Rows.Add
                    (
                        item.LoginPC,
                        item.LoginTime,
                        item.LogoutTime,
                        item.UserName,
                        item.SystemName
                    );
            }
            return dt;
        }
        public bool CreateLoginLog(string login_time, string logout_time, string user_name, Guid system_ID)
        {
            string ipaddress = System.Net.Dns.Resolve(System.Net.Dns.GetHostName()).AddressList[0].ToString();
            var userid = UserRepository.GetQueryable().Where(s => s.UserName == user_name).Select(s => new { userId = s.UserID });
            if (userid.ToArray().Length > 0)
            {
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
            else
            {
                return false;
            }
        }
        public bool UpdateLoginLog(string user_name, string logout_time)
        {
            var LoginLog = LoginLogRepository.GetQueryable().Where(s => s.User.UserName == user_name).Select(s => s).OrderByDescending(s => s.LoginTime).ToArray();
            if (LoginLog.Length > 0)
            {
                LoginLog[0].LogoutTime = logout_time;
                LoginLogRepository.SaveChanges();
                return true;
            }
            else { return false; }
        }
    }
}
