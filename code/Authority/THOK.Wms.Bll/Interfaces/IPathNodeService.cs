using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPathNodeService : IService<PathNode>
    {
        object GetDetails(int page, int rows, string PathID, string PositionID, string PathNodeOrder);

        bool Add(PathNode pathNode, string strResult);

        bool Save(PathNode pathNode, string strResult);

        bool Delete(PathNode pathNode, string strResult);

        object GetPathNode(int page, int rows, string queryString, string value);

        System.Data.DataTable GetPathNode(int page, int rows, string PathID);
    }
}
