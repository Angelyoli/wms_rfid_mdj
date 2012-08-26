using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownUnitDao : BaseDao
    {
        #region 从营系统据下载单位数据

        /// <summary>
        /// 下载单位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitInfo(string unitCode)
        {
            string sql = string.Format(@"SELECT U.*,B.BRAND_N FROM V_WMS_BRAND_UNIT U
                                        LEFT JOIN  V_WMS_BRAND B ON U.BRAND_CODE =B.BRAND_CODE
                                        WHERE (B.BRAND_N <> 'NULL' OR B.BRAND_N !='') and {0}", unitCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询计量单位系列表
        /// </summary>
        /// <param name="ulistCode"></param>
        /// <returns></returns>
        public DataTable GetBrandUlistInfo(string ulistCode)
        {
            string sql = string.Format("SELECT * FROM V_WMS_BRAND_ULIST WHERE {0}", ulistCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 单位表插入数据
        /// </summary>
        /// <param name="ds"></param>
        public void InsertUnit(DataTable unitTable)
        {
            BatchInsert(unitTable, "wms_unit");
        }

        /// <summary>
        /// 创联中间表
        /// </summary>
        /// <param name="ulistCodeTable"></param>
        public void InsertUlist(DataTable ulistCodeTable)
        {
            BatchInsert(ulistCodeTable, "WMS_BRAND_ULIST");
        }

        /// <summary>
        /// 浪潮中间表
        /// </summary>
        /// <param name="ds"></param>
        public void InsertLcUnit(DataTable unitTable)
        {
            BatchInsert(unitTable, "wms_unit_list");
        }

        /// <summary>
        /// 查询仓储单位系列编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetUlistCode()
        {
            string sql = "SELECT BRAND_ULIST_CODE FROM WMS_BRAND_ULIST";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询仓储单位编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitCode()
        {
            string sql = "SELECT unit_code FROM wms_unit";
            return this.ExecuteQuery(sql).Tables[0];
        }


        public DataTable GetUnitProduct()
        {
            string sql = "SELECT unit_list_code FROM wms_unit_list";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查计量单位件单位
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductByUnitCodeTiao()
        {
            string sql = "SELECT * FROM DBO.WMS_UNIT WHERE UNITNAME LIKE '条%'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 根据计量单位系列编号查询数据
        /// </summary>
        /// <param name="ulistCode"></param>
        /// <returns></returns>
        public DataTable FindUnitCodeByUlistCode(string ulistCode)
        {
            string sql = string.Format("SELECT * FROM WMS_BRAND_ULIST WHERE BRAND_ULIST_CODE='{0}'", ulistCode);
            return this.ExecuteQuery(sql).Tables[0];
        }
     
        #endregion

        #region 从营系统据下载单位数据-广西浪潮

        /// <summary>
        /// 下载单位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitCodeInfo(string prodcutCode)
        {
            string sql = string.Format("SELECT BRAND_CODE,BRAND_UNIT_CODE,BRAND_UNIT_NAME,COUNT FROM V_WMS_BRAND_UNIT WHERE BRAND_CODE NOT IN({0})", prodcutCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查计量单位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductByUnitCode(string product)
        {
            string sql = string.Format("SELECT * FROM WMS_UNIT_PRODUCT WHERE PRODUCTCODE='{0}'", product);
            return this.ExecuteQuery(sql).Tables[0];
        }


        public DataTable FindUnitListCOde(string unitListCode)
        {
            string sql = string.Format("SELECT * FROM wms_unit_list WHERE unit_list_code='{0}'", unitListCode);
            return this.ExecuteQuery(sql).Tables[0];
        }
        #endregion
    }
}
