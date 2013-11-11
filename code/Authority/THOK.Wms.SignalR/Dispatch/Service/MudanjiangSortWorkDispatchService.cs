using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.SignalR.Connection;
using THOK.Wms.SignalR.Dispatch.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using THOK.Wms.SignalR.Common;
using System.Transactions;
using THOK.Wms.SignalR.Model;
using System.Threading;
using THOK.Common.Entity;
using THOK.Authority.Dal.Interfaces;

namespace THOK.Wms.SignalR.Dispatch.Service
{
    public class MudanjiangSortWorkDispatchService : Notifier<DispatchSortWorkConnection>, ISortOrderWorkDispatchService
    {
        [Dependency]
        public IStorageLocker Locker { get; set; }

        [Dependency]
        public ISortOrderDispatchRepository SortOrderDispatchRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
        [Dependency]
        public ISortOrderRepository SortOrderRepository { get; set; }
        [Dependency]
        public ISortOrderDetailRepository SortOrderDetailRepository { get; set; }
        [Dependency]
        public ISortingLowerlimitRepository SortingLowerlimitRepository { get; set; }
        [Dependency]
        public ISortingLineRepository SortingLineRepository { get; set; }
        [Dependency]
        public ISortWorkDispatchRepository SortWorkDispatchRepository { get; set; }

        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }

        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IProductRepository ProductRepository { get; set; }

        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        [Dependency]
        public IMoveBillCreater MoveBillCreater { get; set; }
        [Dependency]
        public IOutBillCreater OutBillCreater { get; set; }

