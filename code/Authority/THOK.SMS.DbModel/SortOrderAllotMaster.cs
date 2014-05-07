using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class SortOrderAllotMaster
    {
        public SortOrderAllotMaster()
        {
            this.SortOrderAllotDetails = new List<SortOrderAllotDetail>();
        }
        public string OrderMasterCode { get; set; }
        public int BatchSortId { get; set; }
        public int PackNo { get; set; }
        public string OrderId { get; set; }
        public int CustomerOrder { get; set; }
        public int CustomerDeliverOrder { get; set; }
        public int Quantity { get; set; }
        public int ExportNo { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string Status { get; set; }

        public BatchSort batchSort { get; set; }

        public virtual ICollection<SortOrderAllotDetail> SortOrderAllotDetails { get; set; }

    }
}





