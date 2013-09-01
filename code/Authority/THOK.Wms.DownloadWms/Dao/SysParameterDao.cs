using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.Wms.DownloadWms.Dao
{
    class SysParameterDao : BaseDao
    {
        public Dictionary<string, string> FindParameters()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            DataTable table = ExecuteQuery("SELECT * FROM AUTH_SYSTEM_PARAMETER WHERE PARAMETER_NAME='SALESSYSTEMDBTYPE'").Tables[0];
            foreach (DataRow row in table.Rows)
            {
                d.Add(row["PARAMETER_NAME"].ToString(), row["PARAMETER_VALUE"].ToString());
            }
            return d;
        }
    }
}
