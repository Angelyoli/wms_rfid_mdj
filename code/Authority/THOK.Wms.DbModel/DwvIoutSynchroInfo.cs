using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class DwvIoutSynchroInfo
    {
        public Guid ID { get; set; }
        public string SyncTypeCode { get; set; }
        public string SyncTypeName { get; set; }
        public string Description { get; set; }
        public string UpdateDate { get; set; }
        public string IsImport { get; set; }
        public string Remark { get; set; }
    }
}
