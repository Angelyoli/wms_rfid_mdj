using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPathService : IService<Path>
    {


        

        bool Add(Path path, out string strResult);

        bool Save(Path path, out string strResult);

        //bool Delete(int ID, out string strResult);

        object GetPath(int page, int rows, string queryString, string value);

        System.Data.DataTable GetPath(int page, int rows, string PathName, string Description, string State);

        bool Delete(int pathId, out string strResult);

        object GetDetails(int page, int rows, string ID, string PathName,string RegionID, string Description, string State);
    }
}
