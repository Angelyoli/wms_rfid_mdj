using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Interfaces
{
    public interface ISystemParameterService : IService<SystemParameter>
    {
        object GetSystemParameter(int page, int rows, SystemParameter systemParameter);
        bool SetSystemParameter(SystemParameter systemParameter, string userName, out string error);
        bool AddSystemParameter(SystemParameter systemParameter, string userName, out string error);
        bool DelSystemParameter(int id, out string error);
    }
}
