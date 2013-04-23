using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Navicert
    {
        public Guid ID { get; set; }
        public Guid MasterID { get; set; }
        public string NavicertCode { get; set; }
        public DateTime NavicertDate { get; set; }
        public string TruckPlateNo { get; set; }
        public string ContractCode { get; set; }

        public virtual BillMaster BillMaster { get; set; }
      //  public virtual Contract Contract { get; set; }
    }
}
