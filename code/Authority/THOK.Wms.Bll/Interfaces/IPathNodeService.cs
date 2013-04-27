using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPathNodeService : IService<PathNode>
    {
        object GetDetails(int page, int rows, PathNode pathNode);

        bool Add(PathNode pathNode);

        bool Save(PathNode pathNode);

        bool Delete(PathNode pathNode);

        object GetPathNode(int page, int rows, string queryString, string value);

        System.Data.DataTable GetPathNode(int page, int rows, string id);


    }
}
