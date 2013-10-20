using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Common;
using System.Transactions;
using THOK.Authority.Dal.Interfaces;
using System.Data;

namespace THOK.Wms.Bll.Service
{
    public class SortWorkDispatchService : ServiceBase<SortWorkDispatch>, ISortWorkDispatchService
    {
        [Dependency]
        public ISortWorkDispatchRepository SortWorkDispatchRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public ISortOrderDispatchRepository SortOrderDispatchRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }
        [Dependency]
        public ISortingLowerlimitRepository SortingLowerlimitRepository { get; set; }
        [Dependency]
        public ISortingLineRepository SortingLineRepository { get; set; }
        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }

        [Dependency]
        public IMoveBillCreater MoveBillCreater { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "1":
                    statusStr = "已调度";
                    break;
                case "2":
                    statusStr = "已审核";
                    break;
                case "3":
                    statusStr = "执行中";
                    break;
                case "4":
                    statusStr = "已结单";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string OrderDate, string SortingLineCode, string DispatchStatus)
        {
            IQueryable<SortWorkDispatch> SortWorkDispatchQuery = SortWorkDispatchRepository.GetQueryable();
            var sortWorkDispatch = SortWorkDispatchQuery.Where(s => s.ID == s.ID);
            if (DispatchStatus == string.Empty || DispatchStatus == null)
            {
                sortWorkDispatch = SortWorkDispatchQuery.Where(s => s.DispatchStatus != "4");
            }
           
            if (OrderDate != string.Empty && OrderDate != null)
            {
                OrderDate = Convert.ToDateTime(OrderDate).ToString("yyyyMMdd");
                sortWorkDispatch = sortWorkDispatch.Where(s => s.OrderDate == OrderDate);
            }
            if (SortingLineCode != string.Empty && SortingLineCode != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.SortingLineCode == SortingLineCode);
            }
            if (DispatchStatus != string.Empty && DispatchStatus != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.DispatchStatus == DispatchStatus);
            }
            var temp = sortWorkDispatch.OrderBy(b => new { b.OrderDate, b.SortingLineCode, b.DispatchStatus }).AsEnumerable().Select(b => new
            {
                b.ID,
                b.OrderDate,
                b.SortingLineCode,
                b.SortingLine.SortingLineName,
                b.OutBillNo,
                b.MoveBillNo,
                b.DispatchBatch,
                DispatchStatus = WhatStatus(b.DispatchStatus),
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = (string)b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        public bool Delete(string id, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortOrderDispatch = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == ID);
                if (sortOrderDispatch == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能删除！";
                    return false;
                }
                if (sortOrderDispatch.DispatchStatus != "1")
                {
                    errorInfo = "当前选择的调度记录不是已调度，未能删除！";
                    return false;
                }
                if (sortOrderDispatch.OutBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录出库单不是已录入，未能删除！";
                    return false;
                }
                if (sortOrderDispatch.MoveBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录移库单不是已录入，未能删除！";
                    return false;
                }

                using (var scope = new TransactionScope())
                {
                    //解锁移库冻结量
                    var moveDetail = MoveBillDetailRepository.GetQueryable()
                                                                .Where(m => m.BillNo
                                                                    == sortOrderDispatch.MoveBillNo);

                    var sourceStorages = moveDetail.Select(m => m.OutStorage).ToArray();
                    var targetStorages = moveDetail.Select(m => m.InStorage).ToArray();

                    if (!Locker.Lock(sourceStorages)
                        || !Locker.Lock(targetStorages))
                    {
                        errorInfo = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                        return false;
                    }

                    moveDetail.AsParallel().ForAll(
                        (Action<MoveBillDetail>)delegate(MoveBillDetail m)
                        {
                            if (m.InStorage.ProductCode == m.ProductCode
                                && m.OutStorage.ProductCode == m.ProductCode
                                && m.InStorage.InFrozenQuantity >= m.RealQuantity
                                && m.OutStorage.OutFrozenQuantity >= m.RealQuantity)
                            {
                                m.InStorage.InFrozenQuantity -= m.RealQuantity;
                                m.OutStorage.OutFrozenQuantity -= m.RealQuantity;
                                m.InStorage.LockTag = string.Empty;
                                m.OutStorage.LockTag = string.Empty;
                            }
                            else
                            {
                                throw new Exception("储位的卷烟或移库冻结量与当前分配不符，信息可能被异常修改，不能删除！");
                            }
                        }
                    );

                    Del(MoveBillDetailRepository, sortOrderDispatch.MoveBillMaster.MoveBillDetails);//删除移库细单
                    MoveBillMasterRepository.Delete(sortOrderDispatch.MoveBillMaster);//删除移库主单

                    Del(OutBillDetailRepository, sortOrderDispatch.OutBillMaster.OutBillDetails);//删除出库细单
                    OutBillMasterRepository.Delete(sortOrderDispatch.OutBillMaster);//删除出库主单

                    //修改线路调度表中作业状态
                    var sortDisp = SortOrderDispatchRepository.GetQueryable()
                                                              .Where(s => s.SortWorkDispatchID
                                                                  == sortOrderDispatch.ID);
                    foreach (var item in sortDisp.ToArray())
                    {
                        item.WorkStatus = "1";
                        item.SortWorkDispatchID = null;
                    }
                    SortWorkDispatchRepository.Delete(sortOrderDispatch);
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorInfo = "删除失败，详情：" + e.Message;
                return false;
            }
        }

        public bool Audit(string id, string userName, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortWork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == ID);
                var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);

                if (employee == null)
                {
                    errorInfo = "当前用户不存在或不可用，未能审核！";
                    return false;
                }
                if (sortWork == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能审核！";
                    return false;
                }
                if (sortWork.DispatchStatus != "1")
                {
                    errorInfo = "当前选择的调度记录不是已调度，未能审核！";
                    return false;
                }
                if (sortWork.OutBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录出库单不是已录入，未能审核！";
                    return false;
                }
                if (sortWork.MoveBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录移库单不是已录入，未能审核！";
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    //出库审核
                    var outMaster = OutBillMasterRepository.GetQueryable()
                        .FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                    outMaster.Status = "2";
                    outMaster.VerifyPersonID = employee.ID;
                    outMaster.VerifyDate = DateTime.Now;
                    outMaster.UpdateTime = DateTime.Now;
                    //移库审核
                    var moveMater = MoveBillMasterRepository.GetQueryable()
                        .FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "2";
                    moveMater.VerifyPersonID = employee.ID;
                    moveMater.VerifyDate = DateTime.Now;
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业审核
                    sortWork.DispatchStatus = "2";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorInfo = "审核失败，详情：" + e.Message;
                return false;
            }
        }

        public bool AntiTrial(string id, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortWork = SortWorkDispatchRepository.GetQueryable()
                                                         .FirstOrDefault(s => s.ID == ID);

                if (sortWork == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能反审！";
                    return false;
                }
                if (sortWork.DispatchStatus != "2")
                {
                    errorInfo = "当前选择的调度记录不是已审核，未能反审！";
                    return false;
                }
                if (sortWork.OutBillMaster.Status != "2")
                {
                    errorInfo = "当前选择的调度记录出库单不是已审核，未能反审！";
                    return false;
                }
                if (sortWork.MoveBillMaster.Status != "2")
                {
                    errorInfo = "当前选择的调度记录移库单不是已审核，未能反审！";
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    //出库反审
                    var outMaster = OutBillMasterRepository.GetQueryable().FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                    outMaster.Status = "1";
                    outMaster.UpdateTime = DateTime.Now;
                    //移库反审
                    var moveMater = MoveBillMasterRepository.GetQueryable().FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "1";
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业反审
                    sortWork.DispatchStatus = "1";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorInfo = "反审失败，详情：" + e.Message;
                return false;
            }
        }

        public bool Settle(string id, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortWork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == ID);

                if (sortWork == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能结单！";
                    return false;
                }
                if (sortWork.DispatchStatus == "1")
                {
                    errorInfo = "当前选择的调度记录不是执行中，未能结单！";
                    return false;
                }
                if (sortWork.MoveBillMaster.Status == "1")
                {
                    errorInfo = "当前选择的调度记录移库单不是执行中，未能结单！";
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    //移库细单解锁冻结量
                    var moveDetail = MoveBillDetailRepository.GetQueryable()
                        .Where(m => m.BillNo == sortWork.MoveBillNo && m.Status != "2");

                    if (moveDetail.Any())
                    {
                        var sourceStorages = moveDetail.Select(m => m.OutStorage).ToArray();
                        var targetStorages = moveDetail.Select(m => m.InStorage).ToArray();

                        if (!Locker.Lock(sourceStorages)
                            || !Locker.Lock(targetStorages))
                        {
                            errorInfo = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                            return false;
                        }

                        moveDetail.AsParallel().ForAll(
                            (Action<MoveBillDetail>)delegate(MoveBillDetail m)
                            {
                                if (m.InStorage.ProductCode == m.ProductCode
                                    && m.OutStorage.ProductCode == m.ProductCode
                                    && m.InStorage.InFrozenQuantity >= m.RealQuantity
                                    && m.OutStorage.OutFrozenQuantity >= m.RealQuantity)
                                {
                                    m.InStorage.InFrozenQuantity -= m.RealQuantity;
                                    m.OutStorage.OutFrozenQuantity -= m.RealQuantity;
                                    m.InStorage.LockTag = string.Empty;
                                    m.OutStorage.LockTag = string.Empty;
                                }
                                else
                                {
                                    throw new Exception("储位的卷烟或移库冻结量与当前分配不符，信息可能被异常修改，不能结单！");
                                }
                            }
                        );
                        //解锁分拣线路调度的状态，以便重新做作业调度；
                        foreach (var sortDisp in sortWork.SortOrderDispatchs)
                        {
                            sortDisp.SortWorkDispatchID = null;
                            sortDisp.WorkStatus = "1";
                        }
                    }
                    else
                    {
                        var outAllots = sortWork.OutBillMaster.OutBillAllots;
                        var outDetails = OutBillDetailRepository.GetQueryableIncludeProduct()
                                            .Where(o => o.BillNo == sortWork.OutBillMaster.BillNo);

                        //大品种先自动移库到分拣线
                        AutoMoveToSortingLine(sortWork, outDetails);

                        //出库单作自动出库
                        var storages = StorageRepository.GetQueryable().Where(s => s.CellCode == sortWork.SortingLine.CellCode
                                                                                && s.Quantity - s.OutFrozenQuantity > 0).ToArray();

                        if (!Locker.Lock(storages))
                        {
                            errorInfo = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                            return false;
                        }

                        outDetails.ToArray().AsParallel().ForAll(
                            (Action<OutBillDetail>)delegate(OutBillDetail o)
                            {
                                var ss = storages.Where(s => s.ProductCode == o.ProductCode).ToArray();
                                foreach (var s in ss)
                                {
                                    lock (s)
                                    {
                                        if (o.BillQuantity - o.AllotQuantity > 0)
                                        {
                                            decimal allotQuantity = s.Quantity;
                                            decimal billQuantity = o.BillQuantity - o.AllotQuantity;
                                            allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                                            o.AllotQuantity += allotQuantity;
                                            o.RealQuantity += allotQuantity;
                                            s.Quantity -= allotQuantity;

                                            var billAllot = new OutBillAllot()
                                            {
                                                BillNo = sortWork.OutBillMaster.BillNo,
                                                OutBillDetailId = o.ID,
                                                ProductCode = o.ProductCode,
                                                CellCode = s.CellCode,
                                                StorageCode = s.StorageCode,
                                                UnitCode = o.UnitCode,
                                                AllotQuantity = allotQuantity,
                                                RealQuantity = allotQuantity,
                                                FinishTime = DateTime.Now,
                                                Status = "2"
                                            };
                                            lock (sortWork.OutBillMaster.OutBillAllots)
                                            {
                                                sortWork.OutBillMaster.OutBillAllots.Add(billAllot);
                                            }
                                        }
                                        else
                                            break;
                                    }
                                }

                                if (o.BillQuantity - o.AllotQuantity > 0)
                                {
                                    throw new Exception(sortWork.SortingLine.SortingLineName + " " + o.ProductCode + " " + o.Product.ProductName + "分拣备货区库存不足，未能结单！");
                                }
                            }
                        );

                        storages.AsParallel().ForAll(s => s.LockTag = string.Empty);
                    }

                    //出库结单
                    var outMaster = OutBillMasterRepository.GetQueryable()
                        .FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                    outMaster.Status = "6";
                    outMaster.UpdateTime = DateTime.Now;
                    //移库结单
                    var moveMater = MoveBillMasterRepository.GetQueryable()
                        .FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "4";
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业结单
                    sortWork.DispatchStatus = "4";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (AggregateException ex)
            {
                errorInfo = "结单失败，详情：" + ex.InnerExceptions.Select(i => i.Message).Aggregate((m, n) => m + n);
                return false;
            }
            catch (Exception e)
            {
                errorInfo = "结单失败，详情：" + e.Message;
                return false;
            }
        }

        private void AutoMoveToSortingLine(SortWorkDispatch sortWork, IQueryable<OutBillDetail> outDetails)
        {
            var sortingLowerlimitQuery = SortingLowerlimitRepository.GetQueryable();
            var sortingLineQuery = SortingLineRepository.GetQueryable();
            var storageQuery = StorageRepository.GetQueryable();
            var systemParQuery = SystemParameterRepository.GetQueryable();

            //查询调度是否使用下限 0：否；1：是；
            bool isUselowerlimit = Convert.ToBoolean(systemParQuery
                .Where(s => s.ParameterName == "IsUselowerlimit")
                .Select(s => s.ParameterValue));

            var moveBillMaster = MoveBillMasterRepository.GetQueryable()
                .FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);

            foreach (var outDetail in outDetails.Where(o => o.Product.IsRounding == "2"))
            {
                //获取分拣线下限数量
                decimal lowerlimitQuantity = sortingLowerlimitQuery
                    .Where(s => s.ProductCode == outDetail.ProductCode
                        && s.SortingLineCode == sortWork.SortingLine.SortingLineCode)
                    .Sum(s => s.Quantity);

                //获取分拣备货区库存数量                   
                decimal sortQuantity = storageQuery
                    .Where(s => s.ProductCode == outDetail.ProductCode)
                    .Join(sortingLineQuery,
                        s => s.Cell,
                        l => l.Cell,
                        (s, l) => new { l.SortingLineCode, s.Quantity }
                    )
                    .Where(r => r.SortingLineCode == sortWork.SortingLine.SortingLineCode)
                    .Sum(s => s.Quantity);

                //是否使用下限
                if (!isUselowerlimit)
                    lowerlimitQuantity = 0;

                //获取移库量 = 出库量 + 下限量 - 分拣备货区库存量;
                decimal quantity = Math.Ceiling((outDetail.BillQuantity + lowerlimitQuantity - sortQuantity - (lowerlimitQuantity > 0 ? 30 : 0))
                                        / outDetail.Product.Unit.Count) * outDetail.Product.Unit.Count;

                if (quantity > 0)
                {
                    AlltoMoveBill(moveBillMaster, outDetail.Product, sortWork.SortingLine.Cell, ref quantity);
                }

                if (quantity > 0)
                {
                    //生成移库不完整,可能是库存不足；
                    throw new Exception(sortWork.SortingLine.SortingLineName + " " + outDetail.ProductCode + " " + outDetail.Product.ProductName + "仓储拆盘区库存不足，未能结单！");
                }
            }

            //自动执行移库单；
            foreach (var moveDetail in moveBillMaster.MoveBillDetails.Where(m => m.Status == "0"))
            {
                if (string.IsNullOrEmpty(moveDetail.InStorage.LockTag)
                    && string.IsNullOrEmpty(moveDetail.OutStorage.LockTag)
                    && moveDetail.InStorage.InFrozenQuantity >= moveDetail.RealQuantity
                    && moveDetail.OutStorage.OutFrozenQuantity >= moveDetail.RealQuantity)
                {
                    moveDetail.Status = "2";
                    moveDetail.InStorage.Quantity += moveDetail.RealQuantity;
                    moveDetail.InStorage.InFrozenQuantity -= moveDetail.RealQuantity;
                    if (moveDetail.InStorage.Cell.FirstInFirstOut) moveDetail.InStorage.StorageSequence = moveDetail.InStorage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                    if (!moveDetail.InStorage.Cell.FirstInFirstOut) moveDetail.InStorage.StorageSequence = moveDetail.InStorage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                    moveDetail.InStorage.Rfid = "";
                    moveDetail.OutStorage.Quantity -= moveDetail.RealQuantity;
                    moveDetail.OutStorage.OutFrozenQuantity -= moveDetail.RealQuantity;
                    if (moveDetail.OutStorage.Quantity == 0) moveDetail.OutStorage.StorageSequence = 0;
                    moveDetail.OutStorage.Cell.StorageTime = moveDetail.OutStorage.Cell.Storages.Where(s => s.Quantity > 0).Count() > 0
                        ? moveDetail.OutStorage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime) : DateTime.Now;
                    moveDetail.OutStorage.Rfid = "";

                    //判断移入的时间是否小于移出的时间
                    if (DateTime.Compare(moveDetail.InStorage.StorageTime, moveDetail.OutStorage.StorageTime) == 1)
                        moveDetail.InStorage.StorageTime = moveDetail.OutStorage.StorageTime;

                    moveDetail.FinishTime = DateTime.Now;
                }
            }
        }

        private void AlltoMoveBill(MoveBillMaster moveBillMaster, Product product, Cell cell, ref decimal quantity)
        {
            //选择当前订单操作目标仓库；
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable()            
                .Where(s => s.Cell.WarehouseCode == moveBillMaster.WarehouseCode
                    && s.Quantity - s.OutFrozenQuantity > 0
                    && s.Cell.Area.AllotInOrder > 0
                    && s.Cell.Area.AllotOutOrder > 0
                    && s.Cell.IsActive == "1");

            if (product.IsRounding == "2")
            {
                //分配件烟；大品种拆盘区 
                var storages = storageQuery.Where(s => product.PointAreaCodes.Contains(s.Cell.AreaCode)
                                        && s.ProductCode == product.ProductCode)
                                  .OrderBy(s => new { s.StorageTime, s.Cell.Area.AllotOutOrder, s.Quantity });
                if (quantity > 0) AllotPiece(moveBillMaster, storages, cell, ref quantity);
            }
        }

        private void AllotPiece(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> storages, Cell cell, ref decimal quantity)
        {
            foreach (var storage in storages.ToArray())
            {
                if (quantity > 0)
                {
                    decimal allotQuantity = Math.Floor((storage.Quantity - storage.OutFrozenQuantity) / storage.Product.Unit.Count) 
                        * storage.Product.Unit.Count;
                    decimal billQuantity = Math.Floor(quantity / storage.Product.Unit.Count) 
                        * storage.Product.Unit.Count;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    if (allotQuantity > 0)
                    {
                        var sourceStorage = Locker.LockNoEmptyStorage(storage, storage.Product);
                        var targetStorage = Locker.LockStorage(cell);
                        if (sourceStorage != null && targetStorage != null
                            && targetStorage.Quantity == 0
                            && targetStorage.InFrozenQuantity == 0)
                        {
                            MoveBillCreater.AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, allotQuantity, "1");
                            quantity -= allotQuantity;
                        }
                    }
                }
                else break;
            }
        }

        public  DataTable GetSortWorkDispatch(int page, int rows, string OrderDate, string SortingLineCode, string DispatchStatus)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            IQueryable<SortWorkDispatch> SortWorkDispatchQuery = SortWorkDispatchRepository.GetQueryable();
            var sortWorkDispatch = SortWorkDispatchQuery.Where(s => s.DispatchStatus != "4");
            if (OrderDate != string.Empty && OrderDate != null)
            {
                OrderDate = Convert.ToDateTime(OrderDate).ToString("yyyyMMdd");
                sortWorkDispatch = sortWorkDispatch.Where(s => s.OrderDate == OrderDate);
            }
            if (SortingLineCode != string.Empty && SortingLineCode != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.SortingLineCode == SortingLineCode);
            }
            if (DispatchStatus != string.Empty && DispatchStatus != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.DispatchStatus == DispatchStatus);
            }
            var temp = sortWorkDispatch.OrderBy(b => new { b.OrderDate, b.SortingLineCode, b.DispatchStatus })
                .AsEnumerable().Select(b => new
            {
                b.ID,
                b.OrderDate,
                b.SortingLineCode,
                b.SortingLine.SortingLineName,
                b.OutBillNo,
                b.MoveBillNo,
                b.DispatchBatch,
                DispatchStatus = WhatStatus(b.DispatchStatus),
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
            dt.Columns.Add("订单日期", typeof(string));
            dt.Columns.Add("分拣线编码", typeof(string));
            dt.Columns.Add("分拣线名称", typeof(string));
            dt.Columns.Add("作业调度批次", typeof(string));
            dt.Columns.Add("出库单编号", typeof(string));
            dt.Columns.Add("移库单编号", typeof(string));
            dt.Columns.Add("作业状态", typeof(string));
            dt.Columns.Add("是否可用", typeof(string));
            dt.Columns.Add("更新时间", typeof(string));
            foreach (var t in temp)
            {
                dt.Rows.Add(
                                  t.OrderDate
                                , t.SortingLineCode
                                , t.SortingLineName
                                , t.DispatchBatch
                                , t.OutBillNo
                                , t.MoveBillNo
                                , t.DispatchStatus
                                , t.IsActive
                                , t.UpdateTime
                            );
            }
            return dt;
        }
    }
}
