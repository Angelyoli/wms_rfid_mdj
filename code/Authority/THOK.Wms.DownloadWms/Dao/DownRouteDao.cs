using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownRouteDao : BaseDao
    {
        /// <summary>
        /// 从营销下载送货线路表信息
        /// </summary>
        public DataTable GetRouteInfo(string routeCodeList)
        {
            string sql = string.Format(@"SELECT * FROM V_DWV_ORD_DIST_BILL A
                                         LEFT JOIN V_WMS_DELIVER_LINE B ON A.DELIVER_LINE_CODE=B.DELIVER_LINE_CODE");
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 从分拣下载送货线路表信息
        /// </summary>
        public DataTable GetSortRouteInfo(string routeCodeList)
        {
            string sql = string.Format(@"SELECT DELIVERLINECODE,DELIVERLINENAME FROM WMS_SORT_ORDER WHERE {0}
                                               GROUP BY DELIVERLINECODE,DELIVERLINENAME", routeCodeList);
            return this.ExecuteQuery(sql).Tables[0];
        }

    
        /// <summary>
        /// 查询线路表中的线路代码
        /// </summary>
        /// <returns></returns>
        public DataTable GetRouteCode()
        {
            string sql = " SELECT deliver_line_code FROM wms_deliver_line";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 把线路信息插入数据库
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataSet ds)
        {
            BatchInsert(ds.Tables["DWV_OUT_DELIVER_LINE"], "wms_deliver_line");
        }

      
    }
}
