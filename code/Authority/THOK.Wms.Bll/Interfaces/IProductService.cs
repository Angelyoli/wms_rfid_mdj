﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IProductService:IService<Product>
    {
        object GetDetails(int page, int rows, string ProductName, string ProductCode, string CustomCode, string BrandCode, string UniformCode, string AbcTypeCode, string ShortCode, string PriceLevelCode, string SupplierCode);
        
        bool Add(Product product);
        
        bool Delete(string ProductCode);
        
        bool Save(Product product);

        object OutBillFindProduct(string QueryString, string value);

        object FindProduct(int page, int rows, string QueryString, string value);

        object checkFindProduct(string QueryString, string value);

        object FindProduct();

        object LoadProduct(int page, int rows);

        object GetProductBy(int page, int rows, string QueryString, string Value);

        System.Data.DataTable GetProduct(int page, int rows, string ProductName, string ProductCode, string CustomCode, string BrandCode, string UniformCode, string AbcTypeCode, string ShortCode, string PriceLevelCode, string SupplierCode);
    }
}
