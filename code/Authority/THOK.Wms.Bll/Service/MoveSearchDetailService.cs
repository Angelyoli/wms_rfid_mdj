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
    public class MoveSearchDetailService : ServiceBase<MoveBillDetail>, IMoveSearchDetailService
    {
        [Dependency]
        public IMoveSearchDetailRepository MoveSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IMoveSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveSearchDetailRepository.GetQueryable();
            var moveBillDetails = moveBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo);
            int total = moveBillDetails.Count();
            var moveBillDetail = moveBillDetails.Skip((page - 1) * rows).Take(rows);
            var moveDetail = moveBillDetail.ToArray().Select(i => new
            {
                i.ID,
                i.BillNo,
                i.ProductCode,
                i.Product.ProductName,
                i.UnitCode,
                i.Unit.UnitName,
                i.InCellCode,
                PlaceName_In = i.InCell.CellName,
                i.OutCellCode,
                PlaceName_Out = i.OutCell.CellName,
                RealQuantity = i.RealQuantity / i.Unit.Count,
                i.Status
            });
            return new { total, rows = moveDetail.ToArray() };
        }
        #endregion

        public System.Data.DataTable GetMoveDetail(int page, int rows, string BillNo)
        {
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveSearchDetailRepository.GetQueryable();
            var moveBillDetails = moveBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i => new
            {
                i.ID,
                i.BillNo,
                i.ProductCode,
                i.Product.ProductName,
                i.UnitCode,
                i.Unit.UnitName,
                i.InCellCode,
                PlaceName_In = i.InCell.CellName,
                i.OutCellCode,
                PlaceName_Out = i.OutCell.CellName,
                RealQuantity = i.RealQuantity / i.Unit.Count,
                i.Status
            });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("货位名称(出)", typeof(string));
            dt.Columns.Add("货位名称(入)", typeof(string));
            dt.Columns.Add("产品代码", typeof(string));
            dt.Columns.Add("产品名称", typeof(string));
            dt.Columns.Add("单位编码", typeof(string));
            dt.Columns.Add("单位名称", typeof(string));
            dt.Columns.Add("数量", typeof(int));
            foreach (var m in moveBillDetails)
            {
                dt.Rows.Add
                    (
                        m.ID,
                        m.PlaceName_Out,
                        m.PlaceName_In,
                        m.ProductCode,
                        m.ProductName,
                        m.UnitCode,
                        m.UnitName,
                        m.RealQuantity
                    );
            }
            return dt;
        }
    }
}