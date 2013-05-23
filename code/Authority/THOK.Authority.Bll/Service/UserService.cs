using System;
using System.Linq;
using System.Web.Script.Serialization;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Bll.Models;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;
using THOK.Common;

namespace THOK.Authority.Bll.Service
{
    public class UserService : ServiceBase<User>, IUserService
    {
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        [Dependency]
        public IRoleRepository RoleRepository { get; set; }
        [Dependency]
        public IUserRoleRepository UserRoleRepository { get; set; }
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public ICityRepository CityRepository { get; set; }
        [Dependency]
        public IServerRepository ServerRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
        public bool UpdateUserInfo(string userName, string ipaddress)
        {
            var user = UserRepository.GetSingle(i => i.UserName == userName);
            if (user != null)
            {
                user.UserName = userName;
                user.LoginPC = ipaddress;
                UserRepository.SaveChanges();
                return true;
            }
            else { return false; }
        }
        public string GetUserIp(string userName)
        {
            string loginPC = "";
            var user = UserRepository.GetQueryable().Where(i => i.UserName == userName).ToArray();
            if (user.Count() > 0)
            {
                loginPC = user[0].LoginPC;
                return string.IsNullOrEmpty(loginPC) ? "" : loginPC;
            }
            else
            {
                return "";
            }
        }

