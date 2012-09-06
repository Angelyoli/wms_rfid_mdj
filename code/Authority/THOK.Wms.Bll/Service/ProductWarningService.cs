using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
     public class ProductWarningService:ServiceBase<ProductWarning>,IProductWarningService
    {
         [Dependency]
         public IProductWarningRepository ProductWarningRepository { get; set; }
         [Dependency]
         public IStorageRepository StorageRepository { get; set; }
         [Dependency]
         public IProductRepository ProductRepository { get; set; }

         protected override Type LogPrefix
         {
             get { return this.GetType(); }
         }
         #region  卷烟预警设置的增，删，改，查方法
         public object GetDetail(int page, int rows, string productCode, decimal minLimited, decimal maxLimited, decimal assemblyTime)
         {
             IQueryable<ProductWarning> productWarningQuery = ProductWarningRepository.GetQueryable();
             var productWarning = productWarningQuery.Where(p => p.ProductCode == productCode
                 || p.MinLimited == minLimited || p.MaxLimited == maxLimited || p.AssemblyTime == assemblyTime)
                 .OrderBy(p => p.ProductCode)
                 .Select(p => new 
                 {
                     p.ProductCode,
                     p.MinLimited,
                     p.MaxLimited,
                     p.AssemblyTime,
                     p.Memo
                 });
             int total = productWarning.Count();
             productWarning = productWarning.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = productWarning.ToArray() };
         }

         public bool Add(ProductWarning productWarning)
         {
             var productWarn = new ProductWarning();

             productWarn.ProductCode = productWarning.ProductCode;
             productWarn.MinLimited = productWarning.MinLimited;
             productWarn.MaxLimited = productWarning.MaxLimited;
             productWarn.AssemblyTime = productWarning.AssemblyTime;
             productWarn.Memo = productWarning.Memo;

             ProductWarningRepository.Add(productWarn);
             ProductWarningRepository.SaveChanges();
             return true;
         }

         public bool Delete(string productCode)
         {
             var productWarn = ProductWarningRepository.GetQueryable()
                 .FirstOrDefault(p => p.ProductCode == productCode);

             if (productWarn != null)
             {
                 ProductWarningRepository.Delete(productWarn);
                 ProductWarningRepository.SaveChanges();
             }
             else 
             {
                 return false;
             }
             return true;
         }

         public bool Save(ProductWarning productWarning)
         {
             var productWarn = ProductWarningRepository.GetQueryable()
                 .FirstOrDefault(p => p.ProductCode == productWarning.ProductCode);

             productWarn.ProductCode = productWarning.ProductCode;
             productWarn.MinLimited = productWarning.MinLimited;
             productWarn.MaxLimited = productWarning.MaxLimited;
             productWarn.AssemblyTime = productWarning.AssemblyTime;
             productWarn.Memo = productWarning.Memo;

             ProductWarningRepository.SaveChanges();
             return true;
         }
        #endregion

        #region 产品短缺、超储查询
         public object GetQtyLimitsDetail(int page, int rows, string productCode, decimal minLimited, decimal maxLimited, string unitType)
         {
            if (unitType == null || unitType=="")
            {
                unitType = "1";
            }
             IQueryable<Storage> Quantity = StorageRepository.GetQueryable();
             var Qty = Quantity.Where(t => t.ProductCode == productCode)
                 .GroupBy(t=>t.ProductCode)
                 .Select(t => new 
                 {
                     productCode =t.Max(p=>p.Product.ProductCode),
                     productName=t.Max(p=>p.Product.ProductName),
                     quantity=t.Sum(p=>p.Quantity),
                     UnitName01 =t.Max(p => p.Product.UnitList.Unit01.UnitName),
                     UnitName02 =t.Max(p => p.Product.UnitList.Unit02.UnitName),
                     Count01 =t.Max(p => p.Product.UnitList.Unit01.Count),
                     Count02 = t.Max(p => p.Product.UnitList.Unit02.Count),
                 });
             var productWarn = Qty.Where(q => q.quantity <= minLimited && q.quantity >= maxLimited);
             int total = productWarn.Count();
             productWarn = productWarn.OrderBy(q => q.productCode);
             productWarn = productWarn.Skip((page - 1) * rows).Take(rows);
             if (unitType == "1")
             {
                 string unitName1 = "标准件";
                 decimal count1 = 10000;
                 string unitName2 = "标准条";
                 decimal count2 = 200;
                 var currentstorage = Qty.ToArray().Select(d => new
                 {
                     ProductCode = d.productCode,
                     ProductName = d.productName,
                     UnitName1 = unitName1,
                     UnitName2 = unitName2,
                     Quantity1 = d.quantity / count1,
                     Quantity2 = d.quantity / count2,
                     Quantity = d.quantity
                 });
                 return new { total, rows = currentstorage.ToArray() };
             }
             if (unitType == "2")
             {
                 var currentstorage = Qty.ToArray().Select(d => new
                 {
                     ProductCode = d.productCode,
                     ProductName = d.productName,
                     UnitName1 = d.UnitName01,
                     UnitName2 = d.UnitName02,
                     Quantity1 = d.quantity / d.Count01,
                     Quantity2 = d.quantity / d.Count02,
                     Quantity = d.quantity
                 });
                 return new { total, rows = currentstorage.ToArray() };
             }
             return new { total, rows = productWarn.ToArray() };
         }
        #endregion

        #region 积压产品查询
         //public object GetTimeOut(int page, int rows, string productCode)
         //{
         //    IQueryable<Storage> Storage = StorageRepository.GetQueryable();
         //    IQueryable<ProductWarning> ProductWarn = ProductWarningRepository.GetQueryable();
         //    var productTimeOut = ProductWarn.Where(p => p.ProductCode == productCode).Select(p => p.AssemblyTime).ToArray();
         //    var productInfo = Storage.Where(s => s.Product.ProductCode == productCode
         //        &&decimal.Parse((DateTime.Now-s.StorageTime).TotalDays.ToString())>=productTimeOut[0])
         //        .Select(s => new 
         //        { 
         //            productCode=s.Product.ProductCode,
         //            productName=s.Product.ProductName,
         //            cellCode=s.Cell.CellCode,
         //            cellName=s.Cell.CellName,
         //            quantity=s.Quantity,
         //            storageTime=s.StorageTime,
         //            days=(DateTime.Now-s.StorageTime).TotalDays
         //        });
         //    int total = productInfo.Count();
         //    productInfo = productInfo.OrderBy(p => p.productCode);
         //    productInfo = productInfo.Skip((page - 1) * rows).Take(rows);
         //    return new { total,rows=productInfo.ToArray()};
         //}
         public object GetProductDetails(int page, int rows, string QueryString, string Value)
         {
             string ProductName = "";
             string ProductCode = "";
             if (QueryString == "ProductCode")
             {
                 ProductCode = Value;
             }
             else
             {
                 ProductName = Value;
             }
             IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
             IQueryable<Storage> Storage = StorageRepository.GetQueryable();
             IQueryable<ProductWarning> ProductWarn = ProductWarningRepository.GetQueryable();
             var product = ProductQuery.Where(c => c.ProductName.Contains(ProductName) && c.ProductCode.Contains(ProductCode) && c.IsActive == "1")
               .OrderBy(c => c.ProductCode)
               .Select(c => c.ProductCode).ToArray();
             var productTimeOut = ProductWarn.Where(p => p.ProductCode == product[0]).Select(p => p.AssemblyTime).ToArray();
             var productInfo = Storage.Where(s => s.Product.ProductCode == product[0]
                 && decimal.Parse((DateTime.Now - s.StorageTime).TotalDays.ToString()) >= productTimeOut[0])
                 .Select(s => new
                 {
                     productCode = s.Product.ProductCode,
                     productName = s.Product.ProductName,
                     cellCode = s.Cell.CellCode,
                     cellName = s.Cell.CellName,
                     quantity = s.Quantity,
                     storageTime = s.StorageTime,
                     days = (DateTime.Now - s.StorageTime).TotalDays
                 });
             int total = productInfo.Count();
             productInfo = productInfo.OrderBy(p => p.productCode);
             productInfo = productInfo.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = productInfo.ToArray() };
         }
        #endregion

    }
}
