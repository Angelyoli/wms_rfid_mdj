using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
using THOK.WMS.DownloadWms.Dao;
using System.Threading;
using THOK.WMS.Upload.Bll;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownSortingOrderBll
   {
        UploadBll upload = new UploadBll();
       #region ѡ�����ڴ�Ӫ��ϵͳ���طּ���Ϣ

       /// <summary>
       /// ѡ�����ڴ�Ӫ��ϵͳ���طּ���Ϣ
       /// </summary>
       /// <param name="orderDate"></param>
       /// <param name="endDate"></param>
       /// <returns></returns>
       public bool GetSortingOrderDate(string orderDate, string endDate, out string errorInfo)
       {
           bool tag = false;
           errorInfo=string.Empty;
           using (PersistentManager dbpm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               try
               {
                   //��ѯ�ֿ�7���ڵĶ�����
                   DataTable orderdt = this.GetOrderId(orderDate);
                   string orderlist = UtinString.MakeString(orderdt, "order_id");
                   string orderlistDate = "ORDER_DATE ='" + orderDate + "' AND ORDER_ID NOT IN(" + orderlist + ")";
                   DataTable masterdt = this.GetSortingOrder(orderlistDate);//����ʱ���ѯ������Ϣ

                   string ordermasterlist = UtinString.MakeString(masterdt, "ORDER_ID");//ȡ�ø���ʱ���ѯ�Ķ�����
                   ordermasterlist = "ORDER_ID IN (" + ordermasterlist + ")";
                   DataTable detaildt = this.GetSortingOrderDetail(ordermasterlist);//���ݶ����Ų�ѯ��ϸ
                   if (masterdt.Rows.Count > 0 && detaildt.Rows.Count > 0)
                   {
                       DataSet masterds = this.SaveSortingOrder(masterdt);
                       DataSet detailds = this.SaveSortingOrderDetail(detaildt);
                       this.Insert(masterds, detailds);
                       //�ϱ��ּ𶩵�
                       //upload.uploadSort(masterds, detailds);
                       tag = true;
                   }
                   else
                       errorInfo= "û�п��õ��������أ�";
               }
               catch (Exception e)
               {
                   errorInfo = "����" + e.Message;
               }
           }
           return tag;
       }
       /// <summary>
       /// ѡ�����ڴ�Ӫ��ϵͳ���طּ���Ϣ ����
       /// </summary>
       /// <param name="orderDate"></param>
       /// <param name="endDate"></param>
       /// <returns></returns>
       public bool GetSortingOrderDates(string orderDate, string endDate, out string errorInfo)
       {
           bool tag = false;
           errorInfo = string.Empty;
           using (PersistentManager dbpm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               try
               {
                   //��ѯ�ֿ�7���ڵĶ�����
                   DataTable orderdt = this.GetOrderId(orderDate);
                   string orderlist = UtinString.MakeString(orderdt, "order_id");
                   string orderlistDate = "ORDER_DATE ='" + orderDate + "' AND ORDER_ID NOT IN(" + orderlist + ")";
                   DataTable masterdt = this.GetSortingOrders(orderlistDate);//����ʱ���ѯ������Ϣ

                   string ordermasterlist = UtinString.MakeString(masterdt, "ORDER_ID");//ȡ�ø���ʱ���ѯ�Ķ�����
                   ordermasterlist = "ORDER_ID IN (" + ordermasterlist + ")";
                   DataTable detaildt = this.GetSortingOrderDetails(ordermasterlist);//���ݶ����Ų�ѯ��ϸ
                   if (masterdt.Rows.Count > 0 && detaildt.Rows.Count > 0)
                   {
                       DataSet masterds = this.SaveSortingOrders(masterdt);
                       DataSet detailds = this.SaveSortingOrderDetails(detaildt);
                       this.Insert(masterds, detailds);
                       //�ϱ��ּ𶩵�
                       //upload.uploadSort(masterds, detailds);
                       tag = true;
                   }
                   else
                       errorInfo = "û�п��õ��������أ�";
               }
               catch (Exception e)
               {
                   errorInfo = "����" + e.Message;
               }
           }
           return tag;
       }
       /// <summary>
       /// ��ѯ4��֮�ڵķּ𶩵�
       /// </summary>
       /// <returns></returns>
       public DataTable GetOrderId(string orderDate)
       {
           using (PersistentManager dbpm = new PersistentManager())
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               return dao.GetOrderId(orderDate);
           }
       }

       /// <summary>
       /// �����û�ѡ��Ķ������طּ��߶�������
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
       /// �����û�ѡ��Ķ������طּ��߶������� ����
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrders(string orderid)
       {
           using (PersistentManager dbpm = new PersistentManager("YXConnection"))
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               dao.SetPersistentManager(dbpm);
               return dao.GetSortingOrders(orderid);
           }
       }

       /// <summary>
       /// �����û�ѡ��Ķ������طּ��߶�����ϸ��
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
       /// <summary>
       /// �����û�ѡ��Ķ������طּ��߶�����ϸ�� ����
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrderDetails(string orderid)
       {
           using (PersistentManager dbpm = new PersistentManager("YXConnection"))
           {
               DownSortingOrderDao dao = new DownSortingOrderDao();
               dao.SetPersistentManager(dbpm);
               return dao.GetSortingOrderDetails(orderid);
           }
       }
       /// <summary>
       /// ���涩��������Ϣ���������������DATATABLE
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       public DataSet SaveSortingOrder(DataTable masterdt)
       {
           DataSet ds = this.GenerateEmptyTables();
           foreach (DataRow row in masterdt.Rows)
           {
               DataRow masterrow = ds.Tables["DWV_OUT_ORDER"].NewRow();
               masterrow["order_id"] = row["ORDER_ID"].ToString().Trim();//�������
               masterrow["company_code"] = row["ORG_CODE"].ToString().Trim();//������λ���
               masterrow["sale_region_code"] = row["SALE_REG_CODE"].ToString().Trim();//Ӫ�������
               masterrow["order_date"] = row["ORDER_DATE"].ToString().Trim();//��������
               masterrow["order_type"] = row["ORDER_TYPE"].ToString().Trim();//��������
               masterrow["customer_code"] = row["CUST_CODE"].ToString().Trim();//�ͻ����
               masterrow["customer_name"] = row["CUST_NAME"].ToString().Trim();//�ͻ�����
               masterrow["quantity_sum"] = Convert.ToDecimal(row["QUANTITY_SUM"].ToString());//������
               masterrow["amount_sum"] = Convert.ToDecimal(row["AMOUNT_SUM"].ToString());//�ܽ��
               masterrow["detail_num"] = Convert.ToInt32(row["DETAIL_NUM"].ToString());//��ϸ��
               masterrow["deliver_order"] = row["DELIVER_ORDER"].ToString().Trim();//�䳵����
               masterrow["DeliverDate"] = row["ORDER_TYPE"].ToString().Trim();//�ͻ��������
               masterrow["description"] = row["DIST_BILL_ID"].ToString().Trim();//�ͻ���������
               masterrow["is_active"] = row["ISACTIVE"].ToString().Trim();//�ͻ���·����
               masterrow["update_time"] = DateTime.Now;//�ͻ���·����               
               masterrow["deliver_line_code"] = row["DIST_BILL_ID"].ToString().Trim(); //row["DELIVER_LINE_CODE"].ToString().Trim();// +"_" + row["DIST_BILL_ID"].ToString().Trim();//�ͻ�˳�����
               masterrow["dist_bill_id"] = row["DIST_BILL_ID"].ToString().Trim();//
               ds.Tables["DWV_OUT_ORDER"].Rows.Add(masterrow);
           }
           return ds;
       }

       /// <summary>
       /// ���涩��������Ϣ�����������
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       public DataSet SaveSortingOrders(DataTable masterdt)
       {
           DataSet ds = this.GenerateEmptyTables();
           foreach (DataRow row in masterdt.Rows)
           {
               DataRow masterrow = ds.Tables["DWV_OUT_ORDER"].NewRow();
               masterrow["order_id"] = row["ORDER_ID"].ToString().Trim();//�������
               masterrow["company_code"] = row["ORG_CODE"].ToString().Trim();//������λ���
               masterrow["sale_region_code"] = row["SALE_REG_CODE"].ToString().Trim();//Ӫ�������
               masterrow["order_date"] = row["ORDER_DATE"].ToString().Trim();//��������
               masterrow["order_type"] = row["ORDER_TYPE"].ToString().Trim();//��������
               masterrow["customer_code"] = row["CUST_CODE"].ToString().Trim();//�ͻ����
               masterrow["customer_name"] = row["CUST_NAME"].ToString().Trim();//�ͻ�����
               masterrow["quantity_sum"] = Convert.ToDecimal(row["QUANTITY_SUM"].ToString());//������
               masterrow["amount_sum"] = Convert.ToDecimal(row["AMOUNT_SUM"].ToString());//�ܽ��
               masterrow["detail_num"] = Convert.ToInt32(row["DETAIL_NUM"].ToString());//��ϸ��
               masterrow["deliver_order"] = row["DELIVER_ORDER"].ToString().Trim();//�䳵����
               masterrow["DeliverDate"] = row["ORDER_TYPE"].ToString().Trim();//�ͻ��������
               masterrow["description"] = row["DIST_BILL_ID"].ToString().Trim();//�ͻ���������
               masterrow["is_active"] = row["ISACTIVE"].ToString().Trim();//�ͻ���·����
               masterrow["update_time"] = DateTime.Now;//�ͻ���·����               
               masterrow["deliver_line_code"] = row["DELIVER_LINE_CODE"].ToString().Trim();// row["DIST_BILL_ID"].ToString().Trim(); //row["DELIVER_LINE_CODE"].ToString().Trim();// +"_" + row["DIST_BILL_ID"].ToString().Trim();//�ͻ�˳�����
               masterrow["dist_bill_id"] = row["DIST_BILL_ID"].ToString().Trim();//
               ds.Tables["DWV_OUT_ORDER"].Rows.Add(masterrow);
           }
           return ds;
       }
       /// <summary>
       /// ���涩����ϸ�����������DataTable
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       public DataSet SaveSortingOrderDetail(DataTable detaildt)
       {
           DownSortingOrderDao dao = new DownSortingOrderDao();
           DataTable unitList = dao.GetUnitProduct();
           DataSet ds = this.GenerateEmptyTables();
           try
           {
               int i = 0;
               foreach (DataRow row in detaildt.Rows)
               {
                   DataRow[] list = unitList.Select(string.Format("unit_list_code='{0}'", row["BRAND_N"].ToString().Trim()));
                   DataRow detailrow = ds.Tables["DWV_OUT_ORDER_DETAIL"].NewRow();
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

       /// <summary>
       /// ���涩����ϸ�����������
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       public DataSet SaveSortingOrderDetails(DataTable detaildt)
       {
           DownSortingOrderDao dao = new DownSortingOrderDao();
           DataTable unitList = dao.GetUnitProduct();
           DataSet ds = this.GenerateEmptyTables();
           try
           {
               int i = 0;
               foreach (DataRow row in detaildt.Rows)
               {
                   DataRow detailrow = ds.Tables["DWV_OUT_ORDER_DETAIL"].NewRow();
                   i++;
                   detailrow["order_detail_id"] = row["ORDER_DETAIL_ID"].ToString().Trim() + i;
                   detailrow["order_id"] = row["ORDER_ID"].ToString().Trim();
                   detailrow["product_code"] = row["BRAND_CODE"].ToString().Trim();
                   detailrow["product_name"] = row["BRAND_NAME"].ToString().Trim();
                   detailrow["unit_code"] = row["BRAND_UNIT_CODE"].ToString();
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
       /// �����ص�������ӵ����ݿ⡣
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
       /// �������������ϸ�������
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
           mastertable.Columns.Add("dist_bill_id");
           
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
   }
}
