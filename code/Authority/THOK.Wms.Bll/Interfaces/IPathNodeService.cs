using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPathNodeService : IService<PathNode>
    {
        object GetDetails(int page, int rows, string ID, string PathID, string PositionID, string PathNodeOrder);

        bool Delete(PathNode PathID,string strResult);

        object GetPathNode(int page, int rows, string PathID);

        System.Data.DataTable GetPathNode(int page, int rows, string ID, string PathID, string PositionID, string PathNodeOrder);
        //System.Data.DataTable GetPathNode(int page, int rows, string PathID);

        object GetPathNode(int page, int rows, string queryString, string value);

        bool Add(PathNode ID, string strResult);

        bool Save(PathNode ID, string strResult);
    }
}
