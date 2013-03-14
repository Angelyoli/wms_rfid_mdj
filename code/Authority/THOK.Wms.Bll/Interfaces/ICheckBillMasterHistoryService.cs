using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICheckBillMasterHistoryService
    {
        bool Add(DateTime datetime, out string masterResult, out string detailResult, out string deleteResult);
    }
}
