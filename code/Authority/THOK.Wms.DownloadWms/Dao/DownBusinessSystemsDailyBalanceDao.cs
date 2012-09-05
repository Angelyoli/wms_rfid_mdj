using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.Wms.DownloadWms.Dao
{
   public class DownBusinessSystemsDailyBalanceDao:BaseDao
   {
       #region 下载日表
       public DataTable FindDayEnd(string parameter)
       {
           string sql = string.Format(@"SELECT D.* , B.BRAND_N FROM V_WMS_DAILYBALANCE D
                                        LEFT JOIN V_WMS_BRAND B ON D.PRODUCTCODE=B.BRAND_CODE WHERE {0}", parameter);
           return this.ExecuteQuery(sql).Tables[0];
       }

       public void InsertDayEnd(DataSet ds)
       {
           BatchInsert(ds.Tables["WMS_BUSINESS_SYSTEMS_DAILY_BALANCE"], "WMS_BUSINESS_SYSTEMS_DAILY_BALANCE");
       }

       public void Delete(string settledate)
       {
           string sql = "DELETE WMS_BUSINESS_SYSTEMS_DAILY_BALANCE WHERE SETTLE_DATE ='" + settledate + "'";
           this.ExecuteNonQuery(sql);
       }

       public DataTable GetUnitProduct()
       {
           string sql = "SELECT * FROM WMS_UNIT_LIST";
           return this.ExecuteQuery(sql).Tables[0];
       }

       #endregion 
   }
}
