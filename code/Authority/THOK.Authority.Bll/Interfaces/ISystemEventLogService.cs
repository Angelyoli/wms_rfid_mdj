using THOK.Authority.DbModel;
using System;

namespace THOK.Authority.Bll.Interfaces
{
    public interface ISystemEventLogService : IService<SystemEventLog>
    {
        object GetDetails(int page, int rows, string eventlogtime, string eventtype, string eventname, string frompc, string operateuser, string targetsystem);
        bool CreateEventLog(string EventName, string EventDescription, string OperateUser, string TargetSystem);
        bool CreateLoginLog(string login_time, string logout_time, string user_name, Guid system_ID);
        bool UpdateLoginLog(string user_name,string logout_time);

    }
}
