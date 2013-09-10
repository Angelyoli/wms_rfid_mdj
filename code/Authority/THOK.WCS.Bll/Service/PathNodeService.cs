using System;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.WCS.Dal.Interfaces;
using THOK.WCS.DbModel;
using System.Linq;

namespace THOK.WCS.Bll.Service
{
    public class PathNodeService : ServiceBase<PathNode>, IPathNodeService
    {
        [Dependency]
        public IPathNodeRepository PathNodeRepository { get; set; }
        [Dependency]
        public IPathRepository PathRepository { get; set; }
        [Dependency]
        public IPositionRepository PositionRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows,PathNode pathNode)
        {
            //引入表
            IQueryable<PathNode> pathNodeQuery = PathNodeRepository.GetQueryable();
            IQueryable<Path> pathQuery = PathRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            //关联表
            var pathNodeDetail = pathNodeQuery.Join(pathQuery,
                             pn => pn.PathID,
                             p => p.ID,
                             (pn, p) => new { pn.ID, pn.PathID, pn.PositionID, pn.PathNodeOrder, p.PathName })
                             .Join(positionQuery,
                             pn => pn.PositionID,
                             p => p.ID,
                             (pn, p) => new { pn.ID, pn.PathID, pn.PositionID, pn.PathNodeOrder, pn.PathName,p.PositionName })
                //.Where(p => p.PathName.Contains(PathName) && p.PositionName.Contains(PositionName))
                .OrderBy(p => p.ID);
            int total = pathNodeDetail.Count();
            var pathNodeDetails = pathNodeDetail.Skip((page - 1) * rows).Take(rows);
            var pathNode_Detail = pathNodeDetails.ToArray().Select(p => new
                {
                    p.ID,
                    p.PathID,
                    p.PathName,
                    p.PositionID,
                    p.PositionName,
                    p.PathNodeOrder,
                });
            return new { total, rows = pathNode_Detail.ToArray() };
        }

        public object GetPathNode(int page, int rows, string queryString, string value)
        {
            string id = "", PathID = "";

            if (queryString == "ID")
            {
                id = value;
            }
            else
            {
                PathID = value;
            }
            IQueryable<PathNode> PathNodeQuery = PathNodeRepository.GetQueryable();
            int Id = Convert.ToInt32(id);
            var PathNode = PathNodeQuery.Where(p => p.ID == Id)
                .OrderBy(p => p.ID).AsEnumerable().
                Select(p => new
                {
                    p.ID,
                    p.PathID,
                    p.PositionID,
                    p.PathNodeOrder,
                });
            int total = PathNode.Count();
            PathNode = PathNode.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = PathNode.ToArray() };
        }

        public System.Data.DataTable GetPathNode(int page, int rows, string id)
        {
            int t = -1;

            IQueryable<PathNode> pathNodeQuery = PathNodeRepository.GetQueryable();
            IQueryable<Path> pathQuery = PathRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();

            var pathNode = pathNodeQuery.Join(pathQuery,
                             pn => pn.PathID,
                             p => p.ID,
                             (pn, p) => new { pn.ID, pn.PathID, pn.PositionID, pn.PathNodeOrder, PathName = p.PathName })
                             .Join(positionQuery,
                             pn => pn.PositionID,
                             p => p.ID,
                             (pn, p) => new { pn.ID, pn.PathID, pn.PositionID, pn.PathNodeOrder, pn.PathName, PositionName = p.PositionName })
                //.Where(p => p.ID == PathID)
                //  .Where(p => p.ID == t)
                .OrderBy(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PathName,
                    p.PositionName,
                    p.PathNodeOrder,
                });
            try { t = Convert.ToInt32(id); }
            catch { t = -1; }
            finally
            {
                pathNode = pathNode.Where(p => p.ID == t)
                        .OrderBy(p => p.ID).AsEnumerable()
                        .Select(p => new
                        {
                            p.ID,
                            p.PathName,
                            p.PositionName,
                            p.PathNodeOrder,
                        });
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("路径名称", typeof(string));
            dt.Columns.Add("位置名称", typeof(string));
            dt.Columns.Add("路径节点顺序", typeof(string));
            foreach (var item in pathNode)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.PathName,
                        item.PositionName,
                        item.PathNodeOrder
                    );
            }
            return dt;
        }

        #region 路径节点变成按钮

        public object GetDetails(string PathId)
        {
            IQueryable<PathNode> PathNodeQuery = PathNodeRepository.GetQueryable();
            int Path_ID = Convert.ToInt32(PathId);
            var PathNode = PathNodeQuery.Where(p => p.PathID == Path_ID)
               .OrderBy(p => p.PathNodeOrder).
               Select(p => new
               {
                   p.ID,
                   p.PathID,
                   p.Path.PathName,
                   p.PositionID,
                   p.Position.PositionName,
                   p.PathNodeOrder
               });
            return PathNode.ToArray();
        }

        public bool Add(PathNode pathNode)
        {
            IQueryable<Path> PathQuery = PathRepository.GetQueryable();
            var Path = PathQuery.FirstOrDefault(p => p.ID == pathNode.PathID);
            IQueryable<Position> PositionQuery = PositionRepository.GetQueryable();
            var Position = PositionQuery.FirstOrDefault(p => p.ID == pathNode.PositionID);
            var Path_Node = new PathNode();
            Path_Node.PathID = pathNode.PathID;
            Path_Node.Path = Path;
            Path_Node.PositionID = pathNode.PositionID;
            Path_Node.Position = Position;
            Path_Node.PathNodeOrder = pathNode.PathNodeOrder;
            PathNodeRepository.Add(Path_Node);
            PathNodeRepository.SaveChanges();
            return true;
        }

        public bool Save(PathNode pathNode)
        {
            IQueryable<Position> PositionQuery = PositionRepository.GetQueryable();
            var Position = PositionQuery.FirstOrDefault(p => p.ID == pathNode.PositionID);
            var PathNode = PathNodeRepository.GetQueryable().FirstOrDefault(pn => pn.ID == pathNode.ID);
            PathNode.PositionID = pathNode.PositionID;
            PathNode.Position = Position;
            PathNode.PathNodeOrder = pathNode.PathNodeOrder;
            PathNodeRepository.SaveChanges();
            return true;
        }

        public bool Delete(PathNode pathNode)
        {
            var p = PathNodeRepository.GetQueryable().FirstOrDefault(pn => pn.ID == pathNode.ID);
            if (p != null)
            {
                PathNodeRepository.Delete(p);
                PathNodeRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        #endregion
    }
}