        public bool CheckAdress(string userName, string ipaddress)
        {
            var loginPcAdres = UserRepository.GetQueryable().Where(i => i.UserName == userName).ToArray();
            if (loginPcAdres.Count() > 0)
            {
                if (ipaddress == loginPcAdres[0].LoginPC)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool DeleteUserIp(string userName)
        {
            var user = UserRepository.GetSingle(i => i.UserName == userName);
            if (user != null)
            {
                user.LoginPC = "";
                UserRepository.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public object GetDetails(int page, int rows, string userName, string chineseName, string isLock, string isAdmin, string memo)
        {
            IQueryable<THOK.Authority.DbModel.User> query = UserRepository.GetQueryable();
            var users = query.Where(i => i.UserName.Contains(userName)
                && i.ChineseName.Contains(chineseName)
                && i.Memo.Contains(memo))
                .OrderBy(i => i.UserID)
                .Select(i => new { i.UserID, i.UserName, i.ChineseName, i.Memo, IsLock = i.IsLock ? "是" : "否", IsAdmin = i.IsAdmin ? "是" : "否" });


            int total = users.Count();
            users = users.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = users.ToArray() };
        }

        public bool Add(string userName, string pwd, string chineseName, bool isLock, bool isAdmin, string memo)
        {
            if (Check(userName))
            {
                var user = new User()
                {
                    UserID = Guid.NewGuid(),
                    UserName = userName,
                    Pwd = EncryptPassword(pwd),
                    ChineseName = chineseName,
                    IsLock = isLock,
                    IsAdmin = isAdmin,
                    Memo = memo
                };
                UserRepository.Add(user);
                UserRepository.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public bool Delete(string userID)
        {
            Guid gUserID = new Guid(userID);
            var user = UserRepository.GetQueryable()
                .FirstOrDefault(u => u.UserID == gUserID);
            if (user != null)
            {
                Del(UserRoleRepository, user.UserRoles);
                Del(UserSystemRepository, user.UserSystems);
                UserRepository.Delete(user);
                UserRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string userID, string userName, string pwd, string chineseName, bool isLock, bool isAdmin, string memo)
        {
            Guid gUserID = new Guid(userID);
            var user = UserRepository.GetQueryable()
                .FirstOrDefault(u => u.UserID == gUserID);
            user.UserName = userName;
            user.Pwd = !string.IsNullOrEmpty(pwd) ? EncryptPassword(pwd) : user.Pwd;
            user.ChineseName = chineseName;
            user.IsLock = isLock;
            user.IsAdmin = isAdmin;
            user.Memo = memo;
            UserRepository.SaveChanges();
            return true;
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("值不能为NULL或为空。", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("值不能为NULL或为空。", "password");

            var user = UserRepository.GetSingle(i => i.UserName == userName);

            return user != null && ComparePassword(password, user.Pwd);
        }

        public bool ValidateUserPermission(string userName, string cityId, string systemId)
        {
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(cityId) || String.IsNullOrEmpty(systemId))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetLogOnUrl(string userName, string password, string cityId, string systemId, string serverId)
        {
            string url = "";
            string logOnKey = "";
            if (string.IsNullOrEmpty(serverId))
            {
                url = GetUrlFromCity(new Guid(cityId), out serverId);
            }

            if (string.IsNullOrEmpty(password))
            {
                IQueryable<THOK.Authority.DbModel.User> queryCity = UserRepository.GetQueryable();
                var user = queryCity.Single(c => c.UserName == userName);
                password = user.Pwd;
            }

            if (!string.IsNullOrEmpty(serverId))
            {
                url = GetUrlFromServer(new Guid(serverId));
            }
            var key = new UserLoginInfo()
                    {
                        CityID = cityId,
                        SystemID = systemId,
                        UserName = userName,
                        Password = password,
                        ServerID = serverId
                    };
            logOnKey = Des.EncryptDES((new JavaScriptSerializer()).Serialize(key), "12345678");
            url += @"/Account/LogOn/?LogOnKey=" + Uri.EscapeDataString(logOnKey);
            return url;
        }

        public bool ChangePassword(string userName, string password, string newPassword)
        {
            var user = UserRepository.GetQueryable().FirstOrDefault(u => u.UserName == userName);
            if (ComparePassword(password, user.Pwd))
            {
                user.Pwd = EncryptPassword(newPassword);
                UserRepository.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 验证，用户名不能重复
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool Check(string userName)
        {
            var userNames = UserRepository.GetQueryable().Select(u=>u.UserName).ToArray();
            if(userNames.Contains(userName))
            {
                return false;
            }
            else{
            return true;}
        }
        public string FindUsersForFunction(string strFunctionID)
        {
            throw new NotImplementedException();
        }

        private bool ComparePassword(string password, string hash)
        {
            return EncryptPassword(password) == hash || password == hash;
        }

        private string EncryptPassword(string password)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = x.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        private string GetUrlFromCity(Guid gCityID, out string serverId)
        {
            IQueryable<THOK.Authority.DbModel.City> queryCity = CityRepository.GetQueryable();
            var city = queryCity.Single(c => c.CityID == gCityID);
            var server = city.Servers.OrderBy(s => s.ServerID).First();
            serverId = server.ServerID.ToString();
            return server.Url;
        }

        private string GetUrlFromServer(Guid gServerID)
        {
            IQueryable<THOK.Authority.DbModel.Server> query = ServerRepository.GetQueryable();
            var system = query.Single(s => s.ServerID == gServerID);
            return system.Url;
        }

        public object GetUserRole(string userID)
        {
            Guid uid = new Guid(userID);
            IQueryable<THOK.Authority.DbModel.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Role> queryRole = RoleRepository.GetQueryable();
            var user = queryUser.FirstOrDefault(u => u.UserID == uid);
            var roles = user.UserRoles.OrderBy(r => r.Role.RoleID).Select(r => new { r.UserRoleID, r.User.UserID, r.User.UserName, r.Role.RoleID, r.Role.RoleName });
            return roles.ToArray();
        }

        public object GetRoleInfo(string userID)
        {
            Guid uid = new Guid(userID);
            IQueryable<THOK.Authority.DbModel.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Role> queryRole = RoleRepository.GetQueryable();
            var user = queryUser.FirstOrDefault(u => u.UserID == uid);
            var roleIDs = user.UserRoles.Select(ur => ur.Role.RoleID);
            var role = queryRole.Where(r => !roleIDs.Any(rid => rid == r.RoleID))
                .Select(r => new { r.RoleID, r.RoleName, Description = r.Memo, Status = r.IsLock ? "启用" : "禁用" });
            return role.ToArray();
        }

        public bool DeleteUserRole(string userRoleIdStr)
        {
            string[] userRoleIdList = userRoleIdStr.Split(',');
            for (int i = 0; i < userRoleIdList.Length - 1; i++)
            {
                Guid userRoleId = new Guid(userRoleIdList[i]);
                var UserRole = UserRoleRepository.GetQueryable().FirstOrDefault(ur => ur.UserRoleID == userRoleId);
                if (UserRole != null)
                {
                    UserRoleRepository.Delete(UserRole);
                    UserRoleRepository.SaveChanges();
                }
            }
            return true;
        }

        public bool AddUserRole(string userID, string roleIDStr)
        {
            try
            {
                var user = UserRepository.GetQueryable().FirstOrDefault(u => u.UserID == new Guid(userID));
                string[] roleIdList = roleIDStr.Split(',');
                for (int i = 0; i < roleIdList.Length - 1; i++)
                {
                    Guid rid = new Guid(roleIdList[i]);
                    var role = RoleRepository.GetQueryable().FirstOrDefault(r => r.RoleID == rid);
                    var userRole = new UserRole();
                    userRole.UserRoleID = Guid.NewGuid();
                    userRole.User = user;
                    userRole.Role = role;
                    UserRoleRepository.Add(userRole);
                    UserRoleRepository.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public object GetUser(int page, int rows, string queryString, string value)
        {
            string userName = "", chineseName = "";

            if (queryString == "UserName")
            {
                userName = value;
            }
            else
            {
                chineseName = value;
            }
            IQueryable<THOK.Authority.DbModel.User> query = UserRepository.GetQueryable();
            var users = query.Where(i => i.UserName.Contains(userName) && i.ChineseName.Contains(chineseName) && i.IsAdmin == true)
                .OrderBy(i => i.UserID)
                .Select(i => new
                {
                    i.UserID,
                    i.UserName,
                    i.ChineseName,
                    IsAdmin = i.IsAdmin ? "是" : "否"
                });
            int total = users.Count();
            users = users.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = users.ToArray() };
        }

        public System.Data.DataTable GetUserConten(int page, int rows, string userName, string chineseName, string meMo)
        {
            IQueryable<User> userQuery = UserRepository.GetQueryable();
            var userc = userQuery.Where(c => c.UserName.Contains(userName)
                && c.ChineseName.Contains(chineseName)
                && c.Memo.Contains(meMo))
                .OrderByDescending(c => c.UserName).AsEnumerable()
                .Select(c => new
                {
                    c.UserName,
                    c.ChineseName,
                    IsAdmin=c.IsAdmin?"是":"否",
                    IsLock=c.IsLock?"是":"否",
                    c.Memo,
                });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("用户名称", typeof(string));
            dt.Columns.Add("中文名称", typeof(string));
            dt.Columns.Add("是否是超级管理员", typeof(string));
            dt.Columns.Add("是否锁定", typeof(string));
            dt.Columns.Add("备注", typeof(string));
            foreach (var item in userc)
            {
                dt.Rows.Add
                    (
                        item.UserName,
                        item.ChineseName,
                        item.IsAdmin,
                        item.IsLock,
                        item.Memo
                    );

            }
            return dt;
        }
    }
}

