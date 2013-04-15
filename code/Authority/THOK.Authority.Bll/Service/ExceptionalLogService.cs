using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class ExceptionalLogService : ServiceBase<ExceptionalLog>,IExceptionalLogService
    {
        [Dependency]
        public IExceptionalLogRepository ExceptionalLogRepository { get; set; }
        [Dependency]
        public IModuleRepository ModuleRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool CreateExceptionLog(string ModuleNam, string FunctionName, string ExceptionalType, string ExceptionalDescription, string State)
        {
            var moduleRepository = ModuleRepository.GetQueryable().Where(m => m.ModuleURL == ModuleNam).Select(m => new {modulname=m.ModuleName }).ToArray();
            if (moduleRepository.Length > 0)
            {
                var ExceptionalLogs = new ExceptionalLog()
                {
                    ExceptionalLogID = Guid.NewGuid(),
                    CatchTime = DateTime.Now.ToString(),
                    ModuleName = moduleRepository[0].modulname,
                    FunctionName = FunctionName,
                    ExceptionalType = ExceptionalType,
                    ExceptionalDescription = ExceptionalDescription,
                    State = State,
                };
                ExceptionalLogRepository.Add(ExceptionalLogs);
                ExceptionalLogRepository.SaveChanges();
            }
            return true;
        }

        public object GetDetails(int page, int rows, string moduleName, string functionName)
        {
            IQueryable<ExceptionalLog> exceptionalLogQuery = ExceptionalLogRepository.GetQueryable();
            var exceptionalLog = exceptionalLogQuery
                    .Where(e => e.ModuleName.Contains(moduleName) && e.FunctionName.Contains(functionName))
                    .OrderByDescending(e => e.CatchTime)
                    .Select(e => new { e.ExceptionalLogID, e.CatchTime, e.ModuleName, e.FunctionName,e.ExceptionalType, e.ExceptionalDescription, e.State});

            int total = exceptionalLog.Count();
            exceptionalLog = exceptionalLog.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = exceptionalLog.ToArray() };
        }

        public bool Delete(string exceptionalLogId, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            Guid exceptionid = new Guid(exceptionalLogId);
            var exceptionlog = ExceptionalLogRepository.GetQueryable().FirstOrDefault(lo => lo.ExceptionalLogID == exceptionid);
            if (exceptionlog != null)
            {
                try
                {
                    ExceptionalLogRepository.Delete(exceptionlog);
                    ExceptionalLogRepository.SaveChanges();
                    result = true;
                }
                catch(Exception e)
                {
                    strResult = "原因：" + e;
                }
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
            var exceptionlog = ExceptionalLogRepository.GetQueryable();
            if (exceptionlog != null)
            {
                foreach (var log in exceptionlog)
                {
                    ExceptionalLogRepository.Delete(log);
                }
                ExceptionalLogRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因：未找到当前需要删除的数据！";
            }
            return result;
        }

        public System.Data.DataTable GetExceptionalLog(int page, int rows, string catchTime, string moduleName, string functionName)
        {
            IQueryable<ExceptionalLog> systemEventLogQuery = ExceptionalLogRepository.GetQueryable();
            var eventlogs = systemEventLogQuery
                    .Where(i => i.CatchTime.Contains(catchTime) && i.ModuleName.Contains(moduleName) && i.FunctionName.Contains(functionName))
                    .OrderByDescending(e => e.CatchTime)
                    .Select(e => new { e.CatchTime, e.ModuleName, e.FunctionName, e.ExceptionalType, e.ExceptionalDescription });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("异常时间", typeof(string));
            dt.Columns.Add("模块名称", typeof(string));
            dt.Columns.Add("方法名称", typeof(string));
            dt.Columns.Add("异常类型", typeof(string));
            dt.Columns.Add("异常描述", typeof(string));
            foreach (var item in eventlogs)
            {
                dt.Rows.Add
                    (
                        item.CatchTime,
                        item.ModuleName,
                        item.FunctionName,
                        item.ExceptionalType,
                        item.ExceptionalDescription
                    );
            }
            return dt;
        }
    }
}
