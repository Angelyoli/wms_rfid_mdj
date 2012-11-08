using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.DbModel
{
    public class SystemParameter
    {
        public SystemParameter()
        {
            
        }
        public int Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Remark { get; set; }
        public string UserName { get; set; }
        public Guid? SystemID { get; set; }

        public virtual System System { get; set; }
    }
}
