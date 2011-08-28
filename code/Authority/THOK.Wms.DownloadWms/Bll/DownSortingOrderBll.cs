using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
using THOK.WMS.DownloadWms.Dao;
using System.Threading;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownSortingOrderBll
   {

       #region 手动从营销系统下载分拣信息

       /// <summary>
       /// 手动下载
       /// </summary>
       /// <param name="billno"></param>
       /// <returns></returns>
       public string GetSortingOrderById(string orderid)
       {
           string tag = "true";
           using (PersistentManager dbPm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               try
               {
                   orderid = "ORDER_ID IN (" + orderid + ")";
                   DataTable masterdt = this.GetSortingOrder(orderid);
                   DataTable detaildt = this.GetSortingOrderDetail(orderid);
                   if (masterdt.Rows.Count > 0 && detaildt.Rows.Count>0)
                   {
                       DataSet masterds = this.SaveSortingOrder(masterdt);
                       DataSet detailds = this.SaveSortingOrderDetail(detaildt);
                       this.Insert(masterds, detailds);
                   }
                   else
                       return "没有数据可下载！";
               }
               catch (Exception e)
               {
                   tag = e.Message;
               }
           }
           return tag;
       }
       #endregion

       #region 自动从营销系统下载分拣信息

       /// <summary>
       /// 自动下载订单
       /// </summary>
       /// <returns></returns>
       public string DownSortingOrder()
       {
           string tag = "true";
           using (PersistentManager dbpm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               try
               {
                   DataTable orderdt = this.GetOrderId();
                   string orderlist = UtinString.StringMake(orderdt, "ORDER_ID");
                   orderlist = UtinString.StringMake(orderlist);
                   orderlist = "ORDER_ID NOT IN(" + orderlist + ")";

                   DataTable masterdt = this.GetSortingOrder(orderlist);
                   DataTable detaildt = this.GetSortingOrderDetail(orderlist);
                   if (masterdt.Rows.Count > 0 && detaildt.Rows.Count > 0)
                   {
                       DataSet masterds = this.SaveSortingOrder(masterdt);
                       DataSet detailds = this.SaveSortingOrderDetail(detaildt);
                       this.Insert(masterds, detailds);
                   }
                   else
                       return "没有可用的数据下载！";
               }
               catch (Exception e)
               {
                   tag = e.Message;
               }
           }
           return tag;
       }

       #endregion

       #region 选择日期从营销系统下载分拣信息

       /// <summary>
       /// 选择日期从营销系统下载分拣信息
       /// </summary>
       /// <param name="startDate"></param>
       /// <param name="endDate"></param>
       /// <returns></returns>
       public bool GetSortingOrderDate(string startDate, string endDate, out string errorInfo)
       {
           bool tag = false;
           errorInfo=string.Empty;
           using (PersistentManager dbpm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
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
                       errorInfo= "没有可用的数据下载！";
               }
               catch (Exception e)
               {
                   errorInfo = "错误：" + e.Message;
               }
           }
           return tag;
       }

       #endregion

       #region 分页从营销系统据查询分拣订单数据

       /// <summary>
       /// 查询营销系统分拣订单主表数据进行下载
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
       public DataTable GetSortingOrder(int pageIndex, int pageSize)
       {
           using (PersistentManager dbpm = new PersistentManager("YXConnection"))
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               dao.SetPersistentManager(dbpm);
               DataTable orderdt = this.GetOrderId();
               string orderlist = UtinString.StringMake(orderdt, "ORDER_ID");
               orderlist = UtinString.StringMake(orderlist);
               orderlist = "ORDER_ID NOT IN(" + orderlist + ")";
               return dao.GetSortingOrder(orderlist);
           }
       }

       /// <summary>
       /// 根据时间查询营销系统分拣订单主表数据进行下载
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
       public DataTable GetSortingOrder(int pageIndex, int pageSize,string startDate,string endDate)
       {
           using (PersistentManager dbpm = new PersistentManager("YXConnection"))
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               dao.SetPersistentManager(dbpm);
               DataTable orderdt = this.GetOrderId();
               string orderlist = UtinString.StringMake(orderdt, "ORDER_ID");
               orderlist = UtinString.StringMake(orderlist);
               //orderlist = "ORDER_DATE>='" + startDate + "' AND ORDER_DATE<='" + endDate + "' AND ORDER_ID NOT IN(" + orderlist + ")";
               orderlist = "ORDER_ID NOT IN(" + orderlist + ")";
               DataTable masert = dao.GetSortingOrder("ORDER_ID NOT IN('')");
               DataRow[] orderdr = masert.Select(orderlist);
               return this.SaveSortingOrder(orderdr).Tables[0];
           }
       }

       /// <summary>
       /// 查询营销系统分拣明细表数据
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="inBillNo"></param>
       /// <returns></returns>
       public DataTable GetSortingOrderDetail(int pageIndex, int pageSize, string inBillNo)
       {
           using (PersistentManager dbpm = new PersistentManager("YXConnection"))
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               dao.SetPersistentManager(dbpm);
               inBillNo = "ORDER_ID = '" + inBillNo + "'";
               return dao.GetSortingOrderDetail(inBillNo);
           }
       }

       #endregion

       #region 查询仓储分拣信息

       /// <summary>
       /// 查询4天之内的分拣订单
       /// </summary>
       /// <returns></returns>
       public DataTable GetOrderId()
       {
           using (PersistentManager dbpm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               return dao.GetOrderId();
           }
       }

       /// <summary>
       /// 获取配送中心编码
       /// </summary>
       /// <returns></returns>
       public string GetCompany()
       {
           using (PersistentManager dbpm = new PersistentManager())
           {
               DownDistDao dao = new DownDistDao();
               return dao.GetCompany().ToString();
           }
       }
       #endregion

       #region 构建表等信息

       /// <summary>
       /// 把下载的数据添加到数据库。
       /// </summary>
       /// <param name="masterds"></param>
       /// <param name="detailds"></param>
       public void Insert(DataSet masterds, DataSet detailds)
       {
           using (PersistentManager pm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               if (masterds.Tables["DWV_OUT_ORDER"].Rows.Count > 0)
               {
                   dao.InsertSortingOrder(masterds);
               }
               if (detailds.Tables["DWV_OUT_ORDER_DETAIL"].Rows.Count > 0)
               {
                   dao.InsertSortingOrderDetail(detailds);
               }
           }
       }

       /// <summary>
       /// 保存订单主表信息到虚拟表，传来的是DATATABLE
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       public DataSet SaveSortingOrder(DataTable masterdt)
       {
           DataSet ds = this.GenerateEmptyTables();
           foreach (DataRow row in masterdt.Rows)
           {
               DataRow masterrow = ds.Tables["DWV_OUT_ORDER"].NewRow();
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


       /// <summary>
       /// 保存订单主表信息到虚拟表，传来的是DATAROW
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       public DataSet SaveSortingOrder(DataRow[] masterdr)
       {
           DataSet ds = this.GenerateEmptyTables();
           foreach (DataRow row in masterdr)
           {
               DataRow masterrow = ds.Tables["DWV_OUT_ORDER"].NewRow();
               masterrow["ORDER_ID"] = row["ORDER_ID"].ToString().Trim();//订单编号
               masterrow["ORG_CODE"] = row["ORG_CODE"].ToString().Trim();//所属单位编号
               masterrow["SALE_REG_CODE"] = row["SALE_REG_CODE"].ToString().Trim();//营销部编号
               masterrow["ORDER_DATE"] = row["ORDER_DATE"].ToString().Trim();//订单日期
               masterrow["ORDER_TYPE"] = row["ORDER_TYPE"].ToString().Trim();//订单类型
               masterrow["CUST_CODE"] = row["CUST_CODE"].ToString().Trim();//客户编号
               masterrow["CUST_NAME"] = row["CUST_NAME"].ToString().Trim();//客户名称
               masterrow["QUANTITY_SUM"] = Convert.ToDecimal(row["QUANTITY_SUM"].ToString());//总数量
               masterrow["AMOUNT_SUM"] = Convert.ToDecimal(row["AMOUNT_SUM"].ToString());//总金额
               masterrow["DETAIL_NUM"] = Convert.ToInt32(row["DETAIL_NUM"].ToString());//明细数
               masterrow["DIST_BILL_ID"] = row["DIST_BILL_ID"].ToString().Trim();//配车单号
               masterrow["DIST_STA_CODE"] =  row["DIST_STA_CODE"].ToString().Trim();//送货区域编码
               masterrow["DIST_STA_NAME"] =  row["DIST_STA_NAME"].ToString().Trim();//送货区域名称
               masterrow["DELIVER_LINE_CODE"] = row["DELIVER_LINE_CODE"].ToString().Trim();//送货线路编码
               masterrow["DELIVER_LINE_NAME"] =  row["DELIVER_LINE_NAME"].ToString().Trim();//送货线路名称
               masterrow["DELIVER_ORDER"] = row["DELIVER_ORDER"];//送货顺序编码
               masterrow["ISACTIVE"] = row["ISACTIVE"];//是否可用
               masterrow["UPDATE_DATE"] = row["UPDATE_DATE"];//更新时间
               masterrow["SORTING_CODE"] = "";//分拣组号
               masterrow["IS_IMPORT"] = "0";
               masterrow["ISSORTING"] = "0";
               ds.Tables["DWV_OUT_ORDER"].Rows.Add(masterrow);
           }
           return ds;
       }

       /// <summary>
       /// 保存订单明细到虚拟表，传来DataTable
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       public DataSet SaveSortingOrderDetail(DataTable detaildt)
       {
           DownProductBll pbll=new DownProductBll();
           DownSortingOrderDao dao = new DownSortingOrderDao();
           DataTable unitList = dao.GetUnitProduct();
           DataSet ds = this.GenerateEmptyTables();
           try
           {
               int i=0;
               foreach (DataRow row in detaildt.Rows)
               {
                   DataRow[] list = unitList.Select(string.Format("unit_list_code='{0}'", row["BRAND_N"].ToString().Trim()));
                   //DataTable product = pbll.FindProductInfo(row["BRAND_CODE"].ToString());
                   DataRow detailrow = ds.Tables["DWV_OUT_ORDER_DETAIL"].NewRow();
                   i++;
                   detailrow["order_detail_id"] = row["ORDER_DETAIL_ID"].ToString().Trim() + i;
                   detailrow["order_id"] = row["ORDER_ID"].ToString().Trim();
                   detailrow["product_code"] = row["BRAND_N"].ToString().Trim();
                   detailrow["product_name"] = row["BRAND_NAME"].ToString().Trim();
                   detailrow["unit_code"] = list[0]["unit_code02"].ToString();// product.Rows[0]["unit_code02"].ToString();
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

       /// <summary>
       /// 构建订单主表和细表虚拟表
       /// </summary>
       /// <returns></returns>
       private DataSet GenerateEmptyTables()
       {
           DataSet ds = new DataSet();
           DataTable mastertable = ds.Tables.Add("DWV_OUT_ORDER");
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

           DataTable detailtable = ds.Tables.Add("DWV_OUT_ORDER_DETAIL");
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
       #endregion

       #region 其他查询下载分拣数据的方法


       /// <summary>
       /// 根据用户选择的订单下载分拣线订单主表
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrder(string orderid)
       {
           using (PersistentManager dbpm = new PersistentManager("YXConnection"))
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               dao.SetPersistentManager(dbpm);
               return dao.GetSortingOrder(orderid);
           }
       }

       /// <summary>
       /// 根据用户选择的订单下载分拣线订单明细表
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrderDetail(string orderid)
       {
           using (PersistentManager dbpm = new PersistentManager("YXConnection"))
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               dao.SetPersistentManager(dbpm);
               return dao.GetSortingOrderDetail(orderid);
           }
       }

       #endregion

   }
}
