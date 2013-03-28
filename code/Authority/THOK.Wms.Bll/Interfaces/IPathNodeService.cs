using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPathNodeService : IService<PathNode>
    {
        object GetDetails(int page, int rows, string ID, string PathID, string PositionID, string PathNodeOrder);


        //bool Save(PathNode PathNode, out string strResult);

        //bool Delete(int PathID, out string strResult);

        //object GetPathNode(int page, int rows, string PathID);

        //System.Data.DataTable GetPathNode(int page, int rows, string ID, string PathID, string PositionID, string PathNodeOrder);

        ////bool Add(PathNode ID, out string strResult);

        ////bool Save(PathNode ID, out string strResult);

        //object GetPathNode(int page, int rows, string queryString, string value);

        bool Add(PathNode PathNode, string strResult);

        bool Save(PathNode ID, string strResult);
    }
}
