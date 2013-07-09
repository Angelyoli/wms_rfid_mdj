﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class BrandService : ServiceBase<Brand>, IBrandService
    {
        [Dependency]
        public IBrandRepository BrandRepository { get; set; }

        [Dependency]
        public ISupplierRepository SupplierRepository { get; set; }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IBrandService 成员

        public object GetDetails(int page, int rows, string BrandCode, string BrandName, string IsActive)
        {
            IQueryable<Brand> brandQuery = BrandRepository.GetQueryable();
            var brand = brandQuery.Where(b => b.BrandCode.Contains(BrandCode) && b.BrandName.Contains(BrandName)).OrderBy(b => b.BrandCode).AsEnumerable().Select(b => new { b.BrandCode, b.UniformCode, b.CustomCode, b.BrandName, b.SupplierCode, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (!IsActive.Equals(""))
            {
                brand = brandQuery.Where(b => b.BrandCode.Contains(BrandCode) && b.BrandName.Contains(BrandName) && b.IsActive.Contains(IsActive)).OrderBy(b => b.BrandCode).AsEnumerable().Select(b => new { b.BrandCode, b.UniformCode, b.CustomCode, b.BrandName, b.SupplierCode, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            }
            int total = brand.Count();
            brand = brand.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = brand.ToArray() };
        }

        public new bool Add(Brand brand)
        {
            var br = new Brand();
            br.BrandCode = brand.BrandCode;
            br.UniformCode = brand.UniformCode;
            br.CustomCode = brand.CustomCode;
            br.BrandName = brand.BrandName;
            br.SupplierCode = brand.SupplierCode;
            br.IsActive = brand.IsActive;
            br.UpdateTime = DateTime.Now;

            BrandRepository.Add(br);
            BrandRepository.SaveChanges();
            return true;
        }

        public bool Delete(string BrandCode)
        {
            var brand = BrandRepository.GetQueryable().FirstOrDefault(b => b.BrandCode == BrandCode);

            var supplier = SupplierRepository.GetQueryable().FirstOrDefault(s => s.SupplierCode == BrandCode);


            if (brand != null)
            {
                try
                {

                    if (supplier != null)
                    {
                        SupplierRepository.Delete(supplier);
                        SupplierRepository.SaveChanges();
                    }



                    BrandRepository.Delete(brand);
                    BrandRepository.SaveChanges();
                }
                catch (Exception e)
                { }
            }
            else
                return false;
            return true;
        }

        public bool Save(Brand brand)
        {
            var br = BrandRepository.GetQueryable().FirstOrDefault(b => b.BrandCode == brand.BrandCode);
            br.UniformCode = brand.UniformCode;
            br.CustomCode = brand.CustomCode;
            br.BrandName = brand.BrandName;
            br.SupplierCode = brand.SupplierCode;
            br.IsActive = brand.IsActive;
            br.UpdateTime = DateTime.Now;

            BrandRepository.SaveChanges();
            return true;
        }

        #endregion

        public System.Data.DataTable GetBrand(int page, int rows, string BrandCode, string BrandName, string IsActive)
        {
            IQueryable<Brand> brandQuery = BrandRepository.GetQueryable();
            var brand = brandQuery.Where(b => b.BrandCode.Contains(BrandCode) && b.BrandName.Contains(BrandName))
                .OrderBy(b => b.BrandCode).AsEnumerable()
                .Select(b => new 
                { 
                    b.BrandCode, 
                    b.UniformCode, 
                    b.CustomCode, 
                    b.BrandName, 
                    b.SupplierCode, 
                    IsActive = b.IsActive == "1" ? "可用" : "不可用", 
                    UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (!IsActive.Equals(""))
            {
                brand = brandQuery.Where(b => b.BrandCode.Contains(BrandCode) && b.BrandName.Contains(BrandName) && b.IsActive.Contains(IsActive))
                    .OrderBy(b => b.BrandCode).AsEnumerable()
                    .Select(b => new 
                    { 
                        b.BrandCode, 
                        b.UniformCode, 
                        b.CustomCode, 
                        b.BrandName, 
                        b.SupplierCode, 
                        IsActive = b.IsActive == "1" ? "可用" : "不可用", 
                        UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("统一编码", typeof(string));
            dt.Columns.Add("自定义编码", typeof(string));
            dt.Columns.Add("品牌名称", typeof(string));
            dt.Columns.Add("厂商编码", typeof(string));
            dt.Columns.Add("是否可用", typeof(string));
            dt.Columns.Add("更新时间", typeof(string));
            foreach (var item in brand)
            {
                dt.Rows.Add
                    (
                        item.UniformCode,
                        item.CustomCode,
                        item.BrandName,
                        item.SupplierCode,
                        item.IsActive,
                        item.UpdateTime
                    );
            }
            return dt;
        }
    }
}
