using System;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Interfaces
{
    public interface ILoginLogService : IService<LoginLog>
    {
        object GetDetails(int page, int rows, string userName, string systemName);

        bool Delete(string loginLogId, out string strResult);

        bool Emptys(out string strResult);

        System.Data.DataTable GetLoginLog(int page, int rows, string loginPC, string loginTime, string logoutTime);
    }
}
