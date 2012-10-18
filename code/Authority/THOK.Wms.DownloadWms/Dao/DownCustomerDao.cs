using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownCustomerDao : BaseDao
    {

        /// <summary>
        /// 根据客户编码下载库户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomerInfo(string customerCode)
        {
            string sql = string.Format("SELECT * FROM V_WMS_CUSTOMER WHERE {0}", customerCode);
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 查询客户编码
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomerCode()
        {
            string sql = "SELECT customer_code FROM wms_customer";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="customerDt"></param>
        public void Insert(DataSet customerDs)
        {
            this.BatchInsert(customerDs.Tables["DWV_IORG_CUSTOMER"], "wms_customer");
        }

        #region 上报客户信息
        public void InsertCustom(DataSet customSet)
        {
            DataTable customTable = customSet.Tables["DWV_IORG_CUSTOMER"];
            DataTable custCode = this.ExecuteQuery("SELECT CUST_CODE FROM ms.DWV_IORG_CUSTOMER ").Tables[0];
            string cust_code = "''";
            string sql;
            for (int i = 0; i < custCode.Rows.Count; i++)
            {
                cust_code += ",'" + custCode.Rows[i]["CUST_CODE"] + "'";
            }
            foreach (DataRow row in customTable.Rows)
            {
                if (cust_code != "''" && cust_code.Contains(row["customer_code"].ToString()))
                {
                    sql = string.Format("update DWV_IORG_CUSTOMER SET CUST_CODE='{0}',CUST_N='{1}',CUST_NAME='{2}',ORG_CODE='{3}',SALE_REG_CODE='{4}',CUST_TYPE='{5}',RTL_CUST_TYPE_CODE='{6}'," +
                       "CUST_GEO_TYPE_CODE='{7}',DIST_ADDRESS='{8}',DIST_PHONE='{9}',LICENSE_CODE='{10}',PRINCIPAL_NAME='{11}',UPDATE_DATE='{12}',ISACTIVE='{13}',IS_IMPORT='{14}',N_CUST_CODE='{15}',SEFL_CUST_FLG='{16}',FUNC_CUST_FLG='{17}',BUSI_CIRC_TYPE='{18}',CHAI_FLG='{19}',ALL_DAY_FLG='{20}',BUSI_HOUR='{21}',INFO_TERM='{22}',HEAD_LOGO='{23}',DELI_TIME='{24}',DELIVER_WAY='{25}',PAY_TYPE='{26}' WHERE CUST_CODE='{0}'"
                       , row["customer_code"], row["custom_code"],
                       row["customer_name"], row["company_code"], row["sale_region_code"], row["sale_scope"], row["industry_type"], row["city_or_countryside"], row["address"], row["phone"],
                       row["license_code"], row["principal_name"], row["update_time"], row["is_active"], row["IS_IMPORT"], row["uniform_code"], 1, 0, 1, 2, 2, 1, 3, 3, 2, 1, 2);
                    this.ExecuteNonQuery(sql);
                }
                else
                {
                    sql = string.Format("INSERT INTO DWV_IORG_CUSTOMER(CUST_CODE,CUST_N,CUST_NAME,ORG_CODE,SALE_REG_CODE,CUST_TYPE,RTL_CUST_TYPE_CODE," +
                       "CUST_GEO_TYPE_CODE,DIST_ADDRESS,DIST_PHONE,LICENSE_CODE,PRINCIPAL_NAME,UPDATE_DATE,ISACTIVE,IS_IMPORT,N_CUST_CODE,SEFL_CUST_FLG,FUNC_CUST_FLG,BUSI_CIRC_TYPE,CHAI_FLG,ALL_DAY_FLG,BUSI_HOUR,INFO_TERM,HEAD_LOGO,DELI_TIME,DELIVER_WAY,PAY_TYPE)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}' ,'{21}','{22}','{23}','{24}','{25}','{26}')", row["customer_code"], row["company_code"],
                       row["customer_name"], row["company_code"], row["sale_region_code"], row["sale_scope"], row["industry_type"], row["city_or_countryside"], row["address"], row["phone"],
                       row["license_code"], row["principal_name"], row["update_time"], row["is_active"], row["IS_IMPORT"], row["uniform_code"], 1, 0, 1, 2, 2, 1, 3, 3, 2, 1, 2);
                    this.ExecuteNonQuery(sql);
                }
            }
        }
        #endregion
    }
}
