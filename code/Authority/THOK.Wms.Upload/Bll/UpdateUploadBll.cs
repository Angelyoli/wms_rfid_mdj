using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
using THOK.WMS.Upload.Dao;
using System.Threading;
using System.Xml;
namespace THOK.WMS.Upload.Bll
{
    public class UpdateUploadBll
    {
        #region �ϱ������̵ĳ����ҵ���

        /// <summary>
        /// ���ϱ��������ҵ����������
        /// </summary>
        /// <param name="tableBull"></param>
        public void InsertBull(DataTable tableAllotDetail, string masterTableName, string detailTableName, string outInfoTabel, string employeeCode)
        {
            DataTable table = null;
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao(); 
                dao.SetPersistentManager(persistentManager);
                table = dao.QueryBusiBill(outInfoTabel);
                int s = 0;
                decimal quantity = 0.00M;
                string product = "1";//�ж��Ƿ���ͬһ��Ʒ��
                DataRow[] tableAllotDr = tableAllotDetail.Select("1=1", "PRODUCTCODE");
                foreach (DataRow row in tableAllotDr)
                {
                    s++;
                    if (row["PRODUCTCODE"].ToString() != product)
                        quantity = 0.00M;//�������Ʒ�Ʒ��������
                    DataTable detailTable = dao.GetByOutInfo(row["BILLNO"].ToString(), row["PRODUCTCODE"].ToString(), detailTableName);
                    DataTable masterTable = dao.GetByOutInfo(row["BILLNO"].ToString(), masterTableName);
                    DataTable productStandArdrate = dao.ProductRate(row["PRODUCTCODE"].ToString());//��ȡ��Ʒ�ı���STANDARDRATE
                    string unitCode = dao.GetCellCodeCodeByName(row["CELLCODE"].ToString());///��ȡ��λ�ĵ�λ
                    decimal pieceBarQuantity = dao.FindPieceQuantity(row["PRODUCTCODE"].ToString(), "0");//��ѯԭ����������
                    decimal barQuantity = dao.FindBarQuantity(row["PRODUCTCODE"].ToString(), "1");//��ѯԭ�����������
                    decimal beginQuantity = pieceBarQuantity + barQuantity;//ԭ���
                    decimal endBarQuantity = 0.00M;//��ǰ���������
                    decimal endQuntity = 0.00M;//��ǰ���=ԭ���+��ǰ��������

                    if (unitCode == productStandArdrate.Rows[0]["JIANCODE"].ToString())//�ж��Ǽ�������������Ҫת����������Ҫ
                    {
                    decimal endPieceQuantity = Convert.ToDecimal(row["QUANTITY"]) * Convert.ToDecimal(productStandArdrate.Rows[0]["JIANRATE"].ToString());//�ѵ�ǰ���������ת��Ϊ֧
                        endBarQuantity = (Convert.ToDecimal(endPieceQuantity) / Convert.ToDecimal(productStandArdrate.Rows[0]["TIAORATE"].ToString()));//�ѵ�ǰ�����֧ת����                       
                        quantity = quantity + endBarQuantity;
                    }
                    else
                    {
                        endBarQuantity = Convert.ToDecimal(row["QUANTITY"]);
                        quantity = quantity + endBarQuantity;
                    }

                    if (outInfoTabel == "DWV_IWMS_OUT_BUSI_BILL")
                        endQuntity = beginQuantity - quantity;//�����ȥ
                    else
                        endQuntity = beginQuantity + quantity;//������   

                    DataRow newRow = table.NewRow();
                    newRow["BUSI_ACT_ID"] = DateTime.Now.ToString("yyMMddssff") + s;
                    newRow["BUSI_BILL_DETAIL_ID"] = detailTable.Rows[0]["STORE_BILL_DETAIL_ID"].ToString();
                    newRow["BUSI_BILL_ID"] = row["BILLNO"];
                    newRow["RELATE_BUSI_BILL_ID"] = row["BILLNO"];
                    newRow["STORE_BILL_ID"] = row["BILLNO"];
                    newRow["BRAND_CODE"] = row["PRODUCTCODE"];
                    newRow["BRAND_NAME"] = detailTable.Rows[0]["BRAND_NAME"].ToString();
                    newRow["QUANTITY"] = endBarQuantity;
                    newRow["DIST_CTR_CODE"] = "0101";
                    newRow["ORG_CODE"] = this.QueryOrgCode().ToString();
                    newRow["STORE_ROOM_CODE"] = "001";
                    newRow["STORE_PLACE_CODE"] = "1002";
                    newRow["TARGET_NAME"] = Convert.ToString(dao.GetCellCodeByName(row["CELLCODE"].ToString()));
                    newRow["IN_OUT_TYPE"] = masterTable.Rows[0]["IN_OUT_TYPE"].ToString();
                    newRow["BILL_TYPE"] = masterTable.Rows[0]["BILL_TYPE"].ToString();
                    newRow["BEGIN_STOCK_QUANTITY"] = Convert.ToInt32(beginQuantity);
                    newRow["END_STOCK_QUANTITY"] = Convert.ToInt32(endQuntity);
                    newRow["DISUSE_STATUS"] = "0";
                    newRow["RECKON_STATUS"] = "";
                    newRow["RECKON_DATE"] = "";
                    newRow["UPDATE_CODE"] = "050000";
                    newRow["UPDATE_DATE"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    newRow["IS_IMPORT"] = "0";
                    table.Rows.Add(newRow);
                    product = row["PRODUCTCODE"].ToString();
                }
                dao.InsertBull(table, outInfoTabel);
            }
        }

