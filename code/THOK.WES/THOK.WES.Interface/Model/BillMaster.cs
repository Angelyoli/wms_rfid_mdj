using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.WES.Interface.Model
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
