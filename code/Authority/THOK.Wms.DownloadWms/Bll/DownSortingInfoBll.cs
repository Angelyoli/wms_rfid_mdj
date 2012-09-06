using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Util;
using THOK.Wms.DownloadWms.Dao;
using System.Data;
using THOK.WMS.DownloadWms;

namespace THOK.Wms.DownloadWms.Bll
{
    public class DownSortingInfoBll
    {
        //选择时间下载分拣数据
        public bool GetSortingOrderDate(string startDate, string endDate,string sortingLine, out string errorInfo)
        {
            bool tag = false;
            errorInfo = string.Empty;
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                try
                {
                    string sort = string.Empty;
                    if (sortingLine != string.Empty || sortingLine != null)
                    {
                        sort = " AND SORTINGLINECODE='" + sortingLine + "'";
                    }
                    //查询仓库7天内的订单号
                    DataTable orderdt = this.GetOrderId();
                    string orderlist = UtinString.StringMake(orderdt, "order_id");
                    orderlist = UtinString.StringMake(orderlist);
                    string orderlistDate = "OrderDate >='" + startDate + "' AND OrderDate <='" + endDate + "' AND OrderID NOT IN(" + orderlist + ")" + sort;
                    DataTable masterdt = this.GetSortingOrder(orderlistDate);//根据时间查询订单信息

                    string ordermasterlist = UtinString.StringMake(masterdt, "OrderID");//取得根据时间查询的订单号
                    string ordermasterid = UtinString.StringMake(ordermasterlist);
                    ordermasterid = "OrderID IN (" + ordermasterid + ")";
                    DataTable detaildt = this.GetSortingOrderDetail(ordermasterid);//根据订单号查询明细
                    if (masterdt.Rows.Count > 0 && detaildt.Rows.Count > 0)
                    {
                        DataSet masterds = this.SaveSortingOrder(masterdt);
                        DataSet detailds = this.SaveSortingOrderDetail(detaildt);
                        this.Insert(masterds, detailds);
                        if (sort != string.Empty)
                        {
                            try
                            {
                                DataRow[] masterDisp = masterds.Tables["WMS_SORT_ORDER"].Select("GROUP BY DELIVERLINECODE,ORDERDATE");
                                DataSet dispDs = this.SaveDispatch(masterDisp, sortingLine);
                                this.Insert(dispDs);
                                tag = true;
                            }
                            catch (Exception e)
                            {
                                errorInfo = "调度出错,请手动进行线路调度，出错原因：" + e.Message;
                            }
                            
                        }
                        else
                            errorInfo = "没有选择分拣线！下载完成后，请手动进行线路调度！";
                        //tag = true;
                    }
                    else
                        errorInfo = "没有可用的数据下载！";
                }
                catch (Exception e)
                {
                    errorInfo = "下载错误：" + e.Message;
                }
            }
            return tag;
        }

