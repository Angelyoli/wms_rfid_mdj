using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class ChannelAllot
    {
        public string ChannelAllotCode { get; set; }
        public int BatchSortId { get; set; }
        public string ChannelCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int InQuantity { get; set; }
        public int OutQuantity { get; set; }
        public int RealQuantity { get; set; }
        public int RemainQuantity { get; set; }

        public BatchSort batchSort { get; set; }
        public Channel channel { get; set; }

    }
}