        public void Dispatch(string connectionId, ProgressState ps, CancellationToken cancellationToken, string workDispatchId, string userName)
        {
            Locker.LockKey = workDispatchId;
            ConnectionId = connectionId;
            ps.State = StateType.Start;
            NotifyConnection(ps.Clone());

            IQueryable<SortOrderDispatch> sortOrderDispatchQuery = SortOrderDispatchRepository.GetQueryable();
            IQueryable<SortOrder> sortOrderQuery = SortOrderRepository.GetQueryable();
            IQueryable<SortOrderDetail> sortOrderDetailQuery = SortOrderDetailRepository.GetQueryable();

            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            IQueryable<MoveBillMaster> moveBillMasterQuery = MoveBillMasterRepository.GetQueryable();
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();

            IQueryable<SortingLowerlimit> sortingLowerlimitQuery = SortingLowerlimitRepository.GetQueryable();
            IQueryable<SortingLine> sortingLineQuery = SortingLineRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();

            IQueryable<SortWorkDispatch> sortWorkDispatchQuery = SortWorkDispatchRepository.GetQueryable();

            IQueryable<THOK.Authority.DbModel.SystemParameter> systemParQuery = SystemParameterRepository.GetQueryable();

            //查询调度是否使用下限 0：否；1：是；
            bool isUselowerlimit = Convert.ToBoolean(systemParQuery
                .Where(s => s.ParameterName == "IsUselowerlimit")
                .Select(s=>s.ParameterValue).FirstOrDefault());

            //要调度的作业任务转成数组
            workDispatchId = workDispatchId.Substring(0, workDispatchId.Length - 1);
            int[] work = workDispatchId.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

            var sortingLines = sortOrderDispatchQuery
                    .WhereIn(w => w.ID, work).GroupBy(w => w.SortingLine)
                    .Select(s => new { s.Key.SortingLineCode });
            if (sortWorkDispatchQuery.Where(s => s.DispatchStatus != "4"
                    && sortingLines.All(a => a.SortingLineCode == s.SortingLineCode)).Count() > 0)
            {
                ps.State = StateType.Error;
                ps.Errors.Add("当前调度的分拣线有未结单的数据！不能调度，请结单后在尝试。");
                NotifyConnection(ps.Clone());
                return;
            }

            //调度表未作业的数据
            var sortOrderDispatch = sortOrderDispatchQuery.Join(sortOrderQuery,
                                                 dp => new { dp.OrderDate, dp.DeliverLineCode },
                                                 om => new { om.OrderDate, om.DeliverLineCode },
                                                 (dp, om) => new { dp.ID, dp.WorkStatus, dp.OrderDate, dp.SortingLine, dp.DeliverLineCode, om.OrderID }
                                            ).Join(sortOrderDetailQuery,
                                                 dm => new { dm.OrderID },
                                                 od => new { od.OrderID },
                                                 (dm, od) => new { dm.ID, dm.WorkStatus, dm.OrderDate, dm.SortingLine, od.Product, od.UnitCode, od.Price, od.RealQuantity }
                                            ).WhereIn(s => s.ID, work)
                                             .Where(s => s.WorkStatus == "1")
                                             .GroupBy(r => new { r.OrderDate, r.SortingLine, r.Product, r.UnitCode, r.Price })
                                             .Select(r => new { r.Key.OrderDate, r.Key.SortingLine, r.Key.Product, r.Key.UnitCode, r.Key.Price, SumQuantity = r.Sum(p => p.RealQuantity * r.Key.Product.UnitList.Unit02.Count) })
                                             .AsParallel()
                                             .GroupBy(r => new { r.OrderDate, r.SortingLine })
                                             .Select(r => new { r.Key.OrderDate, r.Key.SortingLine, Products = r })
                                             .OrderBy(s => s.SortingLine.SortingLineCode)
                                             .ToArray();

            var sortingLowerlimit = sortingLowerlimitQuery.Select(s => new { s.Product, s.SortingLine, s.SortType }).ToArray();

            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            string operatePersonID = employee != null ? employee.ID.ToString() : "";
            if (employee == null)
            {
                ps.State = StateType.Error;
                ps.Errors.Add("未找到当前用户，或当前用户不可用！");
                NotifyConnection(ps.Clone());
                return;
            }

            decimal sumAllotQuantity = 0;
            decimal sumAllotLineQuantity = 0;

            foreach (var dispatch in sortOrderDispatch)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    bool hasError = false;
                    ps.State = StateType.Info;
                    ps.Messages.Add("开始调度" + dispatch.SortingLine.SortingLineName);
                    NotifyConnection(ps.Clone());

                    if (dispatch.Products.Count() > 0)
                    {
                        if (cancellationToken.IsCancellationRequested) return;

                        sumAllotLineQuantity = 0;

                        if (cancellationToken.IsCancellationRequested) return;
                        MoveBillMaster moveBillMaster = MoveBillCreater.CreateMoveBillMaster(dispatch.SortingLine.Cell.WarehouseCode,
                                                                                                dispatch.SortingLine.MoveBillTypeCode,
                                                                                                operatePersonID);
                        moveBillMaster.Origin = "2";
                        moveBillMaster.Description = dispatch.SortingLine.SortingLineCode + " 分拣调度生成！";

                        foreach (var product in dispatch.Products.ToArray())
                        {
                            if (product.SumQuantity > 0)
                            {
                                if (cancellationToken.IsCancellationRequested) return;

                                decimal sumBillQuantity = sortOrderDispatch.Sum(t => t.Products.Sum(p => p.SumQuantity));
                                sumAllotQuantity += product.SumQuantity;

                                decimal sumBillProductQuantity = dispatch.Products.Sum(p => p.SumQuantity);
                                sumAllotLineQuantity += product.SumQuantity;

                                ps.State = StateType.Processing;
                                ps.TotalProgressName = "分拣作业调度";
                                ps.TotalProgressValue = (int)(sumAllotQuantity / sumBillQuantity * 100);
                                ps.CurrentProgressName = "正在调度：" + dispatch.SortingLine.SortingLineName;
                                ps.CurrentProgressValue = (int)(sumAllotLineQuantity / sumBillProductQuantity * 100);
                                NotifyConnection(ps.Clone());

                                //获取分拣线下限数量
                                decimal lowerlimitQuantity = sortingLowerlimitQuery
                                    .Where(s => s.ProductCode == product.Product.ProductCode
                                        && s.SortingLineCode == product.SortingLine.SortingLineCode)
                                    .GroupBy(l => new { l.SortingLineCode,l.ProductCode})
                                    .Select(l=>l.Sum(s=>s.Quantity))
                                    .FirstOrDefault();
             
                                //获取分拣备货区库存数量                   
                                decimal sortQuantity = storageQuery
                                    .Where(s => s.ProductCode == product.Product.ProductCode)
                                    .Join(sortingLineQuery,
                                        s => s.Cell,
                                        l => l.Cell,
                                        (s, l) => new { l.SortingLineCode, s.Quantity }
                                    )
                                    .Where(r => r.SortingLineCode == product.SortingLine.SortingLineCode)
                                    .GroupBy(l=>l.SortingLineCode)
                                    .Select(l=>l.Sum(s=>s.Quantity))
                                    .FirstOrDefault();

                                //获取当前这个卷烟库存数量
                                decimal areaSumQuantity = storageQuery
                                    .Where(s => (s.Cell.Area.AreaType == "10" || product.Product.PointAreaCodes.Contains(s.Cell.AreaCode))
                                         && s.ProductCode == product.Product.ProductCode)
                                    .GroupBy(l=>l.Product.ProductCode)
                                    .Select(l=>l.Sum(s=>s.Quantity))
                                    .FirstOrDefault();

                                //是否使用下限
                                if (!isUselowerlimit)
                                    lowerlimitQuantity = 0;

                                //获取移库量 = 出库量 + 下限量 - 分拣备货区库存量;
                                decimal quantity = 0;

                                //取整件
                                decimal pieceQuantity = Math.Ceiling((product.SumQuantity + lowerlimitQuantity - sortQuantity - (lowerlimitQuantity > 0 ? 30 * product.Product.UnitList.Quantity02 * product.Product.UnitList.Quantity03 : 0))
                                            / product.Product.Unit.Count) * product.Product.Unit.Count;

                                if (product.Product.IsRounding == "1")
                                {
                                    //不取整
                                    quantity = product.SumQuantity + lowerlimitQuantity - sortQuantity;
                                }
                                else if (product.Product.IsRounding == "0")
                                {
                                    //取整件
                                    quantity = pieceQuantity;                                
                                }
                                else
                                {
                                    quantity = 0;//整托盘自动移库；
                                    if (pieceQuantity > areaSumQuantity)
                                    {
                                        //不够整件出库
                                        hasError = true;
                                        ps.State = StateType.Error;
                                        ps.Errors.Add(dispatch.SortingLine.SortingLineCode + "线," + product.Product.ProductCode + " " + product.Product.ProductName + ",库存不足！当前总量：" + Convert.ToDecimal(product.SumQuantity / product.Product.UnitList.Unit02.Count) + "(条),缺少：" + Convert.ToDecimal((pieceQuantity - areaSumQuantity) / product.Product.UnitList.Unit02.Count) + "(条)");
                                        NotifyConnection(ps.Clone());
                                    }
                                }

                                if (quantity > 0)
                                {
                                    AlltoMoveBill(moveBillMaster, product.Product, dispatch.SortingLine.Cell, ref quantity, cancellationToken, ps, dispatch.SortingLine.Cell.CellCode);
                                }

                                if (quantity > 0)
                                {
                                    //生成移库不完整,可能是库存不足；
                                    hasError = true;
                                    ps.State = StateType.Error;
                                    ps.Errors.Add(dispatch.SortingLine.SortingLineCode + "线," + product.Product.ProductCode + " " + product.Product.ProductName + ",库存不足！当前总量：" + Convert.ToDecimal(product.SumQuantity / product.Product.UnitList.Unit02.Count) + "(条),缺少：" + Convert.ToDecimal((quantity) / product.Product.UnitList.Unit02.Count) + "(条)");
                                    NotifyConnection(ps.Clone());
                                }
                            }
                        }

                        if (!hasError)
                        {
                            if (cancellationToken.IsCancellationRequested) return;

                            OutBillMaster outBillMaster = OutBillCreater.CreateOutBillMaster(dispatch.SortingLine.Cell.WarehouseCode,
                                                                                                dispatch.SortingLine.OutBillTypeCode,
                                                                                                operatePersonID);
                            outBillMaster.Origin = "2";
                            outBillMaster.Description = dispatch.SortingLine.SortingLineCode + " 分拣调度生成!";
                            //添加出库单细单
                            foreach (var product in dispatch.Products.ToArray())
                            {
                                if (cancellationToken.IsCancellationRequested) return;
                                OutBillCreater.AddToOutBillDetail(outBillMaster, product.Product, product.Price, product.SumQuantity);
                            }

                            //添加出库、移库主单和作业调度表
                            SortWorkDispatch sortWorkDisp = AddSortWorkDispMaster(moveBillMaster, outBillMaster, dispatch.SortingLine.SortingLineCode, dispatch.OrderDate);

                            //修改线路调度作业状态和作业ID
                            var sortDispTemp = sortOrderDispatchQuery.WhereIn(s => s.ID, work)
                                                                     .Where(s => s.OrderDate == dispatch.OrderDate
                                                                     && s.SortingLineCode == dispatch.SortingLine.SortingLineCode);

                            foreach (var sortDisp in sortDispTemp.ToArray())
                            {
                                if (cancellationToken.IsCancellationRequested) return;
                                sortDisp.SortWorkDispatchID = sortWorkDisp.ID;
                                sortDisp.WorkStatus = "2";
                            }

                            if (cancellationToken.IsCancellationRequested) return;
                            SortWorkDispatchRepository.SaveChanges();
                            ps.Messages.Add(dispatch.SortingLine.SortingLineName + " 调度成功！");
                        }
                        else
                        {
                            ps.State = StateType.Info;
                            ps.Messages.Add(dispatch.SortingLine.SortingLineName + " 调度失败！");
                            NotifyConnection(ps.Clone());
                            return;
                        }
                    }
                }
                catch (Exception e)
                {
                    ps.State = StateType.Info;
                    ps.Errors.Add(dispatch.SortingLine.SortingLineName + "作业调度失败！ 详情：" + e.Message);
                    NotifyConnection(ps.Clone());
                    return;
                }
            }

            ps.State = StateType.Info;
            ps.Messages.Add("调度完成!");
            NotifyConnection(ps.Clone());
        }

        private SortWorkDispatch AddSortWorkDispMaster(MoveBillMaster moveBillMaster, OutBillMaster outBillMaster, string sortingLineCode, string orderDate)
        {
            //添加分拣作业调度表
            SortWorkDispatch sortWorkDispatch = new SortWorkDispatch();
            var workDispatch = SortWorkDispatchRepository.GetQueryable()
                                                         .FirstOrDefault(w => w.OrderDate == orderDate
                                                             && w.SortingLineCode == sortingLineCode);
            sortWorkDispatch.ID = Guid.NewGuid();
            sortWorkDispatch.OrderDate = orderDate;
            sortWorkDispatch.SortingLineCode = sortingLineCode;
            sortWorkDispatch.DispatchBatch = workDispatch == null ? "1" : (Convert.ToInt32(workDispatch.DispatchBatch) + 1).ToString();
            sortWorkDispatch.OutBillNo = outBillMaster.BillNo;
            sortWorkDispatch.MoveBillNo = moveBillMaster.BillNo;
            sortWorkDispatch.DispatchStatus = "1";
            sortWorkDispatch.IsActive = "1";
            sortWorkDispatch.UpdateTime = DateTime.Now;
            SortWorkDispatchRepository.Add(sortWorkDispatch);
            return sortWorkDispatch;
        }

        private void AlltoMoveBill(MoveBillMaster moveBillMaster, Product product, Cell cell, ref decimal quantity, CancellationToken cancellationToken, ProgressState ps, string cellCode)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            //选择当前订单操作目标仓库；
            var storages = storageQuery.Where(s => s.Cell.WarehouseCode == moveBillMaster.WarehouseCode
                                            && s.Quantity - s.OutFrozenQuantity > 0
                                            && s.Cell.Area.AllotInOrder > 0
                                            && s.Cell.Area.AllotOutOrder > 0
                                            && s.Cell.IsActive == "1"
                                            && s.IsLock == "0");

            if (product.IsRounding == "1" || product.IsAbnormity == "1")
            {
                //分配条烟；异型烟区
                if (cancellationToken.IsCancellationRequested) return;
                var ss = storages.Where(s => product.PointAreaCodes.Contains(s.Cell.AreaCode)
                                        && s.ProductCode == product.ProductCode)
                                  .OrderBy(s => new { s.StorageTime, s.Cell.Area.AllotOutOrder, s.Quantity });
                if (quantity > 0) AllotBar(moveBillMaster, ss, cell, ref quantity, cancellationToken, ps);
            }
            else if (product.IsRounding == "0")
            {
                //分配件烟；小品种区 
                if (cancellationToken.IsCancellationRequested) return;
                var ss = storages.Where(s => product.PointAreaCodes.Contains(s.Cell.AreaCode)
                                        && s.ProductCode == product.ProductCode)
                                  .OrderBy(s => new { s.StorageTime, s.Cell.Area.AllotOutOrder, s.Quantity });
                if (quantity > 0) AllotPiece(moveBillMaster, ss, cell, ref quantity, cancellationToken, ps);
            }
        }

        private void AllotBar(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> ss, Cell cell, ref decimal quantity, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var s in ss.ToArray())
            {
                if (cancellationToken.IsCancellationRequested) return;
                if (quantity > 0)
                {
                    decimal allotQuantity = s.Quantity - s.OutFrozenQuantity;
                    decimal billQuantity = quantity;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    if (allotQuantity > 0)
                    {
                        var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                        var targetStorage = Locker.LockStorage(cell);
                        if (sourceStorage != null && targetStorage != null
                            && targetStorage.Quantity == 0
                            && targetStorage.InFrozenQuantity == 0)
                        {
                            MoveBillCreater.AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, allotQuantity,"1");
                            quantity -= allotQuantity;
                        }
                        else ps.Errors.Add("可用的移入目标库存记录不足！");                       
                    }
                }
                else break;
            }
        }

        private void AllotPiece(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> ss, Cell cell, ref decimal quantity, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var s in ss.ToArray())
            {
                if (cancellationToken.IsCancellationRequested) return;
                if (quantity > 0)
                {
                    decimal allotQuantity = Math.Floor((s.Quantity - s.OutFrozenQuantity) / s.Product.Unit.Count) * s.Product.Unit.Count;
                    decimal billQuantity = Math.Floor(quantity / s.Product.Unit.Count)
                                            * s.Product.Unit.Count;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    if (allotQuantity > 0)
                    {
                        var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                        var targetStorage = Locker.LockStorage(cell);
                        if (sourceStorage != null && targetStorage != null
                            && targetStorage.Quantity == 0
                            && targetStorage.InFrozenQuantity == 0)
                        {
                            MoveBillCreater.AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, allotQuantity,"1");
                            quantity -= allotQuantity;
                        }
                        else ps.Errors.Add("可用的移入目标库存记录不足！");  
                    }
                }
                else break;
            }
        }

        public bool LowerLimitMoveLibrary(string userName, bool isEnableStocking, out string errorInfo)
        {
            errorInfo = "当前模式，此功能不可用！";
            return false;
        }
    }
}
