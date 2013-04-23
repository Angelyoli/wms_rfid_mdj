using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class BillMaster
    {
        public BillMaster()
        {
            this.BillDetails = new List<BillDetail>();
            //this.InBillAllots = new List<InBillAllot>();
            this.Navicerts = new List<Navicert>();
        }
        public Guid ID { get; set; }
        public string UUID { get; set; }
        public string BillType { get; set; }
        public DateTime BillDate { get; set; }
        public string MakerName { get; set; }
        public DateTime? OperateDate { get; set; }
        public string CigaretteType { get; set; }
        public string BillCompanyCode { get; set; }  
        public string SupplierCode { get; set; }
        public string SupplierType { get; set; }
        public string State { get; set; }

        public virtual ICollection<BillDetail> BillDetails { get; set; }
        //public virtual ICollection<InBillAllot> InBillAllots { get; set; }
        public virtual ICollection<Navicert> Navicerts { get; set; }
    }
}











