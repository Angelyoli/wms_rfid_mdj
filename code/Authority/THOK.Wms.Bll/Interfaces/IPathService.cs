using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPathService : IService<Path>
    {


        //object GetPath(int page, int rows, string queryString, string value);

        //object GetDetails(int page, int rows, string EmployeeCode, string EmployeeName, string DepartmentID, string JobID, string Status, string IsActive);

        //bool Add(Employee employee, out string strResult);

        //bool Save(Employee employee, out string strResult);

        //bool Delete(string demployeeId, out string strResult);

        //object GetDetails(int page, int rows, int ID, string PathName, string Description,string State);

        bool Add(Path path, out string strResult);

        bool Save(Path path, out string strResult);

        //bool Delete(int ID, out string strResult);

        object GetJob(int page, int rows, string queryString, string value);

        System.Data.DataTable GetJob(int page, int rows, string jobCode, string jobName, string isActive);

        bool Delete(int pathId, out string strResult);

        object GetDetails(int page, int rows, string ID, string PathName, string Description, string State);
    }
}
