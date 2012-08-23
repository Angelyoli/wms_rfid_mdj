using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Download.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Download.Service
{
   public class OutBillMasterDownService :IOutBillMasterDownService
    {
        public OutBillMaster[] GetOutBillMaster(string outBillMasters)
        {
            using (SortingDbContext sortdb = new SortingDbContext())
            {
                return sortdb.Database.SqlQuery<OutBillMaster>(@"
                    SELECT [ORDER_ID] AS BillNo
                            ,[ORDER_DATE] AS BillDate
                            ,[BILL_TYPE] AS BillTypeCode
                            ,['1'] AS Origin  
                            ,['1'] AS Status
                            ,[''] AS Description
                            ,[ISACTIVE] AS IsActive
                            ,[UPDATE_DATE] AS UpdateTime
                        FROM [V_WMS_OUT_ORDER]
                    ").ToArray();
            }
        }

        public OutBillDetail[] GetOutBillDetail(string outBillMasters)
        {
            using (SortingDbContext sortdb = new SortingDbContext())
            {
                return sortdb.Database.SqlQuery<OutBillDetail>(@"
                    SELECT [ORDER_ID] AS BillNo
                            ,[BRAND_CODE] AS ProductCode
                            ,[PRICE] AS Price
                            ,[QUANTITY] AS BillQuantity
                            ,[0] AS AllotQuantity
                            ,[0] AS RealQuantity
                            ,[''] AS Description
                        FROM [V_WMS_OUT_ORDER_DETAIL]
                    ").ToArray();
            }
        }
    }
}
