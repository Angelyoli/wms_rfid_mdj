using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class BatchSort
    {
        public BatchSort()
        {
            this.DeliverLineAllots = new List<DeliverLineAllot>();
            this.ChannelAllots = new List<ChannelAllot>();
            this.SortSupplys = new List<SortSupply>();
            this.SortOrderAllotMasters = new List<SortOrderAllotMaster>();
        }
        public int BatchSortId { get; set; }
        public int BatchId { get; set; }
        public string SortingLineCode { get; set; }
        public string Status { get; set; }

        public Batch batch { get; set; }

        public virtual ICollection<DeliverLineAllot> DeliverLineAllots { get; set; }
        public virtual ICollection<ChannelAllot> ChannelAllots { get; set; }
        public virtual ICollection<SortSupply> SortSupplys { get; set; }
        public virtual ICollection<SortOrderAllotMaster> SortOrderAllotMasters { get; set; }

    }
}