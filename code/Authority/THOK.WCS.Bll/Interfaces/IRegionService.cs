using THOK.WCS.DbModel;

namespace THOK.WCS.Bll.Interfaces
{
     public interface IRegionService:IService<Region>
    {
         object GetDetails(int page, int rows, Region reg);

         bool Add(Region region);

         bool Save(Region region);

         bool Delete(int regionId);

         object GetRegion(int page, int rows, string queryString, string value);

         //System.Data.DataTable GetRegion(int page, int rows, string regionName, string state, string t);


         System.Data.DataTable GetRegion(int page, int rows, Region region);
    }
}
