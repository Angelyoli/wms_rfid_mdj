using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class SortSupply
    {
        public string SortSupplyCode { get; set; }
        public int BatchSortId { get; set; }
        public int SupplyId { get; set; }
        public int PackNo { get; set; }
        public string ChannelCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public BatchSort batchSort { get; set; }
        public Channel channel { get; set; }

    }
}






