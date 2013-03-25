using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
     public interface IRegionService:IService<Region>
    {
         object GetDetails(int page, int rows, string ID, string RegionName, string State);
    }
}
