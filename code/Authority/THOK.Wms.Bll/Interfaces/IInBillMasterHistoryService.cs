using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IInBillMasterHistoryService : IService<InBillMaster>
    {
        bool Add(DateTime datetime, out string masterResult, out string detailResult, out string allotResult);
        bool Add2(DateTime datetime, out string strResult);
    }
}
