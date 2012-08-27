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

        #region 从营销系统下载入库数据

        #region 手动从营销系统下载入库数据

        /// <summary>
        /// 手动下载入库数据
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public bool GetInBillManual(string billno, string EmployeeCode)
        {
            bool tag = true;
            Employee = EmployeeCode;
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();

                DataTable inBillTable = this.GetInBillNo();
                string inBillNoList = UtinString.StringMake(inBillTable, "BILLNO");
                inBillNoList = UtinString.StringMake(inBillNoList);
                inBillNoList = "ORDER_ID NOT IN(" + inBillNoList + ")";

                DataTable masterdt = this.InBillMaster(inBillNoList);
                DataTable detaildt = this.InBillDetail(inBillNoList);

                DataRow[] masterdr = masterdt.Select("ORDER_ID  IN (" + billno + ")");
                DataRow[] detaildr = detaildt.Select("ORDER_ID  IN (" + billno + ")");

                if (masterdr.Length > 0 && detaildr.Length > 0)
                {
                    //DataSet detailds = this.InBillDetail(detaildr);
                    //DataSet masterds = this.InBillMaster(masterdr);
                    //this.Insert(masterds, detailds);
                }
                else
                    tag = false;
            }
            return tag;
        }

        #endregion

        #region 自动从营销系统下载入库数据

        public bool DownInBillInfoAuto(string EmployeeCode)
        {
            bool tag = true;
            Employee = EmployeeCode;
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                DataTable WmsInBillTable = this.GetInBillNo();
                string inBillNoList = UtinString.StringMake(WmsInBillTable, "BILLNO");
                inBillNoList = UtinString.StringMake(inBillNoList);
                inBillNoList = "ORDER_ID NOT IN(" + inBillNoList + ")";
                DataTable masterdt = this.InBillMaster(inBillNoList);
                DataTable detaildt = this.InBillDetail(inBillNoList);

                DataRow[] masterdr = masterdt.Select("1=1");
                DataRow[] detaildr = detaildt.Select("1=1");

                if (masterdr.Length > 0 && detaildr.Length > 0)
                {
                    //DataSet detailds = this.InBillDetail(detaildr);
                    //DataSet masterds = this.InBillMaster(masterdr);
                    //this.Insert(masterds, detailds);
                }
                else
                {
                    tag = false;
                }
            }
            return tag;
        }

        #endregion

        #region 选择日期从营销系统下载入库数据

        /// <summary>
        /// 根据日期下载入库数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool GetInBill(string startDate, string endDate, string EmployeeCode, out string errorInfo)
        {
            bool tag = false;
            Employee = EmployeeCode;
            errorInfo = string.Empty;
            using (PersistentManager pm = new PersistentManager())
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
                    DataSet masterds = this.InBillMaster(masterdt, emply.Rows[0]["employee_id"].ToString());

                    DataSet detailds = this.InBillDetail(detaildt);
                    this.Insert(masterds, detailds);
                    tag = true;
                }
                else
                    errorInfo = "没有新的入库单下载！";
            }
            return tag;
        }

        #endregion

        #region 其他下载查询方法
        /// <summary>
        /// 分页查询营销系统数据入库单据主表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataTable GetInBillMaster(int pageIndex, int pageSize)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                using (PersistentManager dbpm = new PersistentManager("YXConnection"))
                {
                    DownInBillDao masterdao = new DownInBillDao();
                    DownInBillDao dao = new DownInBillDao();
                    dao.SetPersistentManager(dbpm);
                    DataTable billnodt = masterdao.GetBillNo();
                    string billnolist = UtinString.StringMake(billnodt, "BILLNO");
                    billnolist = UtinString.StringMake(billnolist);
                    return dao.GetInBillMasterByBillNo(billnolist);
                }
            }
        }

        /// <summary>
        /// 分页查询营销系统数据入库单据明细表
        /// </summary>
        /// <param name="PrimaryKey"></param>
        /// <param name="papeIndex"></param>
        /// <param name="papeSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="inBillNo"></param>
        /// <returns></returns>
        public DataTable GetInBillDetail(string PrimaryKey, int papeIndex, int papeSize, string orderBy, string inBillNo)
        {
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(dbpm);
                return dao.GetInBillDetailByBillNo(inBillNo);
            }
        }

        /// <summary>
        /// 把入库主表数据保存在虚拟表中2011-08-02 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataSet InBillMaster(DataTable inBillMasterdr, string employeeId)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in inBillMasterdr.Rows)
            {
                Guid eid =new Guid(employeeId);
                string createdate = row["ORDER_DATE"].ToString();
                createdate = createdate.Substring(0, 4) + "-" + createdate.Substring(4, 2) + "-" + createdate.Substring(6, 2);
                DataRow masterrow = ds.Tables["WMS_IN_BILLMASTER"].NewRow();
                masterrow["bill_no"] = row["ORDER_ID"].ToString().Trim();
                masterrow["bill_date"] = Convert.ToDateTime(createdate);
                masterrow["bill_type_code"] = "1001";//row["ORDER_TYPE"].ToString().Trim();
                masterrow["warehouse_code"] = "0101";//row["DIST_CTR_CODE"].ToString().Trim();
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
        /// 把入库明细单数据保存在虚拟表,2011-08-02
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataSet InBillDetail(DataTable inBillDetaildr)
        {
            DataSet ds = this.GenerateEmptyTables();            
            foreach (DataRow row in inBillDetaildr.Rows)
            {
                DataTable prodt = FindProductCodeInfo(row["BRAND_CODE"].ToString());//                
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
        /// 把查询的数据添加到仓储数据库
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
        /// 下载入库单主表数据
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
        /// 获取单位比例转换
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public DataTable DownProductRate(string productCode)
        {
            using (PersistentManager dbPm = new PersistentManager())
            {
                DownProductDao dao = new DownProductDao();
                return dao.LcDownProductRate(productCode);
            }
        }

        public DataTable FindProductCodeInfo(string productCode)
        {
            using (PersistentManager dbPm = new PersistentManager())
            {
                DownProductDao dao = new DownProductDao();
                return dao.FindProductCodeInfo(productCode);
            }
        }

        /// <summary>
        /// 下载入库单明细表数据
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
        #endregion

        #endregion

        #region 分拣线退货入库单


        public bool DownReturnInBill(string billNO)
        {
            bool tag = true;
            int count = this.ReturnInBillCount();
            if (count == 0)
            {
                return false;
            }
            else
            {
                string idList = "";
                string memo = "此单据于" + DateTime.Now.ToString("yyMMdd") + "从分拣线退烟入库！";
                this.InsertReturnInBillMaster(billNO, memo);
                idList = this.InsertReturnInBillDetail(billNO);
                this.InsertReturnInBill(billNO, idList);
                idList = UtinString.StringMake(idList);
                this.UpdateReturnInBillState(idList, "1");
                //DataTable returntable = this.ReturnInBill();

            }
            return tag;
        }

        public int ReturnInBillCount()
        {
            using (PersistentManager pm = new PersistentManager("ServerConnection"))
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                return dao.ReturnInBillCount();
            }
        }

        public DataTable ReturnInBill()
        {
            using (PersistentManager pm = new PersistentManager("ServerConnection"))
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                return dao.ReturnInBillInfo();
            }
        }

        public void InsertReturnInBillMaster(string billNo, string memo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                dao.InsertReturnInBillMaster(billNo, memo);
            }
        }

        public void InsertReturnInBill(string idList, string billNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                dao.Insert(billNo, idList);
            }
        }

        public DataTable ReturnInBillInfo()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                return dao.ReturnInBill();
            }
        }

        public string InsertReturnInBillDetail(string billNo)
        {
            DataSet ds = this.GenerateEmptyTables();
            DataTable dt = this.ReturnInBill();
            DownOutBillBll bll = new DownOutBillBll();
            string idList = "";
            foreach (DataRow d in dt.Rows)
            {
                idList += d["ID"].ToString() + ",";
            }

            DataRow[] row = dt.Select("1=1");
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i].RowState != DataRowState.Deleted)
                {
                    decimal quantity = Convert.ToDecimal(row[i]["QUANTITY"].ToString());
                    DataTable prodt = bll.DownProductRate(row[i]["CIGARETTECODE"].ToString());//
                    decimal rate = Convert.ToDecimal(prodt.Rows[0]["JIANRATE"].ToString());//bll.Quantity(row[i]["CIGARETTECODE"].ToString());
                    DataRow[] dr = dt.Select("CIGARETTECODE ='" + row[i]["CIGARETTECODE"] + "' and ID <>'" + row[i]["ID"] + "'");

                    if (dr.Length < 1)
                    {
                        DataRow detailrow = ds.Tables["WMS_IN_BILLDETAIL"].NewRow();
                        detailrow["BILLNO"] = billNo;
                        detailrow["PRODUCTCODE"] = row[i]["CIGARETTECODE"];
                        detailrow["PRICE"] = 0;
                        detailrow["QUANTITY"] = Convert.ToDecimal(row[i]["QUANTITY"].ToString()) / rate;
                        detailrow["INPUTQUANTITY"] = Convert.ToDecimal(row[i]["QUANTITY"].ToString()) / rate;
                        detailrow["UNITCODE"] = prodt.Rows[0]["JIANCODE"];//
                        ds.Tables["WMS_IN_BILLDETAIL"].Rows.Add(detailrow);
                    }
                    else
                    {
                        DataRow drow = ds.Tables["WMS_IN_BILLDETAIL"].NewRow();
                        foreach (DataRow r in dr)
                        {
                            quantity += Convert.ToDecimal(r["QUANTITY"].ToString());
                            row[i]["QUANTITY"] = quantity;
                            r.Delete();
                        }
                        drow["BILLNO"] = billNo;
                        drow["PRODUCTCODE"] = row[i]["CIGARETTECODE"];
                        drow["PRICE"] = 0;
                        drow["QUANTITY"] = Convert.ToDecimal(row[i]["QUANTITY"].ToString()) / rate;
                        drow["INPUTQUANTITY"] = Convert.ToDecimal(row[i]["QUANTITY"].ToString()) / rate;
                        drow["UNITCODE"] = prodt.Rows[0]["JIANCODE"]; // "002";
                        ds.Tables["WMS_IN_BILLDETAIL"].Rows.Add(drow);
                    }
                }
            }

            this.InsertReturnInBillDetail(ds);
            idList = idList.Substring(0, idList.Length - 1);
            return idList;
        }

        public void InsertReturnInBillDetail(DataSet ds)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                dao.InsertInBillDetail(ds);
            }
        }

        public void UpdateReturnInBillState(string idList, string state)
        {
            using (PersistentManager pm = new PersistentManager("ServerConnection"))
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                dao.UpdateReturnInBilLState(idList, state);
            }
        }

        public void DeleteDownReturnInBillInfo(string billNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                dao.Delete(billNo);
            }
        }

        public void DeleteInBill(string billNo)
        {
            string idList = this.SelectIdList(billNo);
            if (idList != null)
            {
                idList = UtinString.StringMake(idList);
                this.DeleteDownReturnInBillInfo(billNo);
                this.UpdateReturnInBillState(idList, "0");
            }
        }

        public string SelectIdList(string billNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                dao.SetPersistentManager(pm);
                return dao.SelectIdList(billNo);
            }
        }
        #endregion

        #region 操作数字仓储数据

        /// <summary>
        /// 查询数字仓储4天内入库单
        /// </summary>
        /// <returns></returns>
        public DataTable GetInBillNo()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownInBillDao dao = new DownInBillDao();
                //dao.SetPersistentManager(pm);
                return dao.GetBillNo();
            }
        }

        /// <summary>
        /// 构建入库虚拟表
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


            DataTable inmastertable = ds.Tables.Add("DWV_IWMS_IN_STORE_BILL");
            inmastertable.Columns.Add("STORE_BILL_ID");
            inmastertable.Columns.Add("DIST_CTR_CODE");
            inmastertable.Columns.Add("QUANTITY_SUM");
            inmastertable.Columns.Add("AMOUNT_SUM");
            inmastertable.Columns.Add("DETAIL_NUM");
            inmastertable.Columns.Add("CREATOR_CODE");
            inmastertable.Columns.Add("AREA_TYPE");
            inmastertable.Columns.Add("CREATE_DATE");
            inmastertable.Columns.Add("BILL_TYPE");
            inmastertable.Columns.Add("BILL_STATUS");
            inmastertable.Columns.Add("IS_IMPORT");
            inmastertable.Columns.Add("IN_OUT_TYPE");
            inmastertable.Columns.Add("DISUSE_STATUS");


            DataTable indetailtable = ds.Tables.Add("DWV_IWMS_IN_STORE_BILL_DETAIL");
            indetailtable.Columns.Add("STORE_BILL_DETAIL_ID");
            indetailtable.Columns.Add("STORE_BILL_ID");
            indetailtable.Columns.Add("BRAND_CODE");
            indetailtable.Columns.Add("BRAND_NAME");
            indetailtable.Columns.Add("QUANTITY");
            indetailtable.Columns.Add("IS_IMPORT");
            indetailtable.Columns.Add("BILL_TYPE");
            return ds;
        }
        #endregion       
    }
}
