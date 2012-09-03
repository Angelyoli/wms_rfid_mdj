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
        public bool GetSortingOrderDate(string startDate, string endDate,string ware, out string errorInfo)
        {
            bool tag = false;
            errorInfo = string.Empty;
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                try
                {
                    //查询仓库7天内的订单号
                    DataTable orderdt = this.GetOrderId();
                    string orderlist = UtinString.StringMake(orderdt, "order_id");
                    orderlist = UtinString.StringMake(orderlist);
                    string orderlistDate = "ORDER_DATE>='" + startDate + "' AND ORDER_DATE<='" + endDate + "' AND ORDER_ID NOT IN(" + orderlist + ")";
                    DataTable masterdt = this.GetSortingOrder(orderlistDate);//根据时间查询订单信息

                    string ordermasterlist = UtinString.StringMake(masterdt, "ORDER_ID");//取得根据时间查询的订单号
                    string ordermasterid = UtinString.StringMake(ordermasterlist);
                    ordermasterid = "ORDER_ID IN (" + ordermasterid + ")";
                    DataTable detaildt = this.GetSortingOrderDetail(ordermasterid);//根据订单号查询明细
                    if (masterdt.Rows.Count > 0 && detaildt.Rows.Count > 0)
                    {
                        DataSet masterds = this.SaveSortingOrder(masterdt);
                        DataSet detailds = this.SaveSortingOrderDetail(detaildt);
                        this.Insert(masterds, detailds);
                        tag = true;
                    }
                    else
                        errorInfo = "没有可用的数据下载！";
                }
                catch (Exception e)
                {
                    errorInfo = "错误：" + e.Message;
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
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                dao.SetPersistentManager(dbpm);
                return dao.GetSortingOrder(parameter);
            }
        }

        //下载分拣细表订单
        public DataTable GetSortingOrderDetail(string parameter)
        {
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownSortingInfoDao dao = new DownSortingInfoDao();
                dao.SetPersistentManager(dbpm);
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
                masterrow["order_id"] = row["ORDER_ID"].ToString().Trim();//订单编号
                masterrow["company_code"] = row["ORG_CODE"].ToString().Trim();//所属单位编号
                masterrow["sale_region_code"] = row["SALE_REG_CODE"].ToString().Trim();//营销部编号
                masterrow["order_date"] = row["ORDER_DATE"].ToString().Trim();//订单日期
                masterrow["order_type"] = row["ORDER_TYPE"].ToString().Trim();//订单类型
                masterrow["customer_code"] = row["CUST_CODE"].ToString().Trim();//客户编号
                masterrow["customer_name"] = row["CUST_NAME"].ToString().Trim();//客户名称
                masterrow["quantity_sum"] = Convert.ToDecimal(row["QUANTITY_SUM"].ToString());//总数量
                masterrow["amount_sum"] = Convert.ToDecimal(row["AMOUNT_SUM"].ToString());//总金额
                masterrow["detail_num"] = Convert.ToInt32(row["DETAIL_NUM"].ToString());//明细数
                masterrow["deliver_order"] = row["DELIVER_ORDER"].ToString().Trim();//配车单号
                masterrow["DeliverDate"] = row["ORDER_TYPE"].ToString().Trim();//送货区域编码
                masterrow["description"] = "";//送货区域名称
                masterrow["is_active"] = row["ISACTIVE"].ToString().Trim();//送货线路编码
                masterrow["update_time"] = DateTime.Now;//送货线路名称
                masterrow["deliver_line_code"] = row["DELIVER_LINE_CODE"].ToString().Trim() + "_" + row["DIST_BILL_ID"].ToString().Trim();//送货顺序编码
                ds.Tables["DWV_OUT_ORDER"].Rows.Add(masterrow);
            }
            return ds;
        }

        //保存细表信息
        public DataSet SaveSortingOrderDetail(DataTable detaildt)
        {
            DownSortingInfoDao dao = new DownSortingInfoDao();
            DataTable unitList = dao.GetUnitList();
            DataSet ds = this.GenerateEmptyTables();
            try
            {
                int i = 0;
                foreach (DataRow row in detaildt.Rows)
                {
                    DataRow[] list = unitList.Select(string.Format("unit_list_code='{0}'", row["BRAND_N"].ToString().Trim()));
                    DataRow detailrow = ds.Tables["WMS_SORT_ORDER_DETAIL"].NewRow();
                    i++;
                    detailrow["order_detail_id"] = row["ORDER_DETAIL_ID"].ToString().Trim() + i;
                    detailrow["order_id"] = row["ORDER_ID"].ToString().Trim();
                    detailrow["product_code"] = row["BRAND_N"].ToString().Trim();
                    detailrow["product_name"] = row["BRAND_NAME"].ToString().Trim();
                    detailrow["unit_code"] = list[0]["unit_code02"].ToString();
                    detailrow["unit_name"] = row["BRAND_UNIT_NAME"].ToString().Trim(); ;
                    detailrow["demand_quantity"] = Convert.ToDecimal(row["QUANTITY"]);
                    detailrow["real_quantity"] = Convert.ToDecimal(row["QUANTITY"]);
                    detailrow["price"] = Convert.ToDecimal(row["PRICE"]);
                    detailrow["amount"] = Convert.ToDecimal(row["AMOUNT"]);
                    detailrow["unit_quantity"] = 50;
                    ds.Tables["DWV_OUT_ORDER_DETAIL"].Rows.Add(detailrow);
                }
                return ds;
            }
            catch (Exception e)
            {
                string s = e.Message;
                return null;
            }
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
            return ds;
        }
    }
}
