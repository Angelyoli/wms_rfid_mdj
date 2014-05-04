using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class BatchStatus
    {
        public int BatchStatusId { get; set; }
        public int BatchId { get; set; }
        public string SortingLineCode { get; set; }
        public string State { get; set; }

        public Batch batch { get; set; }
    }
}
