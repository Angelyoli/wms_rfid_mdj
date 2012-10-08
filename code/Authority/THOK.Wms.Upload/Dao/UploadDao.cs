using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.WMS.Upload.Dao
{
    public class UploadDao:BaseDao
    {
        
        #region 查询卷烟信息数据，上报中烟

        /// <summary>
        /// 查询卷烟信息表【DWV_IINF_BRAND】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryProductInfo()
        {
            string sql = "SELECT * FROM WMS_PRODUCT WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入卷烟信息表【DWV_IINF_BRAND】，中烟数据库
        /// </summary>
        /// <param name="brandTable"></param>
        public void InsertProduct(DataTable brandTable)
        {
            foreach (DataRow row in brandTable.Rows)
            {
                string Sql = "select unit_code01 from wms_unit_list where unit_list_code='" + row["unit_list_code"] + "'";
                string qtyUnit = this.ExecuteQuery(Sql).Tables[0].ToString();
                string sql = string.Format("INSERT INTO ms.DWV_IINF_BRAND(BRAND_CODE,BRAND_TYPE,BRAND_NAME,UP_CODE,BARCODE_BAR,PRICE_LEVEL_CODE,IS_FILTERTIP,IS_NEW,IS_FAMOUS" +
                   " ,IS_MAINPRODUCT,IS_MAINPROVINCE,BELONG_REGION,IS_ABNORMITY_BRAND,BUY_PRICE,TRADE_PRICE,RETAIL_PRICE,COST_PRICE,QTY_UNIT" +
                   ",BARCODE_ONE_PROJECT ,UPDATE_DATE,ISACTIVE,N_UNIFY_CODE,IS_CONFISCATE,IS_IMPORT)" +
                   " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13},{14},{15},{16},{17},'{18}','{19}','{20}','{21}','{22}','{23}')", row["product_code"], row["product_type_code"], row["product_name"], row["product_code"], row["bar_barcode"], row["price_level_code"],
                   row["is_filter_tip"], row["is_new"], row["is_famous"], row["is_main_product"], row["is_province_main_product"], row["belong_region"],
                   row["is_abnormity"], row["buy_price"], row["trade_price"], row["retail_price"], row["cost_price"], qtyUnit, row["one_project_barcode"], row["update_time"], row["is_active"], row["uniform_code"], row["is_confiscate"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 修改卷烟信息表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateProduct()
        {
            string sql = "UPDATE WMS_PRODUCT SET IS_IMPORT='1' WHERE IS_IMPORT='0'";
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询组织结构表数据，上报中烟

        /// <summary>
        /// 查询组织结构表【DWV_IORG_ORGANIZATION】 ，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryOrganization()
        {
            string sql = "SELECT * FROM wms_company WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 插入组织结构表【DWV_IORG_ORGANIZATION】，中烟数据库
        /// </summary>
        /// <param name="organTable"></param>
        public void InsertOrganization(DataTable organTable)
        {
            foreach (DataRow row in organTable.Rows)
            {
                string date = Convert.ToDateTime(row["UPDATE_DATE"]).ToString("yyyyMMddHHmmss");
                string sql = string.Format("INSERT INTO DWV_IORG_ORGANIZATION(ORGANIZATION_CODE,ORGANIZATION_NAME,ORGANIZATION_TYPE,UP_CODE" +
                " ,N_ORGANIZATION_CODE,STORE_ROOM_AREA,STORE_ROOM_NUM,STORE_ROOM_CAPACITY,SORTING_NUM,UPDATE_DATE,ISACTIVE,IS_IMPORT)" +
                "VALUES('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8},'{9}','{10}','{11}')", row["company_code"], row["company_name"], row["company_type"], row["parent_company_id"],
                row["uniform_code"]??"", row["warehouse_space"], row["warehouse_count"], row["warehouse_capacity"], row["sorting_count"],
                date, row["is_active"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 修改组织结构表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateOrganization()
        {
            string sql = string.Format("UPDATE wms_company SET IS_IMPORT='1' WHERE IS_IMPORT='0'");
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询人员信息表数据，上报中烟

        /// <summary>
        /// 查询人员信息表【DWV_IORG_PERSON】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryPerson()
        {
            string sql = "SELECT * FROM wms_employee WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 插入人员信息表【DWV_IORG_PERSON】，中烟数据库
        /// </summary>
        /// <param name="presonTable"></param>
        public void InsertPreson(DataTable presonTable)
        {
            foreach (DataRow row in presonTable.Rows)
            {
                string date = Convert.ToDateTime(row["UPDATE_DATE"]).ToString("yyyyMMddHHmmss");
                string sql = string.Format("INSERT INTO DWV_IORG_PERSON(PERSON_CODE,PERSON_N,PERSON_NAME,SEX," +
                    " UPDATE_DATE,ISACTIVE,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", row["employee_code"],
                    row["employee_code"], row["employee_name"], row["sex"], date, row["is_active"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }


        /// <summary>
        /// 修改人员信息表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdatePerson()
        {
            string sql = "UPDATE wms_employee SET IS_IMPORT='1' WHERE IS_IMPORT='0'";
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询客户信息表数据，上报中烟

        /// <summary>
        /// 查询客户信息表【DWV_IORG_CUSTOMER】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryCustomer()
        {
            string sql = "SELECT * FROM wms_customer WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 插入客户信息表【DWV_IORG_CUSTOMER】，中烟数据库
        /// </summary>
        /// <param name="customerTable"></param>
        public void InsertCustomer(DataTable customerTable)
        {
            DataTable custCode = this.ExecuteQuery("SELECT custom_code FROM wms_customer ").Tables[0];
            string cust_code = "''";
            string sql;
            for (int i = 0; i < custCode.Rows.Count; i++)
            {
                cust_code += ",'" + custCode.Rows[i]["custom_code"] + "'";
            }
            foreach (DataRow row in customerTable.Rows)
            {
                if (cust_code != "''" && cust_code.Contains(row["custom_code"].ToString()))
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


        /// <summary>
        /// 修改客户信息表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateCustomer(string customerCode)
        {
            string sql = "UPDATE wms_customer SET IS_IMPORT='1' WHERE IS_IMPORT='0'";
            this.ExecuteNonQuery(sql);
        }


        #endregion


        #region 查询仓库库存表数据，上报中烟

        /// <summary>
        /// 查询仓库库存表【DWV_IWMS_STORE_STOCK】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryStoreStock()
        {
            string sql = @"select product_code,area_type,brand_batch,sum(quantity)as quantity
                   from  wms_storage group by product_code,brand_batch,area_type";
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 插入仓库库存表【DWV_IWMS_STORE_STOCK】
        /// </summary>
        /// <param name="stockTable"></param>
        public void InsertStoreStock(DataTable stockTable)
        {
            string sql = "DELETE FROM DWV_IWMS_STORE_STOCK";
            this.ExecuteNonQuery(sql);
            foreach (DataRow row in stockTable.Rows)
            {
                 sql = string.Format("INSERT INTO DWV_IWMS_STORE_STOCK(STORE_PLACE_CODE,BRAND_CODE,AREA_TYPE,BRAND_BATCH,DIST_CTR_CODE,QUANTITY,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','0101',{4},'{5}')",
                  10002, row["currentproduct"], row["AREA_TYPE"], row["BRAND_BATCH"], row["QUANTITY"], 0);
                this.ExecuteNonQuery(sql);              
            }         
        }

        /// <summary>
        /// 修改仓库库存表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateStoreStock(string storeStockCode)
        {
            string sql = string.Format("UPDATE DWV_IWMS_STORE_STOCK SET IS_IMPORT='1' WHERE IS_IMPORT='0'");
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询业务库存表数据，上报中烟


        /// <summary>
        /// 查询业务库存表【DWV_IWMS_BUSI_STOCK】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryBusiStock()
        {
            string sql = "SELECT * FROM V_DWV_IWMS_BUSI_STOCK WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 插入业务库存表【DWV_IWMS_BUSI_STOCK】
        /// </summary>
        /// <param name="stockTable"></param>
        public void InsertBustStock(DataTable stockTable)
        {
            string sql = "DELETE FROM DWV_IWMS_BUSI_STOCK";
            this.ExecuteNonQuery(sql);
            foreach (DataRow row in stockTable.Rows)
            {

                sql = string.Format("INSERT INTO DWV_IWMS_BUSI_STOCK(ORG_CODE,BRAND_CODE,DIST_CTR_CODE,QUANTITY,IS_IMPORT)VALUES('{0}','{1}','0101',{2},'{3}')",
                 row["ORG_CODE"], row["BRAND_CODE"], row["QUANTITY"], row["IS_IMPORT"]);

                this.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 修改业务库存表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateBusiStock(string busiStockCode)
        {
            string sql = string.Format("UPDATE DWV_IWMS_BUSI_STOCK SET IS_IMPORT='1' WHERE IS_IMPORT='0'");
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询仓库入库单据主表数据，上报中烟


        /// <summary>
        /// 查询仓库入库单据主表【DWV_IWMS_IN_STORE_BILL】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryInMasterBill()
        {
            string sql = "SELECT * FROM V_DWV_IWMS_IN_STORE_BILL WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入仓库入库单据主表【DWV_IWMS_IN_STORE_BILL】，中烟数据库
        /// </summary>
        /// <param name="masterTable"></param>
        public void InsertInMasterBill(DataTable inMasterTable)
        {
            string sql;
            foreach (DataRow row in inMasterTable.Rows)
            {
                sql = string.Format("INSERT INTO DWV_IWMS_IN_STORE_BILL(STORE_BILL_ID,RELATE_STORE_BILL_ID,RELATE_BUSI_BILL_NUM,DIST_CTR_CODE,AREA_TYPE,QUANTITY_SUM," +
                   "AMOUNT_SUM,DETAIL_NUM,CREATOR_CODE,CREATE_DATE,AUDITOR_CODE,AUDIT_DATE,ASSIGNER_CODE,ASSIGN_DATE,AFFIRM_CODE,AFFIRM_DATE," +
                   "IN_OUT_TYPE,BILL_TYPE,BILL_STATUS,DISUSE_STATUS,IS_IMPORT)VALUES('{0}','{1}',{2},'{3}','{4}',{5},{6},{7},'{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')",
                   row["STORE_BILL_ID"], row["RELATE_STORE_BILL_ID"], row["RELATE_BUSI_BILL_NUM"], row["DIST_CTR_CODE"], row["AREA_TYPE"], row["QUANTITY_SUM"], row["AMOUNT_SUM"], row["DETAIL_NUM"],
                   row["CREATOR_CODE"], row["CREATE_DATE"], row["AUDITOR_CODE"], row["AUDIT_DATE"], row["ASSIGNER_CODE"], row["ASSIGN_DATE"], row["AFFIRM_CODE"],
                   row["AFFIRM_DATE"], row["IN_OUT_TYPE"], row["BILL_TYPE"], row["BILL_STATUS"], row["DISUSE_STATUS"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
            sql = "UPDATE DWV_IWMS_IN_STORE_BILL SET DIST_CTR_CODE='0101',STORAGE_TYPE='4',STORAGE_CODE='10002',STORAGE_NAME='平顶山储位'";
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改仓库入库单据主表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateInMaster(string inMasterCode)
        {
            string sql = "UPDATE DWV_IWMS_IN_STORE_BILL SET IS_IMPORT='1' WHERE store_bill_id in (SELECT store_bill_id FROM V_DWV_IWMS_IN_STORE_BILL WHERE IS_IMPORT ='0')";
            this.ExecuteNonQuery(sql);
        }


        #endregion


        #region 查询仓库入库单据细表数据，上报中烟


        /// <summary>
        /// 查询仓库入库单据细表【DWV_IWMS_IN_STORE_BILL_DETAIL】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryInDetailBill()
        {
            string sql = "SELECT * FROM V_DWV_IWMS_IN_STORE_BILL_DETAIL WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 插入仓库入库单据细表【DWV_IWMS_IN_STORE_BILL_DETAIL】，中烟数据库
        /// </summary>
        /// <param name="detailTable"></param>
        public void InsertInDetailBill(DataTable inDetailTable)
        {
            foreach (DataRow row in inDetailTable.Rows)
            {
                string sql = string.Format("INSERT INTO DWV_IWMS_IN_STORE_BILL_DETAIL(STORE_BILL_DETAIL_ID,STORE_BILL_ID,BRAND_CODE,BRAND_NAME,QUANTITY,IS_IMPORT)" +
                    "VALUES('{0}','{1}','{2}','{3}',{4},'{5}')",
                    row["STORE_BILL_DETAIL_ID"], row["STORE_BILL_ID"], row["BRAND_N"], row["BRAND_NAME"], row["QUANTITY"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }


        /// <summary>
        /// 修改仓库入库单据细表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateInDetail(string inDetailCode)
        {
            string sql = "UPDATE DWV_IWMS_IN_STORE_BILL_DETAIL SET IS_IMPORT='1' WHERE store_bill_id in (SELECT store_bill_id FROM V_DWV_IWMS_IN_STORE_BILL_DETAIL WHERE IS_IMPORT ='0')";
            this.ExecuteNonQuery(sql);
        }
        #endregion


        #region 查询入库业务单据表数据，上报中烟


        /// <summary>
        /// 查询入库业务单据表【DWV_IWMS_IN_BUSI_BILL】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryInBusiBill()
        {
            string sql = "SELECT * FROM V_DWV_IWMS_IN_BUSI_BILL WHERE IS_IMPORT ='0' ORDER BY BUSI_BILL_ID,BRAND_NAME,END_STOCK_QUANTITY";
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        ///  插入入库业务单据表【DWV_IWMS_IN_BUSI_BILL】，中烟数据库
        /// </summary>
        /// <param name="busiTable"></param>
        public void InsertInBusiBill(DataTable inBusiTable)
        {
            string sql;
            foreach (DataRow row in inBusiTable.Rows)
            {
                 sql = string.Format("INSERT INTO DWV_IWMS_IN_BUSI_BILL(BUSI_ACT_ID,BUSI_BILL_DETAIL_ID,BUSI_BILL_ID,RELATE_BUSI_BILL_ID,STORE_BILL_ID,BRAND_CODE," +
                 "BRAND_NAME,QUANTITY,DIST_CTR_CODE,ORG_CODE,STORE_ROOM_CODE,STORE_PLACE_CODE,TARGET_NAME,IN_OUT_TYPE,BILL_TYPE,BEGIN_STOCK_QUANTITY," +
                 "END_STOCK_QUANTITY,DISUSE_STATUS,RECKON_STATUS,RECKON_DATE,UPDATE_CODE,UPDATE_DATE,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15},{16},'{17}','{18}','{19}','{20}','{21}','{22}')",
                 row["BUSI_ACT_ID"], row["BUSI_BILL_DETAIL_ID"], row["BUSI_BILL_ID"], row["RELATE_BUSI_BILL_ID"], row["STORE_BILL_ID"], row["BRAND_N"], row["BRAND_NAME"], row["QUANTITY"],
                 row["DIST_CTR_CODE"], row["ORG_CODE"], row["STORE_ROOM_CODE"], row["STORE_PLACE_CODE"], row["TARGET_NAME"], row["IN_OUT_TYPE"], row["BILL_TYPE"],
                 row["BEGIN_STOCK_QUANTITY"], row["END_STOCK_QUANTITY"], row["DISUSE_STATUS"], row["RECKON_STATUS"], row["RECKON_DATE"], row["UPDATE_CODE"], row["UPDATE_DATE"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
            sql = "update  DWV_IWMS_IN_BUSI_BILL set STORE_ROOM_CODE='1002',STORE_PLACE_CODE='10002' ";
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改入库业务单据表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateInBusi(string inBusiCode)
        {
            string sql = "UPDATE DWV_IWMS_IN_BUSI_BILL SET IS_IMPORT='1' WHERE busi_bill_id in (SELECT busi_bill_id FROM V_DWV_IWMS_IN_BUSI_BILL WHERE IS_IMPORT ='0')";
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询仓库出库单据主表数据，上报中烟


        /// <summary>
        /// 查询仓库出库单据主表【DWV_IWMS_OUT_STORE_BILL】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryOutMasterBill()
        {
            string sql = "SELECT * FROM V_DWV_IWMS_OUT_STORE_BILL WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入仓库出库单据主表【DWV_IWMS_OUT_STORE_BILL】，中烟数据库
        /// </summary>
        /// <param name="outMasterTable"></param>
        public void InsertOutMasertBill(DataTable outMasterTable)
        {
            string sql;
            foreach (DataRow row in outMasterTable.Rows)
            {
                sql = string.Format("INSERT INTO DWV_IWMS_OUT_STORE_BILL(STORE_BILL_ID,RELATE_STORE_BILL_ID,RELATE_BUSI_BILL_NUM,DIST_CTR_CODE,AREA_TYPE,QUANTITY_SUM," +
                   "AMOUNT_SUM,DETAIL_NUM,CREATOR_CODE,CREATE_DATE,AUDITOR_CODE,AUDIT_DATE,ASSIGNER_CODE,ASSIGN_DATE,AFFIRM_CODE,AFFIRM_DATE," +
                   "IN_OUT_TYPE,BILL_TYPE,BILL_STATUS,DISUSE_STATUS,IS_IMPORT)VALUES('{0}','{1}',{2},'{3}','{4}',{5},{6},{7},'{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')",
                   row["STORE_BILL_ID"], row["RELATE_STORE_BILL_ID"], row["RELATE_BUSI_BILL_NUM"], row["DIST_CTR_CODE"], row["AREA_TYPE"], row["QUANTITY_SUM"], row["AMOUNT_SUM"], row["DETAIL_NUM"],
                   row["CREATOR_CODE"], row["CREATE_DATE"], row["AUDITOR_CODE"], row["AUDIT_DATE"], row["ASSIGNER_CODE"], row["ASSIGN_DATE"], row["AFFIRM_CODE"],
                   row["AFFIRM_DATE"], row["IN_OUT_TYPE"], row["BILL_TYPE"], row["BILL_STATUS"], row["DISUSE_STATUS"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);

            }
            sql = "UPDATE DWV_IWMS_OUT_STORE_BILL SET DIST_CTR_CODE='0101',STORAGE_TYPE='4',STORAGE_CODE='10002',STORAGE_NAME='平顶山储位'";
            this.ExecuteNonQuery(sql);
           
        }

        /// <summary>
        /// 修改仓库出库单据主表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateOutMaster(string outMasterCode)
        {
            string sql = "UPDATE DWV_IWMS_OUT_STORE_BILL SET IS_IMPORT='1' WHERE STORE_BILL_ID in (SELECT STORE_BILL_ID FROM V_DWV_IWMS_OUT_STORE_BILL WHERE IS_IMPORT ='0')";
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询仓库出库单据细表数据，上报中烟


        /// <summary>
        /// 查询仓库出库单据细表【DWV_IWMS_OUT_STORE_BILL_DETAIL】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryOutDetailBill()
        {
            string sql = "SELECT * FROM V_DWV_IWMS_OUT_STORE_BILL_DETAIL  WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入仓库出库单据细表【DWV_IWMS_OUT_STORE_BILL_DETAIL】，中烟数据库
        /// </summary>
        /// <param name="detailTable"></param>
        public void InsertOutDetailBill(DataTable outDetailTable)
        {
            //string sql = "update DWV_IWMS_OUT_STORE_BILL_DETAIL set QUANTITY=-QUANTITY where quantity>0 ";
            //this.ExecuteNonQuery(sql);
            foreach (DataRow row in outDetailTable.Rows)
            {
               string sql = string.Format("INSERT INTO DWV_IWMS_OUT_STORE_BILL_DETAIL(STORE_BILL_DETAIL_ID,STORE_BILL_ID,BRAND_CODE,BRAND_NAME,QUANTITY,IS_IMPORT)" +
                    "VALUES('{0}','{1}','{2}','{3}',{4},'{5}')",
                    row["STORE_BILL_DETAIL_ID"], row["STORE_BILL_ID"], row["BRAND_N"], row["BRAND_NAME"], row["QUANTITY"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }


        /// <summary>
        /// 修改仓库出库单据细表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateOutDetail(string outDetailCode)
        {
            string sql = "UPDATE DWV_IWMS_OUT_STORE_BILL_DETAIL SET IS_IMPORT='1' WHERE STORE_BILL_ID in (SELECT store_bill_id FROM V_DWV_IWMS_OUT_STORE_BILL_DETAIL WHERE IS_IMPORT ='0')";
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询出库业务单据表数据，上报中烟

        /// <summary>
        /// 查询出库业务单据表【DWV_IWMS_OUT_BUSI_BILL】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryOutBusiBill()
        {
            string sql = "SELECT * FROM V_DWV_IWMS_OUT_BUSI_BILL WHERE IS_IMPORT ='0' ORDER BY BUSI_BILL_ID,BRAND_NAME,END_STOCK_QUANTITY ";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        ///  插入出库业务单据表【DWV_IWMS_OUT_BUSI_BILL】，中烟数据库
        /// </summary>
        /// <param name="busiTable"></param>
        public void InsertOutBusiBill(DataTable outBusiTable)
        {
            string sql;
            foreach (DataRow row in outBusiTable.Rows)
            {
                sql = string.Format("INSERT INTO DWV_IWMS_OUT_BUSI_BILL(BUSI_ACT_ID,BUSI_BILL_DETAIL_ID,BUSI_BILL_ID,RELATE_BUSI_BILL_ID,STORE_BILL_ID,BRAND_CODE," +
                   "BRAND_NAME,QUANTITY,DIST_CTR_CODE,ORG_CODE,STORE_ROOM_CODE,STORE_PLACE_CODE,TARGET_NAME,IN_OUT_TYPE,BILL_TYPE,BEGIN_STOCK_QUANTITY," +
                   "END_STOCK_QUANTITY,DISUSE_STATUS,RECKON_STATUS,RECKON_DATE,UPDATE_CODE,UPDATE_DATE,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15},{16},'{17}','{18}','{19}','{20}','{21}','{22}')",
                   row["BUSI_ACT_ID"], row["BUSI_BILL_DETAIL_ID"], row["BUSI_BILL_ID"], row["RELATE_BUSI_BILL_ID"], row["STORE_BILL_ID"], row["BRAND_N"], row["BRAND_NAME"], row["QUANTITY"],
                   row["DIST_CTR_CODE"], row["ORG_CODE"], row["STORE_ROOM_CODE"], row["STORE_PLACE_CODE"], row["TARGET_NAME"], row["IN_OUT_TYPE"], row["BILL_TYPE"],
                   row["BEGIN_STOCK_QUANTITY"], row["END_STOCK_QUANTITY"], row["DISUSE_STATUS"], row["RECKON_STATUS"], row["RECKON_DATE"], row["UPDATE_CODE"], row["UPDATE_DATE"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
            sql = "update  DWV_IWMS_OUT_BUSI_BILL set STORE_ROOM_CODE='1002',STORE_PLACE_CODE='10002' ";
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改出库业务单据表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateOutBusi(string outBusiCode)
        {
            string sql = "UPDATE DWV_IWMS_OUT_BUSI_BILL SET IS_IMPORT='1' WHERE busi_bill_id in (SELECT busi_bill_id FROM V_DWV_IWMS_OUT_BUSI_BILL WHERE IS_IMPORT ='0')";
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询同步状态表数据，上报中烟


        /// <summary>
        /// 查询同步状态表【DWV_IOUT_SYNCHRO_INFO】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QuerySynchroInfo()
        {
            string sql = "SELECT * FROM DWV_IOUT_SYNCHRO_INFO WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        public DataTable QuerySynchroInfo( string begintime,string endtime)
        {
            string sql = "SELECT * FROM DWV_IOUT_SYNCHRO_INFO WHERE UPDATE_DATE>='"+Convert.ToDateTime(begintime)+"'AND UPDATE_DATE<='"+Convert.ToDateTime(endtime)+"'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入同步状态表【DWV_IOUT_SYNCHRO_INFO】
        /// </summary>
        /// <param name="synchroTable"></param>
        public void InsertSynchro(DataTable synchroTable)
        {
            DataTable custCode = this.ExecuteQuery("SELECT SYNC_TYPE_CODE FROM DWV_IOUT_SYNCHRO_INFO ").Tables[0];
            string cust_code = "''";
            string sql;
            for (int i = 0; i < custCode.Rows.Count; i++)
            {
                cust_code += ",'" + custCode.Rows[i]["SYNC_TYPE_CODE"] + "'";
            }
            foreach (DataRow row in synchroTable.Rows)
            {
                if (cust_code != "''" && cust_code.Contains(row["SYNC_TYPE_CODE"].ToString()))
                {
                    sql = string.Format("update DWV_IOUT_SYNCHRO_INFO SET UPDATE_DATE='{0}'", row["UPDATE_DATE"]);
                    this.ExecuteNonQuery(sql);
                }
                else
                {
                    sql = string.Format("INSERT INTO DWV_IOUT_SYNCHRO_INFO(SYNC_TYPE_CODE,SYNC_TYPE_NAME,UPDATE_DATE,IS_IMPORT,REMARK)VALUES('{0}','{1}','{2}','{3}','{4}')",
                        row["SYNC_TYPE_CODE"], row["SYNC_TYPE_NAME"], row["UPDATE_DATE"], row["IS_IMPORT"], row["REMARK"]);
                    this.ExecuteNonQuery(sql);
                }
            }
        }

        /// <summary>
        /// 修改同步状态表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateSyachro(string syachroCode)
        {
            string dtOrder = DateTime.Now.AddDays(-30d).ToString("yyyyMMdd");
            string sql = string.Format("UPDATE DWV_IOUT_SYNCHRO_INFO SET IS_IMPORT='1' WHERE IS_IMPORT='0'");
            this.ExecuteNonQuery(sql);
            sql = string.Format("DELETE FROM DWV_IOUT_SYNCHRO_INFO");
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询分拣订单主表数据，上报中烟


        /// <summary>
        /// 查询分拣订单主表【DWV_OUT_ORDER】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryIordMasterOrder()
        {
            string sql = "SELECT * FROM V_WMS_SORT_ORDER WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入分拣订单主表【DWV_IORD_ORDER】，中烟数据库
        /// </summary>
        /// <param name="orderMasterTable"></param>
        public void InsertIordOrder(DataTable orderMasterTable)
        {
            foreach (DataRow row in orderMasterTable.Rows)
            {
                string sql = string.Format("INSERT INTO DWV_IORD_ORDER(ORDER_ID,ORG_CODE,SALE_REG_CODE,ORDER_DATE,ORDER_TYPE," +
                    "CUST_CODE,CUST_NAME,QUANTITY_SUM,AMOUNT_SUM,DETAIL_NUM,DELIVER_ORDER,ISACTIVE,UPDATE_DATE,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9},{10},'{11}','{12}','{13}')",
                    row["ORDER_ID"], row["ORG_CODE"], row["SALE_REG_CODE"], row["ORDER_DATE"], row["ORDER_TYPE"], row["CUST_N"], row["CUST_NAME"],
                    row["QUANTITY_SUM"], row["AMOUNT_SUM"], row["DETAIL_NUM"], row["DELIVER_ORDER"], row["ISACTIVE"], row["UPDATE_DATE"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }


        /// <summary>
        /// 修改分拣订单主表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateOrderMaster(string orderMasterCode)
        {
            string sql = "UPDATE DWV_OUT_ORDER SET IS_IMPORT='1' WHERE IS_IMPORT='0'";
            this.ExecuteNonQuery(sql);
        }
        #endregion


        #region 查询分拣订单细表数据，上报中烟

        /// <summary>
        /// 查询分拣订单细表【DWV_OUT_ORDER_DETAIL】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryIordDetailOrder()
        {
            string sql = "SELECT * FROM V_WMS_SORT_ORDER_DETAIL WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入分拣订单细表【DWV_IORD_ORDER_DETAIL】，中烟数据库
        /// </summary>
        /// <param name="orderDetailTable"></param>
        public void InsertIordOrderDetail(DataTable orderDetailTable)
        {
            foreach (DataRow row in orderDetailTable.Rows)
            {
                string sql = string.Format("INSERT INTO DWV_IORD_ORDER_DETAIL(ORDER_DETAIL_ID,ORDER_ID,BRAND_CODE,BRAND_NAME,BRAND_UNIT_NAME," +
                    "QTY_DEMAND,QUANTITY,PRICE,AMOUNT,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8},'{9}')",
                    row["ORDER_DETAIL_ID"], row["ORDER_ID"], row["BRAND_CODE"], row["BRAND_NAME"], row["BRAND_UNIT_NAME"], row["QTY_DEMAND"], row["QUANTITY"],
                    row["PRICE"], row["AMOUNT"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }


        /// <summary>
        /// 修改分拣订单细表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateOrderDetail(string orderDetailCode)
        {
            string sql = "UPDATE DWV_OUT_ORDER_DETAIL SET IS_IMPORT='1' WHERE IS_IMPORT='0'";
            this.ExecuteNonQuery(sql);
        }


        #endregion


        #region 查询分拣情况表数据，上报中烟


        /// <summary>
        /// 查询分拣情况表【DWV_IORD_SORT_STATUS】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QuerySortStatus()
        {
            string sql = "SELECT * FROM V_DWV_IORD_SORT_STATUS WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入分拣情况表【DWV_IORD_SORT_STATUS】，中烟数据库
        /// </summary>
        /// <param name="orderDetailTable"></param>
        public void InsertSortStatus(DataTable sortStatusTable)
        {
            foreach (DataRow row in sortStatusTable.Rows)
            {
                string sql = string.Format("INSERT INTO DWV_IORD_SORT_STATUS(SORT_BILL_ID,ORG_CODE,SORTING_CODE,SORT_DATE,SORT_SPEC," +
                    "SORT_QUANTITY,SORT_ORDER_NUM,SORT_BEGIN_DATE,SORT_END_DATE,SORT_COST_TIME,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}',{4},{5},{6},'{7}','{8}',{9},'{10}')",
                    row["SORT_BILL_ID"], row["ORG_CODE"], row["SORTING_CODE"], row["SORT_DATE"], row["SORT_SPEC"], row["SORT_QUANTITY"], row["SORT_ORDER_NUM"],
                    row["SORT_BEGIN_DATE"], row["SORT_END_DATE"], row["SORT_COST_TIME"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 修改分拣情况表信息上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateSortStatus(string sortStatusCode)
        {
            string sql = "UPDATE DWV_IORD_SORT_STATUS SET IS_IMPORT='1' WHERE IS_IMPORT='0'";
            this.ExecuteNonQuery(sql);
        }


        #endregion


        #region 查询分拣线信息表数据，上报中烟

        /// <summary>
        /// 查询分拣线信息表【DWV_IDPS_SORTING】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryIdpsSorting()
        {
            string sql = "SELECT * FROM V_DWV_IDPS_SORTING WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入分拣线信息表【DWV_IORD_SORT_STATUS】，中烟数据库
        /// </summary>
        /// <param name="orderDetailTable"></param>
        public void InsertIdpsSorting(DataTable SortingTable)
        {
            foreach (DataRow row in SortingTable.Rows)
            {
                string sql = string.Format("INSERT INTO DWV_IDPS_SORTING(SORTING_CODE,SORTING_NAME,SORTING_TYPE,ISACTIVE,UPDATE_DATE," +
                    "IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                    row["SORTING_CODE"], row["SORTING_NAME"], row["SORTING_TYPE"], row["ISACTIVE"], row["UPDATE_DATE"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 修改分拣线信息表上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateSorting(string sortingCode)
        {
            string sql = string.Format("UPDATE DWV_DPS_SORTING SET IS_IMPORT='1' WHERE IS_IMPORT='0'");
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询仓储属性表

        /// <summary>
        /// 查询仓储属性表【DWV_IBAS_STORAGE】，上报中烟
        /// </summary>
        /// <returns></returns>
        public DataTable QueryIbasSorting()
        {
            string sql = "SELECT * FROM DWV_IBAS_STORAGE WHERE IS_IMPORT ='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入仓储属性表【DWV_IORD_SORT_STATUS】，中烟数据库
        /// </summary>
        /// <param name="orderDetailTable"></param>
        public void InsertIbasSorting(DataTable IbasSortingTable)
        {
            foreach (DataRow row in IbasSortingTable.Rows)
            {
                string date = Convert.ToDateTime(row["UPDATE_DATE"]).ToString("yyyyMMddHHmmss");
                string sql = string.Format("INSERT INTO DWV_IBAS_STORAGE(STORAGE_CODE,STORAGE_TYPE,ORDER_NUM,CONTAINER,STORAGE_NAME,UP_CODE,DIST_CTR_CODE,N_ORG_CODE,N_STORE_ROOM_CODE,CAPACITY," +
                    "HORIZONTAL_NUM,VERTICAL_NUM,AREA_TYPE,UPDATE_DATE,ISACTIVE,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},{10},{11},'{12}','{13}','{14}','{15}')",
                    row["STORAGE_CODE"], row["STORAGE_TYPE"], row["ORDER_NUM"], row["CONTAINER"], row["STORAGE_NAME"], row["UP_CODE"], row["DIST_CTR_CODE"], row["N_ORG_CODE"], row["N_STORE_ROOM_CODE"], row["CAPACITY"], row["HORIZONTAL_NUM"], row["VERTICAL_NUM"], row["AREA_TYPE"],
                    date, row["ISACTIVE"], row["IS_IMPORT"]);
                this.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 修改仓储属性表上报状态
        /// </summary>
        /// <param name="productCode"></param>
        public void UpdateIbsaSorting(string IbasSortingCode)
        {
            string sql = string.Format("UPDATE DWV_IBAS_STORAGE SET IS_IMPORT='1' WHERE IS_IMPORT='0'");
            this.ExecuteNonQuery(sql);
        }

        #endregion


        #region 查询其他数据

        /// <summary>
        /// 给同步表中插入数据
        /// </summary>
        /// <param name="syncCode"></param>
        /// <param name="syncName"></param>
        public void InsertSynchroInfo(string syncCode, string syncName)
        {
            string sql = string.Format("INSERT INTO DWV_IOUT_SYNCHRO_INFO([SYNC_TYPE_CODE],[SYNC_TYPE_NAME],[DESCRIPTION],[UPDATE_DATE],[IS_IMPORT],[REMARK]) " +
            "VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
              syncCode, syncName, " ", DateTime.Now.ToString("yyyyMMddHHmmss"), "0", " ");
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 根据单据号查询日结状态
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public string GetOrderCodeByDayStatus(string orderCode)
        {
            string sql = string.Format("{0}", orderCode);
            return Convert.ToString(this.ExecuteScalar(sql).ToString());
        }

        /// <summary>
        /// 查询货位表业务库存件数量
        /// </summary>
        /// <returns></returns>
        public DataTable QueryCellQuantity()
        {
            string sql = @"SELECT '01' AS ORG_CODE,CURRENTPRODUCT,SUM(QTY_STA) AS QUANTITY
                            FROM V_WMS_WH_CELL WHERE CURRENTPRODUCT IS NOT NULL GROUP BY CURRENTPRODUCT";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询货位表业务库存条数量
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public DataTable QueryCellTiao(string product)
        {
            string sql = string.Format("SELECT ISNULL(SUM(QUANTITY),0) AS QUANTITY FROM  WMS_WH_CELL WHERE CURRENTPRODUCT='{0}' AND AREATYPE='1'", product);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 插入数据到业务库存表
        /// </summary>
        /// <param name="busiTable"></param>
        public void InsertBusiStockQuntity(DataTable busiTable)
        {
            this.BatchInsert(busiTable, "DWV_IWMS_BUSI_STOCK");
        }

        /// <summary>
        /// 获取配送中心编码
        /// </summary>
        /// <returns></returns>
        public string GetCompany()
        {
            //string sql = "SELECT DIST_CTR_CODE FROM DWV_OUT_DIST_CTR";
            //return this.ExecuteScalar(sql).ToString();
            return "0101";
        }

        /// <summary>
        /// 支转换为件的换算
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public DataTable ProductRate(string productCode)
        {
            string sql = "SELECT A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE," +
                "(SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.JIANCODE=B.UNITCODE AND A.PRODUCTCODE='" + productCode + "') AS JIANRATE," +
                "(SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.TIAOCODE=B.UNITCODE AND A.PRODUCTCODE='" + productCode + "') AS TIAORATE " +
                "FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE  A.PRODUCTCODE='" + productCode + "' GROUP BY A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE";
            return this.ExecuteQuery(sql).Tables[0];
        }

        #endregion

        #region 上报数据
        /// <summary>
        /// 插入组织结构表【DWV_IORG_ORGANIZATION】，中烟数据库
        /// </summary>
        /// <param name="organTable"></param>
        public void InsertCompany(DataSet organSet)
        {
            DataTable organTable = organSet.Tables["wms_company"];
            DataTable custCode = this.ExecuteQuery("SELECT ORGANIZATION_CODE FROM DWV_IORG_ORGANIZATION ").Tables[0];
            string cust_code = "''";
            string sql;
            string date = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss");
            for (int i = 0; i < custCode.Rows.Count; i++)
            {
                cust_code += ",'" + custCode.Rows[i]["ORGANIZATION_CODE"] + "'";
            }
            foreach (DataRow row in organTable.Rows)
            {
                if (cust_code != "''" && cust_code.Contains(row["company_code"].ToString()))
                {
                    sql = string.Format("update DWV_IORG_ORGANIZATION SET ORGANIZATION_CODE='{0}',ORGANIZATION_NAME='{1}',ORGANIZATION_TYPE='{2}',UP_CODE='{3}'" +
                    " ,N_ORGANIZATION_CODE='{4}',STORE_ROOM_AREA={5},STORE_ROOM_NUM={6},STORE_ROOM_CAPACITY={7},SORTING_NUM={8},UPDATE_DATE='{9}',ISACTIVE='{10}',IS_IMPORT='{11}')" 
                   , row["company_code"], row["company_name"], row["company_type"], row["parent_company_id"],
                    row["uniform_code"], row["warehouse_space"], row["warehouse_count"], row["warehouse_capacity"], row["sorting_count"],
                    date, row["is_active"],'0');
                    this.ExecuteNonQuery(sql);
                }
                else
                {
                    sql = string.Format("INSERT INTO DWV_IORG_ORGANIZATION(ORGANIZATION_CODE,ORGANIZATION_NAME,ORGANIZATION_TYPE,UP_CODE" +
                   " ,N_ORGANIZATION_CODE,STORE_ROOM_AREA,STORE_ROOM_NUM,STORE_ROOM_CAPACITY,SORTING_NUM,UPDATE_DATE,ISACTIVE,IS_IMPORT)" +
                   "VALUES('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8},'{9}','{10}','{11}')", row["company_code"], row["company_name"], row["company_type"], row["parent_company_id"],
                   row["uniform_code"] ?? "", row["warehouse_space"], row["warehouse_count"], row["warehouse_capacity"], row["sorting_count"],
                   date, row["is_active"], row["IS_IMPORT"]);
                    this.ExecuteNonQuery(sql);
                }
            }
            }
        /// <summary>
        /// 员工信息上报
        /// </summary>
        /// <param name="employeeSet"></param>
        public void InsertEmployee(DataSet employeeSet)
        {
            DataTable employeeTable = employeeSet.Tables["wms_employee"];
            DataTable custCode = this.ExecuteQuery("SELECT PERSON_CODE FROM DWV_IORG_PERSON ").Tables[0];
            string cust_code = "''";
            string sql;
            string date = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss");
            for (int i = 0; i < custCode.Rows.Count; i++)
            {
                cust_code += ",'" + custCode.Rows[i]["PERSON_CODE"] + "'";
            }
            foreach (DataRow row in employeeTable.Rows)
            {
                if (cust_code != "''" && cust_code.Contains(row["employee_code"].ToString()))
                {
                     sql = string.Format("UPDATE DWV_IORG_PERSON SET PERSON_CODE='{0}',PERSON_N='{1}',PERSON_NAME='{2}',SEX='{3}'," +
                    " UPDATE_DATE='{4}',ISACTIVE='{5}',IS_IMPORT='{6}'", row["employee_code"],
                    row["employee_no"], row["employee_name"], row["sex"], date, row["is_active"],'0');
                    this.ExecuteNonQuery(sql);
                }
                else
                {
                     sql = string.Format("INSERT INTO DWV_IORG_PERSON(PERSON_CODE,PERSON_N,PERSON_NAME,SEX," +
                    " UPDATE_DATE,ISACTIVE,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", row["employee_code"],
                    row["employee_code"], row["employee_name"], row["sex"], date, row["is_active"],'0');
                    this.ExecuteNonQuery(sql);
                }
            }
        }

        /// <summary>
        /// 仓储信息上报
        /// </summary>
        /// <param name="employeeSet"></param>
        public void InsertCell(DataSet cellSet)
        {
            DataTable cellTable = cellSet.Tables["wms_cell"];
            DataTable custCode = this.ExecuteQuery("SELECT STORAGE_CODE FROM DWV_IBAS_STORAGE ").Tables[0];
            string cust_code = "''";
            string sql;
            string date = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss");
            for (int i = 0; i < custCode.Rows.Count; i++)
            {
                cust_code += ",'" + custCode.Rows[i]["STORAGE_CODE"] + "'";
            }
            foreach (DataRow row in cellTable.Rows)
            {
                if (cust_code != "''" && cust_code.Contains(row["employee_code"].ToString()))
                {
                    sql = string.Format("UPDATE DWV_IBAS_STORAGE SET STORAGE_CODE='{0}',STORAGE_TYPE='{1}',ORDER_NUM='{2}',CONTAINER='{3}',STORAGE_NAME='{4}',UP_CODE='{5}',DIST_CTR_CODE='{6}',N_ORG_CODE='{7}',N_STORE_ROOM_CODE='{8}',CAPACITY='{9}'," +
                 "HORIZONTAL_NUM='{10}',VERTICAL_NUM='{11}',AREA_TYPE='{12}',UPDATE_DATE='{13}',ISACTIVE='{14}',IS_IMPORT='{15}'",
                 row["STORAGE_CODE"], row["STORAGE_TYPE"], row["ORDER_NUM"], row["CONTAINER"], row["STORAGE_NAME"], row["UP_CODE"], row["DIST_CTR_CODE"], row["N_ORG_CODE"], row["N_STORE_ROOM_CODE"], row["CAPACITY"], row["HORIZONTAL_NUM"], row["VERTICAL_NUM"], row["AREA_TYPE"],
                 date, row["ISACTIVE"],'0');
                    this.ExecuteNonQuery(sql);
                }
                else
                {
                    sql = string.Format("INSERT INTO DWV_IBAS_STORAGE(STORAGE_CODE,STORAGE_TYPE,ORDER_NUM,CONTAINER,STORAGE_NAME,UP_CODE,DIST_CTR_CODE,N_ORG_CODE,N_STORE_ROOM_CODE,CAPACITY," +
                 "HORIZONTAL_NUM,VERTICAL_NUM,AREA_TYPE,UPDATE_DATE,ISACTIVE,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},{10},{11},'{12}','{13}','{14}','{15}')",
                 row["STORAGE_CODE"], row["STORAGE_TYPE"], row["ORDER_NUM"], row["CONTAINER"], row["STORAGE_NAME"], row["UP_CODE"], row["DIST_CTR_CODE"], row["N_ORG_CODE"], row["N_STORE_ROOM_CODE"], row["CAPACITY"], row["HORIZONTAL_NUM"], row["VERTICAL_NUM"], row["AREA_TYPE"],
                 date, row["ISACTIVE"],'0');
                    this.ExecuteNonQuery(sql);
                }
            }
        }
        /// <summary>
        /// 仓库库存表
        /// </summary>
        /// <param name="stockTable"></param>
        public void InsertStoreStock(DataSet stockSet)
        {
            DataTable stockTable = stockSet.Tables["wms_storage"];
            string sql = "DELETE FROM DWV_IWMS_STORE_STOCK";
            this.ExecuteNonQuery(sql);
            foreach (DataRow row in stockTable.Rows)
            {
                sql = string.Format("INSERT INTO DWV_IWMS_STORE_STOCK(STORE_PLACE_CODE,BRAND_CODE,AREA_TYPE,BRAND_BATCH,DIST_CTR_CODE,QUANTITY,IS_IMPORT)VALUES('{0}','{1}','{2}','{3}','{4}',{5},'{6}')",
                 row["cell_code"], row["product_code"], row["area_type"], row["brand_batch"], row["dist_ctr_code"], row["quantity"],'0');
                this.ExecuteNonQuery(sql);
            }
        }
        /// <summary>
        /// 业务库存表
        /// </summary>
        /// <param name="stockTable"></param>
        public void InsertBustStock(DataSet stockSet)
        {
            DataTable stockTable = stockSet.Tables["wms_busistorage"];
            string sql = "DELETE FROM DWV_IWMS_BUSI_STOCK";
            this.ExecuteNonQuery(sql);
            foreach (DataRow row in stockTable.Rows)
            {

                sql = string.Format("INSERT INTO DWV_IWMS_BUSI_STOCK(ORG_CODE,BRAND_CODE,DIST_CTR_CODE,QUANTITY,IS_IMPORT)VALUES('{0}','{1}','{2}',{3},'{4}')",
                 row["ORG_CODE"], row["BRAND_CODE"],row["DIST_CTR_CODE"], row["QUANTITY"],'0');

                this.ExecuteNonQuery(sql);
            }
        }

        #endregion
    }
}
