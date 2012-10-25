using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.VehicleMounted.Model
{
    public class Result
    {
        public bool IsSuccess = false;
        public string Message = string.Empty;
        public BillMaster[] BillMasters = new BillMaster[] { };
        public BillDetail[] BillDetails = new BillDetail[] { };
    }
}
