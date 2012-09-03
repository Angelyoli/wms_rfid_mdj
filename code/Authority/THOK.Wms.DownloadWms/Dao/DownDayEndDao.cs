using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.Wms.DownloadWms.Dao
{
   public class DownDayEndDao:BaseDao
   {
       #region 下载日表
       public DataTable FindDayEnd()
       {
           string sql = "SELECT * FROM V_WMS_DAILYBALANCE ";
           return this.ExecuteQuery(sql).Tables[0];
       }
       #endregion 
   }
}
