using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IBillDetailService : IService<BillDetail>
    {
        bool Add(BillDetail billDetail, out string strResult);

        bool Save(BillDetail billDetail, out string strResult);
    }
}
