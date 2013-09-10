using THOK.WCS.DbModel;

namespace THOK.WCS.Bll.Interfaces
{
    public interface IProductSizeService : IService<ProductSize>
    {
        object GetDetails(int page, int rows, ProductSize productSize);

        bool Add(ProductSize productSize);

        bool Save(ProductSize productSize);

        bool Delete(int productSizeId);

        object GetProductSize(int page, int rows, string queryString, string value);

        System.Data.DataTable GetProductSize(int page, int rows, ProductSize productSize);
    }
}
