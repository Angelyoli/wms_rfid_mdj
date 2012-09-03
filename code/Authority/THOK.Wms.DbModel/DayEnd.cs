using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class DayEnd
    {
        public DayEnd()
        { }
        public string DayEndID { get; set; }
        public DateTime DaySettleDate { get; set; }
        public string DayWarehouseCode { get; set; }
        public string DayProductCode { get; set; }
        public string DayUnitCode { get; set; }
        public decimal DayBeginning { get; set; }
        public decimal DayEntryAmount { get; set; }
        public decimal DayDeliveryAmount { get; set; }
        public decimal DayProfitAmount { get; set; }
        public decimal DayLossAmount { get; set; }
        public decimal DayEnding { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
