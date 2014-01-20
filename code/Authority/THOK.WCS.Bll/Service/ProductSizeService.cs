using System;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.WCS.Dal.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.WCS.DbModel;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.WCS.Bll.Service
{
    public class ProductSizeService : ServiceBase<ProductSize>, IProductSizeService
    {
        [Dependency]
        public IProductSizeRepository ProductSizeRepository { get; set; }
        [Dependency]
        public IProductRepository ProductRepository { get; set; }
        [Dependency]
        public ISizeRepository SizeRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, ProductSize productSize)
        {
            IQueryable<ProductSize> productSizeQuery = ProductSizeRepository.GetQueryable();
            IQueryable<Product> productQuery = ProductRepository.GetQueryable();
            //IQueryable<Size> sizeQuery = SizeRepository.GetQueryable();

            var productSizeDetail = productSizeQuery.Where(p =>
                p.ProductCode.Contains(productSize.ProductCode)).OrderBy(p => p.ID);
            var productSizeDetail1 = productSizeDetail;
            //if (productSize.SizeNo != null && productSize.SizeNo != 0)
            //{
            //    productSizeDetail1 = productSizeDetail.Where(p => p.SizeNo == productSize.SizeNo).OrderBy(p => p.ID);
            //}
            var productSizeDetail2 = productSizeDetail1;
            if (productSize.ProductNo != null && productSize.ProductNo != 0)
            {
                productSizeDetail2 = productSizeDetail1.Where(p => p.ProductNo == productSize.ProductNo).OrderBy(p => p.ID);
            }
            var productSizeDetail3 = productSizeDetail2;
            if (productSize.AreaNo != null && productSize.AreaNo != 0)
            {
                productSizeDetail3 = productSizeDetail2.Where(p => p.AreaNo == productSize.AreaNo).OrderBy(p => p.ID);
            }
            int total = productSizeDetail3.Count();
            var productSizeDetails = productSizeDetail3.Skip((page - 1) * rows).Take(rows);
            var temp = productSizeDetails
                    .Join(productQuery
                        , ps => ps.ProductCode
                        , p => p.ProductCode
                        , (ps, p) => new { ps.ID, ps.ProductCode, ps.ProductNo, ps.SizeNo, ps.AreaNo, ps.Length, ps.Width, ps.Height, p.ProductName })
                  //.Join(sizeQuery
                  //    , ps => ps.SizeNo
                  //    , s => s.SizeNo
                  //    , (ps, s) => new { ps.ID, ps.ProductCode ,ps.ProductNo, ps.SizeNo, ps.AreaNo,ps.ProductName ,s.Length, s.Width, s.Height })
                .Where(p => p.ProductCode.Contains(productSize.ProductCode))
                .OrderBy(p => p.ID).AsEnumerable().Select(p => new
            {
                p.ID,
                p.ProductCode,
                p.ProductName,
                p.ProductNo,
                //p.SizeNo,
                AreaNo = p.AreaNo == 1 ? "密集库一区" : p.AreaNo == 2 ? "密集库二区" : p.AreaNo == 3 ? "大品种一区" : p.AreaNo == 4 ? "大品种二区" : p.AreaNo == 5 ? "小品种" : p.AreaNo == 6 ? "异型烟" : "异常",
                p.Length,
                p.Width,
                p.Height
            });
            return new { total, rows = temp.ToArray() };
        }

        public bool Add(ProductSize productSize)
        {
            var maxProductNo = ProductSizeRepository.GetQueryable().Where(p => p.ProductCode != "emptypallet").Max(p => p.ProductNo);
            var pro = new ProductSize(); 
            pro.ID = productSize.ID;
            pro.ProductCode = productSize.ProductCode;
            if (productSize.ProductNo != 0)
            {
                pro.ProductNo = productSize.ProductNo;
            }
            else
            {
                pro.ProductNo = maxProductNo + 1;
            }
            //pro.SizeNo = productSize.SizeNo;
            pro.AreaNo = productSize.AreaNo;
            pro.Length = productSize.Length;
            pro.Width = productSize.Width;
            pro.Height = productSize.Height;

            ProductSizeRepository.Add(pro);
            ProductSizeRepository.SaveChanges();
            return true;
        }

        public bool Save(ProductSize productSize)
        {
            var pro = ProductSizeRepository.GetQueryable().FirstOrDefault(s => s.ID == productSize.ID);
            pro.ID = productSize.ID;
            pro.ProductCode = productSize.ProductCode;
            pro.ProductNo = productSize.ProductNo;
            //pro.SizeNo = productSize.SizeNo;
            pro.AreaNo = productSize.AreaNo;
            pro.Length = productSize.Length;
            pro.Width = productSize.Width;
            pro.Height = productSize.Height;

            ProductSizeRepository.SaveChanges();
            return true;
        }

        public bool Delete(int productSizeId)
        {
            var si = ProductSizeRepository.GetQueryable().FirstOrDefault(s => s.ID == productSizeId);
            if (si != null)
            {
                ProductSizeRepository.Delete(si);
                ProductSizeRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public object GetProductSize(int page, int rows, string queryString, string value)
        {
            string id = "", productCode = "";

            if (queryString == "id")
            {
                id = value;
            }
            else
            {
                productCode = value;
            }
            IQueryable<ProductSize> productSizeQuery = ProductSizeRepository.GetQueryable();
            int Id = Convert.ToInt32(id);
            var productSize = productSizeQuery.Where(p => p.ID == Id && p.ProductCode.Contains(productCode))
                .OrderBy(p => p.ID).AsEnumerable().
                Select(p => new
                {
                    p.ID,
                    p.ProductCode,
                    p.ProductNo,
                    p.SizeNo,
                    p.AreaNo,
                    p.Length,
                    p.Width,
                    p.Height
                });
            int total = productSize.Count();
            productSize = productSize.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = productSize.ToArray() };
        }

        public System.Data.DataTable GetProductSize(int page, int rows, ProductSize productSize)
        {
            IQueryable<ProductSize> productSizeQuery = ProductSizeRepository.GetQueryable();
            IQueryable<Product> productQuery = ProductRepository.GetQueryable();

            var productSizeDetail = productSizeQuery.Where(p =>
                p.ProductCode.Contains(productSize.ProductCode))
                .OrderBy(p => p.ID);

            //var productSize_Detail = productSizeDetail.ToArray().Select(p => new
            //    {
            //        p.ID,
            //        p.ProductCode,
            //        p.ProductNo,
            //        p.ProductName,
            //        p.SizeNo,
            //        p.AreaNo
            //    });
            var productSize_Detail = productSizeDetail.Join(productQuery,
                ps => ps.ProductCode,
                p => p.ProductCode,
                (ps, p) => new { ps.ID, ps.ProductCode, ps.ProductNo, ps.SizeNo, ps.AreaNo, ps.Length, ps.Width, ps.Height, p.ProductName })
                .Where(p => p.ProductCode.Contains(productSize.ProductCode))
                .OrderBy(p => p.ID).AsEnumerable().Select(p => new
                {
                    p.ID,
                    p.ProductCode,
                    p.ProductName,
                    p.ProductNo,
                    //p.SizeNo,
                    p.Length,
                    p.Width,
                    p.Height,
                    p.AreaNo
                });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("商品ID", typeof(string));
            dt.Columns.Add("商品代码", typeof(string));
            dt.Columns.Add("商品名称", typeof(string));
            dt.Columns.Add("商品简码", typeof(string));
            //dt.Columns.Add("件烟尺寸编号", typeof(string));
            dt.Columns.Add("长度", typeof(string));
            dt.Columns.Add("宽度", typeof(string));
            dt.Columns.Add("高度", typeof(string));
            dt.Columns.Add("存储库区号", typeof(string));
            foreach (var item in productSize_Detail)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.ProductCode,
                        item.ProductName,
                        item.ProductNo,
                        //item.SizeNo,
                        item.AreaNo,
                        item.Length,
                        item.Width,
                        item.Height
                    );
            }
            return dt;
        }
    }
}
