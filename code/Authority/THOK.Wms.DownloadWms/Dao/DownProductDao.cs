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
                case "mdjyc-db2"://牡丹江烟草db2
                    sql = string.Format("SELECT V_WMS_BRAND.*,BRAND_N AS BRANDCODE FROM V_WMS_BRAND WHERE {0}", codeList);
                    break;
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
            foreach (DataRow row in ds.Tables["WMS_PRODUCT"].Rows)
            {
                //PIECE_BARCODE='{14}',
                string sql = @"IF '{0}' IN (SELECT PRODUCT_CODE FROM WMS_PRODUCT) 
                                BEGIN 
                                    UPDATE WMS_PRODUCT SET IS_ABNORMITY='{29}',PRODUCT_NAME = '{1}',CUSTOM_CODE='{3}' WHERE PRODUCT_CODE = '{0}' 
                                END 
                             ELSE 
                                BEGIN 
                                    INSERT INTO WMS_PRODUCT VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}' ,'{7}','{8}','{9}','{10}','{11}','{12}' ,'{13}','{14}' ,'{15}','{16}','{17}',{18},{19},{20},{21},'{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}',{33},'{34}','{35}')
                               END";
                sql = string.Format(sql, row["product_code"], row["product_name"], row["uniform_code"], row["custom_code"], row["short_code"], row["unit_list_code"], row["unit_code"], row["supplier_code"], row["brand_code"], row["abc_type_code"], row["product_type_code"], row["pack_type_code"], row["price_level_code"], row["statistic_type"], row["piece_barcode"], row["bar_barcode"], row["package_barcode"], row["one_project_barcode"], row["buy_price"], row["trade_price"], row["retail_price"], row["cost_price"], row["is_filter_tip"], row["is_new"], row["is_famous"], row["is_main_product"], row["is_province_main_product"], row["belong_region"], row["is_confiscate"], row["is_abnormity"], row["description"], row["is_active"], row["update_time"], row["is_rounding"], row["cell_max_product_quantity"], row["point_area_codes"]);
                ExecuteNonQuery(sql);
            }
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
