using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class SortOrderAllotDetail
    {
        public string OrderDetailCode { get; set; }
        public string OrderMasterCode { get; set; }
        public string ChannelCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public SortOrderAllotMaster sortOrderAllotMaster { get; set; }
        public Channel channel { get; set; }

    }
}





