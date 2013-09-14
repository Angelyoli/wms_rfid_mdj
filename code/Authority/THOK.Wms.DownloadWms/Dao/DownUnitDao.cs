using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.Wms.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownUnitDao : BaseDao
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
        /// 下载单位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitInfo(string unitCode)
        {
            string sql = "";
            dbTypeName = this.SalesSystemDao();
            switch (dbTypeName)
            {
                case "gxyc-db2"://广西烟草db2
                    sql = string.Format(@"SELECT U.*,B.BRAND_N AS BARNDCODE FROM V_WMS_BRAND_UNIT U
                                        LEFT JOIN  V_WMS_BRAND B ON U.BRAND_CODE =B.BRAND_CODE
                                        WHERE (B.BRAND_N <> 'NULL' OR B.BRAND_N !='') AND {0}", unitCode);
                    break;
                case "gzyc-oracle"://贵州烟草oracle
                    sql = string.Format(@"SELECT U.*,U.BRAND_CODE AS BARNDCODE FROM V_WMS_BRAND_UNIT U
                                        LEFT JOIN  V_WMS_BRAND B ON U.BRAND_CODE =B.BRAND_CODE
                                        WHERE {0}", unitCode);
                    break;
                default://默认广西烟草
                    string.Format(@"SELECT U.*,B.BRAND_N AS BARNDCODE FROM V_WMS_BRAND_UNIT U
                                        LEFT JOIN  V_WMS_BRAND B ON U.BRAND_CODE =B.BRAND_CODE
                                        WHERE (B.BRAND_N <> 'NULL' OR B.BRAND_N !='') AND {0}", unitCode);
                    break;
            }
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 下载单位信息 平顶山
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitInfos(string unitCode)
        {
            string sql = string.Format("SELECT * FROM IC.V_WMS_BRAND_UNIT WHERE {0}", unitCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询计量单位系列表
        /// </summary>
        /// <param name="ulistCode"></param>
        /// <returns></returns>
        public DataTable GetBrandUlistInfo(string ulistCode)
        {
            string sql = string.Format("SELECT * FROM IC.V_WMS_BRAND_ULIST WHERE {0}", ulistCode);
            return this.ExecuteQuery(sql).Tables[0];
        }
        /// <summary>
        /// 平顶山插入单位系列表
        /// </summary>
        /// <param name="ds"></param>
        public void InsertUlist(DataTable ulistCodeTable)
        {
            BatchInsert(ulistCodeTable, "wms_unit_list");
        }

        /// <summary>
        /// 查询仓储单位系列编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetUlistCode()
        {
            string sql = "SELECT unit_list_code FROM wms_unit_list";
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
        /// 插入单位系列表
        /// </summary>
        /// <param name="ds"></param>
        public void InsertLcUnit(DataTable unitTable)
        {
            BatchInsert(unitTable, "wms_unit_list");
        }

        /// <summary>
        /// 查询仓储单位编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitCode()
        {
            string sql = "SELECT unit_code FROM WMS_UNIT";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询单位系列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitProduct()
        {
            string sql = "SELECT * FROM WMS_UNIT_LIST";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 根据单位系列编码查询单位系列表
        /// </summary>
        /// <param name="unitListCode"></param>
        /// <returns></returns>
        public DataTable FindUnitListCOde(string unitListCode)
        {
            string sql = string.Format("SELECT * FROM WMS_UNIT_LIST WHERE UNIT_LIST_CODE='{0}'", unitListCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

    }
}
