using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IBillMasterService : IService<BillMaster>
    {
        bool Add(BillMaster billMaster, out string strResult);
    }
}
