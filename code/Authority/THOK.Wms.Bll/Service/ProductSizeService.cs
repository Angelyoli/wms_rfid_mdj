using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Service
{
    public class ProductSizeService : ServiceBase<ProductSize>, IProductSizeService
    {
        [Dependency]
        public IProductSizeRepository ProductSizeRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
    }
}
