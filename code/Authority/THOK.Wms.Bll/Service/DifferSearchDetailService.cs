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
    public class DifferSearchDetailService : ServiceBase<ProfitLossBillDetail>, IDifferSearchDetailService
    {
        [Dependency]
        public IDifferSearchDetailRepository DifferSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IDifferSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            IQueryable<ProfitLossBillDetail> differBillDetailQuery = DifferSearchDetailRepository.GetQueryable();
            var differBillDetails = differBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo);
            int total = differBillDetails.Count();
            var differBillDetail = differBillDetails.Skip((page - 1) * rows).Take(rows);
            var differDetail = differBillDetail.ToArray().Select(i => new
            {
                i.ID,
                i.BillNo,
                i.ProductCode,
                i.UnitCode,
                i.Unit.UnitName,
                i.Product.ProductName,
                i.CellCode,
                i.Storage.Cell.CellName,
                Quantity = i.Quantity / i.Unit.Count
            });
            return new { total, rows = differDetail.ToArray() };
        }
        #endregion

        public System.Data.DataTable GetDifferDetail(int page, int rows, string BillNo)
        {
            IQueryable<ProfitLossBillDetail> differBillDetailQuery = DifferSearchDetailRepository.GetQueryable();
            var differBillDetails = differBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i => new
            {
                i.ID,
                i.BillNo,
                i.ProductCode,
                i.UnitCode,
                i.Unit.UnitName,
                i.Product.ProductName,
                i.CellCode,
                i.Storage.Cell.CellName,
                Quantity = i.Quantity / i.Unit.Count
            });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("序号",typeof(string));
            dt.Columns.Add("货位名称",typeof(string));
            dt.Columns.Add("产品代码",typeof(string));
            dt.Columns.Add("产品名称",typeof(string));
            dt.Columns.Add("单位编码",typeof(string));
            dt.Columns.Add("单位名称",typeof(string));
            dt.Columns.Add("损益数量",typeof(string));
            foreach (var d in differBillDetails)
            {
                dt.Rows.Add
                    (
                        d.ID,
                        d.CellName,
                        d.ProductCode,
                        d.ProductName,
                        d.UnitCode,
                        d.UnitName,
                        d.Quantity
                    );
            }
            return dt;
        }
    }
}