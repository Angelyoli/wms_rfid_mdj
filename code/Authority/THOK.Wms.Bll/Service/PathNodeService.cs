using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.Wms.Bll.Service
{
    public class PathNodeService : ServiceBase<PathNode>, IPathNodeService
    {
        //private object PathID;
        [Dependency]
        public IPathNodeRepository PathNodeRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string PathID, string PositionID, string PathNodeOrder)
        {
            IQueryable<PathNode> pathNodeQuery = PathNodeRepository.GetQueryable();
            var pathNode = pathNodeQuery
                //.Where(p => p.ID == PathID)
                .OrderBy(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PathID,
                    p.PositionID,
                    p.PathNodeOrder,
                });

            int pathID = -1, positionID = -1, pathNodeOrder = -1;
            if ((PathID != "" && PathID != null) && (PositionID == "" || PositionID == null) && (PathNodeOrder != "" && PathNodeOrder != null))
            {
                try { pathID = Convert.ToInt32(PathID); }
                catch { pathID = -1; }
                finally { pathNode = pathNode.Where(p => p.PathID == pathID); }
            }
            if ((PathID == "" || PathID == null) && (PositionID != "" && PositionID != null) && (PathNodeOrder != "" && PathNodeOrder != null))
            {
                try { positionID = Convert.ToInt32(PositionID); }
                catch { positionID = -1; }
                finally { pathNode = pathNode.Where(p => p.PositionID == positionID); }
            }
            if ((PathID == "" || PathID == null) && (PositionID != "" && PositionID != null) && (PathNodeOrder != "" && PathNodeOrder != null))
            {
                try { pathNodeOrder = Convert.ToInt32(PositionID); }
                catch { pathNodeOrder = -1; }
                finally { pathNode = pathNode.Where(p => p.PathNodeOrder == pathNodeOrder); }
            }
            if ((PathID != "" && PathID != null) && (PositionID != "" && PositionID != null) && (PathNodeOrder != "" && PathNodeOrder != null))
            {
                try
                {
                    pathID = Convert.ToInt32(PathID);
                    positionID = Convert.ToInt32(PositionID);
                    pathNodeOrder = Convert.ToInt32(PositionID);
                }
                catch { pathNodeOrder = -1; }
                finally { pathNode = pathNode.Where(p => p.PathID == pathID && p.PositionID == positionID && p.PathNodeOrder == pathNodeOrder); }
            }
            pathNode = pathNode.OrderBy(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PathID,
                    p.PositionID,
                    p.PathNodeOrder,
                });
            int total = pathNode.Count();
            pathNode = pathNode.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = pathNode.ToArray() };
        }

        public bool Add(PathNode pathNode, string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var p = new PathNode();
            if (p != null)
            {
                try
                {
                    p.PathID = pathNode.PathID;
                    p.PositionID = pathNode.PositionID;
                    p.PathNodeOrder = pathNode.PathNodeOrder;

                    PathNodeRepository.Add(p);
                    PathNodeRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            else
            {
                strResult = "原因：找不到当前登陆用户！请重新登陆！";
            }
            return result;
        }

        public bool Save(PathNode pathNode, string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var p = PathNodeRepository.GetQueryable().FirstOrDefault(pn => pn.PathID == pathNode.PathID);
            //var p = new PathNode();
            if (p != null)
            {
                try
                {
                    p.PathID = pathNode.PathID;
                    p.PositionID = pathNode.PositionID;
                    p.PathNodeOrder = pathNode.PathNodeOrder;

                    PathNodeRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            else
            {
                strResult = "原因：未找到当前需要修改的数据！";
            }
            return result;
        }

        public bool Delete(PathNode pathNode, string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var p = PathNodeRepository.GetQueryable().FirstOrDefault(pn => pn.ID == pathNode.ID);
            if (p != null)
            {
                try
                {
                    PathNodeRepository.Delete(p);
                    PathNodeRepository.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {
                    strResult = "原因：已在使用";
                }
            }
            else
            {
                strResult = "原因：未找到当前需要删除的数据！";
            }
            return result;
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

        public System.Data.DataTable GetPathNode(int page, int rows, string PathID)
        {
            IQueryable<PathNode> PathNodeQuery = PathNodeRepository.GetQueryable();
            var PathNode = PathNodeQuery
                //.Where(p => p.ID == PathID)
                .OrderBy(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PathID,
                    p.PositionID,
                    p.PathNodeOrder,
                });
            if (!PathID.Equals(""))
            {
                PathNode = PathNodeQuery
                    //.Where(p => p.ID == PathID)
                    .OrderBy(p => p.ID).AsEnumerable()
                    .Select(p => new
                    {
                        p.ID,
                        p.PathID,
                        p.PositionID,
                        p.PathNodeOrder,
                    });
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("路径ID", typeof(string));
            dt.Columns.Add("位置ID", typeof(string));
            dt.Columns.Add("路径节点顺序", typeof(string));
            foreach (var item in PathNode)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.PathID,
                        item.PositionID,
                        item.PathNodeOrder
                    );
            }
            return dt;
        }
    }
}