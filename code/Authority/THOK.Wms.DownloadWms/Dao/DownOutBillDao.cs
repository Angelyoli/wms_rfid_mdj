using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownOutBillDao : BaseDao
    {
        /// <summary>
        /// 从营销系统下载出库单主表单据数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetOutBillMaster(string outBillNo)
        {
            string sql = string.Format("SELECT * FROM V_WMS_OUT_ORDER WHERE {0}", outBillNo);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 从营销系统下载出库单明细表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetOutBillDetail(string outBillNo)
        {
            string sql = string.Format("SELECT * FROM V_WMS_OUT_ORDER_DETAIL WHERE {0}", outBillNo);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 添加主表数据到表 WMS_OUT_BILLMASTER
        /// </summary>
        /// <param name="ds"></param>
        public void InsertOutBillMaster(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_OUT_BILLMASTER"], "wms_out_bill_master");
        }

        /// <summary>
        /// 查询当前登陆的操作员
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable FindEmployee(string userName)
        {
            string sql = "SELECT * FROM WMS_EMPLOYEE WHERE USER_NAME='" + userName + "'";
            return this.ExecuteQuery(sql).Tables[0];
        } 
        /// <summary>
        /// 添加明细表数据到表 WMS_OUT_BILLDETAIL
        /// </summary>
        /// <param name="ds"></param>
        public void InsertOutBillDetail(DataSet ds)
        {
            if (ds.Tables["WMS_OUT_BILLDETAILA"].Rows.Count > 0)
            {
                BatchInsert(ds.Tables["WMS_OUT_BILLDETAILA"], "wms_out_bill_detail");
            }
        }

        /// <summary>
        /// 查询数字仓储出库7天之内的单据
        /// </summary>
        /// <returns></returns>
        public DataTable GetOutBillNo()
        {
            string sql = "SELECT bill_no FROM wms_out_bill_master WHERE bill_date>=DATEADD(DAY, -4, CONVERT(VARCHAR(14), GETDATE(), 112)) ORDER BY bill_date";
            return this.ExecuteQuery(sql).Tables[0];
        }

    }
}
