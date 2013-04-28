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

        bool Save(BillMaster billMaster, out string strResult);

        bool Delete(string contractCode, string uuid, out string strResult);
    }
}
