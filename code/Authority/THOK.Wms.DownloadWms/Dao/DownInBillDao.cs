using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.Wms.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownInBillDao : BaseDao
    {
        public string dbTypeName = "";
        public string SalesSystemDao()
        {
            SysParameterDao parameterDao = new SysParameterDao();
            Dictionary<string, string> parameter = parameterDao.FindParameters();

            //�ִ�ҵ�����ݽӿڷ��������ݿ�����
            if (parameter["SalesSystemDBType"] != "")
                dbTypeName = parameter["SalesSystemDBType"];

            return dbTypeName;
        }

        /// <summary>
        /// ��ѯӪ��ϵͳ��ⵥ������
        /// </summary>
        /// <returns></returns>
        public DataTable GetInBillMaster(string inBillNoList)
        {
            string sql = string.Format("SELECT * FROM V_WMS_IN_ORDER WHERE {0} ", inBillNoList);
            return this.ExecuteQuery(sql).Tables[0];
        }
        /// <summary>
        /// ��ѯӪ��ϵͳ��ⵥ������ ����
        /// </summary>
        /// <returns></returns>
        public DataTable GetInBillMasters(string inBillNoList)
        {
            string sql = string.Format("SELECT * FROM V_WMS_IN_ORDER WHERE {0} AND QUANTITY_SUM>0", inBillNoList);
            return this.ExecuteQuery(sql).Tables[0];
        }
        /// <summary>
        /// ��ѯӪ��ϵͳ�����ϸ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetInBillDetail(string inBillNoList)
        {
            string sql = "";
            dbTypeName = this.SalesSystemDao();
            switch (dbTypeName)
            {
                case "gxyc-db2"://�����̲�db2
                    sql = string.Format("SELECT A.*,B.BARND_N AS BRANDCODE FROM V_WMS_IN_ORDER_DETAIL A LEFT JOIN V_WMS_BRAND B ON A.BRAND_CODE=B.BRAND_CODE WHERE {0} ", inBillNoList);
                    break;
                case "gzyc-oracle"://�����̲�oracle
                    sql = string.Format("SELECT V_WMS_IN_ORDER_DETAIL.*,BRAND_CODE AS BRANDCODE FROM V_WMS_IN_ORDER_DETAIL WHERE {0} ", inBillNoList);
                    break;
                default://Ĭ�Ϲ����̲�
                    sql = string.Format("SELECT A.*,B.BARND_N AS BRANDCODE FROM V_WMS_IN_ORDER_DETAIL A LEFT JOIN V_WMS_BRAND B ON A.BRAND_CODE=B.BRAND_CODE WHERE {0} ", inBillNoList);
                    break;
            }

            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ��ѯ����7���ڵ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetBillNo()
        {
            string sql = "SELECT bill_no FROM wms_in_bill_master WHERE bill_date>=DATEADD(DAY, -4, CONVERT(VARCHAR(14), GETDATE(), 112)) ORDER BY bill_date";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ������������
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
        /// ������ϸ������
        /// </summary>
        /// <param name="ds"></param>
        public void InsertInBillDetail(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_IN_BILLDETAIL"], "wms_in_bill_detail");
        }

        /// <summary>
        /// ��ѯ��ǰ��½�Ĳ���Ա
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable FindEmployee(string userName)
        {
            string sql = "SELECT * FROM WMS_EMPLOYEE WHERE USER_NAME='" + userName + "'";
            return this.ExecuteQuery(sql).Tables[0];
        }
    }
}
