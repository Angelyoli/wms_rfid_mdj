using System;
using THOK.Wms.AutomotiveSystems.Models;
using THOK.Wms.AutomotiveSystems.Interfaces;
using THOK.Wms.Dal.Interfaces;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Transactions;
using THOK.Wms.SignalR.Common;
using THOK.Wms.DbModel;
using Entities.Extensions;

namespace THOK.Wms.AutomotiveSystems.Service
{
    public class TaskService : ITaskService
    {
        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public ICheckBillMasterRepository CheckBillMasterRepository { get; set; }
        [Dependency]
        public ICheckBillDetailRepository CheckBillDetailRepository { get; set; }

        [Dependency]
        public ISortWorkDispatchRepository SortWorkDispatchRepository { get; set; }

        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }
 
        public void GetBillMaster(string[] BillTypes, Result result)
        {
            BillMaster[] billMasters = new BillMaster[] { };
            try
            {
                foreach (var billType in BillTypes)
                {
                    switch (billType)
                    {
                        case "1"://入库单
                            var inBillMasters = InBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "4" || i.Status == "5")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "1" })
                                .ToArray();
                            billMasters = billMasters.Concat(inBillMasters).ToArray();
                            break;
                        case "2"://出库单
                            var outBillMasters = OutBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "4" || i.Status == "5")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "2" })
                                .ToArray();
                            billMasters = billMasters.Concat(outBillMasters).ToArray();
                            break;
                        case "3"://移库单
                            var moveBillMasters = MoveBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "2" || i.Status == "3")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "3" })
                                .ToArray();
                            billMasters = billMasters.Concat(moveBillMasters).ToArray();
                            break;
                        case "4"://盘点单
                            var checkBillMasters = CheckBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "2" || i.Status == "3")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "4" })
                                .ToArray();
                            billMasters = billMasters.Concat(checkBillMasters).ToArray();
                            break;
                        default:
                            break;
                    }
                }
                result.IsSuccess = true;
                result.BillMasters = billMasters;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "调用服务器服务查询订单主表失败，详情：" + e.Message;
            }
        }

        public void GetBillDetail(BillMaster[] billMasters, string productCode, string OperateType, string OperateAreas, string Operator, Result result)
        {
            BillDetail[] billDetails = new BillDetail[] { };
            try
            {
                foreach (var billMaster in billMasters)
                {
                    string billNo = billMaster.BillNo;
                    switch (billMaster.BillType)
                    {
                        case "1"://入库单
                            var inBillDetails = InBillAllotRepository.GetQueryable().ToArray().AsEnumerable()
                                .Where(i => i.BillNo == billNo
                                    && (i.ProductCode == productCode || productCode == string.Empty)                                    
                                    && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator))
                                    && (OperateAreas.Contains(i.Cell.Layer.ToString())
                                        || (OperateAreas.Contains("0") && i.Cell.Area.AreaType=="3")))
                                .Select(i => new BillDetail() { 
                                    BillNo = i.BillNo, 
                                    BillType = "1" ,

                                    DetailID = i.ID,
                                    StorageName = i.Cell.CellName,
                                    StorageRfid = i.Cell.Rfid,
                                    TargetStorageName = "",
                                    TargetStorageRfid = "",

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity =Math.Floor(i.AllotQuantity/i.Product.UnitList.Unit01.Count),
                                    BarQuantity = Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = Math.Floor(i.AllotQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                    OperatorCode = string.Empty,
                                    Operator = i.Operator,
                                    Status = i.Status,
                                })
                                .ToArray();
                            billDetails = billDetails.Concat(inBillDetails).ToArray();
                            break;
                        case "2"://出库单
                            var outBillDetails = OutBillAllotRepository.GetQueryable().ToArray().AsEnumerable()
                                .Where(i => i.BillNo == billNo
                                    && (i.CanRealOperate == "1" || OperateType != "Real")
                                    && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator))
                                    && (OperateAreas.Contains(i.Cell.Layer.ToString())
                                        || (OperateAreas.Contains("0") && i.Cell.Area.AreaType == "3")))
                                .Select(i => new BillDetail()
                                {
                                    BillNo = i.BillNo,
                                    BillType = "2",

                                    DetailID = i.ID,
                                    StorageName = i.Cell.CellName,
                                    StorageRfid = i.Cell.Rfid,
                                    TargetStorageName = "",
                                    TargetStorageRfid = "",

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity = Math.Floor(i.AllotQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = Math.Floor(i.AllotQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                    OperatorCode = string.Empty,
                                    Operator = i.Operator,
                                    Status = i.Status,
                                })
                                .ToArray();
                            billDetails = billDetails.Concat(outBillDetails).ToArray();

                            var outBillMaster = OutBillMasterRepository.GetQueryable()
                                .Where(i => i.BillNo == billNo)
                                .FirstOrDefault();
                            if (outBillMaster != null && outBillMaster.MoveBillMasterBillNo != null)
                            {
                                billNo = outBillMaster.MoveBillMasterBillNo;
                                //todo;
                                var moveBillDetailss = MoveBillDetailRepository.GetQueryable()
                                        .Where(i => i.BillNo == billNo
                                            && (i.CanRealOperate == "1" || OperateType != "Real")
                                            && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator))
                                            && (OperateAreas.Contains(i.InCell.Layer.ToString())
                                                || (OperateAreas.Contains("0") && i.InCell.Area.AreaType == "3")))
                                        .ToArray()
                                        .Select(i => new BillDetail()
                                        {
                                            BillNo = i.BillNo,
                                            BillType = "3",

                                            DetailID = i.ID,
                                            StorageName = i.OutCell.CellName,
                                            StorageRfid = i.OutCell.Rfid,
                                            TargetStorageName = i.InCell.CellName,
                                            TargetStorageRfid = i.InCell.Rfid,

                                            ProductCode = i.ProductCode,
                                            ProductName = i.Product.ProductName,

                                            PieceQuantity = Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                            BarQuantity = Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                            OperatePieceQuantity = Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                            OperateBarQuantity = Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                            OperatorCode = string.Empty,
                                            Operator = i.Operator,
                                            Status = i.Status,
                                        })
                                        .ToArray();
                                billDetails = billDetails.Concat(moveBillDetailss).ToArray();
                            }
                            break;                           
                        case "3"://移库单
                            var moveBillDetails = MoveBillDetailRepository.GetQueryable().ToArray().AsEnumerable()
                                .Where(i => i.BillNo == billNo
                                    && (i.CanRealOperate == "1" || OperateType != "Real")
                                    && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator))
                                    && (OperateAreas.Contains(i.InCell.Layer.ToString()) 
                                        || (OperateAreas.Contains("0") && i.InCell.Area.AreaType == "3")))                               
                                .Select(i => new BillDetail()
                                {
                                    BillNo = i.BillNo,
                                    BillType = "3",

                                    DetailID = i.ID,
                                    StorageName = i.OutCell.CellName,
                                    StorageRfid = i.OutCell.Rfid,
                                    TargetStorageName = i.InCell.CellName,
                                    TargetStorageRfid = i.InCell.Rfid,

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity = Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                    OperatorCode = string.Empty,
                                    Operator = i.Operator,
                                    Status = i.Status,
                                })
                                .ToArray();
                            billDetails = billDetails.Concat(moveBillDetails).ToArray();
                            break;
                        case "4"://盘点单
                            var checkBillDetails = CheckBillDetailRepository.GetQueryable().ToArray().AsEnumerable()
                                .Where(i => i.BillNo == billNo
                                    && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator))
                                    && (OperateAreas.Contains(i.Cell.Layer.ToString())
                                        || (OperateAreas.Contains("0") && i.Cell.Area.AreaType == "3")))
                                .Select(i => new BillDetail()
                                {
                                    BillNo = i.BillNo,
                                    BillType = "4",

                                    DetailID = i.ID,
                                    StorageName = i.Cell.CellName,
                                    StorageRfid = i.Cell.Rfid,
                                    TargetStorageName = "",
                                    TargetStorageRfid = "",

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity = Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                    OperatorCode = string.Empty,
                                    Operator = i.Operator,
                                    Status = i.Status,
                                })
                                .ToArray();
                            billDetails = billDetails.Concat(checkBillDetails).ToArray();
                            break;
                        default:
                            break;
                    }
                }
                result.IsSuccess = true;
                result.BillDetails = billDetails.OrderByDescending(i=>i.Status).OrderBy(b=>b.StorageName).ToArray();
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "调用服务器服务查询订单细表失败，详情：" + e.Message;
            }
        }

        public void Apply(BillDetail[] billDetails, string useTag, Result result)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    foreach (var billDetail in billDetails)
                    {
                        switch (billDetail.BillType)
                        {
                            case "1":
                                var inAllot = InBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "0")
                                    .FirstOrDefault();
                                if (inAllot != null)
                                {
                                    inAllot.Status = "1";
                                    inAllot.Operator = billDetail.Operator;
                                    if (useTag == "1")
                                    {
                                        OperateToLabelServer(inAllot.BillNo, inAllot.ID.ToString(), inAllot.Cell.CellName,
                                            billDetail.BillType, inAllot.Product.ProductName, (int)billDetail.PieceQuantity,
                                            (int)billDetail.BarQuantity, "");
                                    }
                                }
                                break;
                            case "2":
                                var outAllot = OutBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "0")
                                    .FirstOrDefault();
                                if (outAllot != null)
                                {
                                    outAllot.Status = "1";
                                    outAllot.Operator = billDetail.Operator;
                                    if (useTag == "1")
                                    {
                                        OperateToLabelServer(outAllot.BillNo, outAllot.ID.ToString(), outAllot.Cell.CellName,
                                            billDetail.BillType, outAllot.Product.ProductName, (int)billDetail.PieceQuantity,
                                            (int)billDetail.BarQuantity, "");
                                    }
                                }
                                break;
                            case "3":
                                var moveDetail = MoveBillDetailRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "0")
                                    .FirstOrDefault();
                                if (moveDetail != null)
                                {
                                    moveDetail.Status = "1";
                                    moveDetail.Operator = billDetail.Operator;
                                    if (useTag == "1")
                                    {
                                        OperateToLabelServer(moveDetail.BillNo, moveDetail.ID.ToString(), moveDetail.OutCell.CellName,
                                             billDetail.BillType, moveDetail.Product.ProductName, (int)billDetail.PieceQuantity,
                                             (int)billDetail.BarQuantity, moveDetail.InCell.CellName);
                                    }
                                }
                                break;
                            case "4":
                                var checkDetail = CheckBillDetailRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "0")
                                    .FirstOrDefault();
                                if (checkDetail != null)
                                {
                                    checkDetail.Status = "1";
                                    checkDetail.Operator = billDetail.Operator;
                                    if (useTag == "1")
                                    {
                                        OperateToLabelServer(checkDetail.BillNo, checkDetail.ID.ToString(), checkDetail.Cell.CellName,
                                            billDetail.BillType, checkDetail.Product.ProductName, (int)billDetail.PieceQuantity,
                                            (int)billDetail.BarQuantity, "");
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    InBillAllotRepository.SaveChanges();
                    scope.Complete();
                }
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "调用服务器服务申请作业任务失败，详情：" + e.Message;
            }
        }

        public void Cancel(BillDetail[] billDetails, string useTag, Result result)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    foreach (var billDetail in billDetails)
                    {
                        switch (billDetail.BillType)
                        {
                            case "1":
                                var inAllot = InBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (inAllot != null)
                                {
                                    inAllot.Status = "0";
                                    inAllot.Operator = string.Empty;
                                    if (useTag == "1")                                    
                                        CancelOperateToLabelServer(inAllot.BillNo, inAllot.ID.ToString(), inAllot.Cell.CellName);
                                }
                                break;
                            case "2":
                                var outAllot = OutBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (outAllot != null)
                                {
                                    outAllot.Status = "0";
                                    outAllot.Operator = string.Empty;
                                    if (useTag == "1")    
                                        CancelOperateToLabelServer(outAllot.BillNo, outAllot.ID.ToString(), outAllot.Cell.CellName);
                                }
                                break;
                            case "3":
                                var moveDetail = MoveBillDetailRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (moveDetail != null)
                                {
                                    moveDetail.Status = "0";
                                    moveDetail.Operator = string.Empty;
                                    if (useTag == "1")    
                                        CancelOperateToLabelServer(moveDetail.BillNo, moveDetail.ID.ToString(), moveDetail.OutCell.CellName);
                                }
                                break;
                            case "4":
                                var checkDetail = CheckBillDetailRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (checkDetail != null)
                                {
                                    checkDetail.Status = "0";
                                    checkDetail.Operator = string.Empty;
                                    if (useTag == "1")    
                                        CancelOperateToLabelServer(checkDetail.BillNo, checkDetail.ID.ToString(), checkDetail.Cell.CellCode);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    InBillAllotRepository.SaveChanges();
                    scope.Complete();
                }
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "调用服务器服务取消作业任务失败，详情：" + e.Message;
            }
        }

        public void Execute(BillDetail[] billDetails, string useTag, Result result)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    foreach (var billDetail in billDetails)
                    {
                        switch (billDetail.BillType)
                        {
                            case "1":
                                var inAllot = InBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (inAllot != null
                                    && (inAllot.InBillMaster.Status == "4"
                                    || inAllot.InBillMaster.Status == "5"
                                    ))
                                {
                                    decimal quantity = billDetail.OperatePieceQuantity * inAllot.Product.UnitList.Unit01.Count
                                        + billDetail.OperateBarQuantity * inAllot.Product.UnitList.Unit02.Count;
                                    if (string.IsNullOrEmpty(inAllot.Storage.LockTag)
                                        && inAllot.AllotQuantity >= quantity
                                        && inAllot.Storage.InFrozenQuantity >= quantity)
                                    {
                                        inAllot.Status = "2";
                                        inAllot.RealQuantity += quantity;
                                        inAllot.Storage.Quantity += quantity;
                                        inAllot.Storage.InFrozenQuantity -= quantity;
                                        inAllot.InBillDetail.RealQuantity += quantity;
                                        inAllot.InBillMaster.Status = "5";
                                        if (inAllot.InBillMaster.InBillAllots.All(c => c.Status == "2"))
                                        {
                                            inAllot.InBillMaster.Status = "6";
                                        }
                                        if (useTag == "1")    
                                            CancelOperateToLabelServer(inAllot.BillNo, inAllot.ID.ToString(), inAllot.Cell.CellName);
                                    }
                                }
                                break;
                            case "2":
                                var outAllot = OutBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (outAllot != null
                                    && (outAllot.OutBillMaster.Status == "4"
                                    || outAllot.OutBillMaster.Status == "5"
                                    ))
                                {
                                    decimal quantity = billDetail.OperatePieceQuantity * outAllot.Product.UnitList.Unit01.Count
                                        + billDetail.OperateBarQuantity * outAllot.Product.UnitList.Unit02.Count;
                                    if (string.IsNullOrEmpty(outAllot.Storage.LockTag)
                                        && outAllot.AllotQuantity >= quantity
                                        && outAllot.Storage.OutFrozenQuantity >= quantity)
                                    {
                                        outAllot.Status = "2";
                                        outAllot.RealQuantity += quantity;
                                        outAllot.Storage.Quantity -= quantity;
                                        outAllot.Storage.OutFrozenQuantity -= quantity;
                                        outAllot.OutBillDetail.RealQuantity += quantity;
                                        outAllot.OutBillMaster.Status = "5"; 
                                        if (outAllot.OutBillMaster.OutBillAllots.All(c => c.Status == "2"))
                                        {
                                            outAllot.OutBillMaster.Status = "6";
                                        }
                                        if (useTag == "1")    
                                            CancelOperateToLabelServer(outAllot.BillNo, outAllot.ID.ToString(), outAllot.Cell.CellName);
                                    }
                                }
                                break;
                            case "3":
                                var moveDetail = MoveBillDetailRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (moveDetail != null
                                    && (moveDetail.MoveBillMaster.Status =="2"
                                    || moveDetail.MoveBillMaster.Status =="3"
                                    ))
                                {
                                    if (string.IsNullOrEmpty(moveDetail.InStorage.LockTag)
                                        && string.IsNullOrEmpty(moveDetail.OutStorage.LockTag)
                                        && moveDetail.InStorage.InFrozenQuantity >= moveDetail.RealQuantity
                                        && moveDetail.OutStorage.OutFrozenQuantity >= moveDetail.RealQuantity)
                                    {
                                        moveDetail.Status = "2";
                                        moveDetail.InStorage.Quantity += moveDetail.RealQuantity;
                                        moveDetail.InStorage.InFrozenQuantity -= moveDetail.RealQuantity;
                                        moveDetail.OutStorage.Quantity -= moveDetail.RealQuantity;
                                        moveDetail.OutStorage.OutFrozenQuantity -= moveDetail.RealQuantity;
                                        moveDetail.MoveBillMaster.Status = "3";

                                        var sortwork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.MoveBillMaster.BillNo == moveDetail.MoveBillMaster.BillNo && s.DispatchStatus == "2");
                                        if (sortwork != null)
                                        {
                                            sortwork.DispatchStatus = "3";
                                        }
                                        if (moveDetail.MoveBillMaster.MoveBillDetails.All(c => c.Status == "2"))
                                        {
                                            moveDetail.MoveBillMaster.Status = "4";
                                            string errorInfo = "";
                                            MoveBillDetailRepository.SaveChanges();
                                            SettleSortWokDispatch(moveDetail.BillNo, ref errorInfo);
                                        }
                                        if (useTag == "1")
                                            CancelOperateToLabelServer(moveDetail.BillNo, moveDetail.ID.ToString(), moveDetail.OutCell.CellName);
                                    }
                                }
                                break;
                            case "4":
                                var checkDetail = CheckBillDetailRepository.GetQueryable()
                                    .Where(i => i.BillNo == billDetail.BillNo
                                        && i.ID == billDetail.DetailID
                                        && i.Status == "1"
                                        && i.Operator == billDetail.Operator)
                                    .FirstOrDefault();
                                if (checkDetail != null 
                                    && (checkDetail.CheckBillMaster.Status=="2" 
                                    || checkDetail.CheckBillMaster.Status=="3"))
                                {
                                    decimal quantity = billDetail.OperatePieceQuantity * checkDetail.Product.UnitList.Unit01.Count
                                                       + billDetail.OperateBarQuantity * checkDetail.Product.UnitList.Unit02.Count;

                                    checkDetail.Status = "2";
                                    checkDetail.RealQuantity = quantity;
                                    checkDetail.Storage.IsLock = "0";
                                    checkDetail.CheckBillMaster.Status = "3";
                                    if (checkDetail.CheckBillMaster.CheckBillDetails.All(c => c.Status == "2"))
                                    {
                                        checkDetail.CheckBillMaster.Status = "4";
                                    }
                                    if (useTag == "1")    
                                        CancelOperateToLabelServer(checkDetail.BillNo, checkDetail.ID.ToString(), checkDetail.Cell.CellCode);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    InBillAllotRepository.SaveChanges();
                    scope.Complete();
                }
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "调用服务器服务执行作业任务失败，详情：" + e.Message;
            }
        }

        private bool SettleSortWokDispatch(string moveBillNo,ref string errorInfo)
        {
            var sortWork = SortWorkDispatchRepository.GetQueryable()
                .FirstOrDefault(s => s.MoveBillNo == moveBillNo);

            if (sortWork!= null)
            {
                //出库单作自动出库
                var storages = StorageRepository.GetQueryable()
                    .Where(s => s.CellCode == sortWork.SortingLine.CellCode
                       && s.Quantity - s.OutFrozenQuantity > 0).ToArray();

                sortWork.OutBillMaster.OutBillDetails.AsParallel().ForAll(
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

                //出库结单
                var outMaster = OutBillMasterRepository.GetQueryable()
                    .FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                outMaster.Status = "6";
                outMaster.UpdateTime = DateTime.Now;
                //分拣作业结单
                sortWork.DispatchStatus = "4";
                sortWork.UpdateTime = DateTime.Now;
                return true;
            }
            return true;
        }

        private void OperateToLabelServer(string billId, string detailId, string storageId, string operateType, string tobaccoName, int piece, int item, string targetStorageName)
        {
            string sql = @"INSERT INTO SY_SHOWINFO 
                            VALUES('{0}','{1}','{2}','{3}','{4}',{5},{6},0,0,0,'{7}');";
            string tmp = "";
            tmp = tmp + (piece > 0 ? string.Format("{0}件", piece) : "");
            tmp = tmp + (item > 0 ? string.Format("{0}条", item) : "");
            tmp = tmp + (targetStorageName.Length > 0 ? string.Format(@"->{0}", targetStorageName) : "");
            sql = string.Format(sql, billId, detailId, storageId, operateType, tobaccoName, piece, item, tmp);
            StorageRepository.GetObjectSet().ExecuteStoreCommand(sql);               
        }

        private void CancelOperateToLabelServer(string billId, string detailId, string storageId)
        {
            string sql = @" DELETE SY_SHOWINFO 
                            WHERE STORAGEID = '{0}' AND ORDERMASTERID = '{1}' AND ORDERDETAILID = '{2}'

                            UPDATE SY_SHOWINFO SET
                            READSTATE = 0,HARDWAREREADSTATE=0
                            WHERE STORAGEID = '{0}' AND CONFIRMSTATE = 0 AND READSTATE = 1

                            UPDATE STORAGES SET
                            ACT = '',PRODUCTNAME='',CONTENTS='',NUMBERSHOW='',[SIGN]=0
                            WHERE STORAGEID = '{0}'";
            sql = string.Format(sql, storageId, billId, detailId);
            StorageRepository.GetObjectSet().ExecuteStoreCommand(sql);
        }
    }
}
