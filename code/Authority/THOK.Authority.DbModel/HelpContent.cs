using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class HelpContent
    {
        public HelpContent()
        {
            this.HelpContents = new List<HelpContent>();
        }

        public Guid ID { get; set; }
        public string ContentCode { get; set; }
        public string ContentName { get; set; }
        public string ContentText { get; set; }
        public string ContentPath { get; set; }
        public string NodeType { get; set; }
        public Guid? FatherNodeID { get; set; }
        public Guid? ModuleID { get; set; }
        public int NodeOrder { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual HelpContent FatherNode { get; set; }
        public virtual Module Module { get; set; }

        public virtual ICollection<HelpContent> HelpContents { get; set; }
    }
}
