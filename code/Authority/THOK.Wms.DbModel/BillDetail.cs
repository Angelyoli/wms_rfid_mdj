using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class BillDetail
    {
        public Guid ID { get; set; }
        public Guid MasterID { get; set; }
        public string PieceCigarCode { get; set; }
        public string BoxCigarCode { get; set; }
        public int BillQuantity { get; set; }
        public int FixedQuantity { get; set; }
        public int RealQuantity { get; set; }

        public virtual BillMaster BillMaster { get; set; }

    }
}









