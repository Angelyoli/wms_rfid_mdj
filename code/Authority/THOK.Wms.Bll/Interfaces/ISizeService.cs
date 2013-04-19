using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISizeService : IService<Size>
    {
       object GetDetails(int page, int rows,string SizeName, string SizeNo);

        bool Add(Size size);

        bool Save(Size size);

        bool Delete(int sizeId);

        object GetSize(int page, int rows, string queryString, string value);

        System.Data.DataTable GetSize(int page, int rows, string sizeName);

        
    }
}
