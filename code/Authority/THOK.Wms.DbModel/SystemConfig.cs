using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class SystemConfig
    {
        public SystemConfig() 
        {
        }
        public int Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Remark { get; set; }
        public string Username { get; set; }
        public string System { get; set; }
    }
}
