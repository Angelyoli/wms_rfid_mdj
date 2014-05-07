using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class Channel
    {
        public Channel()
        {
            this.ChannelAllots = new List<ChannelAllot>();
            this.SortSupplys = new List<SortSupply>();
            this.SortOrderAllotDetails = new List<SortOrderAllotDetail>();
        }
        public string ChannelCode { get; set; }
        public string SortingLineCode { get; set; }
        public string ChannelName { get; set; }
        public string ChannelType { get; set; }
        public string LedCode { get; set; }
        public string DefaultProductCode { get; set; }
        public int RemainQuantity { get; set; }
        public int MiddleQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public int GroupNo { get; set; }
        public int OrderNo { get; set; }
        public int Address { get; set; }
        public string CellCode { get; set; }
        public string Status { get; set; }

        public Led led { get; set; }

        public virtual ICollection<ChannelAllot> ChannelAllots { get; set; }
        public virtual ICollection<SortSupply> SortSupplys { get; set; }
        public virtual ICollection<SortOrderAllotDetail> SortOrderAllotDetails { get; set; }

    }
}