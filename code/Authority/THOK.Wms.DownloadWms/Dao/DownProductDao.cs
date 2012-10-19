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
        #region 上报卷烟信息
        public void InsertProduct(DataSet brandSet)
        {
            DataTable brandTable = brandSet.Tables["WMS_PRODUCT"];
            DataTable brandCode = this.ExecuteQuery("SELECT BRAND_CODE FROM ms.DWV_IINF_BRAND ").Tables[0];
            string brand_code = "''";
            string sql;
            for (int i = 0; i < brandCode.Rows.Count; i++)
            {
                brand_code += ",'" + brandCode.Rows[i]["BRAND_CODE"] + "'";
            }
            foreach (DataRow row in brandTable.Rows)
            {
                string Sql = "select unit_code01 from wms_unit_list where unit_list_code='" + row["unit_list_code"] + "'";
                string qtyUnit = this.ExecuteQuery(Sql).Tables[0].ToString();
                if (brand_code != "''" && brand_code.Contains(row["product_code"].ToString()))
                {
                     sql = string.Format("UPDATE  ms.DWV_IINF_BRAND SET BRAND_CODE='{0}',BRAND_TYPE='{1}',BRAND_NAME='{2}',UP_CODE='{3}',BARCODE_BAR='{4}',PRICE_LEVEL_CODE='{5}',IS_FILTERTIP='{6}',IS_NEW='{7}',IS_FAMOUS='{8}'" +
                       " ,IS_MAINPRODUCT='{9}',IS_MAINPROVINCE='{10}',BELONG_REGION='{11}',IS_ABNORMITY_BRAND='{12}',BUY_PRICE={13},TRADE_PRICE={14},RETAIL_PRICE={15},COST_PRICE={16},QTY_UNIT={17}" +
                       ",BARCODE_ONE_PROJECT='{18}' ,UPDATE_DATE='{19}',ISACTIVE='{20}',N_UNIFY_CODE='{21}',IS_CONFISCATE='{22}',IS_IMPORT='{23}')" , row["product_code"], row["product_type_code"], row["product_name"], row["product_code"], row["bar_barcode"], row["price_level_code"],
                       row["is_filter_tip"], row["is_new"], row["is_famous"], row["is_main_product"], row["is_province_main_product"], row["belong_region"],
                       row["is_abnormity"], row["buy_price"], row["trade_price"], row["retail_price"], row["cost_price"], qtyUnit, row["one_project_barcode"], row["update_time"], row["is_active"], row["uniform_code"], row["is_confiscate"], '0');
                    this.ExecuteNonQuery(sql);
                }
                else 
                {
                    sql = string.Format("INSERT INTO ms.DWV_IINF_BRAND(BRAND_CODE,BRAND_TYPE,BRAND_NAME,UP_CODE,BARCODE_BAR,PRICE_LEVEL_CODE,IS_FILTERTIP,IS_NEW,IS_FAMOUS" +
                       " ,IS_MAINPRODUCT,IS_MAINPROVINCE,BELONG_REGION,IS_ABNORMITY_BRAND,BUY_PRICE,TRADE_PRICE,RETAIL_PRICE,COST_PRICE,QTY_UNIT" +
                       ",BARCODE_ONE_PROJECT ,UPDATE_DATE,ISACTIVE,N_UNIFY_CODE,IS_CONFISCATE,IS_IMPORT)" +
                       " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13},{14},{15},{16},{17},'{18}','{19}','{20}','{21}','{22}','{23}')", row["product_code"], row["product_type_code"], row["product_name"], row["product_code"], row["bar_barcode"], row["price_level_code"],
                       row["is_filter_tip"], row["is_new"], row["is_famous"], row["is_main_product"], row["is_province_main_product"], row["belong_region"],
                       row["is_abnormity"], row["buy_price"], row["trade_price"], row["retail_price"], row["cost_price"], qtyUnit, row["one_project_barcode"], row["update_time"], row["is_active"], row["uniform_code"], row["is_confiscate"], '0');
                    this.ExecuteNonQuery(sql);
                }
            }
        }
        #endregion
        /// <summary>
        /// 查询数字仓储产品编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductCode()
        {
            string sql = "SELECT CUSTOM_CODE FROM WMS_PRODUCT";
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
