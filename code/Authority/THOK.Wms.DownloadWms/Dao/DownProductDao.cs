using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
using THOK.Wms.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownProductDao : BaseDao
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
        /// ���ؾ��̲�Ʒ��Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductInfo(string codeList)
        {
            string sql = "";
            dbTypeName = this.SalesSystemDao();
            switch (dbTypeName)
            {
                case "gxyc-db2"://�����̲�db2
                    sql = string.Format("SELECT V_WMS_BRAND.*,BRAND_N AS BRANDCODE FROM V_WMS_BRAND WHERE {0}", codeList);
                    break;
                case "gzyc-oracle"://�����̲�oracle
                    sql = string.Format("SELECT V_WMS_BRAND.*,BRAND_CODE AS BRANDCODE FROM V_WMS_BRAND WHERE {0}", codeList);
                    break;
                default://Ĭ�Ϲ����̲�
                    sql = string.Format("SELECT V_WMS_BRAND.*,BRAND_N AS BRANDCODE FROM V_WMS_BRAND WHERE {0}", codeList);
                    break;
            }

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
            string sql = "SELECT * FROM WMS_PRODUCT";
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
