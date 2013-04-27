using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class ContractDetail
    {
        public ContractDetail()
        {
        }
        public Guid ID { get; set; }
        public string ContractCode { get; set; }
        public string BrandCode { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public int Amount { get; set; }
        public int TaxAmount { get; set; }

        public virtual Contract Contract { get; set; }
    }
}
