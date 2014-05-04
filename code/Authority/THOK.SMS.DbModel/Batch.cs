﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class Batch
    {
        public Batch()
       {
           this.BatchStatuss = new List<BatchStatus>();
       }

        public int BatchId { get; set; }
        public DateTime OrderDate { get; set; }
        public int BatchNo { get; set; }
        public string BatchName { get; set; }
        public DateTime OperateDate { get; set; }
        public Guid OperatePersonId { get; set; }
        public int OptimizeSchedule { get; set; }
        public Guid VerifyPersonId { get; set; }
        public string Description { get; set; }
        public int ProjectBatchNo { get; set; }
        public string State { get; set; }

        public virtual ICollection<BatchStatus> BatchStatuss { get; set; }
    }
}
