using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.AutomotiveSystems.Models
{
    public class TaskParameter
    {
        public string Method { get; set; }
        public string[] BillTypes { get; set; }
        public BillMaster[] BillMasters { get; set; }
        public BillDetail[] BillDetails { get; set; }
    }
}
