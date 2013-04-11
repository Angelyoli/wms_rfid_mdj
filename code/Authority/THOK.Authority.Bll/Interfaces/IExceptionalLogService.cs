using THOK.Authority.DbModel;
using System;

namespace THOK.Authority.Bll.Interfaces
{
    public interface IExceptionalLogService : IService<ExceptionalLog>
    {
        bool CreateExceptionLog(string ModuleNam, string FunctionName, string ExceptionalType, string ExceptionalDescription, string State);
    }
}
