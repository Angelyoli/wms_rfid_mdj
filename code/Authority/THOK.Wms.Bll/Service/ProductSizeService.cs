using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.Wms.Bll.Service
{
    public class ProductSizeService : ServiceBase<ProductSize>, IProductSizeService
    {
        [Dependency]
        public IProductSizeRepository ProductSizeRepository { get; set; }
        [Dependency]
        public IProductRepository ProductRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        //public object GetDetails(int page, int rows, string ProductCode, string SizeNo, string AreaNo)
        //{
        //    IQueryable<ProductSize> productSizeQuery = ProductSizeRepository.GetQueryable().Select();
        //    var productSize = productSizeQuery.Where(p => p.ProductCode.Contains(ProductCode))
        //        .OrderBy(p => p.ID).AsEnumerable()
        //        .Select(p => new
        //        {
        //            p.ID,
        //            p.ProductCode,
        //            p.ProductNo,
        //            p.SizeNo,
        //            p.AreaNo
        //        });
        //    int sizeno = -1, areano = -1;
        //    if ((SizeNo != "" && SizeNo != null) && (AreaNo == "" || AreaNo == null))
        //    {
        //        try { sizeno = Convert.ToInt32(SizeNo); }
        //        catch { sizeno = -1; }
        //        finally { productSize = productSize.Where(p => p.SizeNo == sizeno); }
        //    }
        //    if ((SizeNo == "" || SizeNo == null) && (AreaNo != "" && AreaNo != null))
        //    {
        //        try { areano = Convert.ToInt32(AreaNo); }
        //        catch { areano = -1; }
        //        finally { productSize = productSize.Where(p => p.AreaNo == areano); }
        //    }
        //    if ((SizeNo != "" && SizeNo != null) && (AreaNo != "" && AreaNo != null))
        //    {
        //        try { sizeno = Convert.ToInt32(SizeNo); areano = Convert.ToInt32(AreaNo); }
        //        catch { areano = -1; }
        //        finally { productSize = productSize.Where(p => p.SizeNo == sizeno && p.AreaNo == areano); }
        //    }
        //    productSize = productSize.OrderBy(p => p.ID).AsEnumerable()
        //        .Select(p => new
        //        {
        //            p.ID,
        //            p.ProductCode,
        //            p.ProductNo,
        //            p.SizeNo,
        //            p.AreaNo
        //        });
        //    int total = productSize.Count();
        //    productSize = productSize.Skip((page - 1) * rows).Take(rows);
        //    return new { total, rows = productSize.ToArray() };
        //}


        public object GetDetails(int page, int rows, ProductSize productSize)
        {

            IQueryable<ProductSize> productSizeQuery = ProductSizeRepository.GetQueryable();
            IQueryable<Product> productQuery = ProductRepository.GetQueryable();

            var productSizeDetail = productSizeQuery.Where(p =>
                p.ProductCode.Contains(productSize.ProductCode)).OrderBy(p => p.ID);
            var productSizeDetail1 = productSizeDetail;
            if (productSize.SizeNo != null && productSize.SizeNo != 0)
            {
                productSizeDetail1 = productSizeDetail.Where(p => p.SizeNo == productSize.SizeNo).OrderBy(p => p.ID);
            }
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
            var productSize_Detail = productSizeDetails.Join(productQuery,
                ps => ps.ProductCode,
                p => p.ProductCode,
                (ps, p) => new { ps.ID, ps.ProductCode, ps.ProductNo, ps.SizeNo, ps.AreaNo, p.ProductName })
                .Where(p => p.ProductCode.Contains(productSize.ProductCode))
                .OrderBy(p => p.ID).AsEnumerable().Select(p => new
            {
                p.ID,
                p.ProductCode,
                p.ProductName,
                p.ProductNo,
                p.SizeNo,
                p.AreaNo
            });
            ////var productSize_Detail = productSizeDetails.ToArray().Select(p=> new
            ////{
            ////    p.ID,
            ////    p.ProductCode,
            ////    ProductName = p.ProductCode,
            ////    p.ProductNo,
            ////    p.SizeNo,
            ////    p.AreaNo
            ////});
            return new { total, rows = productSize_Detail.ToArray() };
        }


        //public bool Add(ProductSize productSize, out string strResult)
        //{
        //    strResult = string.Empty;
        //    bool result = false;
        //    var pro = new ProductSize();
        //    if (pro != null)
        //    {
        //        try
        //        {
        //            pro.ID = productSize.ID;
        //            pro.ProductCode = productSize.ProductCode;
        //            pro.SizeNo = productSize.SizeNo;
        //            pro.AreaNo = productSize.AreaNo;

        //            ProductSizeRepository.Add(pro);
        //            ProductSizeRepository.SaveChanges();
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            strResult = "原因：" + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        strResult = "原因：找不到当前登陆用户！请重新登陆！";
        //    }
        //    return result;
        //}

        public bool Add(ProductSize productSize)
        {
            var pro = new ProductSize(); 
                    pro.ID = productSize.ID;
                    pro.ProductCode = productSize.ProductCode;
                    
                    pro.SizeNo = productSize.SizeNo;
                    pro.AreaNo = productSize.AreaNo;

                    ProductSizeRepository.Add(pro);
                    ProductSizeRepository.SaveChanges();
            return true;
        }

        public bool Save(ProductSize productSize)
        {
            var pro = ProductSizeRepository.GetQueryable().FirstOrDefault(s => s.ID == productSize.ID);
                    pro.ID = productSize.ID;
                    pro.ProductCode = productSize.ProductCode;
                    
                    pro.SizeNo = productSize.SizeNo;
                    pro.AreaNo = productSize.AreaNo;

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
                    p.SizeNo,
                    p.AreaNo
                });
            int total = productSize.Count();
            productSize = productSize.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = productSize.ToArray() };
        }

        public System.Data.DataTable GetProductSize(int page, int rows, string productCode)
        {
            IQueryable<ProductSize> productSizeQuery = ProductSizeRepository.GetQueryable();
            var productSize = productSizeQuery.Where(p => p.ProductCode.Contains(productCode))
                .OrderBy(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.ProductCode,
                    p.SizeNo,
                    p.AreaNo
                });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("商品ID", typeof(string));
            dt.Columns.Add("商品代码", typeof(string));
            dt.Columns.Add("件烟尺寸编号", typeof(string));
            dt.Columns.Add("存储库区号", typeof(string));
            foreach (var item in productSize)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.ProductCode,
                        item.SizeNo,
                        item.AreaNo
                    );
            }
            return dt;
        }
    }
}
