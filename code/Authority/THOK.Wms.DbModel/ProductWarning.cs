using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class ProductWarning
    {
        public ProductWarning()
        {
            this.Storages = new List<Storage>();
        }

        public string ProductCode { get; set; }
        public decimal ?MinLimited { get; set; }
        public decimal ?MaxLimited { get; set; }
        public decimal ?AssemblyTime { get;set;}
        public string Memo { get; set; }

        public virtual Storage  Storage{ get; set; }
        public virtual Unit Unit { get; set; }

        public virtual ICollection<Storage> Storages { get; set; }
    }
}
