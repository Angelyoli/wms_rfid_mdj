using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IProductSizeService : IService<ProductSize>
    {
        //object GetDetails(int page, int rows, string ProductCode, string SizeNo, string AreaNo);
        object GetDetails(int page, int rows, ProductSize productSize);

        bool Add(ProductSize productSize);

        bool Save(ProductSize productSize);

        bool Delete(int productSizeId);

        object GetProductSize(int page, int rows, string queryString, string value);

        System.Data.DataTable GetProductSize(int page, int rows, string productCode);
    }
}
