using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.Wms.Bll.Service
{
    public class RegionService : ServiceBase<Region>, IRegionService
    {
        [Dependency]
        public IRegionRepository RegionRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string ID, string RegionName, string State)
        {
            IQueryable<Region> regionQuery = RegionRepository.GetQueryable();
            var region = regionQuery.Where(r => r.RegionName.Contains(RegionName) && r.State.Contains(State))
                .OrderBy(r => r.ID).AsEnumerable()
                .Select(r => new
                {
                    r.ID,
                    r.RegionName,
                    r.Description,
                    State = r.State == "01" ? "可用" : "不可用",
                });
            if (ID != "" && ID != null)
            {
                try
                {
                    int k = Convert.ToInt32(ID);
                    region = regionQuery.Where(r => r.RegionName.Contains(RegionName) && r.State.Contains(State) && r.ID == k)
                        .OrderBy(r => r.ID).AsEnumerable()
                        .Select(r => new
                        {
                            r.ID,
                            r.RegionName,
                            r.Description,
                            State = r.State == "01" ? "可用" : "不可用",
                        });
                }
                finally { }
            }
            int total = region.Count();
            region = region.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = region.ToArray() };
        }
    }
}
