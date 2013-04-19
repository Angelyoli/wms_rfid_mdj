using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class ExceptionalLog
    {
        public Guid ExceptionalLogID { get; set; }
        public string CatchTime { get; set; }
        public string ModuleName { get; set; }
        public string FunctionName { get; set; }
        public string ExceptionalType { get; set; }
        public string ExceptionalDescription { get; set; }
        public string State { get; set; }
    }
}
