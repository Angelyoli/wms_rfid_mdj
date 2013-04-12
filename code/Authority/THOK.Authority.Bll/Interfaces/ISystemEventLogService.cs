using THOK.Authority.DbModel;
using System;

namespace THOK.Authority.Bll.Interfaces
{
    public interface ISystemEventLogService : IService<SystemEventLog>
    {
        object GetDetails(int page, int rows, string eventname,string operateuser, string targetsystem);
        bool CreateEventLog(string EventName, string EventDescription, string OperateUser, string TargetSystem);
        bool CreateLoginLog(string login_time, string logout_time, string user_name, Guid system_ID);
        bool UpdateLoginLog(string user_name,string logout_time);

        bool Delete(string eventLogId, out string strResult);

        bool Emptys(out string strResult);

        System.Data.DataTable GetSystemEventLog(int page, int rows, string eventLogTime, string eventName, string fromPC, string operateUser, string targetSystem);

    }
}
