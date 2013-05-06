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
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }

        public virtual Contract Contract { get; set; }
    }
}
