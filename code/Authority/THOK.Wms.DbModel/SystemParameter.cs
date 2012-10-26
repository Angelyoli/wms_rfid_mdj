using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class SystemParameter
    {
        public SystemParameter() 
        {
            //this.SystemParameters = new List<SystemParameter>();
        }
        public int Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Remark { get; set; }
        public string UserName { get; set; }
        public Guid? SystemID { get; set; }

        //public virtual ICollection<SystemParameter> SystemParameters { get; set; }
    }
}
