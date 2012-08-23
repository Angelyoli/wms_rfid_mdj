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
        public Product[] GetProduct(string productCodes)
        {
            using (SortingDbContext sortdb = new SortingDbContext())
            {
                return sortdb.Database.SqlQuery<Product>(@"
                    SELECT [BRAND_CODE] AS ProductCode
                            ,[BRAND_NAME] AS ProductName
                            ,[N_UNIFY_CODE] AS UniformCode
                            ,[BRAND_N] AS CustomCode
                            ,[SHORT_CODE] AS ShortCode
                            ,[''] AS UnitListCode
                            ,[UNIT_CODE] AS UnitCode
                            ,[''] AS SupplierCode
                            ,[''] AS BrandCode
                            ,[''] AS AbcTypeCode
                            ,[PRICE_LEVEL_CODE] AS ProductTypeCode
                            ,[''] AS PackTypeCode
                            ,[PRICE_LEVEL_CODE] AS PriceLevelCode
                            ,[''] AS StatisticType
                            ,[BARCODE_PIECE] AS PieceBarcode
                            ,[BARCODE_BAR] AS BarBarcode
                            ,[BARCODE_PACKAGE] AS PackageBarcode
                            ,[''] AS OneProjectBarcode
                            ,[BUY_PRICE] AS BuyPrice
                            ,[TRADE_PRICE] AS TradePrice
                            ,[RETAIL_PRICE] AS RetailPrice
                            ,[COST_PRICE] AS CostPrice
                            ,[IS_FILTERTIP] AS IsFilterTip
                            ,[IS_NEW] AS IsNew
                            ,[IS_FAMOUS] AS IsFamous
                            ,[IS_MAINPRODUCT] AS IsMainProduct
                            ,[IS_MAINPROVINCE] AS IsProvinceMainProduct
                            ,[BELONG_REGION] AS BelongRegion
                            ,[IS_CONFISCATE] AS IsConfiscate
                            ,[IS_ABNORMITY_BRAND] AS IsAbnormity
                            ,[''] AS Description
                            ,[ISACTIVE] AS IsActive
                            ,[UPDATE_DATE] AS UpdateTime
                        FROM [V_WMS_BRAND]
                    ").ToArray();
            }
        }

        public Supplier[] GetSupplier(string SupplierCodes)
        {
            using (SortingDbContext sortdb = new SortingDbContext())
            {
                return sortdb.Database.SqlQuery<Supplier>(@"
                    SELECT [FACTORY_CODE] AS SupplierCode
                            ,[N_FACTORY_CODE] AS UniformCode
                            ,[FACTORY_N] AS CustomCode
                            ,[FACTORY_NAME] AS SupplierName
                            ,[PROVINCE_NAME] AS ProvinceName
                            ,[ISACTIVE] AS IsActive
                            ,getdate() AS UpdateTime
                        FROM [V_WMS_FACTORY]
                    ").ToArray();
            }
        }
    }
}
