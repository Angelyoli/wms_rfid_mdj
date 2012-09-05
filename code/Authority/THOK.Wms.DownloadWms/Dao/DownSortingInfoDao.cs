﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.Wms.DownloadWms.Dao
{
    public class DownSortingInfoDao : BaseDao
    {
        public DataTable GetSortingOrder(string parameter)
        {
            string sql = string.Format(@"SELECT S.*,C.COMPANY_CODE,C.SALE_REGION_CODE,C.SALE_SCOPE,C.DELIVER_ORDER FROM WMS_SORT_ORDER S
                                            LEFT JOIN WMS_CUSTOMER C ON S.CUSTOMER_CODE=C.CUSTOMER_CODE WHERE {0}", parameter);
            return this.ExecuteQuery(sql).Tables[0];
        }

        public DataTable GetSortingDetail(string parameter)
        {
            string sql = string.Format(@"SELECT S.*,P.TRADE_PRICE,(P.TRADE_PRICE*REALQUANTITY) AS AMOUNT_PRICE,
                                                    U.UNIT_CODE02,U.QUANTITY01 FROM WMS_SORT_ORDER_DETAIL S
                                                    LEFT JOIN WMS_PRODUCT P ON S.PRODUCT_CODE=P.PRODUCT_CODE
                                                    LEFT JOIN WMS_UNIT_LIST U ON P.UNIT_LIST_CODE=U.UNIT_LIST_CODE WHERE {0}", parameter);
            return this.ExecuteQuery(sql).Tables[0];
        }
        
        public void InsertSortingOrder(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_SORT_ORDER"], "WMS_SORT_ORDER");
        }
       
        public void InsertSortingOrderDetail(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_SORT_ORDER_DETAIL"], "WMS_SORT_ORDER_DETAIL");
        }

        public void InsertDispatch(DataSet ds)
        {
            BatchInsert(ds.Tables["WMS_SORT_ORDER_DISPATCH"], "WMS_SORT_ORDER_DISPATCH");
        }

        public DataTable GetOrderId()
        {
            string sql = " SELECT ORDER_ID FROM WMS_SORT_ORDER WHERE ORDER_DATE>DATEADD(DAY, -3, CONVERT(VARCHAR(14), GETDATE(), 112)) ";
            return this.ExecuteQuery(sql).Tables[0];
        }

        public DataTable GetUnitList()
        {
            string sql = "SELECT * FROM WMS_UNIT_LIST";
            return this.ExecuteQuery(sql).Tables[0];
        }
    }
}