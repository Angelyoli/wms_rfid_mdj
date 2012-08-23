using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Download.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Download.Service
{
    public class ProductDownService : IProductDownService
    {
        public Product[] GetProduct()
        {
            using (SortingDbContext sortdb = new SortingDbContext())
            {
                return sortdb.Database.SqlQuery<Product>(@"
                    SELECT [BRAND_CODE] AS ProductCode
                            ,[BRAND_NAME] AS ProductName
                            ,[N_UNIFY_CODE] AS UniformCode
                            ,[BRAND_N] AS CustomCode
                            ,[SHORT_CODE] AS ShortCode
                            ,[customer_code] AS UnitListCode
                            ,[customer_name] AS UnitCode
                            ,[quantity_sum] AS SupplierCode
                            ,[amount_sum] AS BrandCode
                            ,[detail_num] AS AbcTypeCode
                            ,[deliver_order] AS ProductTypeCode
                            ,[DeliverDate] AS PackTypeCode
                            ,[description] AS PriceLevelCode
                            ,[is_active] AS StatisticType
                            ,[update_time] AS PieceBarcode
                            ,[deliver_line_code] AS BarBarcode
                             ,[deliver_line_code] AS PackageBarcode
                             ,[deliver_line_code] AS OneProjectBarcode
                             ,[deliver_line_code] AS BuyPrice
                             ,[deliver_line_code] AS TradePrice
                             ,[deliver_line_code] AS RetailPrice
                             ,[deliver_line_code] AS CostPrice
                             ,[deliver_line_code] AS IsFilterTip
                             ,[deliver_line_code] AS IsNew
                             ,[deliver_line_code] AS IsFamous
                             ,[deliver_line_code] AS IsMainProduct
                             ,[deliver_line_code] AS IsProvinceMainProduct
                             ,[deliver_line_code] AS BelongRegion
                             ,[deliver_line_code] AS IsConfiscate
                             ,[deliver_line_code] AS IsAbnormity
                             ,[deliver_line_code] AS Description
                             ,[deliver_line_code] AS IsActive
                             ,[deliver_line_code] AS UpdateTime
                        FROM [V_WMS_BRAND]
                    "
                    )
                    .ToArray();
            }
        }
    }
}
