using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WMS.DownloadWms.Dao;
using THOK.WMS.Upload.Bll;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownCustomerBll
    {
        UploadBll upload = new UploadBll();
        #region ��Ӫ��ϵͳ���ؿͻ���Ϣ

        /// <summary>
        /// ���ؿͻ���Ϣ
        /// </summary>
        /// <returns></returns>
        public bool DownCustomerInfo()
        {
            bool tag = true;
            DataTable customerCodeDt = this.GetCustomerCode();
            string CusromerList = UtinString.MakeString(customerCodeDt, "customer_code");
            //CusromerList = UtinString.StringMake(CusromerList);
            CusromerList = " CUST_CODE NOT IN (" + CusromerList + ")";
            DataTable customerDt = this.GetCustomerInfo(CusromerList);
            if (customerDt.Rows.Count > 0)
            {
                DataSet custDs = this.Insert(customerDt);
                this.Insert(custDs);
                //�ϱ��ͻ���Ϣ����
                //upload.InsertCustom(custDs);
            }            
            return tag;
        }

        /// <summary>
        /// ��ѯ�����ع��Ŀͻ�����
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomerCode()
        {
            using (PersistentManager dbPm = new PersistentManager())
            {
                DownCustomerDao dao = new DownCustomerDao();
                return dao.GetCustomerCode();
            }
        }


        /// <summary>
        /// ���ؿͻ���Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomerInfo(string customerCode)
        {
            using (PersistentManager dbPm = new PersistentManager("YXConnection"))
            {
                DownCustomerDao dao = new DownCustomerDao();
                dao.SetPersistentManager(dbPm);
                return dao.GetCustomerInfo(customerCode);
            }
        }


        /// <summary>
        ///���������
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataSet Insert(DataTable custTable)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in custTable.Rows)
            {
                DataRow inbrddr = ds.Tables["DWV_IORG_CUSTOMER"].NewRow();
                inbrddr["customer_code"] = row["CUST_CODE"].ToString().Trim();
                inbrddr["custom_code"] = row["CUST_N"].ToString().Trim();
                inbrddr["customer_name"] = row["CUST_NAME"].ToString().Trim();
                inbrddr["company_code"] = row["ORG_CODE"].ToString().Trim();
                inbrddr["sale_region_code"] = row["SALE_REG_CODE"].ToString().Trim();
                inbrddr["uniform_code"] = row["N_CUST_CODE"].ToString().Trim();
                inbrddr["customer_type"] = row["CUST_TYPE"].ToString().Trim() == "" ? "1" : row["CUST_TYPE"];
                inbrddr["sale_scope"] = row["sale_scope"].ToString().Trim();
                inbrddr["industry_type"] = row["RTL_CUST_TYPE_CODE"].ToString().Trim();
                inbrddr["city_or_countryside"] = row["CUST_GEO_TYPE_CODE"].ToString().Trim();
                inbrddr["deliver_line_code"] = row["DELIVER_LINE_CODE"].ToString().Trim();
                inbrddr["deliver_order"] = row["DELIVER_ORDER"];
                inbrddr["address"] = row["DIST_ADDRESS"].ToString().Trim();
                inbrddr["phone"] = row["DIST_PHONE"].ToString().Trim() == "" ? "1" : row["DIST_PHONE"].ToString().Trim();
                inbrddr["license_type"] = row["LICENSE_TYPE"].ToString().Trim();
                inbrddr["license_code"] = row["LICENSE_CODE"].ToString().Trim();
                inbrddr["principal_name"] = row["PRINCIPAL_NAME"].ToString().Trim();
                inbrddr["principal_phone"] = row["PRINCIPAL_TEL"].ToString().Trim();
                inbrddr["principal_address"] = row["PRINCIPAL_ADDRESS"].ToString().Trim();
                inbrddr["management_name"] = row["MANAGEMENT_NAME"].ToString().Trim();
                inbrddr["management_phone"] = row["MANAGEMENT_TEL"].ToString().Trim();
                inbrddr["bank"] = row["BANK"].ToString().Trim();
                inbrddr["bank_accounts"] = row["BANK_ACCOUNTS"].ToString().Trim();
                inbrddr["description"] = "";
                inbrddr["is_active"] = row["ISACTIVE"].ToString();
                inbrddr["update_time"] = DateTime.Now;
                ds.Tables["DWV_IORG_CUSTOMER"].Rows.Add(inbrddr);
            }
            return ds;
        }
       

        /// <summary>
        /// �������ݵ����ݱ�
        /// </summary>
        /// <param name="customerDt"></param>
        public void Insert(DataSet customerDs)
        {
            using (PersistentManager dbPm = new PersistentManager())
            {
                DownCustomerDao dao = new DownCustomerDao();
                dao.Insert(customerDs);
            }
        }
        /// <summary>
        /// ����һ���ͻ������
        /// </summary>
        /// <returns></returns>
        private DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable inbrtable = ds.Tables.Add("DWV_IORG_CUSTOMER");
            inbrtable.Columns.Add("customer_code");
            inbrtable.Columns.Add("custom_code");
            inbrtable.Columns.Add("customer_name");
            inbrtable.Columns.Add("company_code");
            inbrtable.Columns.Add("sale_region_code");
            inbrtable.Columns.Add("uniform_code");
            inbrtable.Columns.Add("customer_type");
            inbrtable.Columns.Add("sale_scope");
            inbrtable.Columns.Add("industry_type");
            inbrtable.Columns.Add("city_or_countryside");
            inbrtable.Columns.Add("deliver_line_code");
            inbrtable.Columns.Add("deliver_order");
            inbrtable.Columns.Add("address");
            inbrtable.Columns.Add("phone");            
            inbrtable.Columns.Add("license_type");
            inbrtable.Columns.Add("license_code");
            inbrtable.Columns.Add("principal_name");
            inbrtable.Columns.Add("principal_phone");
            inbrtable.Columns.Add("principal_address");
            inbrtable.Columns.Add("management_name");
            inbrtable.Columns.Add("management_phone");
            inbrtable.Columns.Add("bank");//һ�Ź���������
            inbrtable.Columns.Add("bank_accounts");
            inbrtable.Columns.Add("description");            
            inbrtable.Columns.Add("is_active");
            inbrtable.Columns.Add("update_time");
            return ds;
        }
        #endregion
    }
}
