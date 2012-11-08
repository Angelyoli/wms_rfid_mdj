using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class SystemParameterService : ServiceBase<SystemParameter>, ISystemParameterService
    {
        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }
        [Dependency]
        public ISystemRepository SystemRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetSystemParameter(int page, int rows, SystemParameter systemParameter)
        {
            var query = SystemParameterRepository.GetQueryable()
                            .Where(a => a.ParameterName.Contains(systemParameter.ParameterName)
                                || a.ParameterValue.Contains(systemParameter.ParameterValue)
                                || a.Remark.Contains(systemParameter.Remark)
                                || a.UserName.Contains(systemParameter.UserName)
                                || a.SystemID == systemParameter.SystemID);
            if (!systemParameter.ParameterName.Equals(string.Empty))
            {
                query.Where(a => a.ParameterName == systemParameter.ParameterName);
            }
            if (!systemParameter.ParameterValue.Equals(string.Empty))
            {
                query.Where(a => a.ParameterValue == systemParameter.ParameterValue);
            }
            if (!systemParameter.Remark.Equals(string.Empty))
            {
                query.Where(a => a.Remark == systemParameter.Remark);
            }
            if (!systemParameter.UserName.Equals(string.Empty))
            {
                query.Where(a => a.UserName == systemParameter.UserName);
            }
            if (systemParameter.SystemID != null)
            {
                query.Where(a => a.SystemID == systemParameter.SystemID);
            }
            query = query.OrderBy(a => a.Id);
            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);
            var info = query.ToArray().Select(a => new
            {
                Id = a.Id,
                a.ParameterName,
                a.ParameterValue,
                a.Remark,
                a.UserName,
                a.SystemID,
                SystemName = a.SystemID == null ? "" : a.System.SystemName
            });
            return new { total, rows = info.ToArray() };
        }

        public bool SetSystemParameter(SystemParameter systemParameter, string userName, out string error)
        {
            error = string.Empty;
            bool result = false;

            var query = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.Id == systemParameter.Id);
            if (query != null)
            {
                try
                {
                    query.ParameterName = systemParameter.ParameterName;
                    query.ParameterValue = systemParameter.ParameterValue;
                    query.Remark = systemParameter.Remark;
                    query.UserName = userName;
                    query.SystemID = systemParameter.SystemID;

                    SystemParameterRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    error = "原因：" + ex.Message;
                }
            }
            else
            {
                error = "原因：未找到当前需要修改的数据！";
            }
            return result;
        }

        public bool AddSystemParameter(SystemParameter systemParameter, string userName, out string error)
        {
            error = string.Empty;
            bool result = false;
            var query = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.Id == systemParameter.Id);
            if (query == null)
            {
                SystemParameter sp = new SystemParameter();
                if (sp != null)
                {
                    try
                    {
                        sp.ParameterName = systemParameter.ParameterName;
                        sp.ParameterValue = systemParameter.ParameterValue;
                        sp.Remark = systemParameter.Remark;
                        sp.UserName = userName;
                        sp.SystemID = systemParameter.SystemID;

                        SystemParameterRepository.Add(sp);
                        SystemParameterRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        error = "原因：" + ex.Message;
                    }
                }
                else
                {
                    error = "原因：找不到当前登陆用户！请重新登陆！";
                }
            }
            else
            {
                error = "原因：该编号已存在！";
            }
            return result;
        }

        public bool DelSystemParameter(int id, out string error)
        {
            error = string.Empty;
            bool result = false;

            var query = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.Id == id);
            if (query != null)
            {
                try
                {
                    SystemParameterRepository.Delete(query);
                    SystemParameterRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    error = "原因：" + ex.Message;
                }
            }
            else
            {
                error = "原因：未找到当前需要删除的数据！";
            }

            return result;
        }
    }
}
