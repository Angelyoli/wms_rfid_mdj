using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownProductDao : BaseDao
    {
        #region 从营系统据下载产品信息

        /// <summary>
        /// 下载卷烟产品信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductInfo(string codeList)
        {
            string sql = string.Format(" SELECT * FROM V_WMS_BRAND WHERE {0}", codeList);
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

        #endregion


        /// <summary>
        /// 查询数字仓储产品编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductCode()
        {
            string sql = "SELECT custom_code FROM wms_product";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 根据编码筛选查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetProductCode(string code)
        {
            string sql = string.Format("SELECT TOP 10 PRODUCTCODE FROM WMS_PRODUCT WHERE PRODUCTCODE LIKE '{0}%'", code);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询卷烟产品信息
        /// </summary>
        /// <returns></returns>
        public DataTable ProductInfo()
        {
            string sql = "SELECT * FROM wms_product";
            return this.ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindProductCodeInfo(string productCode)
        {
            string sql = "SELECT * FROM wms_product where custom_code='" + productCode + "'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询卷烟产品信息
        /// </summary>
        /// <returns></returns>
        public DataTable FindProductInfo(string productCode)
        {
            string sql = @"select product_code,b.unit_code02 from wms_product a
                            left join wms_unit_list b on a.unit_list_code=b.unit_list_code
                            where custom_code='{0}'";
            sql = string.Format(sql, productCode);
            return this.ExecuteQuery(sql).Tables[0];
        }


        public DataTable FindUnitListInfo(string unitListCode)
        {
            string sql = "SELECT * FROM wms_unit_list WHERE unit_list_code='" + unitListCode + "'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 支转换为件的换算
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public DataTable DownProductRate(string productCode)
        {
            string sql = @"SELECT A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE,A.TIAOCODE,
                (SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.JIANCODE=B.UNITCODE AND A.PRODUCTCODE='{0}') AS JIANRATE,
                (SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.TIAOCODE=B.UNITCODE AND A.PRODUCTCODE='{0}') AS TIAORATE 
                 FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE  A.PRODUCTCODE='{0}' GROUP BY A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE,A.TIAOCODE";
            sql = string.Format(sql, productCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 支转换为件的换算
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public DataTable LcDownProductRate(string productCode)
        {
            string sql = @"SELECT A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE,A.TIAOCODE,
                (SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.JIANCODE=B.UNITCODE AND A.PRODUCTN='{0}') AS JIANRATE,
                (SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.TIAOCODE=B.UNITCODE AND A.PRODUCTN='{0}') AS TIAORATE 
                 FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE  A.PRODUCTN='{0}' GROUP BY A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE,A.TIAOCODE";
            sql = string.Format(sql, productCode);
            return this.ExecuteQuery(sql).Tables[0];
        }
    }
}
