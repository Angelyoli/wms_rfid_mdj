using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.WCS.Bll.Models
{
    public class Inspur
    {
        public string Param { get; set; }
        public string User { get; set; }
        public string Time { get; set; }
        public string BillNo { get; set; }
        public string ProductCode { get; set; }
        public decimal RealQuantity { get; set; }
    }
}
