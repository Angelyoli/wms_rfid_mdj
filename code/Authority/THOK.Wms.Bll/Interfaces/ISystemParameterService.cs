using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISystemParameterService : IService<SystemParameter>
    {
        object GetSystemParameter(string parameterName);
        bool SetSystemParameter(SystemParameter systemParameter, out string error);
        bool AddSystemParameter(SystemParameter systemParameter, out string error);
    }
}
