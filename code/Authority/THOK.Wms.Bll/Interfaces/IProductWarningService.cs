using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IProductWarningService:IService<ProductWarning>
    {
        object GetDetail(int page, int rows, string productCode, decimal minLimited, decimal maxLimited, decimal assemblyTime);

        bool Add(ProductWarning productWarning);

        bool Delete(string productCode);

        bool Save(ProductWarning productWarning);
    }
}
