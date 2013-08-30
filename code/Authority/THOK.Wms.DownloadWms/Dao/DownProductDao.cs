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

            //仓储业务数据接口服务器数据库类型
            if (parameter["SalesSystemDBType"] != "")
                dbTypeName = parameter["SalesSystemDBType"];

            return dbTypeName;
        }
        /// <summary>
        /// 下载卷烟产品信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductInfo(string codeList)
        {
            string sql = "";
            dbTypeName = this.SalesSystemDao();
            switch (dbTypeName)
            {
                case "gxyc-db2"://广西烟草db2
                    sql = string.Format("SELECT V_WMS_BRAND.*,BRAND_N AS BRANDCODE FROM V_WMS_BRAND WHERE {0}", codeList);
                    break;
                case "gzyc-oracle"://贵州烟草oracle
                    sql = string.Format("SELECT V_WMS_BRAND.*,BRAND_CODE AS BRANDCODE FROM V_WMS_BRAND WHERE {0}", codeList);
                    break;
                default://默认广西烟草
                    sql = string.Format("SELECT V_WMS_BRAND.*,BRAND_N AS BRANDCODE FROM V_WMS_BRAND WHERE {0}", codeList);
                    break;
            }

            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_PRODUCT"], "wms_product");
        }
        
        /// <summary>
        /// 查询数字仓储产品编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductCode()
        {
            string sql = "SELECT * FROM WMS_PRODUCT";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询卷烟信息
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
