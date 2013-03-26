using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class PathService : ServiceBase<Path>, IPathService
    {
        [Dependency]
        public IPathRepository PathRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        //public object GetDetails(int page, int rows, int ID, string PathName, string Description, string State)
        //{
        //    IQueryable<Path> jobQuery = PathRepository.GetQueryable();
        //    var path = jobQuery.Where(p => p.PathName.Contains(PathName))
        //        .OrderByDescending(p => p.ID).AsEnumerable()
        //        .Select(p => new
        //        {
        //            p.ID,
        //            p.PathName,
        //            p.OriginRegionID,
        //            p.TargetRegionID,
        //            p.Description,
        //            State = p.State == "1" ? "可用" : "不可用",
        //            //UpdateTime = p.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
        //        });
        //    if (!State.Equals(""))
        //    {
        //        path = jobQuery.Where(p => p.PathName.Contains(PathName) && p.State.Contains(State))
        //             .OrderByDescending(p => p.ID).AsEnumerable()
        //            .Select(p => new
        //            {
        //                p.ID,
        //                p.PathName,
        //                p.OriginRegionID,
        //                p.TargetRegionID,
        //                p.Description,
        //                State = p.State == "1" ? "可用" : "不可用",
        //            });
        //    }
        //    int total = path.Count();
        //    path = path.Skip((page - 1) * rows).Take(rows);
        //    return new { total, rows = path.ToArray() };
        //}


       public bool Add(Path path, out string strResult)
       {
           strResult = string.Empty;
           bool result = false;
           var pa = new Path();
           var pathExist = PathRepository.GetQueryable().FirstOrDefault(p => p.ID == path.ID);
           if (pathExist == null)
           {
               if (pa != null)
               {
                   try
                   {
                       pa.ID = path.ID;
                       pa.PathName = path.PathName;
                       pa.Description = path.Description;
                       pa.State = path.State;
                       pa.OriginRegionID = path.OriginRegionID;
                       pa.TargetRegionID = pa.TargetRegionID;

                       PathRepository.Add(pa);
                       PathRepository.SaveChanges();
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
           }
           else
           {
               strResult = "原因：该编号已存在！";
           }
           return result;
       }


       public bool Save(Path path, out string strResult)
       {
           strResult = string.Empty;
           bool result = false;
           var pa = PathRepository.GetQueryable().FirstOrDefault(p => p.ID == path.ID);

           if (pa != null)
           {
               try
               {
                   pa.ID = path.ID;
                   pa.PathName = path.PathName;
                   pa.Description = path.Description;
                   pa.State = path.State;
                   pa.OriginRegionID = path.OriginRegionID;
                   pa.TargetRegionID = path.TargetRegionID;
                   PathRepository.SaveChanges();
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


       public bool Delete(int pathId, out string strResult)
       {
           strResult = string.Empty;
           bool result = false;
           //Guid pathId = new Guid(pathId);
           var path = PathRepository.GetQueryable().FirstOrDefault(p => p.ID == pathId);
           if (path != null)
           {
               try
               {
                   PathRepository.Delete(path);
                   PathRepository.SaveChanges();
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


       //public object GetJob(int page, int rows, string queryString, string value)
       //{
       //    throw new NotImplementedException();
       //}


       public System.Data.DataTable GetJob(int page, int rows, string PathName, string Description, string State)
       {
           IQueryable<Path> jobQuery = PathRepository.GetQueryable();
           var path = jobQuery.Where(p => p.PathName.Contains(PathName))
                .OrderByDescending(p => p.ID).AsEnumerable()
               .Select(p => new
               {
                   p.ID,
                   p.PathName,
                   p.OriginRegionID,
                   p.TargetRegionID,
                   p.Description,
                   State = p.State == "1" ? "可用" : "不可用",
               });
           if (!State.Equals(""))
           {
               path = jobQuery.Where(p => p.PathName.Contains(PathName) && p.State.Contains(State))
                    .OrderByDescending(p => p.ID).AsEnumerable()
                   .Select(p => new
                   {
                       p.ID,
                       p.PathName,
                       p.OriginRegionID,
                       p.TargetRegionID,
                       p.Description,
                       State = p.State == "1" ? "可用" : "不可用",
                   });
           }
           System.Data.DataTable dt = new System.Data.DataTable();
           dt.Columns.Add("路径ID", typeof(int));
           dt.Columns.Add("路径名称", typeof(string));
           dt.Columns.Add("描述", typeof(string));
           dt.Columns.Add("是否可用", typeof(string));
           //dt.Columns.Add("更新时间", typeof(string));
           dt.Columns.Add("起始区域ID", typeof(int));
           dt.Columns.Add("目标区域ID", typeof(int));
           foreach (var item in path)
           {
               dt.Rows.Add
                   (
                       item.ID,
                       item.PathName,
                       item.Description,
                       item.State,
                       item.TargetRegionID,
                       item.OriginRegionID
                   );
           }
           return dt;
       }


       public object GetJob(int page, int rows, string queryString, string value)
       {
           throw new NotImplementedException();
       }

       public object GetDetails(int page, int rows, string ID, string PathName, string Description, string State)
       {
           IQueryable<Path> jobQuery = PathRepository.GetQueryable();
           var path = jobQuery.Where(p => p.PathName.Contains(PathName))
               .OrderByDescending(p => p.ID).AsEnumerable()
               .Select(p => new
               {
                   p.ID,
                   p.PathName,
                   p.OriginRegionID,
                   p.TargetRegionID,
                   p.Description,
                   State = p.State == "1" ? "可用" : "不可用",
                   //UpdateTime = p.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
               });
           if (!State.Equals(""))
           {
               path = jobQuery.Where(p => p.PathName.Contains(PathName) && p.State.Contains(State))
                    .OrderByDescending(p => p.ID).AsEnumerable()
                   .Select(p => new
                   {
                       p.ID,
                       p.PathName,
                       p.OriginRegionID,
                       p.TargetRegionID,
                       p.Description,
                       State = p.State == "1" ? "可用" : "不可用",
                   });
           }
           int total = path.Count();
           path = path.Skip((page - 1) * rows).Take(rows);
           return new { total, rows = path.ToArray() };
       }
    }
}