        //查询数仓3天内分拣订单
        public DataTable GetOrderId()
        {
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                return dao.GetOrderId();
            }
        }

        //下载分拣主表订单
        public DataTable GetSortingOrder(string parameter)
        {
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                //dao.SetPersistentManager(dbpm);
                return dao.GetSortingOrder(parameter);
            }
        }

        //下载分拣细表订单
        public DataTable GetSortingOrderDetail(string parameter)
        {
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                //dao.SetPersistentManager(dbpm);
                return dao.GetSortingDetail(parameter);
            }
        }

        //保存主表信息
        public DataSet SaveSortingOrder(DataTable masterdt)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in masterdt.Rows)
            {
                DataRow masterrow = ds.Tables["WMS_SORT_ORDER"].NewRow();
                masterrow["order_id"] = row["OrderID"].ToString().Trim();//订单编号
                masterrow["company_code"] = row["COMPANY_CODE"].ToString().Trim();//所属单位编号
                masterrow["sale_region_code"] = row["SALE_REGION_CODE"].ToString().Trim();//营销部编号
                masterrow["order_date"] = row["OrderDate"].ToString().Trim();//订单日期
                masterrow["order_type"] = row["SALE_SCOPE"].ToString().Trim();//订单类型
                masterrow["customer_code"] = row["CustomerCode"].ToString().Trim();//客户编号
                masterrow["customer_name"] = row["CustomerName"].ToString().Trim();//客户名称
                masterrow["quantity_sum"] = Convert.ToDecimal(row["QuantitySum"].ToString());//总数量
                masterrow["amount_sum"] = 0;//总金额
                masterrow["detail_num"] = 0;//明细数
                masterrow["deliver_order"] = row["DELIVER_ORDER"].ToString().Trim();
                masterrow["DeliverDate"] = DateTime.Now;
                masterrow["description"] = "";
                masterrow["is_active"] = "1";
                masterrow["update_time"] = DateTime.Now;
                masterrow["deliver_line_code"] = row["DeliverLineCode"].ToString().Trim() + "_" + row["DIST_BILL_ID"].ToString().Trim();//送货顺序编码
                ds.Tables["WMS_SORT_ORDER"].Rows.Add(masterrow);
            }
            return ds;
        }

        //保存细表信息
        public DataSet SaveSortingOrderDetail(DataTable detaildt)
        {
            //DownSortingInfoDao dao = new DownSortingInfoDao();
            //DataTable unitList = dao.GetUnitList();
            DataSet ds = this.GenerateEmptyTables();
            try
            {
                //int i = 0;
                foreach (DataRow row in detaildt.Rows)
                {
                    //DataRow[] list = unitList.Select(string.Format("unit_list_code='{0}'", row["ProductCode"].ToString().Trim()));
                    DataRow detailrow = ds.Tables["WMS_SORT_ORDER_DETAIL"].NewRow();
                    //i++;
                    detailrow["order_detail_id"] = row["OrderDetailID"].ToString().Trim();
                    detailrow["order_id"] = row["OrderID"].ToString().Trim();
                    detailrow["product_code"] = row["ProductCode"].ToString().Trim();
                    detailrow["product_name"] = row["ProductName"].ToString().Trim();
                    detailrow["unit_code"] = row["UNIT_CODE02"].ToString();
                    detailrow["unit_name"] = "条" ;
                    detailrow["demand_quantity"] = Convert.ToDecimal(row["RealQuantity"]);
                    detailrow["real_quantity"] = Convert.ToDecimal(row["RealQuantity"]);
                    detailrow["price"] = Convert.ToDecimal(row["TRADE_PRICE"]);
                    detailrow["amount"] = Convert.ToDecimal(row["AMOUNT_PRICE"]);
                    detailrow["unit_quantity"] = row["QUANTITY01"].ToString();
                    ds.Tables["WMS_SORT_ORDER_DETAIL"].Rows.Add(detailrow);
                }
                return ds;
            }
            catch (Exception e)
            {
                string s = e.Message;
                return null;
            }
        }

        //保存线路调度表信息
        public DataSet SaveDispatch(DataRow[] masterRow, string sortingLine)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in masterRow)
            {
                DataRow masterrow = ds.Tables["WMS_SORT_ORDER_DISPATCH"].NewRow();
                masterrow["order_date"] = row["OrderDate"].ToString().Trim();//订单时间
                masterrow["sorting_line_code"] = sortingLine;//调度分拣线
                masterrow["deliver_line_code"] = row["DeliverLineCode"].ToString().Trim();//调度线路编码
                masterrow["is_active"] = "1";//是否可用
                masterrow["update_time"] = DateTime.Now;//调度时间
                masterrow["sort_work_dispatch_id"] = null;//作业调度ID
                masterrow["work_status"] = "1";//调度状态
                ds.Tables["WMS_SORT_ORDER_DISPATCH"].Rows.Add(masterrow);
            }
            return ds;
        }

        //保存到数据库
        public void Insert(DataSet masterds, DataSet detailds)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                if (masterds.Tables["WMS_SORT_ORDER"].Rows.Count > 0)
                {
                    dao.InsertSortingOrder(masterds);
                }
                if (detailds.Tables["WMS_SORT_ORDER_DETAIL"].Rows.Count > 0)
                {
                    dao.InsertSortingOrderDetail(detailds);
                }
            }
        }

        //保存线路调度结果
        public void Insert(DataSet dispatchDs)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                if (dispatchDs.Tables["WMS_SORT_ORDER_DISPATCH"].Rows.Count > 0)
                {
                    dao.InsertDispatch(dispatchDs);
                }
            }
        }

        //生成虚拟表
        private DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable mastertable = ds.Tables.Add("WMS_SORT_ORDER");
            mastertable.Columns.Add("order_id");
            mastertable.Columns.Add("company_code");
            mastertable.Columns.Add("sale_region_code");
            mastertable.Columns.Add("order_date");
            mastertable.Columns.Add("order_type");
            mastertable.Columns.Add("customer_code");
            mastertable.Columns.Add("customer_name");
            mastertable.Columns.Add("quantity_sum");
            mastertable.Columns.Add("amount_sum");
            mastertable.Columns.Add("detail_num");
            mastertable.Columns.Add("deliver_order");
            mastertable.Columns.Add("DeliverDate");
            mastertable.Columns.Add("description");
            mastertable.Columns.Add("is_active");
            mastertable.Columns.Add("update_time");
            mastertable.Columns.Add("deliver_line_code");

            DataTable detailtable = ds.Tables.Add("WMS_SORT_ORDER_DETAIL");
            detailtable.Columns.Add("order_detail_id");
            detailtable.Columns.Add("order_id");
            detailtable.Columns.Add("product_code");
            detailtable.Columns.Add("product_name");
            detailtable.Columns.Add("unit_code");
            detailtable.Columns.Add("unit_name");
            detailtable.Columns.Add("demand_quantity");
            detailtable.Columns.Add("real_quantity");
            detailtable.Columns.Add("price");
            detailtable.Columns.Add("amount");
            detailtable.Columns.Add("unit_quantity");

            DataTable dispatchtable = ds.Tables.Add("WMS_SORT_ORDER_DISPATCH");
            detailtable.Columns.Add("id");
            detailtable.Columns.Add("order_date");
            detailtable.Columns.Add("sorting_line_code");
            detailtable.Columns.Add("deliver_line_code");
            detailtable.Columns.Add("is_active");
            detailtable.Columns.Add("update_time");
            detailtable.Columns.Add("sort_work_dispatch_id");
            detailtable.Columns.Add("work_status");
            return ds;
        }
    }
}
