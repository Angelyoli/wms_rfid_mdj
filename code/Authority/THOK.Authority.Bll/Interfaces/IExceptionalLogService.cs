using THOK.Authority.DbModel;
using System;

namespace THOK.Authority.Bll.Interfaces
{
    public interface IExceptionalLogService : IService<ExceptionalLog>
    {
        bool CreateExceptionLog(string ModuleNam, string FunctionName, string ExceptionalType, string ExceptionalDescription, string State);

        object GetDetails(int page, int rows, string moduleName, string functionName);

        bool Delete(string eventLogId, out string strResult);

        bool Emptys(out string strResult);

        System.Data.DataTable GetExceptionalLog(int page, int rows, string catchTime, string moduleName, string functionName);
    }
}
