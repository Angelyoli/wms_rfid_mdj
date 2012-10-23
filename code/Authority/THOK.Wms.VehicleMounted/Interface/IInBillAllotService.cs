using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.VehicleMounted.Interface
{
    public interface IInBillAllotService : IService<InBillAllot>
    {
        object SearchInBillAllot(string billNo, string status, int page, int rows);
    }
}
