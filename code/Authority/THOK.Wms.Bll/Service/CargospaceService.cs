using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class CargospaceService : ServiceBase<Storage>, ICargospaceService
    {
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICargospaceService 成员

        public object GetCellDetails(int page, int rows, string type, string id)
        {
            try
            {
                IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
                var storages = storageQuery.Where(s => s.StorageCode != null);
                if (type == "ware")
                {
                    storages = storages.Where(s => s.Cell.Shelf.Area.Warehouse.WarehouseCode == id);
                }
                else if (type == "area")
                {
                    storages = storageQuery.Where(s => s.Cell.Shelf.Area.AreaCode == id);
                }
                else if (type == "shelf")
                {
                    storages = storageQuery.Where(s => s.Cell.Shelf.ShelfCode == id);
                }
                else if (type == "cell")
                {
                    storages = storageQuery.Where(s => s.Cell.CellCode == id);
                }

                var temp = storages.OrderBy(s => s.Product.ProductName).Where(s => s.Quantity > 0);
                int total = temp.Count();
                temp = temp.Skip((page - 1) * rows).Take(rows);
                var Storage = temp.ToArray().ToArray().Select(s => new
                {
                    s.StorageCode,
                    s.Cell.CellCode,
                    s.Cell.CellName,
                    s.Product.ProductCode,
                    s.Product.ProductName,
                    s.Product.Unit.UnitCode,
                    s.Product.Unit.UnitName,
                    Quantity = s.Quantity / s.Product.Unit.Count,
                    IsActive = s.IsActive == "1" ? "可用" : "不可用",
                    StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                    UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                });
                return new { total, rows = Storage.ToArray() };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public System.Data.DataTable GetCargospace(int page, int rows, string type, string id)
        {
            try
            {
                IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
                var storages = storageQuery.Where(s => s.StorageCode != null);
                if (type == "ware")
                {
                    storages = storages.Where(s => s.Cell.Shelf.Area.Warehouse.WarehouseCode == id);
                }
                else if (type == "area")
                {
                    storages = storageQuery.Where(s => s.Cell.Shelf.Area.AreaCode == id);
                }
                else if (type == "shelf")
                {
                    storages = storageQuery.Where(s => s.Cell.Shelf.ShelfCode == id);
                }
                else if (type == "cell")
                {
                    storages = storageQuery.Where(s => s.Cell.CellCode == id);
                }

                var temp = storages.OrderBy(s => s.Product.ProductName).Where(s => s.Quantity > 0);
                
                var Storage = temp.ToArray().ToArray().Select(s => new
                {
                    s.StorageCode,
                    s.Cell.CellCode,
                    s.Cell.CellName,
                    s.Product.ProductCode,
                    s.Product.ProductName,
                    s.Product.Unit.UnitCode,
                    s.Product.Unit.UnitName,
                    Quantity = s.Quantity / s.Product.Unit.Count,
                    IsActive = s.IsActive == "1" ? "可用" : "不可用",
                    StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                    UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                });
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("货位编码", typeof(string));
                dt.Columns.Add("货位名称", typeof(string));
                dt.Columns.Add("商品编码", typeof(string));
                dt.Columns.Add("商品名称", typeof(string));
                dt.Columns.Add("单位编码", typeof(string));
                dt.Columns.Add("单位名称", typeof(string));
                dt.Columns.Add("数量", typeof(decimal));
                dt.Columns.Add("储存时间", typeof(string));
                dt.Columns.Add("更新时间", typeof(string));
                foreach (var item in Storage)
                {
                    dt.Rows.Add
                        (
                            item.CellCode,
                            item.CellName,
                            item.ProductCode,
                            item.ProductName,
                            item.UnitCode,
                            item.UnitName,
                            item.Quantity,
                            item.StorageTime,
                            item.UpdateTime
                        );
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
