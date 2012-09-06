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
    }
}
