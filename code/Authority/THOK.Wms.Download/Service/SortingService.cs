using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Download.Service
{
    public class SortingService
    {
        public SortOrder[] GetSortOrder()
        {
            using (SortingDbContext sortdb = new SortingDbContext())
            {
                return sortdb.Database.SqlQuery<SortOrder>(@"
                    SELECT [order_id] AS OrderID
                            ,[company_code] AS CompanyCode
                            ,[sale_region_code] AS SaleRegionCode
                            ,[order_date] AS OrderDate
                            ,[order_type] AS OrderType
                            ,[customer_code] AS CustomerCode
                            ,[customer_name] AS CustomerName
                            ,[quantity_sum] AS QuantitySum
                            ,[amount_sum] AS AmountSum
                            ,[detail_num] AS DetailNum
                            ,[deliver_order] AS DeliverOrder
                            ,[DeliverDate] AS DeliverDate
                            ,[description] AS Description
                            ,[is_active] AS IsActive
                            ,[update_time] AS UpdateTime
                            ,[deliver_line_code] AS DeliverLineCode
                        FROM [dbo].[wms_sort_order]
                    "
                    )
                    .ToArray();
            }
            return null;
        }
    }
}
