using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
using THOK.WMS.DownloadWms.Dao;
using System.Threading;
using THOK.Wms.Dal.Interfaces;
//using Microsoft.Practices.Unity;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownInBillBll
    {
        private string Employee = "";

        #region ѡ�����ڴ�Ӫ��ϵͳ�����������

        /// <summary>
        /// �������������������
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool GetInBill(string startDate, string endDate, string EmployeeCode,string wareCode,string billtype, out string errorInfo)
        {
            bool tag = false;
            Employee = EmployeeCode;
            errorInfo = string.Empty;
            using (PersistentManager pm = new PersistentManager())
            {
                try
                {
                    DownInBillDao dao = new DownInBillDao();
                    DataTable emply = dao.FindEmployee(EmployeeCode);
                    DataTable inMasterBillNo = this.GetInBillNo();
                    string billnolist = UtinString.StringMake(inMasterBillNo, "bill_no");
                    billnolist = UtinString.StringMake(billnolist);
                    billnolist = string.Format("ORDER_DATE >='{0}' AND ORDER_DATE <='{1}' AND ORDER_ID NOT IN({2})", startDate, endDate, billnolist);
                    DataTable masterdt = this.InBillMaster(billnolist);

                    string inDetailList = UtinString.StringMake(masterdt, "ORDER_ID");
                    inDetailList = UtinString.StringMake(inDetailList);
                    inDetailList = "ORDER_ID IN(" + inDetailList + ")";
                    DataTable detaildt = this.InBillDetail(inDetailList);

                    if (masterdt.Rows.Count > 0 && detaildt.Rows.Count > 0)
                    {
                        DataSet masterds = this.InBillMaster(masterdt, emply.Rows[0]["employee_id"].ToString(), wareCode, billtype);

                        DataSet detailds = this.InBillDetail(detaildt);
                        this.Insert(masterds, detailds);
                        tag = true;
                    }
                    else
                        errorInfo = "û���µ���ⵥ���أ�";
                }
                catch (Exception e)
                {
                    errorInfo = "������ⵥʧ�ܣ�ԭ��" + e.Message;
                }
            }
            return tag;
        }
        /// <summary>
        /// ����������� ����
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="EmployeeCode"></param>
        /// <param name="wareCode"></param>
        /// <param name="billtype"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool GetInBills(string startDate, string endDate, string EmployeeCode, string wareCode, string billtype, out string errorInfo)
        {
            bool tag = false;
            Employee = EmployeeCode;
            errorInfo = string.Empty;
            using (PersistentManager pm = new PersistentManager())
            {
                try
                {
                    DownInBillDao dao = new DownInBillDao();
                    DataTable emply = dao.FindEmployee(EmployeeCode);
                    DataTable inMasterBillNo = this.GetInBillNo();
                    string billnolist = UtinString.StringMake(inMasterBillNo, "bill_no");
                    billnolist = UtinString.StringMake(billnolist);
                    billnolist = string.Format("ORDER_DATE >='{0}' AND ORDER_DATE <='{1}' AND ORDER_ID NOT IN({2})", startDate, endDate, billnolist);
                    DataTable masterdt = this.InBillMasters(billnolist);

                    string inDetailList = UtinString.StringMake(masterdt, "ORDER_ID");
                    inDetailList = UtinString.StringMake(inDetailList);
                    inDetailList = "ORDER_ID IN(" + inDetailList + ")";
                    DataTable detaildt = this.InBillDetail(inDetailList);

                    if (masterdt.Rows.Count > 0 && detaildt.Rows.Count > 0)
                    {
                        DataSet masterds = this.InBillMaster(masterdt, emply.Rows[0]["employee_id"].ToString(), wareCode, billtype);

                        DataSet detailds = this.InBillDetail(detaildt);
                        this.Insert(masterds, detailds);
                        tag = true;
                    }
                    else
                        errorInfo = "û���µ���ⵥ���أ�";
                }
                catch (Exception e)
                {
                    errorInfo = "������ⵥʧ�ܣ�ԭ��" + e.Message;
                }
            }
            return tag;
        }
        /// <summary>
        /// ��ѯ���ֲִ�4������ⵥ
        /// </summary>
        /// <returns></returns>
        public DataTable GetInBillNo()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                return dao.GetBillNo();
            }
        }

        /// <summary>
        /// ������ⵥ��������
        /// </summary>
        /// <returns></returns>
        public DataTable InBillMaster(string inBillNoList)
        {
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(dbpm);
                return dao.GetInBillMaster(inBillNoList);
            }
        }
        /// <summary>
        /// ������ⵥ�������� ����
        /// </summary>
        /// <returns></returns>
        public DataTable InBillMasters(string inBillNoList)
        {
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(dbpm);
                return dao.GetInBillMasters(inBillNoList);
            }
        }
        /// <summary>
        /// ������ⵥ��ϸ������
        /// </summary>
        /// <returns></returns>
        public DataTable InBillDetail(string inBillNoList)
        {
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(dbpm);
                return dao.GetInBillDetail(inBillNoList);
            }
        }

        /// <summary>
        /// ������������ݱ������������2011-08-02 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataSet InBillMaster(DataTable inBillMasterdr, string employeeId,string wareCode,string billType)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in inBillMasterdr.Rows)
            {
                Guid eid = new Guid(employeeId);
                string createdate = row["ORDER_DATE"].ToString();
                createdate = createdate.Substring(0, 4) + "-" + createdate.Substring(4, 2) + "-" + createdate.Substring(6, 2);
                DataRow masterrow = ds.Tables["WMS_IN_BILLMASTER"].NewRow();
                masterrow["bill_no"] = row["ORDER_ID"].ToString().Trim();
                masterrow["bill_date"] = Convert.ToDateTime(createdate);
                masterrow["bill_type_code"] = billType;//row["ORDER_TYPE"].ToString().Trim();
                masterrow["warehouse_code"] = wareCode;//row["DIST_CTR_CODE"].ToString().Trim();
                masterrow["status"] = "1";
                masterrow["verify_date"] = null;
                masterrow["is_active"] = "1";
                masterrow["update_time"] = DateTime.Now;
                masterrow["operate_person_id"] = eid;
                //masterrow["verify_person_id"] = null;
                masterrow["description"] = "";
                masterrow["lock_tag"] = "";
                masterrow["target_cell_code"] = null;
                ds.Tables["WMS_IN_BILLMASTER"].Rows.Add(masterrow);
            }
            return ds;
        }

        /// <summary>
        /// �������ϸ�����ݱ����������,2011-08-02
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataSet InBillDetail(DataTable inBillDetaildr)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in inBillDetaildr.Rows)
            {
                DataTable prodt = FindProductCodeInfo(" CUSTOM_CODE='" + row["BRAND_CODE"].ToString() + "'");//                
                DataRow detailrow = ds.Tables["WMS_IN_BILLDETAIL"].NewRow();
                detailrow["bill_no"] = row["ORDER_ID"].ToString().Trim();
                detailrow["product_code"] = prodt.Rows[0]["product_code"];
                detailrow["price"] = Convert.ToDecimal(row["PRICE"]);
                detailrow["bill_quantity"] = Convert.ToDecimal(row["QUANTITY"]);
                detailrow["allot_quantity"] = 0;
                detailrow["unit_code"] = prodt.Rows[0]["unit_code"];
                detailrow["description"] = "";
                detailrow["real_quantity"] = 0;
                ds.Tables["WMS_IN_BILLDETAIL"].Rows.Add(detailrow);
            }
            return ds;
        }

        /// <summary>
        /// ���ݾ��̱����ѯ��λ��Ϣ
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public DataTable FindProductCodeInfo(string productCode)
        {
            using (PersistentManager dbPm = new PersistentManager())
            {
                DownProductDao dao = new DownProductDao();
                return dao.FindProductCodeInfo(productCode);
            }
        }

        /// <summary>
        /// �Ѳ�ѯ��������ӵ��ִ����ݿ�
        /// </summary>
        /// <param name="masterds"></param>
        /// <param name="detailds"></param>
        public void Insert(DataSet masterds, DataSet detailds)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                if (masterds.Tables["WMS_IN_BILLMASTER"].Rows.Count > 0)
                {
                    dao.InsertInBillMaster(masterds);
                }
                if (detailds.Tables["WMS_IN_BILLDETAIL"].Rows.Count > 0)
                {
                    dao.InsertInBillDetail(detailds);
                }
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        private DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable mastertable = ds.Tables.Add("WMS_IN_BILLMASTER");
            mastertable.Columns.Add("bill_no");
            mastertable.Columns.Add("bill_date");
            mastertable.Columns.Add("bill_type_code");
            mastertable.Columns.Add("warehouse_code");
            mastertable.Columns.Add("status");
            mastertable.Columns.Add("verify_date");
            mastertable.Columns.Add("description");
            mastertable.Columns.Add("is_active");
            mastertable.Columns.Add("update_time");
            mastertable.Columns.Add("operate_person_id");
            mastertable.Columns.Add("verify_person_id");
            mastertable.Columns.Add("lock_tag");
            mastertable.Columns.Add("row_version");
            mastertable.Columns.Add("target_cell_code");

            DataTable detailtable = ds.Tables.Add("WMS_IN_BILLDETAIL");
            detailtable.Columns.Add("id");
            detailtable.Columns.Add("bill_no");
            detailtable.Columns.Add("product_code");
            detailtable.Columns.Add("unit_code");
            detailtable.Columns.Add("price");
            detailtable.Columns.Add("bill_quantity");
            detailtable.Columns.Add("allot_quantity");
            detailtable.Columns.Add("real_quantity");
            detailtable.Columns.Add("description");


            DataTable middletable = ds.Tables.Add("WMS_MIDDLE_IN_BILLDETAIL");
            middletable.Columns.Add("bill_no");
            middletable.Columns.Add("bill_date");
            return ds;
        }

        #endregion

        
    }
}