        /// <summary>
        /// �����ϴ������̵ĳ������ҵ���
        /// </summary>
        public void InsertBull(DataTable table, string outInfoTabel)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao(); 
                dao.SetPersistentManager(persistentManager);
                dao.InsertBull(table, outInfoTabel);
            }
        }

        /// <summary>
        /// �ֶ���ӵ�������
        /// </summary>
        /// <param name="infoTableName"></param>
        /// <param name="bill"></param>
        /// <param name="type"></param>
        /// <param name="state"></param>
        /// <param name="operate"></param>
        /// <param name="isUpdate"></param>
        public void InsertBillMaster(string infoTableName, string bill, string type, string state, string operate, bool isUpdate)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                dao.SetPersistentManager(persistentManager);
                if (isUpdate)
                {
                    DataTable table = dao.QueryBusiBill(infoTableName);
                    DataRow newRow = table.NewRow();
                    newRow["STORE_BILL_ID"] = bill.ToString().Trim();
                    newRow["DIST_CTR_CODE"] = dao.GetCompany().ToString();
                    newRow["AREA_TYPE"] = "0901";
                    newRow["CREATOR_CODE"] = operate;
                    newRow["CREATE_DATE"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    newRow["IN_OUT_TYPE"] = "1202";
                    newRow["BILL_TYPE"] = type;
                    newRow["BILL_STATUS"] = state;
                    newRow["DISUSE_STATUS"] = "0";
                    newRow["IS_IMPORT"] = "0";
                    table.Rows.Add(newRow);
                    dao.InsertMaster(infoTableName, table);
                }
                else
                {
                    string sql = string.Format("UPDATE {0} SET CREATE_DATE='{2}',BILL_TYPE='{3}',BILL_STATUS='{4}' WHERE STORE_BILL_ID='{5}'",
                        infoTableName, operate, DateTime.Now.ToString("yyyyMMddHHmmss"), type, state, bill);
                    dao.UpdateTable(sql);
                }
            }
        }

        /// <summary>
        /// �ֶ���ӵ���ϸ��
        /// </summary>
        /// <param name="infoTableName"></param>
        /// <param name="id"></param>
        /// <param name="bill"></param>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <param name="isUpdate"></param>
        /// <param name="queryTable"></param>
        public void InsertDetail(string infoTableName, string id, string bill, string product, decimal quantity, bool isUpdate, string billDetailTable)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao(); 
                dao.SetPersistentManager(persistentManager);
                DataTable productTable = dao.ProductRate(product);
                decimal quan = Convert.ToDecimal(quantity * Convert.ToInt32(productTable.Rows[0]["JIANRATE"].ToString()) / Convert.ToInt32(productTable.Rows[0]["TIAORATE"].ToString()));
                if (isUpdate)
                {
                    Thread.Sleep(500);//ȷ�������Ѿ����
                    //string sql = string.Format("SELECT ID FROM {2} WHERE PRODUCTCODE='{0}' AND BILLNO='{1}'", product, bill, billDetailTable);
                    //string stokeId = dao.GetDate(sql).ToString();
                    DataTable table = dao.QueryBusiBill(infoTableName);
                    DataRow newRow = table.NewRow();
                    newRow["STORE_BILL_DETAIL_ID"] = id;
                    newRow["STORE_BILL_ID"] = bill.ToString().Trim();
                    newRow["BRAND_CODE"] = product;
                    newRow["BRAND_NAME"] = this.QueryProduceName(product).ToString();
                    newRow["QUANTITY"] = quan;
                    newRow["IS_IMPORT"] = "0";
                    newRow["BILL_TYPE"] = "1001";
                    table.Rows.Add(newRow);
                    dao.InsertMaster(infoTableName, table);
                }
                else
                {
                    string productname = this.QueryProduceName(product).ToString();
                    string sql = string.Format("UPDATE {0} SET STORE_BILL_ID='{1}',BRAND_CODE='{2}',BRAND_NAME='{3}',QUANTITY='{4}' WHERE STORE_BILL_DETAIL_ID='{5}'",
                        infoTableName, bill, product, productname, Convert.ToDecimal(quan), id.ToString());
                    dao.UpdateTable(sql);
                }
            }
        }

        /// <summary>
        /// ���ݾ��̱����ѯ����
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public string QueryProduceName(string productCode)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao(); 
                string sql = string.Format("SELECT PRODUCTNAME FROM WMS_PRODUCT WHERE PRODUCTCODE='{0}'", productCode);
                return Convert.ToString(dao.GetDate(sql));
            }
        }

        /// <summary>
        /// ��ȡ������λ����
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public string QueryOrgCode()
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao(); 
                string sql = "SELECT SUBSTRING(ORG_CODE,0,5)+SUBSTRING(ORG_CODE,11,5) FROM DWV_OUT_ORG";
                return Convert.ToString(dao.GetDate(sql));
            }
        }
        #endregion        

        #region �޸��������������

        /// <summary>
        /// ��ˣ��޸����������������ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void inBillAudot(string EmployeeCode, string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_IN_STORE_BILL SET  BILL_STATUS='2', AUDITOR_CODE='{0}',AUDIT_DATE='{1}',QUANTITY_SUM=(select sum(quantity) from DWV_IWMS_IN_STORE_BILL_DETAIL where STORE_BILL_ID='{2}')  WHERE STORE_BILL_ID='{3}'", EmployeeCode, System.DateTime.Now.ToString("yyyyMMddHHmmss"), BillNo,BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ���ˣ��޸����������������ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void inRevBillAudot(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_IN_STORE_BILL SET BILL_STATUS='1', AUDITOR_CODE='',AUDIT_DATE=NULL WHERE STORE_BILL_ID='{0}'", BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ���䣬�޸����������������ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void inUpdateAllot(string EmployeeCode, string BillNo)
        {
            string[] aryBillNo = BillNo.Split(',');
            string BillNoList = "''";
            for (int i = 0; i < aryBillNo.Length; i++)
            {
                BillNoList += ",'" + aryBillNo[i] + "'";
            }
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_IN_STORE_BILL SET BILL_STATUS='3', ASSIGNER_CODE='{0}',ASSIGN_DATE='{1}' WHERE STORE_BILL_ID  IN({2})", EmployeeCode, System.DateTime.Now.ToString("yyyyMMddHHmmss"), BillNoList);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ȡ�����䣬�޸����������������ˡ�״̬��ʱ�䲢ɾ��������
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void inDeleteAllot(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("DELETE FROM DWV_IWMS_IN_BUSI_BILL WHERE BUSI_BILL_ID='{0}';UPDATE DWV_IWMS_IN_STORE_BILL SET BILL_STATUS='2', ASSIGNER_CODE='',ASSIGN_DATE=NULL WHERE STORE_BILL_ID='{0}'", BillNo);              
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ����ȷ�ϣ��޸������������ȷ���ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void inConfirmAllot(string EmployeeCode, string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_IN_STORE_BILL SET BILL_STATUS='4', AFFIRM_CODE='{0}',AFFIRM_DATE='{1}' WHERE STORE_BILL_ID='{2}'", EmployeeCode, System.DateTime.Now.ToString("yyyyMMddHHmmss"), BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ȡ��ȷ�ϣ��޸������������ȷ���ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void inCancelAllot(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_IN_STORE_BILL SET BILL_STATUS='3', AFFIRM_CODE='',AFFIRM_DATE=NULL WHERE STORE_BILL_ID='{0}'", BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ɾ����ⵥ�������ϸ��
        /// </summary>
        /// <param name="BillNo"></param>
        public void deleteInBill(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("DELETE DWV_IWMS_IN_STORE_BILL_DETAIL WHERE STORE_BILL_ID IN({0})", BillNo);
                dao.SetData(sql);
                sql = string.Format("DELETE DWV_IWMS_IN_STORE_BILL WHERE STORE_BILL_ID IN({0})", BillNo);
                dao.SetData(sql);
            }
        }
        #endregion

        #region �޸ĳ�������������

        /// <summary>
        /// ��ˣ��޸����̳�����������ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void outBillAudot(string EmployeeCode, string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_OUT_STORE_BILL SET BILL_STATUS='2', AUDITOR_CODE='{0}',AUDIT_DATE='{1}',QUANTITY_SUM=(select sum(quantity) from DWV_IWMS_OUT_STORE_BILL_DETAIL where STORE_BILL_ID='{2}') WHERE STORE_BILL_ID='{3}'", EmployeeCode, System.DateTime.Now.ToString("yyyyMMddHHmmss"), BillNo, BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ���ˣ��޸����̳�����������ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void outReBbillAudot(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_OUT_STORE_BILL SET BILL_STATUS='1', AUDITOR_CODE='',AUDIT_DATE=NULL WHERE STORE_BILL_ID='{0}'", BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ���䣬�޸����̳�����������ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void outUpdateAudot(string EmployeeCode,string BillNo)
        {
            string[] aryBillNo = BillNo.Split(',');
            string BillNoList = "''";
            for (int i = 0; i < aryBillNo.Length; i++)
            {
                BillNoList += ",'" + aryBillNo[i] + "'";
            }
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_OUT_STORE_BILL SET BILL_STATUS='3', ASSIGNER_CODE='{0}',ASSIGN_DATE='{1}' WHERE STORE_BILL_ID IN({2})", EmployeeCode, System.DateTime.Now.ToString("yyyyMMddHHmmss"), BillNoList);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ȡ�����䣬�޸����̳�����������ˡ�״̬��ʱ��,��ɾ�������
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void outDeleteAudot(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("DELETE FROM DWV_IWMS_OUT_BUSI_BILL WHERE BUSI_BILL_ID='{0}';UPDATE DWV_IWMS_OUT_STORE_BILL SET BILL_STATUS='2', ASSIGNER_CODE='',ASSIGN_DATE=NULL WHERE STORE_BILL_ID='{0}'", BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ȷ�Ϸ��䣬�޸����̳�������ȷ���ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void outConfirmAudot(string EmployeeCode, string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_OUT_STORE_BILL SET BILL_STATUS='4', AFFIRM_CODE='{0}',AFFIRM_DATE='{1}' WHERE STORE_BILL_ID='{2}'", EmployeeCode, System.DateTime.Now.ToString("yyyyMMddHHmmss"), BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ȡ��ȷ�ϣ��޸����̳�������ȷ���ˡ�״̬��ʱ��
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BillNo"></param>
        public void outCancelAudot(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("UPDATE DWV_IWMS_OUT_STORE_BILL SET BILL_STATUS='3', AFFIRM_CODE='',AFFIRM_DATE=NULL WHERE STORE_BILL_ID='{0}'", BillNo);
                dao.SetData(sql);
            }
        }

        /// <summary>
        /// ɾ�����ⵥ�������ϸ��
        /// </summary>
        /// <param name="BillNo"></param>
        public void deleteOutBill(string BillNo)
        {
            using (PersistentManager persistentManager = new PersistentManager())
            {
                UpdateUploadDao dao = new UpdateUploadDao();
                string sql = string.Format("DELETE DWV_IWMS_OUT_STORE_BILL_DETAIL WHERE STORE_BILL_ID IN({0})",BillNo);
                dao.SetData(sql);
                sql = string.Format("DELETE DWV_IWMS_OUT_STORE_BILL WHERE STORE_BILL_ID IN({0})", BillNo);
                dao.SetData(sql);
            }
        }

        #endregion
    }
}
