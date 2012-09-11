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
         [Dependency]
         public IUnitRepository UnitRepository { get; set; }
         [Dependency]
         public IDailyBalanceRepository DailyBalanceRepository { get; set; }

         protected override Type LogPrefix
         {
             get { return this.GetType(); }
         }
         #region  卷烟预警设置的增，删，改，查方法
         public object GetDetail(int page, int rows, string productCode, decimal minLimited, decimal maxLimited, decimal assemblyTime)
         {
             IQueryable<ProductWarning> productWarningQuery = ProductWarningRepository.GetQueryable();
             var productName = ProductRepository.GetQueryable().FirstOrDefault(p=>p.ProductCode==productCode);
             var productWarning = productWarningQuery.Where(p => p.ProductCode.Contains( productCode)
                 && p.MinLimited == minLimited && p.MaxLimited == maxLimited && p.AssemblyTime == assemblyTime)
                 .OrderBy(p => p.ProductCode)
                 .Select(p => new 
                 {
                     p.ProductCode,
                     ProductName=productName.ProductName,
                     p.UnitCode,
                     p.Unit.UnitName,
                     MinLimited=p.MinLimited/p.Unit.Count,
                     MaxLimited=p.MaxLimited/p.Unit.Count,
                     p.AssemblyTime,
                     p.Memo
                 });
             int total = productWarning.Count();
             productWarning = productWarning.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = productWarning.ToArray() };
         }

         public bool Add(ProductWarning productWarning)
         {
             var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode ==productWarning.UnitCode);
             var productWarn = new ProductWarning();
             productWarn.ProductCode = productWarning.ProductCode;
             productWarn.UnitCode = productWarning.UnitCode;
             productWarn.MinLimited = productWarning.MinLimited * unit.Count;
             productWarn.MaxLimited = productWarning.MaxLimited * unit.Count;
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
             var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == productWarning.UnitCode);
             productWarn.ProductCode = productWarning.ProductCode;
             productWarn.UnitCode = productWarning.UnitCode;
             productWarn.MinLimited = productWarning.MinLimited * unit.Count;
             productWarn.MaxLimited = productWarning.MaxLimited * unit.Count;
             productWarn.AssemblyTime = productWarning.AssemblyTime;
             productWarn.Memo = productWarning.Memo;

             ProductWarningRepository.SaveChanges();
             return true;
         }
        #endregion

        #region 产品短缺、超储查询
         public object GetQtyLimitsDetail(int page, int rows, string productCode, decimal minLimited, decimal maxLimited, string unitCode)
         {
             IQueryable<Storage> Quantity = StorageRepository.GetQueryable();
             var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == unitCode);
             var Qty = Quantity.Where(t => t.ProductCode.Contains(productCode))
                 .GroupBy(t=>t.ProductCode)
                 .Select(t => new 
                 {
                     ProductCode =t.Max(p=>p.Product.ProductCode),
                     ProductName=t.Max(p=>p.Product.ProductName),
                     //UnitCode=t.Max(p=>p.ProductWarning.UnitCode),
                     //UnitName = t.Max(p => p.ProductWarning.Unit.UnitName),
                     Quantity = t.Sum(p => p.Quantity/unit.Count),
                     //MinLimited = t.Max(p => p.ProductWarning.MinLimited / unit.Count),
                     //MaxLimited = t.Max(p => p.ProductWarning.MaxLimited / unit.Count)
                 });
             var productWarn = Qty.Where(q => q.Quantity <= minLimited && q.Quantity >= maxLimited);
             int total = productWarn.Count();
             productWarn = productWarn.OrderBy(q => q.ProductCode);
             productWarn = productWarn.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = productWarn.ToArray() };
         }
        #endregion

        #region 积压产品查询
         public object GetProductDetails(int page, int rows, string productCode, decimal assemblyTime)
         {
             //IQueryable<Storage> Storage = StorageRepository.GetQueryable();
             //IQueryable<ProductWarning> ProductWarning = ProductWarningRepository.GetQueryable();
             //var productInfo = Storage.Where(s => s.Product.ProductCode.Contains(s.ProductWarning.ProductCode)
             //    && s.Product.ProductCode.Contains(productCode)
             //    && decimal.Parse((DateTime.Now - s.StorageTime).TotalDays.ToString()) >= assemblyTime)
             //    .Select(s => new
             //    {
             //        ProductCode = s.Product.ProductCode,
             //        productName = s.Product.ProductName,
             //        cellCode = s.Cell.CellCode,
             //        cellName = s.Cell.CellName,
             //        quantity = s.Quantity/s.Product.Unit.Count,
             //        storageTime = s.StorageTime,
             //        days = (DateTime.Now - s.StorageTime).TotalDays
             //    });
             //int total = productInfo.Count();
             //productInfo = productInfo.OrderBy(p => p.ProductCode);
             //productInfo = productInfo.Skip((page - 1) * rows).Take(rows);
             //return new { total, rows = productInfo.ToArray() };
             return null;
         }
        #endregion

        #region
         public object GetStorageByTime()
         {
             IQueryable<DailyBalance> EndQuantity = DailyBalanceRepository.GetQueryable();
             var storageQuantity = EndQuantity.GroupBy(e=>e.SettleDate).Select(e => new 
             {
                TimeInter=e.Max(m=>m.SettleDate).Date,
                TotalQuantity=e.Sum(s=>s.Ending)
             });
             return storageQuantity.ToArray();
         }
        #endregion

    }
}
