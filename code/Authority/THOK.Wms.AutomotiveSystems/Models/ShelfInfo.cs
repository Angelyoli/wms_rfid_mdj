using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.AutomotiveSystems.Models
{
    public class ShelfInfo
    {
        public string ShelfCode = string.Empty;
        public string ShelfName = string.Empty;
        public string CellCode = string.Empty;

        public string CellName =string.Empty;
        public string ProductCode = string.Empty;
        public string ProductName = string.Empty;
        public decimal QuantityTiao = 0;
        public decimal QuantityJian = 0;
        public string WareCode = string.Empty;
        public string WareName = string.Empty;
        public string IsActive = string.Empty;
        public int ColNum = 0;
        public int RowNum = 0;
        public string Shelf = string.Empty;
        //public DateTime UpdateDate =DateTime.Now;
       
    }
}
