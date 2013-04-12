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


        public object GetDetails(int page, int rows,string eventname,string operateuser, string targetsystem)
        {
            IQueryable<SystemEventLog> systemEventLogQuery = SystemEventLogRepository.GetQueryable();
            var eventlogs = systemEventLogQuery
                    .Where(i => i.EventName.Contains(eventname) && i.OperateUser.Contains(operateuser) && i.TargetSystem.Contains(targetsystem))
                    .OrderByDescending(e=>e.EventLogTime)
                    .Select(e => new { e.EventLogID, e.EventName, e.EventType, e.FromPC, e.EventDescription, e.EventLogTime, e.OperateUser, e.TargetSystem});


            int total = eventlogs.Count();
            eventlogs = eventlogs.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = eventlogs.ToArray() };
        }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool CreateEventLog(string EventName, string EventDescription, string OperateUser, Guid TargetSystem)
        {
            string userPC = System.Net.Dns.Resolve(System.Net.Dns.GetHostName()).AddressList[0].ToString();
            var systemname= SystemRepository.GetQueryable().Where(s => s.SystemID == TargetSystem).Select(s => new { systemname=s.SystemName });

            if (systemname.ToArray().Length > 0)
            {
                var EventLogs = new SystemEventLog()
                {
                    EventLogID = Guid.NewGuid(),
                    EventLogTime = DateTime.Now.ToString(),
                    EventType = "1",
                    EventName = EventName,
                    EventDescription = EventDescription,
                    FromPC = userPC,
                    OperateUser = OperateUser,
                    TargetSystem = systemname.ToArray()[0].systemname
                };
                SystemEventLogRepository.Add(EventLogs);
                SystemEventLogRepository.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(string eventLogId, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            Guid eventid = new Guid(eventLogId);
            var eventlog = SystemEventLogRepository.GetQueryable().FirstOrDefault(lo => lo.EventLogID == eventid);
            if (eventlog != null)
            {
                try
                {
                    SystemEventLogRepository.Delete(eventlog);
                    SystemEventLogRepository.SaveChanges();
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
            var eventlog = SystemEventLogRepository.GetQueryable();
            if (eventlog != null)
            {
                foreach (var log in eventlog)
                {
                    SystemEventLogRepository.Delete(log);
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

        public System.Data.DataTable GetSystemEventLog(int page, int rows, string eventLogTime, string eventName, string fromPC, string operateUser, string targetSystem)
        {
            IQueryable<SystemEventLog> systemEventLogQuery = SystemEventLogRepository.GetQueryable();
            var eventlogs = systemEventLogQuery
                    .Where(i => i.EventLogTime.Contains(eventLogTime) && i.EventName.Contains(eventName) && i.FromPC.Contains(fromPC) && i.OperateUser.Contains(operateUser) && i.TargetSystem.Contains(targetSystem))
                    .OrderByDescending(e => e.EventLogTime)
                    .Select(e => new { e.EventName, e.EventType, e.FromPC, e.EventDescription, e.EventLogTime, e.OperateUser, e.TargetSystem });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("业务时间", typeof(string));
            dt.Columns.Add("业务名称", typeof(string));
            dt.Columns.Add("业务描述", typeof(string));
            dt.Columns.Add("所用电脑", typeof(string));
            dt.Columns.Add("操作用户", typeof(string));
            dt.Columns.Add("对象系统", typeof(string));
            foreach (var item in eventlogs)
            {
                dt.Rows.Add
                    (
                        item.EventLogTime,
                        item.EventName,
                        item.EventDescription,
                        item.FromPC,
                        item.OperateUser,
                        item.TargetSystem
                    );
            }
            return dt;
        }
    }
}
