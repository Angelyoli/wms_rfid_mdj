using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Util;

namespace THOK.Wms.DownloadWms.Dao
{
   public class AddXmlValueDao : BaseDao
    {
       public void Insert(string sql)
       {
           this.ExecuteNonQuery(sql);
       }
    }
}
