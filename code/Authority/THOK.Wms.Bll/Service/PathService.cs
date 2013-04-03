using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Data;
using THOK.WMS.Upload.Bll;

namespace THOK.Wms.Bll.Service
{
    public class PathService : ServiceBase<Path>, IPathService
    {
        [Dependency]
        public IPathRepository PathRepository { get; set; }

        public IRegionRepository RegionRepository { get; set; }

        UploadBll Upload = new UploadBll();


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(Path path, out string strResult)
       {
          strResult = string.Empty;
            bool result = false;
            var pa = new Path();
            var re = new Region();
            var region = RegionRepository.GetQueryable().FirstOrDefault(r => r.ID == path.ID);
            //var department = DepartmentRepository.GetQueryable().FirstOrDefault(d => d.ID == employee.DepartmentID);
            var empExist = PathRepository.GetQueryable().FirstOrDefault(e => e.ID == path.ID);
            if (empExist == null)
            {
                if (pa != null)
                {
                    try
                    {
                        //pa.ID = Guid.NewGuid();
                        pa.ID = path.ID;
                        pa.PathName = path.PathName;
                        pa.Description = path.Description;
                        //pa.Department = department;
                        pa.OriginRegion = path.OriginRegion;
                        pa.TargetRegion = path.TargetRegion;
                        pa.TargetRegionID = region.ID;
                        pa.OriginRegionID = region.ID;
                       
                        pa.State = path.State;
                        //pa.UserName = employee.UserName;
                        //pa.UpdateTime = DateTime.Now;
                        PathRepository.Add(pa);
                        PathRepository.SaveChanges();
                        //人员信息上报
                        //DataSet ds = this.Insert(emp);
                        //Upload.UploadEmployee(ds);
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
           //var department = DepartmentRepository.GetQueryable().FirstOrDefault(d => d.ID == employee.DepartmentID);
           var region = RegionRepository.GetQueryable().FirstOrDefault(r => r.ID == path.ID);

           if (pa != null)
           {
               try
               {
                   //pa.ID = Guid.NewGuid();
                   pa.ID = path.ID;
                   pa.PathName = path.PathName;
                   pa.Description = path.Description;
                   //pa.Department = department;
                   pa.OriginRegion = path.OriginRegion;
                   pa.TargetRegion = path.TargetRegion;
                   pa.TargetRegionID = region.ID;
                   pa.OriginRegionID = region.ID;

                   pa.State = path.State;
                   //pa.UserName = employee.UserName;
                   //pa.UpdateTime = DateTime.Now;
                   PathRepository.Add(pa);
                   PathRepository.SaveChanges();
                   //人员信息上报
                   //DataSet ds = this.Insert(emp);
                   //Upload.UploadEmployee(ds);
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


       public object GetPath(int page, int rows,string value)
       {
           IQueryable<Path> pathQuery = PathRepository.GetQueryable();
           var path = pathQuery.Where(p => p.PathName.Contains(value) && p.State == "01")
               .OrderBy(p => p.ID).AsEnumerable().
               Select(p => new
               {
                   p.ID,
                   p.PathName,
                   State = p.State == "01" ? "可用" : "不可用",
               });
           int total = path.Count();
           path = path.Skip((page - 1) * rows).Take(rows);
           return new { total, rows = path.ToArray() };
       }


       public System.Data.DataTable GetPath(int page, int rows, string PathName,  string Description, string State)
       {
           IQueryable<Path> pathQuery = PathRepository.GetQueryable();
           var path = pathQuery.Where(p => p.PathName.Contains(PathName))
                .OrderByDescending(p => p.ID).AsEnumerable()
               .Select(p => new
               {
                   p.ID,
                   p.PathName,
                   p.OriginRegionID,
                   p.TargetRegionID,
                   p.Description,
                   //p.OriginRegion,
                   //p.TargetRegion,
                   State = p.State == "1" ? "可用" : "不可用",
               });
           if (!State.Equals(""))
           {
               path = pathQuery.Where(p => p.PathName.Contains(PathName) && p.State.Contains(State))
                    .OrderByDescending(p => p.ID).AsEnumerable()
                   .Select(p => new
                   {
                       p.ID,
                       p.PathName,
                       p.OriginRegionID,
                       p.TargetRegionID,
                       p.Description,
                       //p.TargetRegion,
                       //p.OriginRegion,
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


      

       public object GetDetails(int page, int rows, string ID, string PathName, string RegionID , string Description, string State)
       {

           IQueryable<Path> pathQuery = PathRepository.GetQueryable();
           var path = pathQuery.Where(p => p.PathName.Contains(PathName)
               && p.State.Contains(State));

           //if (!DepartmentID.Equals(string.Empty))
           //{
           //    Guid departID = new Guid(DepartmentID);
           //    path = path.Where(e => e.DepartmentID == departID);
           //}
           if (!RegionID.Equals(string.Empty))
           {
               Guid jobID = new Guid(RegionID);
               //path = path.Where(p =>p.ID == );
           }

           var temp = path.AsEnumerable().OrderByDescending(p => p.ID).AsEnumerable().Select(p => new
           {
               p.ID,
               p.PathName,
               p.OriginRegionID,
               p.TargetRegionID,
               p.Description,
               State = p.State == "1" ? "可用" : "不可用",
           });
           int total = temp.Count();
           temp = temp.Skip((page - 1) * rows).Take(rows);
           return new { total, rows = temp.ToArray() };
       }


      
    }
}


