using System;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.WCS.Dal.Interfaces;
using THOK.WCS.DbModel;
using System.Linq;

namespace THOK.WCS.Bll.Service
{
    public class RegionService : ServiceBase<Region>, IRegionService
    {
        [Dependency]
        public IRegionRepository RegionRepository { get; set; }

        [Dependency]
        public IPositionRepository PositionRepository { get; set; }

        [Dependency]
        public IPathRepository PathRepository { get; set; }

        [Dependency]
        public IPathNodeRepository PathNodeRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, Region reg)
        {
            IQueryable<Region> regionQuery = RegionRepository.GetQueryable();
            var regDetail = regionQuery.Where(s =>
               s.RegionName.Contains(reg.RegionName)
               && s.State.Contains(reg.State)).OrderBy(ul => ul.RegionName);
            int total = regDetail.Count();
            var regDetails = regDetail.Skip((page - 1) * rows).Take(rows);
            var reg_Detail = regDetails.ToArray().Select(s => new
            {
                s.ID,
                s.RegionName,
                s.Description,
                State = s.State == "01" ? "可用" : "不可用"
            });
            return new { total, rows = reg_Detail.ToArray() };
        }

        public bool Add(Region region)
        {
            var reg = new Region();
            reg.ID = region.ID;
            reg.RegionName = region.RegionName;
            reg.State = region.State;
            reg.Description = region.Description;
            RegionRepository.Add(reg);
            RegionRepository.SaveChanges();
            return true;
        }

        public bool Save(Region region)
        {
            var reg = RegionRepository.GetQueryable().FirstOrDefault(s => s.ID == region.ID);
            reg.ID = region.ID;
            reg.RegionName = region.RegionName;
            reg.State = region.State;
            reg.Description = region.Description;
            RegionRepository.SaveChanges();
            return true;
        }

        public bool Delete(int regionId)
        {
            var reg = RegionRepository.GetQueryable().FirstOrDefault(s => s.ID == regionId );
            var position = PositionRepository.GetQueryable().FirstOrDefault(p=> p.RegionID == regionId);
            var path = PathRepository.GetQueryable().FirstOrDefault(pa => pa.TargetRegionID == regionId);
            var path1 = PathRepository.GetQueryable().FirstOrDefault(pa => pa.OriginRegionID == regionId);

           

            if (reg != null)
            {
                try
                {
                    if (position != null)
                    {
                        PositionRepository.Delete(position);
                        PositionRepository.SaveChanges();
                    }
                    if (path != null)
                    {
                        var pathNode = PathNodeRepository.GetQueryable().FirstOrDefault(p => p.PathID == path.ID);
                        if (pathNode != null)
                        {
                            PathNodeRepository.Delete(pathNode);
                            PathNodeRepository.SaveChanges();
                        }
                        PathRepository.Delete(path);
                        PathRepository.SaveChanges();
                    }
                    if (path1 != null)
                    {
                        var pathNode1 = PathNodeRepository.GetQueryable().FirstOrDefault(p => p.PathID == path1.ID);
                        if (pathNode1 != null)
                        {
                            PathNodeRepository.Delete(pathNode1);
                            PathNodeRepository.SaveChanges();
                        }
                        PathRepository.Delete(path1);
                        PathRepository.SaveChanges();
                    }
                    RegionRepository.Delete(reg);
                    RegionRepository.SaveChanges();
                }

                catch (Exception e) 
                { 
                
                }
            }
            else
                return false;
            return true;
        }

        public object GetRegion(int page, int rows, string queryString, string value)
        {

            string id = "", regionName = "";

            if (queryString == "id")
            {
                id = value;

            //string regionName = "";
            //int id=-1;
            //if (queryString == "ID" && value!="")
            //{
            //    try { id = Convert.ToInt32(value); }
            //    catch { id =0; }

            }
            else
            {
                regionName = value;
            }

            IQueryable<Region> employeeQuery = RegionRepository.GetQueryable();
            var region = employeeQuery.Where(r =>r.RegionName.Contains(regionName) && r.State == "01")
                .OrderBy(r => r.ID).AsEnumerable()
                .Select(r => new
                {
                    r.ID,
                    r.Description,
                    r.RegionName,
                    State = r.State == "01" ? "可用" : "不可用",

            //IQueryable<Region> regionQuery = RegionRepository.GetQueryable();
            //var region = regionQuery.Where(r=> r.State == "01")
            //    .OrderBy(r => r.ID).AsEnumerable().
            //    Select(r => new
            //    {
            //        r.ID,
            //        r.RegionName,
            //        State = r.State == "01" ? "可用" : "不可用"
            //    });
            //if (id >=0)
            //{
            //    region = region.Where(r => r.ID == id);
            //}
            //else 
            //{
            //    region = region.Where(r => r.RegionName.Contains(regionName));
            //}
            //region = region.AsEnumerable().
            //    Select(r => new
            //    {
            //        r.ID,
            //        r.RegionName,
            //        r.State

            });
            int total = region.Count();
            region = region.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = region.ToArray() };
           
        }

        public System.Data.DataTable GetRegion(int page, int rows, Region region)
        {
            IQueryable<Region> regionQuery = RegionRepository.GetQueryable();
            var RegionDetail = regionQuery.Where(r =>
               r.RegionName.Contains(region.RegionName)
               && r.Description.Contains(region.Description)
               && r.State.Contains(region.State)).OrderBy(ul => ul.RegionName);
            //int total = RegionDetail.Count();
            //var sRMDetails = RegionDetail.Skip((page - 1) * rows).Take(rows);
            var sRM_Detail = RegionDetail.ToArray().Select(r => new
            {
                r.ID,
                r.RegionName,
                r.Description,
                State = r.State == "01" ? "可用" : "不可用"
            });
        
           System.Data.DataTable dt = new System.Data.DataTable();
           dt.Columns.Add("区域编码", typeof(string));
           dt.Columns.Add("区域名称", typeof(string));
           dt.Columns.Add("描述", typeof(string));
           dt.Columns.Add("是否可用", typeof(string));
           foreach (var item in sRM_Detail)
           {
               dt.Rows.Add
                   (
                       item.ID,
                       item.RegionName,
                       item.Description,
                       item.State
                    );
            }
            return dt;
        }

    }
}

//            var srm = regionQuery.Where(r => r.RegionName.Contains(regionName))
//                .OrderBy(r => r.ID).AsEnumerable()
//                .Select(r => new
//                {
//                    r.ID,
//                    r.RegionName,
//                    r.Description,
//                    State = r.State == "01" ? "可用" : "不可用"
//                });
//            if (!state.Equals(""))
//            {
//                srm = regionQuery.Where(r =>r.RegionName.Contains(regionName) && r.State.Contains(state))
//                    .OrderBy(r => r.ID).AsEnumerable()
//                    .Select(r => new
//                    {
//                        r.ID,
//                        r.RegionName,
//                        r.Description,
//                        State = r.State == "01" ? "可用" : "不可用"
//                    });
//            }
//            System.Data.DataTable dt = new System.Data.DataTable();
//            dt.Columns.Add("区域编码", typeof(string));
//            dt.Columns.Add("区域名称", typeof(string));
//            dt.Columns.Add("描述", typeof(string));
//            dt.Columns.Add("是否可用", typeof(string));
//            foreach (var item in srm)
//            {
//                dt.Rows.Add
//                    (
//                        item.ID,
//                        item.RegionName,
//                        item.Description,
//                        item.State
//                    );
//            }
//            return dt;
//        }

//    }
//}
