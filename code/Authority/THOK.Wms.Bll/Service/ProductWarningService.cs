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
         [Dependency]
         public ICellRepository CellRepository { get; set; }

         protected override Type LogPrefix
         {
             get { return this.GetType(); }
         }
        #region  卷烟预警设置的增，删，改，查方法
         public object GetDetail(int page, int rows, string productCode, decimal minLimited, decimal maxLimited, decimal assemblyTime)
         {
             IQueryable<ProductWarning> productWarningQuery = ProductWarningRepository.GetQueryable();
             var productWarning = productWarningQuery.Where(p => p.ProductCode.Contains(productCode)).OrderBy(p => p.ProductCode).Select(p => p).ToArray();
             if (productCode != "")
             {
                 productWarning = productWarning.Where(p => p.ProductCode == productCode).ToArray();
             }
             if (minLimited != 100000)
             {
                 productWarning = productWarning.Where(p => p.MinLimited <= minLimited).ToArray();
             }
             if (maxLimited != 100000)
             {
                 productWarning = productWarning.Where(p => p.MaxLimited >= maxLimited).ToArray();
             }
             if (assemblyTime != 3600)
             {
                 productWarning = productWarning.Where(p => p.AssemblyTime >= assemblyTime).ToArray();
             }
             var productWarn = productWarning
                 .Select(p => new
                 {
                     p.ProductCode,
                     ProductName = ProductRepository.GetQueryable().FirstOrDefault(q => q.ProductCode==p.ProductCode).ProductName,
                     p.UnitCode,
                     p.Unit.UnitName,
                     MinLimited = p.MinLimited / p.Unit.Count,
                     MaxLimited = p.MaxLimited / p.Unit.Count,
                     p.AssemblyTime,
                     p.Memo
                 });
             int total = productWarn.Count();
             productWarn = productWarn.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = productWarn.ToArray() };
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
             IQueryable<ProductWarning> ProductWarningQuery = ProductWarningRepository.GetQueryable();
             var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == unitCode);
             var ProductWarning = ProductWarningQuery.Where(p => p.ProductCode.Contains(productCode)).ToArray();
             if(productCode!="")
             {
                 ProductWarning = ProductWarning.Where(p=>p.ProductCode == productCode).ToArray();
             }
              if (minLimited != 100000)
             {
                 ProductWarning = ProductWarning.Where(p=>p.MinLimited<=minLimited * unit.Count).ToArray();
             }
             if (maxLimited != 100000)
             {
                 ProductWarning = ProductWarning.Where(p => p.MaxLimited >= maxLimited * unit.Count).ToArray();
             }
             var productWarning = ProductWarning.Select(t => new 
                 {
                     ProductCode =t.ProductCode,
                     ProductName = ProductRepository.GetQueryable().FirstOrDefault(q => q.ProductCode == t.ProductCode).ProductName,
                     UnitCode=t.UnitCode,
                     UnitName = t.Unit.UnitName,
                     Quantity = StorageRepository.GetQueryable().AsEnumerable().Where(s=>s.ProductCode==t.ProductCode).Sum(s=>s.Quantity)/t.Unit.Count,
                     MinLimited =t.MinLimited/t.Unit.Count,
                     MaxLimited = t.MaxLimited / t.Unit.Count
                 });
             productWarning = productWarning.Where(p=>p.Quantity>=p.MaxLimited ||p.Quantity<=p.MinLimited);
             int total = productWarning.Count();
             productWarning = productWarning.OrderBy(q => q.ProductCode);
             productWarning = productWarning.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = productWarning.ToArray() };
         }
        #endregion

        #region 积压产品查询
         public object GetProductDetails(int page, int rows, string productCode, decimal assemblyTime)
         {
             IQueryable<Storage> StorageQuery = StorageRepository.GetQueryable();
             IQueryable<ProductWarning> ProductWarningQuery = ProductWarningRepository.GetQueryable();
             var ProductWarning = ProductWarningQuery.Where(p => p.ProductCode.Contains(productCode));
             var storage = StorageQuery.Where(s => s.ProductCode.Contains(productCode));
             var Storages = storage.Join(ProductWarning, s => s.ProductCode, p => p.ProductCode, (s, p) => new { storage = s, ProductWarning = p }).ToArray();

             Storages = Storages.Where(s => !string.IsNullOrEmpty(s.ProductWarning.AssemblyTime.ToString())).ToArray();
             if (Storages.Count() > 0)
             {
                 if (productCode != "")
                 {
                     Storages = Storages.Where(s => s.storage.ProductCode == productCode).ToArray();
                 }
                 if (assemblyTime != 360)
                 {
                     Storages = Storages.Where(s => s.ProductWarning.AssemblyTime >= assemblyTime).ToArray();
                 }
                 else
                 {
                     Storages = Storages.Where(s =>s.ProductWarning.AssemblyTime<=(DateTime.Now - s.storage.StorageTime).Days).ToArray();
                 }
             }
             var ProductTimeOut = Storages.AsEnumerable()
                 .Select(s => new
                 {
                     ProductCode = s.storage.ProductCode,
                     ProductName = s.storage.Product.ProductName,
                     cellCode = s.storage.CellCode,
                     cellName = s.storage.Cell.CellName,
                     quantity = s.storage.Quantity / s.storage.Product.Unit.Count,
                     storageTime = s.storage.StorageTime.ToString("yyyy-MM-dd hh:mm:ss"),
                     days = (DateTime.Now - s.storage.StorageTime).Days
                 });
             int total = ProductTimeOut.Count();
             ProductTimeOut = ProductTimeOut.OrderBy(p => p.ProductCode);
             ProductTimeOut = ProductTimeOut.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = ProductTimeOut.ToArray() };
         }
        #endregion

        #region 预警提示信息
         public object GetWarningPrompt()
         {
             var ProductWarning= ProductWarningRepository.GetQueryable();
             var StorageQuery = StorageRepository.GetQueryable();
             var Storages = StorageQuery.Join(ProductWarning, s => s.ProductCode, p => p.ProductCode, (s, p) => new { storage = s, ProductWarning = p }).ToArray();
             var TimeOutWarning = Storages.Where(s => !string.IsNullOrEmpty(s.ProductWarning.AssemblyTime.ToString())).ToArray();
             var QuantityLimitsWarning = Storages.Where(s => !string.IsNullOrEmpty(s.ProductWarning.MinLimited.ToString()))
                                         .GroupBy(s=>s.storage.ProductCode)
                                         .Select(s=>new{
                                            productCode= s.Max(t=>t.storage.ProductCode),
                                            quantityTotal=s.Sum(t=>t.storage.Quantity),
                                            minlimits=s.Max(t=>t.ProductWarning.MinLimited)
                                         }).ToArray();
             var QuantityLimitsWarnings = Storages.Where(s =>! string.IsNullOrEmpty(s.ProductWarning.MaxLimited.ToString()))
                                        .GroupBy(s => s.storage.ProductCode)
                                        .Select(s => new
                                        {
                                            productCode = s.Max(t => t.storage.ProductCode),
                                            quantityTotal = s.Sum(t => t.storage.Quantity),
                                            maxlimits = s.Max(t => t.ProductWarning.MaxLimited)
                                        }).ToArray();
             if (TimeOutWarning.Count() > 0)
             {
                TimeOutWarning=TimeOutWarning.Where(t=>(DateTime.Now-t.storage.StorageTime).Days>=t.ProductWarning.AssemblyTime).ToArray();
             }
             if (QuantityLimitsWarning.Count() >= 0)
             {
                 QuantityLimitsWarning = QuantityLimitsWarning.Where(q=>q.quantityTotal<=q.minlimits).ToArray();
             }
             if (QuantityLimitsWarnings.Count() >= 0)
             {
                 QuantityLimitsWarnings = QuantityLimitsWarnings.Where(q => q.quantityTotal <= q.maxlimits).ToArray();
             }
             int total=TimeOutWarning.Count() + QuantityLimitsWarning.Count() + QuantityLimitsWarning.Count();
             //var timeOutWarning = TimeOutWarning.Select(s => new { AssemblyTime = total });
             //return timeOutWarning.ToArray();
             return total;
         }
         #endregion

        #region 库存分析数据
         public object GetStorageByTime()
         {
             IQueryable<DailyBalance> EndQuantity = DailyBalanceRepository.GetQueryable();
             var storageQuantity = EndQuantity.OrderBy(e=>e.SettleDate).GroupBy(e=>e.SettleDate).Select(e => new 
             {
                TimeInter=e.Max(m=>m.SettleDate),
                TotalQuantity=e.Sum(s=>s.Ending/s.Unit.Count)
             }).ToArray();
             int j = storageQuantity.Count();
             decimal[,] array = new decimal[j, 2];
             for (int i = 0; i < j; i++)
             {
                 array[i, 0] =decimal.Parse(storageQuantity[i].TimeInter.Date.ToFileTimeUtc().ToString());
                 array[i, 1] = storageQuantity[i].TotalQuantity;
             }
             return array;
         }
        #endregion

        #region  货位分析数据
         public object GetCellInfo()
         {
             //IQueryable<Cell> CellQuery = CellRepository.GetQueryable();
             //IQueryable<Storage> StorageQuery = StorageRepository.GetQueryable();
             //int cellQuantity = CellQuery.Count();
             //int storageQuantity = StorageQuery.Where(s => s.IsLock == "1" || s.Quantity > 0 || s.InFrozenQuantity > 0 || s.OutFrozenQuantity > 0).Count();

             return null;
         }
        #endregion

    }
}
