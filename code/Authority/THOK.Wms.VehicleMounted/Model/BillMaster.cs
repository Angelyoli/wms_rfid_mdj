using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.VehicleMounted.Model
{
    public class BillMaster
    {
        private string billNo;
        public string BillNo
        {
            get { return billNo; }
            set { billNo = value; }
        }

        private string billType;
        public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }
    }
}
