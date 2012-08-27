using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownSortingOrderDao : BaseDao
   {
       #region 从营销系统下载分拣信息

       /// <summary>
       /// 根据条件下载分拣订单主表信息
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrder(string orderid)
       {
           string sql = string.Format(@"SELECT a.*,b.DIST_BILL_ID,b.DELIVERYMAN_CODE,b.DELIVERYMAN_NAME FROM V_WMS_SORT_ORDER A
                                        LEFT JOIN V_DWV_ORD_DIST_BILL B ON A.DIST_BILL_ID=B.DIST_BILL_ID WHERE {0} AND QUANTITY_SUM>0", orderid);
           return this.ExecuteQuery(sql).Tables[0];
       }

       /// <summary>
       /// 根据条件下载分拣订单明细表信息
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrderDetail(string orderid)
       {
           string sql = string.Format(@"SELECT A.* , B.BRAND_N FROM V_WMS_SORT_ORDER_DETAIL A
                                        LEFT JOIN V_WMS_BRAND B ON A.BRAND_CODE=B.BRAND_CODE WHERE {0} ", orderid);
           return this.ExecuteQuery(sql).Tables[0];
       }

       /// <summary>
       /// 下载所有分拣订单明细表信息
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrderDetail()
       {
           string sql = " SELECT * FROM V_WMS_SORT_ORDER_DETAIL";
           return this.ExecuteQuery(sql).Tables[0];
       }

       /// <summary>
       /// 下载前清理当前时间七天之内的数据
       /// </summary>
       public void DeleteOrder()
       {
           string dtOrder = DateTime.Now.AddDays(-2d).ToString("yyyyMMdd");
           //DateTime historyDate = dtOrder.AddDays(-8d).ToShortDateString();
           string sql = string.Format("DELETE FROM DWV_OUT_ORDER WHERE ORDER_DATE < '{0}'", dtOrder);
           this.ExecuteNonQuery(sql);
           sql = "DELETE FROM DWV_OUT_ORDER WHERE ORDER_DATE < '{0}'";
           this.ExecuteNonQuery(sql);
       }

       #endregion

       #region 查询仓储分拣信息

       public void InsertSortingOrder(DataTable masertdt, DataTable detaildt)
       {
           BatchInsert(masertdt, "DWV_OUT_ORDER");
           BatchInsert(detaildt, "DWV_OUT_ORDER_DETAIL");
       }

       /// <summary>
       /// 添加主表数据到表 DWV_OUT_ORDER
       /// </summary>
       /// <param name="ds"></param>
       public void InsertSortingOrder(DataSet ds)
       {
           BatchInsert(ds.Tables["DWV_OUT_ORDER"], "wms_sort_order");
       }

       /// <summary>
       /// 添加明细表数据到表 DWV_OUT_ORDER_DETAIL
       /// </summary>
       /// <param name="ds"></param>
       public void InsertSortingOrderDetail(DataSet ds)
       {
           BatchInsert(ds.Tables["DWV_OUT_ORDER_DETAIL"], "wms_sort_order_detail");
       }

       /// <summary>
       /// 查询3天之内的数据
       /// </summary>
       /// <returns></returns>
       public DataTable GetOrderId()
       {
           string sql = " SELECT order_id FROM wms_sort_order WHERE order_date>DATEADD(DAY, -3, CONVERT(VARCHAR(14), GETDATE(), 112)) ";
           return this.ExecuteQuery(sql).Tables[0];
       }

       public DataTable GetUnitProduct()
       {
           string sql = "SELECT * FROM WMS_UNIT_LIST";
           return this.ExecuteQuery(sql).Tables[0];
       }

       #endregion

   }
}
