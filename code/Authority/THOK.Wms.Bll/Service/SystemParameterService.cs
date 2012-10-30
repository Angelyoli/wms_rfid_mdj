using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class SystemParameterService : ServiceBase<SystemParameter>, ISystemParameterService
    {
        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetSystemParameter(string parameterName)
        {
            var query = SystemParameterRepository.GetQueryable()
                            .Where(a => a.ParameterName.Contains(parameterName))
                            .Select(a => new
                            {
                                a.Id,
                                a.ParameterName,
                                a.ParameterValue,
                                a.Remark,
                                a.UserName,
                                a.SystemID
                            });
            return query;
        }
        public bool SetSystemParameter(SystemParameter systemParameter, out string error)
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
                    query.UserName = "admin";
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
        public bool AddSystemParameter(SystemParameter systemParameter, out string error)
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
                        sp.UserName = "admin";
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
    }
}
