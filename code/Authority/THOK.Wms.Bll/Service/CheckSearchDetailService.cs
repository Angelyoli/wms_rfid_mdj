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
    public class CheckSearchDetailService : ServiceBase<CheckBillDetail>, ICheckSearchDetailService
    {
        [Dependency]
        public ICheckSearchDetailRepository CheckSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICheckSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            IQueryable<CheckBillDetail> checkBillDetailQuery = CheckSearchDetailRepository.GetQueryable();
            var checkBillDetails = checkBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo);
            int total = checkBillDetails.Count();
            var checkBillDetail = checkBillDetails.Skip((page - 1) * rows).Take(rows);
            var checkDetail = checkBillDetail.ToArray().Select(i => new
            {
                i.ID,
                i.BillNo,
                i.ProductCode,
                i.Product.ProductName,
                i.UnitCode,
                i.Unit.UnitName,
                i.CellCode,
                i.Cell.CellName,
                i.StorageCode,
                RealQuantity = i.RealQuantity / i.Unit.Count,
                Quantity = i.Quantity / i.Unit.Count,
                DifiierQuantity = (i.RealQuantity - i.Quantity) / i.Unit.Count,
                i.Status
            });
            return new { total, rows = checkDetail.ToArray() };
        }
        #endregion

        public System.Data.DataTable GetCheckDetail(int page, int rows, string BillNo)
        {
            IQueryable<CheckBillDetail> checkBillDetailQuery = CheckSearchDetailRepository.GetQueryable();
            var checkBillDetails = checkBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i => new
            {
                i.ID,
                i.BillNo,
                i.ProductCode,
                i.Product.ProductName,
                i.UnitCode,
                i.Unit.UnitName,
                i.CellCode,
                i.Cell.CellName,
                i.StorageCode,
                RealQuantity = i.RealQuantity / i.Unit.Count,
                Quantity = i.Quantity / i.Unit.Count,
                DifiierQuantity = (i.RealQuantity - i.Quantity) / i.Unit.Count,
                i.Status
            });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("货位名称", typeof(string));
            dt.Columns.Add("产品代码", typeof(string));
            dt.Columns.Add("产品名称", typeof(string));
            dt.Columns.Add("单位编码", typeof(string));
            dt.Columns.Add("单位名称", typeof(string));
            dt.Columns.Add("账面数量", typeof(int));
            dt.Columns.Add("盘点数量", typeof(int));
            dt.Columns.Add("差异数量", typeof(int));
            foreach (var c in checkBillDetails)
            {
                dt.Rows.Add
                    (
                        c.ID,
                        c.CellName,
                        c.ProductCode,
                        c.ProductName,
                        c.UnitCode,
                        c.UnitName,
                        c.Quantity,
                        c.RealQuantity,
                        c.DifiierQuantity
                    );
            }
            return dt;
        }
    }
}
