using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
using THOK.WMS.DownloadWms.Dao;
using THOK.WMS.Upload.Bll;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownProductBll
    {
        UploadBll upload = new UploadBll();
        #region ��Ӫϵͳ���²�Ʒ��Ϣ

        /// <summary>
        /// ���ز�Ʒ��Ϣ ͬʱ�ϱ�
        /// </summary>
        /// <returns></returns>
        public bool DownProductInfo()
        {
            bool tag = true;
            try
            {
                DataTable codedt = this.GetProductCode();
                string codeList = UtinString.MakeString(codedt, "custom_code");
                codeList = "BRANDCODE NOT IN (" + codeList + ")";
                DataTable bradCodeTable = this.GetProductInfo("1=1");
                DataRow[] bradCodeDr = bradCodeTable.Select(codeList);
                if (bradCodeDr.Length > 0)
                {
                    DataSet brandCodeDs = this.Insert(bradCodeDr);
                    this.Insert(brandCodeDs);
                    //�ϱ�����
                    //upload.InsertProduct(brandCodeDs);
                }
                else
                    tag = false;
            }
            catch (Exception e)
            {
                throw new Exception("���ؾ���ʧ�ܣ�ԭ��" + e.Message);
            }
            return tag;
        }
        /// <summary>
        /// ���ز�Ʒ��Ϣ���� ͬʱ�ϱ�
        /// </summary>
        /// <returns></returns>
        public bool DownProductInfos()
        {
            bool tag = true;
            DataTable codedt = this.GetProductCode();
            string codeList = UtinString.MakeString(codedt, "custom_code");
            codeList = "BRAND_CODE NOT IN (" + codeList + ")";
            DataTable bradCodeTable = this.GetProductInfo(codeList);
            if (bradCodeTable.Rows.Count > 0)
            {
                DataSet brandCodeDs = this.Inserts(bradCodeTable);
                DataSet brandCode = this.InsertsUpload(bradCodeTable);
                this.Insert(brandCodeDs);
                //�ϱ�����
                upload.InsertProduct(brandCode);
            }
            else
            {
                tag = false;
            }
            return tag;
        }
        /// <summary>
        /// ��ѯ���̲�Ʒ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductCode()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownProductDao dao = new DownProductDao();
                return dao.GetProductCode();
            }
        }

        /// <summary>
        /// ���ؾ��̲�Ʒ��Ϣ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductInfo(string codeList)
        {
            using (PersistentManager dbPm = new PersistentManager("YXConnection"))
            {
                DownProductDao dao = new DownProductDao();
                dao.SetPersistentManager(dbPm);
                return dao.GetProductInfo(codeList);
            }
        }

        /// <summary>
        /// ���ݾ��̱����ѯ������λ��Ϣ
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public DataTable FindUnitListCode(string product)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                return dao.FindUnitListCOde(product);
            }
        }

        /// <summary>
        /// �������ݵ������
        /// </summary>
        /// <param name="brandTable"></param>
        /// <returns></returns>
        public DataSet Insert(DataRow[] brandTable)
        {
            DownUnitBll bll = new DownUnitBll();
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in brandTable)
            {
                DataTable ulistCodeTable = this.FindUnitListCode(row["BRANDCODE"].ToString().Trim());
                DataRow inbrddr = ds.Tables["WMS_PRODUCT"].NewRow();
                inbrddr["product_code"] = row["BRANDCODE"];
                inbrddr["product_name"] = row["BRAND_NAME"];
                inbrddr["uniform_code"] = row["N_UNIFY_CODE"];
                inbrddr["custom_code"] = row["BRAND_CODE"];
                inbrddr["short_code"] = row["SHORT_CODE"];
                inbrddr["unit_list_code"] = ulistCodeTable.Rows[0]["unit_list_code"].ToString();
                inbrddr["unit_code"] = ulistCodeTable.Rows[0]["unit_code01"].ToString();
                inbrddr["supplier_code"] = "10863500";
                inbrddr["brand_code"] = "PP001";
                inbrddr["abc_type_code"] = "";
                inbrddr["product_type_code"] = row["BRAND_TYPE"];
                inbrddr["pack_type_code"] = "";
                inbrddr["price_level_code"] = "";
                inbrddr["statistic_type"] = "";
                inbrddr["piece_barcode"] = row["BARCODE_PIECE"];
                inbrddr["bar_barcode"] = "";//row["BARCODE_BAR"];
                inbrddr["package_barcode"] = row["BARCODE_PACKAGE"];
                inbrddr["one_project_barcode"] = row["BARCODE_ONE_PROJECT"];
                inbrddr["buy_price"] = row["BUY_PRICE"];
                inbrddr["trade_price"] = row["TRADE_PRICE"];
                inbrddr["retail_price"] = row["RETAIL_PRICE"];
                inbrddr["cost_price"] = row["COST_PRICE"];
                inbrddr["is_filter_tip"] = row["IS_FILTERTIP"];
                inbrddr["is_new"] = row["IS_NEW"];
                inbrddr["is_famous"] = row["IS_FAMOUS"];
                inbrddr["is_main_product"] = row["IS_MAINPRODUCT"];
                inbrddr["is_province_main_product"] = row["IS_MAINPROVINCE"];
                inbrddr["belong_region"] = row["BELONG_REGION"];
                inbrddr["is_confiscate"] = row["IS_CONFISCATE"];
                inbrddr["is_abnormity"] = row["IS_ABNORMITY_BRAND"];
                inbrddr["description"] = "";
                inbrddr["is_active"] = row["IsActive"];
                inbrddr["update_time"] = DateTime.Now;
                inbrddr["is_rounding"] = "1";
                inbrddr["cell_max_product_quantity"] = "30";
                inbrddr["point_area_codes"] = "";
                ds.Tables["WMS_PRODUCT"].Rows.Add(inbrddr);
            }
            return ds;
        }
        /// <summary>
        /// �������ݵ������ ����
        /// </summary>
        /// <param name="brandTable"></param>
        /// <returns></returns>
        public DataSet Inserts(DataTable brandTable)
        {
            DownUnitBll bll = new DownUnitBll();
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in brandTable.Rows)
            {
                DataTable ulistCodeTable = this.FindUnitListCode(row["BRAND_ULIST_CODE"].ToString().Trim());
                DataRow inbrddr = ds.Tables["WMS_PRODUCT"].NewRow();
                inbrddr["product_code"] = row["BRAND_N"];
                inbrddr["product_name"] = row["BRAND_NAME"];
                inbrddr["uniform_code"] = row["N_BRAND_CODE"];
                inbrddr["custom_code"] = row["BRAND_CODE"];
                inbrddr["short_code"] = row["SHORT_CODE"];
                inbrddr["unit_list_code"] = row["BRAND_ULIST_CODE"].ToString();
                inbrddr["unit_code"] = ulistCodeTable.Rows[0]["unit_code01"].ToString();
                inbrddr["supplier_code"] = "10863500";
                inbrddr["brand_code"] = "PP001";
                inbrddr["abc_type_code"] = "";
                inbrddr["product_type_code"] = row["BRAND_TYPE"];
                inbrddr["pack_type_code"] = "";
                inbrddr["price_level_code"] = "";
                inbrddr["statistic_type"] = "";
                inbrddr["piece_barcode"] = row["BARCODE_PIECE"];
                inbrddr["bar_barcode"] = row["BARCODE_BAR"];
                inbrddr["package_barcode"] = row["BARCODE_PACKAGE"];
                inbrddr["one_project_barcode"] = row["BARCODE_ONE_PROJECT"];
                inbrddr["buy_price"] = string.IsNullOrEmpty(row["BUY_PRICE"].ToString()) ? 0 : row["BUY_PRICE"];
                inbrddr["trade_price"] = string.IsNullOrEmpty(row["TRADE_PRICE"].ToString()) ? 0 : row["TRADE_PRICE"];
                inbrddr["retail_price"] = string.IsNullOrEmpty(row["RETAIL_PRICE"].ToString()) ? 0 : row["RETAIL_PRICE"];
                inbrddr["cost_price"] = string.IsNullOrEmpty(row["COST_PRICE"].ToString()) ? 0 : row["COST_PRICE"];
                inbrddr["is_filter_tip"] = row["IS_FILTERTIP"];
                inbrddr["is_new"] = row["IS_NEW"];
                inbrddr["is_famous"] = row["IS_FAMOUS"];
                inbrddr["is_main_product"] = row["IS_MAINPRODUCT"];
                inbrddr["is_province_main_product"] = row["IS_MAINPROVINCE"];
                inbrddr["belong_region"] = row["BELONG_REGION"];
                inbrddr["is_confiscate"] = row["IS_CONFISCATE"];
                inbrddr["is_abnormity"] = row["IS_ABNORMITY_BRAND"];
                inbrddr["description"] = "";
                inbrddr["is_active"] = row["IsActive"];
                inbrddr["update_time"] = DateTime.Now;
                ds.Tables["WMS_PRODUCT"].Rows.Add(inbrddr);
            }
            return ds;
        }
        /// <summary>
        /// �������ݵ������ ����
        /// </summary>
        /// <param name="brandTable"></param>
        /// <returns></returns>
        public DataSet InsertsUpload(DataTable brandTable)
        {
            DownUnitBll bll = new DownUnitBll();
            DataSet ds = this.GenerateEmptyTable();
            foreach (DataRow row in brandTable.Rows)
            {
                DataTable ulistCodeTable = this.FindUnitListCode(row["BRAND_ULIST_CODE"].ToString().Trim());
                DataRow inbrddr = ds.Tables["WMS_PRODUCT"].NewRow();
                inbrddr["product_code"] = row["BRAND_N"];
                inbrddr["product_name"] = row["BRAND_NAME"];
                inbrddr["uniform_code"] = row["N_BRAND_CODE"];
                inbrddr["custom_code"] = row["BRAND_CODE"];
                inbrddr["short_code"] = row["SHORT_CODE"];
                inbrddr["unit_list_code"] = row["BRAND_ULIST_CODE"].ToString();
                inbrddr["unit_code"] = ulistCodeTable.Rows[0]["unit_code01"].ToString();
                inbrddr["supplier_code"] = "10863500";
                inbrddr["brand_code"] = "PP001";
                inbrddr["abc_type_code"] = "";
                inbrddr["product_type_code"] = row["BRAND_TYPE"];
                inbrddr["pack_type_code"] = "";
                inbrddr["price_level_code"] = "";
                inbrddr["statistic_type"] = "";
                inbrddr["piece_barcode"] = row["BARCODE_PIECE"];
                inbrddr["bar_barcode"] = row["BARCODE_BAR"];
                inbrddr["package_barcode"] = row["BARCODE_PACKAGE"];
                inbrddr["one_project_barcode"] = row["BARCODE_ONE_PROJECT"];
                inbrddr["buy_price"] = string.IsNullOrEmpty(row["BUY_PRICE"].ToString()) ? 0 : row["BUY_PRICE"];
                inbrddr["trade_price"] = string.IsNullOrEmpty(row["TRADE_PRICE"].ToString()) ? 0 : row["TRADE_PRICE"];
                inbrddr["retail_price"] = string.IsNullOrEmpty(row["RETAIL_PRICE"].ToString()) ? 0 : row["RETAIL_PRICE"];
                inbrddr["cost_price"] = string.IsNullOrEmpty(row["COST_PRICE"].ToString()) ? 0 : row["COST_PRICE"];
                inbrddr["qtyUnit"] = row["QTY_UNIT"];
                inbrddr["is_filter_tip"] = row["IS_FILTERTIP"];
                inbrddr["is_new"] = row["IS_NEW"];
                inbrddr["is_famous"] = row["IS_FAMOUS"];
                inbrddr["is_main_product"] = row["IS_MAINPRODUCT"];
                inbrddr["is_province_main_product"] = row["IS_MAINPROVINCE"];
                inbrddr["belong_region"] = row["BELONG_REGION"];
                inbrddr["is_confiscate"] = row["IS_CONFISCATE"];
                inbrddr["is_abnormity"] = row["IS_ABNORMITY_BRAND"];
                inbrddr["description"] = "";
                inbrddr["is_active"] = row["IsActive"];
                inbrddr["update_time"] = DateTime.Now;
                ds.Tables["WMS_PRODUCT"].Rows.Add(inbrddr);
            }
            return ds;
        }
        /// <summary>
        /// �������������
        /// </summary>
        /// <returns></returns>
        private DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable inbrtable = ds.Tables.Add("WMS_PRODUCT");
            inbrtable.Columns.Add("product_code");
            inbrtable.Columns.Add("product_name");
            inbrtable.Columns.Add("uniform_code");
            inbrtable.Columns.Add("custom_code");
            inbrtable.Columns.Add("short_code");
            inbrtable.Columns.Add("unit_list_code");
            inbrtable.Columns.Add("unit_code");
            inbrtable.Columns.Add("supplier_code");
            inbrtable.Columns.Add("brand_code");
            inbrtable.Columns.Add("abc_type_code");
            inbrtable.Columns.Add("product_type_code");
            inbrtable.Columns.Add("pack_type_code");
            inbrtable.Columns.Add("price_level_code");
            inbrtable.Columns.Add("statistic_type");
            inbrtable.Columns.Add("piece_barcode");
            inbrtable.Columns.Add("bar_barcode");
            inbrtable.Columns.Add("package_barcode");
            inbrtable.Columns.Add("one_project_barcode");
            inbrtable.Columns.Add("buy_price");
            inbrtable.Columns.Add("trade_price");
            inbrtable.Columns.Add("retail_price");
            inbrtable.Columns.Add("cost_price");
            inbrtable.Columns.Add("is_filter_tip");
            inbrtable.Columns.Add("is_new");
            inbrtable.Columns.Add("is_famous");//һ�Ź���������
            inbrtable.Columns.Add("is_main_product");
            inbrtable.Columns.Add("is_province_main_product");
            inbrtable.Columns.Add("belong_region");
            inbrtable.Columns.Add("is_confiscate");
            inbrtable.Columns.Add("is_abnormity");
            inbrtable.Columns.Add("description");
            inbrtable.Columns.Add("is_active");
            inbrtable.Columns.Add("update_time");
            inbrtable.Columns.Add("is_rounding");
            inbrtable.Columns.Add("cell_max_product_quantity");
            inbrtable.Columns.Add("point_area_codes");
            return ds;
        }
        /// <summary>
        /// ������������� �ϱ���
        /// </summary>
        /// <returns></returns>
        private DataSet GenerateEmptyTable()
        {
            DataSet ds = new DataSet();
            DataTable inbrtable = ds.Tables.Add("WMS_PRODUCT");
            inbrtable.Columns.Add("product_code");
            inbrtable.Columns.Add("product_name");
            inbrtable.Columns.Add("uniform_code");
            inbrtable.Columns.Add("custom_code");
            inbrtable.Columns.Add("short_code");
            inbrtable.Columns.Add("unit_list_code");
            inbrtable.Columns.Add("unit_code");
            inbrtable.Columns.Add("supplier_code");
            inbrtable.Columns.Add("brand_code");
            inbrtable.Columns.Add("abc_type_code");
            inbrtable.Columns.Add("product_type_code");
            inbrtable.Columns.Add("pack_type_code");
            inbrtable.Columns.Add("price_level_code");
            inbrtable.Columns.Add("statistic_type");
            inbrtable.Columns.Add("piece_barcode");
            inbrtable.Columns.Add("bar_barcode");
            inbrtable.Columns.Add("package_barcode");
            inbrtable.Columns.Add("one_project_barcode");
            inbrtable.Columns.Add("buy_price");
            inbrtable.Columns.Add("trade_price");
            inbrtable.Columns.Add("retail_price");
            inbrtable.Columns.Add("cost_price");
            inbrtable.Columns.Add("qtyUnit");
            inbrtable.Columns.Add("is_filter_tip");
            inbrtable.Columns.Add("is_new");
            inbrtable.Columns.Add("is_famous");//һ�Ź���������
            inbrtable.Columns.Add("is_main_product");
            inbrtable.Columns.Add("is_province_main_product");
            inbrtable.Columns.Add("belong_region");
            inbrtable.Columns.Add("is_confiscate");
            inbrtable.Columns.Add("is_abnormity");
            inbrtable.Columns.Add("description");
            inbrtable.Columns.Add("is_active");
            inbrtable.Columns.Add("update_time");

            return ds;
        }
        /// <summary>
        /// �����ݲ��뵽���ݿ�
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataSet ds)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownProductDao dao = new DownProductDao();
                dao.Insert(ds);
            }
        }
        #endregion
    }
}
