using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownProductDao : BaseDao
    {
        /// <summary>
        /// ���ؾ��̲�Ʒ��Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductInfo(string codeList)
        {
            string sql = string.Format(" SELECT * FROM V_WMS_BRAND WHERE {0}", codeList);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_PRODUCT"], "wms_product");
        }
        
        /// <summary>
        /// ��ѯ���ֲִ���Ʒ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductCode()
        {
            string sql = "SELECT CUSTOM_CODE FROM WMS_PRODUCT";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ��ѯ������Ϣ
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public DataTable FindProductCodeInfo(string productCode)
        {
            string sql = "SELECT * FROM WMS_PRODUCT WHERE  " + productCode;
            return this.ExecuteQuery(sql).Tables[0];
        }
        
    }
}
