using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class DeliverLineAllot
    {
        public string DeliverLineAllotCode { get; set; }
        public int BatchSortId { get; set; }
        public string DeliverLineCode { get; set; }
        public int DeliverQuantity { get; set; }
        public string Status { get; set; }

        public BatchSort batchSort { get; set; }

    }
}
