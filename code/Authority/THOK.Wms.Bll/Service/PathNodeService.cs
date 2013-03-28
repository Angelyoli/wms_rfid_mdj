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
        private object PathID;
        [Dependency]
        public IPathNodeRepository PathNodeRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string ID, string PathID, string PositionID, string PathNodeOrder)
        {

            string i = "";
            IQueryable<PathNode> PathNodeQuery = PathNodeRepository.GetQueryable();
            int Id = Convert.ToInt32(i);
            var PathNode = PathNodeQuery.Where(p => p.ID == Id)
                .OrderBy(p => p.ID).AsEnumerable().
                Select(p => new
                {
                    p.ID,
                    p.PathID,
                    p.PositionID,
                    p.PathNodeOrder,
                });
            int id = -1;
            int sizeno = -1;
            if ((ID != "" && ID != null) && (PositionID == "" || PositionID == null))
            {
                try
                {
                    id = Convert.ToInt32(ID);
                }
                catch
                {
                    id = -1;
                }
                finally
                {
                    var pn = PathNodeQuery.Where(p => p.ID == Id)
                        .OrderBy(p => p.ID).AsEnumerable().
                        Select(p => new
                        {
                            p.ID,
                            p.PathID,
                            p.PositionID,
                            p.PathNodeOrder,
                        });
                }
            }
            else if ((ID == "" || ID == null) && (PositionID != "" && PositionID != null))
            {
                try
                {
                    sizeno = Convert.ToInt32(PositionID);
                }
                catch
                {
                    sizeno = -1;
                }
                finally
                {
                    var pn = PathNodeQuery.Where(p => p.ID == Id)
                    .OrderBy(p => p.ID).AsEnumerable().
                    Select(p => new
                    {
                        p.ID,
                        p.PathID,
                        p.PositionID,
                        p.PathNodeOrder,
                    });
                }
            }
            else if ((ID != "" && ID != null) && (PositionID != "" && PositionID != null))
            {
                try
                {
                    id = Convert.ToInt32(ID);
                    sizeno = Convert.ToInt32(PositionID);
                }
                catch
                {
                    id = -1;
                }
                finally
                {
                    var pn = PathNodeQuery.Where(p => p.ID == Id)
                    .OrderBy(p => p.ID).AsEnumerable().
                    Select(p => new
                    {
                        p.ID,
                        p.PathID,
                        p.PositionID,
                        p.PathNodeOrder,
                    });
                }
            }
            int total = PathNode.Count();
            PathNode = PathNode.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = PathNode.ToArray() };
        }



        public bool Add(PathNode PathNode, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var pn = new PathNode();
            if (pn != null)
            {
                try
                {
                    //pn.PathID = pn.PathID;
                    pn.PositionID = PathNode.PositionID;
                    pn.PathNodeOrder = PathNode.PathNodeOrder;

                    PathNodeRepository.Add(pn);
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

        public bool Save(PathNode PathNode, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var pn = PathNodeRepository.GetQueryable().FirstOrDefault(p => p.ID == PathNode.ID);

            if (pn != null)
            {
                try
                {
                    pn.PathID = pn.PathID;
                    pn.PositionID = PathNode.PositionID;
                    pn.PathNodeOrder = PathNode.PathNodeOrder;

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


    //    public bool Delete(int PathID, out string strResult)
    //    {
    //        strResult = string.Empty;
    //        bool result = false;
    //        var PathNode = PathNodeRepository.GetQueryable().FirstOrDefault(p => p.ID == PathID);
    //        if (PathNode != null)
    //        {
    //            try
    //            {
    //                PathNodeRepository.Delete(PathNode);
    //                PathNodeRepository.SaveChanges();
    //                result = true;
    //            }
    //            catch (Exception)
    //            {
    //                strResult = "原因：已在使用";
    //            }
    //        }
    //        else
    //        {
    //            strResult = "原因：未找到当前需要删除的数据！";
    //        }
    //        return result;
    //    }

    //    //public object GetDetails(int page, int rows, string ID, string PathID, string PositionID, string PathNodeOrder)
    //    //public object GetDetails(int page, int rows, string ID, string SRMName, string Description, string State)
    //    public object GetPathNode(int page, int rows, string queryString, string value)
    //    {
    //        string id = "", PathID = "";

    //        if (queryString == "id")
    //        {
    //            id = value;
    //        }
    //        else
    //        {
    //            PathID = value;
    //        }
    //        IQueryable<PathNode> PathNodeQuery = PathNodeRepository.GetQueryable();
    //        int Id = Convert.ToInt32(id);
    //        var PathNode = PathNodeQuery.Where(p => p.ID == Id )
    //            .OrderBy(p => p.ID).AsEnumerable().
    //            Select(p => new
    //            {
    //                p.ID,
    //                p.PathID,
    //                p.PositionID,
    //                p.PathNodeOrder,
    //            });
    //        int total = PathNode.Count();
    //        PathNode = PathNode.Skip((page - 1) * rows).Take(rows);
    //        return new { total, rows = PathNode.ToArray() };
    //    }

    //    public System.Data.DataTable GetPathNode(int page, int rows, string PathID, string state, string t)
    //    {
    //        string id = "";
    //        IQueryable<PathNode> PathNodeQuery = PathNodeRepository.GetQueryable();
    //        int Id = Convert.ToInt32(id);
    //        var PathNode = PathNodeQuery.Where(p => p.ID == Id)
    //            .OrderBy(p => p.ID).AsEnumerable()
    //            .Select(p => new
    //            {
    //                p.ID,
    //                p.PathID,
    //                p.PositionID,
    //                p.PathNodeOrder,
    //            });
    //        if (!state.Equals(""))
    //        {
    //            PathNode = PathNodeQuery.Where(p => p.ID == Id)
    //                .OrderBy(p => p.ID).AsEnumerable()
    //                .Select(p => new
    //                {
    //                    p.ID,
    //                    p.PathID,
    //                    p.PositionID,
    //                    p.PathNodeOrder,
    //                });
    //        }
    //        System.Data.DataTable dt = new System.Data.DataTable();
    //        dt.Columns.Add("ID", typeof(string));
    //        dt.Columns.Add("路径ID", typeof(string));
    //        dt.Columns.Add("位置ID", typeof(string));
    //        dt.Columns.Add("路径节点顺序", typeof(string));
    //        foreach (var item in PathNode)
    //        {
    //            dt.Rows.Add
    //                (
    //                    item.ID,
    //                    item.PathID,
    //                    item.PositionID,
    //                    item.PathNodeOrder
    //                );
    //        }
    //        return dt;
    //    }

        public bool Add(PathNode PathNode, string strResult)
        {
            throw new NotImplementedException();
        }


        public bool Save(PathNode ID, string strResult)
        {
            throw new NotImplementedException();
        }
    }
}
