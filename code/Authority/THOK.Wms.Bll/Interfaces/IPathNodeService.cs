using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPathNodeService : IService<PathNode>
    {
    //    object GetDetails(int page, int rows, string ID, string PathID, string PositionID, string PathNodeOrder);

        bool Add(PathNode PathNode, out string strResult);

        bool Save(PathNode PathNode, out string strResult);

        bool Delete(int PathID, out string strResult);

        //object GetPathNode(int page, int rows, string queryString, string value);

        //System.Data.DataTable GetPathNode(int page, int rows, string srmName, string state, string t);
    }
}
