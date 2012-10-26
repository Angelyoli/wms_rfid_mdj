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
        /// ��Ӫ�������ͻ���·����Ϣ
        /// </summary>
        public DataTable GetRouteInfo(string routeCodeList)
        {
            string sql = string.Format(@"SELECT * FROM V_DWV_ORD_DIST_BILL A
                                         LEFT JOIN V_WMS_DELIVER_LINE B ON A.DELIVER_LINE_CODE=B.DELIVER_LINE_CODE");
            //sql = "SELECT * FROM V_WMS_DELIVER_LINE";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// �ӷּ������ͻ���·����Ϣ
        /// </summary>
        public DataTable GetSortRouteInfo(string routeCodeList)
        {
            string sql = string.Format(@"SELECT DIST_BILL_ID,DELIVERYMAN_CODE,DELIVERYMAN_NAME,DELIVERLINECODE,DELIVERLINENAME FROM SORTORDER {0}
                                               GROUP BY DIST_BILL_ID,DELIVERYMAN_CODE,DELIVERYMAN_NAME,DELIVERLINECODE,DELIVERLINENAME", routeCodeList);
            return this.ExecuteQuery(sql).Tables[0];
        }

    
        /// <summary>
        /// ��ѯ��·���е���·����
        /// </summary>
        /// <returns></returns>
        public DataTable GetRouteCode()
        {
            string sql = " SELECT deliver_line_code FROM wms_deliver_line";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ����·��Ϣ�������ݿ�
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataSet ds)
        {
            BatchInsert(ds.Tables["DWV_OUT_DELIVER_LINE"], "wms_deliver_line");
        }


        //ɾ��7��֮ǰ����·�����ȱ��ּ��м��ͷּ������ϸ��,��ҵ���ȱ�
        public void DeleteTable()
        {
            string sql = @" DELETE SORTORDERDETAIL WHERE ORDERID IN(
                            SELECT ORDERID FROM  SORTORDER WHERE ORDERDATE<
                            CONVERT(VARCHAR(14),DATEADD(DAY, -7, CONVERT(VARCHAR(100), GETDATE(), 112)),112))

                            DELETE SORTORDER WHERE ORDERDATE<
                            CONVERT(VARCHAR(14),DATEADD(DAY, -7, CONVERT(VARCHAR(100), GETDATE(), 112)),112)

                            DELETE WMS_SORT_ORDER_DETAIL WHERE ORDER_ID IN(
                            SELECT ORDER_ID FROM  WMS_SORT_ORDER WHERE ORDER_DATE<
                            DATEADD(DAY, -7, CONVERT(VARCHAR(14), GETDATE(), 112)))

                            DELETE WMS_SORT_ORDER WHERE ORDER_DATE<
                            DATEADD(DAY, -7, CONVERT(VARCHAR(14), GETDATE(), 112))

                            DELETE WMS_SORT_ORDER_DISPATCH WHERE ORDER_DATE<
                            DATEADD(DAY, -7, CONVERT(VARCHAR(14), GETDATE(), 112))

                            DELETE WMS_DELIVER_LINE WHERE UPDATE_TIME<
                            DATEADD(DAY, -7, CONVERT(VARCHAR(14), GETDATE(), 112)) 

                            DELETE WMS_SORT_WORK_DISPATCH WHERE ORDER_DATE<
                            DATEADD(DAY, -7, CONVERT(VARCHAR(14), GETDATE(), 112))";
            this.ExecuteNonQuery(sql);
        }
      
    }
}
