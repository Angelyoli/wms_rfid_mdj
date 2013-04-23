using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Contract
    {
        public Contract()
        {
            this.ContractDetails = new List<ContractDetail>();
            this.Navicerts = new List<Navicert>();
        }

        public string ContractCode { get; set; }
        public Guid MasterID { get; set; }
        public string SupplySideCode { get; set; }
        public string DemandSideCode { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime StartDade { get; set; }
        public DateTime EndDate { get; set; }
        public string SendPlaceCode { get; set; }
        public string SendAddress { get; set; }
        public string ReceivePlaceCode { get; set; }
        public string ReceiveAddress { get; set; }
        public string SaleDate { get; set; }
        public string State { get; set; }

        public virtual ICollection<ContractDetail> ContractDetails { get; set; }
        public virtual ICollection<Navicert> Navicerts { get; set; }
    }
}
