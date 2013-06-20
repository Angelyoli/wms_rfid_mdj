﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.Wms.DownloadWms.Dao
{
    public class DownDecidePlanDao : BaseDao
    {
        /// <summary>
        /// 查询决策系统入库单据主表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMiddleInBillMaster(string inBillNoList)
        {
            string sql = string.Format(@"SELECT ID AS BILL_NO,BILL_DATE AS BILL_DATE FROM INTER_BILL_MASTER WHERE {0}", inBillNoList);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询决策系统入库明细表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMiddleInBillDetail(string inBillNoList)
        {
            string sql = string.Format(@"SELECT MASTER_ID AS BILL_NO,SUBSTRING(BOX_CIGAR_CODE,8,5) AS PRODUCT_CODE,BILL_QUANTITY AS QUANTITY FROM INTER_BILL_DETAIL WHERE 1=1 {0}",inBillNoList);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入主表数据
        /// </summary>
        /// <param name="ds"></param>
        public void InsertInBillMaster(DataSet ds)
        {
            foreach (DataRow row in ds.Tables["WMS_IN_BILLMASTER"].Rows)
            {
                string sql = "INSERT INTO wms_in_bill_master(bill_no,bill_date,bill_type_code,warehouse_code,status,is_active,update_time,operate_person_id" +
                   ") VALUES('" + row["bill_no"] + "','" + row["bill_date"] + "','" + row["bill_type_code"] + "'," +
                   "'" + row["warehouse_code"] + "','" + row["status"] + "','" + row["is_active"] + "','" + row["update_time"] + "','" + row["operate_person_id"] + "')";
                this.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 插入明细表数据
        /// </summary>
        /// <param name="ds"></param>
        public void InsertInBillDetail(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_IN_BILLDETAIL"], "wms_in_bill_detail");
        }

        /// <summary>
        /// 插入数据到中间表
        /// </summary>
        /// <param name="ds"></param>
        public void InsertMiddle(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_MIDDLE_IN_BILLDETAIL"], "WMS_MIDDLE_IN_BILL");
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
        
        public DataTable GetInBillMasterDateTime(string dateTimeStr)
        {
            string sql = "SELECT * FROM WMS_IN_BILL_MASTER WHERE bill_date>='" + dateTimeStr + "' ORDER BY BILL_NO DESC";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询中间表7天内单据
        /// </summary>
        /// <returns></returns>
        public DataTable GetMiddleBillNo()
        {
            string sql = "SELECT * FROM WMS_MIDDLE_IN_BILL WHERE BILL_DATE>=DATEADD(DAY, -4, CONVERT(VARCHAR(14), GETDATE(), 112)) ORDER BY BILL_DATE";
            return this.ExecuteQuery(sql).Tables[0];
        }


        public DataTable GetMiddleBillNo(string billNo)
        {
            string sql = string.Format("SELECT * FROM WMS_MIDDLE_IN_BILL WHERE BILL_NO='{0}'", billNo);
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 根据单位系列编码查询单位系列表
        /// </summary>
        /// <param name="unitListCode"></param>
        /// <returns></returns>
        public DataTable FindUnitListCode(string unitListCode)
        {
            string sql = string.Format(@"SELECT * FROM WMS_UNIT_LIST A LEFT JOIN WMS_PRODUCT B ON B.PRODUCT_CODE=A.UNIT_LIST_CODE 
                                         WHERE A.UNIT_LIST_CODE='{0}'", unitListCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        public void DeleteMiddleBill(string billno)
        {
            string sql = string.Format(@"DELETE WMS_MIDDLE_IN_BILL WHERE IN_BILL_NO='{0}'", billno);
            this.ExecuteNonQuery(sql);
        }
    }
}
