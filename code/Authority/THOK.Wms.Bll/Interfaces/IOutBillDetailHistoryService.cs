using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IOutBillDetailHistoryService
    {
        bool Add(DateTime datetime);
    }
}
