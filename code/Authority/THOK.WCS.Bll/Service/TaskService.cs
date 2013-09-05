using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.DbModel;

namespace THOK.WCS.Bll.Service
{
    public class TaskService:ITaskService
    {
        [Dependency]
        public ITaskRepository TaskRepository { get; set; }

        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }

        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
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
        public ICellRepository CellRepository { get; set; }

        [Dependency]
        public IPositionRepository PositionRepository { get; set; }

        [Dependency]
        public ICellPositionRepository CellPositionRepository { get; set; }

        [Dependency]
        public IPathRepository PathRepository { get; set; }

        public bool CreateNewTaskFromInBill(string billNo)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskFromOutBill(string billNo)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskFromMoveBill(string billNo)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskFromCheckBill(string billNo)
        {
            throw new NotImplementedException();
        }


        public bool CreateNewTaskForEmptyPalletStack(int positionID)
        {
            string palletCode = "00002A";
            int palletCount = 10;
            var position = PositionRepository.GetQueryable()
                .Where(i=>i.ID == positionID).FirstOrDefault();

            if (position != null)
            {
                var positionQuery = PositionRepository.GetQueryable()
                    .Where(i => i.SRMName == position.SRMName
                        && i.AbleStockInPallet && i.ID != positionID);
                var cellPositionQuery = CellPositionRepository.GetQueryable()
                    .Where(i => i.StockOutPositionID != positionID
                        && positionQuery.Contains(i.StockInPosition));
                var cellQuery = CellRepository.GetQueryable()
                    .Where(i => i.IsSingle == "1"
                        && cellPositionQuery.Any(p => p.CellCode == i.CellCode)
                        && (i.Storages.Count == 0
                            || i.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                                && s.Quantity == 0
                                && s.InFrozenQuantity == 0)
                            || i.Storages.Any(s => s.ProductCode == palletCode
                                && s.Quantity + s.InFrozenQuantity < palletCount
                                && s.OutFrozenQuantity == 0)));

                var cell = cellQuery.FirstOrDefault();
                if (cell != null)
                {
                    var cellPosition = CellPositionRepository.GetQueryable()
                        .Where(cp => cp.CellCode == cell.CellCode).FirstOrDefault();
                    if (!cell.Storages.Any())
                    {
                        var storage = new Storage()
                        {
                            Cell = cell,
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            IsActive = "0",
                            StorageTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        lock (cell.Storages)
                        {
                            cell.Storages.Add(storage);
                        }
                    }
                    var targetStorage = cell.Storages.FirstOrDefault();
                    if (targetStorage != null && cellPosition != null)
                    {
                        targetStorage.InFrozenQuantity += 1;
                        var newTask = new Task();
                        newTask.TaskType = "02";
                        newTask.TaskLevel = 0;
                        //newTask.PathID = path.ID;
                        newTask.ProductCode = palletCode;
                        newTask.ProductName = "空托盘";
                        newTask.OriginStorageCode = "";
                        newTask.TargetStorageCode = cell.CellCode;                        
                        newTask.OriginPositionID = positionID;
                        newTask.TargetPositionID = cellPosition.StockInPositionID;
                        newTask.CurrentPositionID = positionID;
                        newTask.CurrentPositionState = "02";
                        newTask.State = "01";
                        newTask.TagState = "01";//拟不使用
                        newTask.Quantity = 1;
                        newTask.TaskQuantity = 1;
                        newTask.OperateQuantity = 0;
                        //newTask.OrderID = inItem.BillNo;
                        newTask.OrderType = "05";
                        //newTask.AllotID = inItem.ID;
                        newTask.DownloadState = "1";
                        newTask.StorageSequence = 0;
                        TaskRepository.Add(newTask);
                        TaskRepository.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CreateNewTaskForEmptyPalletSupply(int positionID)
        {
            string palletCode = "00002A";
            var storageQuery = StorageRepository.GetQueryable()
                .Where(i => i.ProductCode == palletCode
                    && i.Quantity - i.OutFrozenQuantity > 0
                    && i.OutFrozenQuantity == 0
                    && i.InFrozenQuantity == 0)
                .OrderBy(i=>i.StorageTime);

            var storage = storageQuery.FirstOrDefault();
            var position = PositionRepository.GetQueryable()
                .Where(i => i.ID == positionID).FirstOrDefault();

            var positionCell = CellPositionRepository.GetQueryable()
                .Where(i => i.StockInPositionID == positionID).FirstOrDefault();

            if (storage != null && position != null )
            {
                var cellPosition = CellPositionRepository.GetQueryable()
                    .Where(cp => cp.CellCode == storage.CellCode).FirstOrDefault();
                
                if (cellPosition != null)
                {
                    var path = PathRepository.GetQueryable()
                        .Where(p=>p.OriginRegion.ID == cellPosition.StockOutPosition.Region.ID
                            && p.TargetRegion.ID == position.Region.ID)
                            .FirstOrDefault();
                    if (path != null)
                    {
                        var quantity = storage.Quantity - storage.OutFrozenQuantity;
                        storage.OutFrozenQuantity += quantity;

                        var newTask = new Task();
                        newTask.TaskType = "01";
                        newTask.TaskLevel = 0;
                        newTask.PathID = path.ID;
                        newTask.ProductCode = palletCode;
                        newTask.ProductName = "空托盘";
                        newTask.OriginStorageCode = storage.CellCode;
                        newTask.TargetStorageCode = positionCell != null ? positionCell.CellCode:"";
                        newTask.OriginPositionID = cellPosition.StockOutPositionID;
                        newTask.TargetPositionID = positionID;
                        newTask.CurrentPositionID = cellPosition.StockOutPositionID;
                        newTask.CurrentPositionState = "02";
                        newTask.State = "01";
                        newTask.TagState = "01";//拟不使用
                        newTask.Quantity = Convert.ToInt32(storage.Quantity);
                        newTask.TaskQuantity = Convert.ToInt32(quantity);
                        newTask.OperateQuantity = 0;
                        //newTask.OrderID = inItem.BillNo;
                        newTask.OrderType = "06";
                        //newTask.AllotID = inItem.ID;
                        newTask.DownloadState = "1";
                        newTask.StorageSequence = 0;
                        TaskRepository.Add(newTask);
                        TaskRepository.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CreateNewTaskForMoveBackRemain(int taskID)
        {
            var task = TaskRepository.GetQueryable().Where(i => i.ID == taskID).FirstOrDefault();
            if (task != null)
            {               
                var newTask = new Task();
                newTask.TaskType = "01";
                newTask.TaskLevel = 0;
                //newTask.PathID = path.ID;
                newTask.ProductCode = task.ProductCode;
                newTask.ProductName = task.ProductName;
                newTask.OriginStorageCode = task.TargetStorageCode;
                newTask.TargetStorageCode = task.OriginStorageCode;
                newTask.OriginPositionID = task.CurrentPositionID;
                newTask.TargetPositionID = task.OriginPositionID;
                newTask.CurrentPositionID = task.CurrentPositionID;
                newTask.CurrentPositionState = "02";
                newTask.State = "01";
                newTask.TagState = "01";//拟不使用
                newTask.Quantity = task.Quantity - task.OperateQuantity ;
                newTask.TaskQuantity = task.Quantity - task.OperateQuantity;
                newTask.OperateQuantity = 0;
                //newTask.OrderID = inItem.BillNo;
                newTask.OrderType = "07";
                //newTask.AllotID = inItem.ID;
                newTask.DownloadState = "1";
                newTask.StorageSequence = 0;
                TaskRepository.Add(newTask);
                TaskRepository.SaveChanges();
                return true;
            }        
            return false;
        }


        public bool FinishTask(int taskID)
        {
            var task = TaskRepository.GetQueryable().Where(i => i.ID == taskID).FirstOrDefault();
            if (task != null && task.State == "04")
            {
                return FinishTask(task.ID, task.OrderType, task.OrderID, task.AllotID,task.OriginStorageCode,task.TargetStorageCode);
            }
            return false;
        }

        public bool FinishTask(int taskID, string orderType, string orderID, int allotID, string originStorageCode, string targetStorageCode)
        {
                switch (orderType)
                {
                    case "01"://入库
                        return FinishInBillTask(orderID, allotID);
                        break;
                    case "02"://出库
                        return FinishOutBillTask(orderID, allotID);
                        break;
                    case "03"://移库
                        return FinishMoveBillTask(orderID, allotID);
                        break;
                    case "04"://盘点
                        return FinishCheckBillTask(orderID, allotID);
                        break;
                    case "05"://空托盘叠入库存
                        return FinishEmptyPalletStackTask(targetStorageCode);
                        break;
                    case "06"://空托盘移出库存
                        return FinishEmptyPalletSupplyTask(originStorageCode);
                        break;
                    default:
                        break;
                }
            return false;
        }

        private bool FinishInBillTask(string orderID ,int allotID)
        {
            var inAllot = InBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == orderID
                                        && i.ID == allotID
                                        && i.Status == "0")
                                    .FirstOrDefault();
            if (inAllot != null
                && (inAllot.InBillMaster.Status == "4"
                || inAllot.InBillMaster.Status == "5"
                ))
            {
                decimal quantity = inAllot.AllotQuantity;
                if (string.IsNullOrEmpty(inAllot.Storage.LockTag)
                    && inAllot.AllotQuantity >= quantity
                    && inAllot.Storage.InFrozenQuantity >= quantity)
                {
                    inAllot.Status = "2";
                    inAllot.Storage.Rfid = "";
                    inAllot.RealQuantity += quantity;
                    inAllot.Storage.Quantity += quantity;
                    inAllot.Storage.StorageTime = DateTime.Now;
                    inAllot.Storage.InFrozenQuantity -= quantity;
                    if (inAllot.Storage.Cell.FirstInFirstOut) inAllot.Storage.StorageSequence = inAllot.Storage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                    if (!inAllot.Storage.Cell.FirstInFirstOut) inAllot.Storage.StorageSequence = inAllot.Storage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                    inAllot.Storage.Cell.StorageTime = inAllot.Storage.Cell.Storages.Where(s=>s.Quantity >0).Min(s => s.StorageTime);
                    inAllot.InBillDetail.RealQuantity += quantity;
                    inAllot.InBillMaster.Status = "5";
                    inAllot.FinishTime = DateTime.Now;
                    if (inAllot.InBillMaster.InBillAllots.All(c => c.Status == "2"))
                    {
                        inAllot.InBillMaster.Status = "6";
                    }
                    InBillAllotRepository.SaveChanges();
                    return true;
                }
                else
                {
                    //"需确认入库的数据别人在操作或完成的数量不对，完成出错！";
                    return false;
                }
            }
            else
            {
                //"需确认入库的数据查询为空或者主单状态不对，完成出错！";
                return false;
            }
        }

        private bool FinishOutBillTask(string orderID, int allotID)
        {
            var outAllot = OutBillAllotRepository.GetQueryable()
                                                .Where(i => i.BillNo == orderID
                                                    && i.ID == allotID
                                                    && i.Status == "0")
                                                .FirstOrDefault();
            if (outAllot != null
                && (outAllot.OutBillMaster.Status == "4"
                || outAllot.OutBillMaster.Status == "5"
                ))
            {
                decimal quantity = outAllot.AllotQuantity;
                if (string.IsNullOrEmpty(outAllot.Storage.LockTag)
                    && outAllot.AllotQuantity >= quantity
                    && outAllot.Storage.OutFrozenQuantity >= quantity)
                {
                    outAllot.Status = "2";
                    outAllot.RealQuantity += quantity;
                    outAllot.Storage.Quantity -= quantity;
                    if (outAllot.Storage.Quantity == 0)
                        outAllot.Storage.Rfid = "";
                    outAllot.Storage.OutFrozenQuantity -= quantity;
                    if (outAllot.Storage.Quantity == 0) outAllot.Storage.StorageSequence = 0;
                    outAllot.Storage.Cell.StorageTime = outAllot.Storage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime);
                    outAllot.OutBillDetail.RealQuantity += quantity;
                    outAllot.OutBillMaster.Status = "5";
                    outAllot.FinishTime = DateTime.Now;
                    if (outAllot.OutBillMaster.OutBillAllots.All(c => c.Status == "2"))
                    {
                        outAllot.OutBillMaster.Status = "6";
                    }
                    OutBillAllotRepository.SaveChanges();

                    return true;
                }
                else
                {
                    //"需确认出库的数据别人在操作或完成的数量不对，完成出错！";
                    return false;
                }
            }
            else
            {
                //"需确认出库的数据查询为空或者主单状态不对，完成出错！";
                return false;
            }
        }

        private bool FinishMoveBillTask(string orderID, int allotID)
        {
            var moveDetail = MoveBillDetailRepository.GetQueryable()
                                                .Where(i => i.BillNo == orderID
                                                    && i.ID == allotID
                                                    && i.Status == "0")
                                                .FirstOrDefault();
            if (moveDetail != null
                && (moveDetail.MoveBillMaster.Status == "2"
                || moveDetail.MoveBillMaster.Status == "3"
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
                    if (moveDetail.InStorage.Cell.FirstInFirstOut) moveDetail.InStorage.StorageSequence = moveDetail.InStorage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                    if (!moveDetail.InStorage.Cell.FirstInFirstOut) moveDetail.InStorage.StorageSequence = moveDetail.InStorage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                    moveDetail.InStorage.Rfid = "";
                    moveDetail.OutStorage.Quantity -= moveDetail.RealQuantity;
                    moveDetail.OutStorage.OutFrozenQuantity -= moveDetail.RealQuantity;
                    if (moveDetail.OutStorage.Quantity == 0) moveDetail.OutStorage.StorageSequence = 0;
                    moveDetail.OutStorage.Cell.StorageTime = moveDetail.OutStorage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime);
                    moveDetail.OutStorage.Rfid = "";

                    //判断移入的时间是否小于移出的时间
                    if (DateTime.Compare(moveDetail.InStorage.StorageTime, moveDetail.OutStorage.StorageTime) == 1)
                        moveDetail.InStorage.StorageTime = moveDetail.OutStorage.StorageTime;
                    moveDetail.MoveBillMaster.Status = "3";
                    moveDetail.FinishTime = DateTime.Now;
                    var sortwork = SortWorkDispatchRepository.GetQueryable()
                                                        .Where(s => s.MoveBillMaster.BillNo == moveDetail.MoveBillMaster.BillNo 
                                                            && s.DispatchStatus == "2")
                                                        .FirstOrDefault();
                    //修改分拣调度作业状态
                    if (sortwork != null)
                    {
                        sortwork.DispatchStatus = "3";
                    }
                    if (moveDetail.MoveBillMaster.MoveBillDetails.All(c => c.Status == "2"))
                    {
                        moveDetail.MoveBillMaster.Status = "4";                                           
                    }

                    MoveBillDetailRepository.SaveChanges();  
                    return true;
                }
                else
                {
                    //"需确认移库的数据别人在操作或者完成的数量不对，完成出错！";
                    return false;
                }
            }
            else
            {
                //"需确认移库的数据查询为空或者主单状态不对，完成出错！";
                return false;
            }
        }

        private bool FinishCheckBillTask(string orderID, int allotID)
        {
            var checkDetail = CheckBillDetailRepository.GetQueryable()
                                                    .Where(i => i.BillNo == orderID
                                                        && i.ID == allotID
                                                        && i.Status == "0")
                                                    .FirstOrDefault();
            if (checkDetail != null
                && (checkDetail.CheckBillMaster.Status == "2"
                || checkDetail.CheckBillMaster.Status == "3"))
            {
                decimal quantity = checkDetail.Quantity;

                checkDetail.Status = "2";
                checkDetail.RealQuantity = quantity;
                checkDetail.Storage.IsLock = "0";
                checkDetail.CheckBillMaster.Status = "3";
                checkDetail.FinishTime = DateTime.Now;
                if (checkDetail.CheckBillMaster.CheckBillDetails.All(c => c.Status == "2"))
                {
                    checkDetail.CheckBillMaster.Status = "4";
                }
                CheckBillDetailRepository.SaveChanges();
                return true;
            }
            else
            {
                //"需确认盘点的数据查询为空或者主单状态不对，完成出错！";
                return false;
            }
        }

        private bool FinishEmptyPalletStackTask(string cellCode)
        {
            var cell = CellRepository.GetQueryable().Where(i => i.CellCode == cellCode).FirstOrDefault();
            if (cell != null && cell.Storages.FirstOrDefault() != null)
            {
                if (cell.Storages.FirstOrDefault().InFrozenQuantity >= 1)
                {
                    cell.Storages.FirstOrDefault().InFrozenQuantity -= 1;
                    cell.Storages.FirstOrDefault().Quantity += 1;
                    cell.Storages.FirstOrDefault().StorageSequence = 0;
                    cell.Storages.FirstOrDefault().StorageTime = DateTime.Now;
                    CellRepository.SaveChanges();
                }
            }
            return true;
        }

        private bool FinishEmptyPalletSupplyTask(string cellCode)
        {
            var cell = CellRepository.GetQueryable().Where(i => i.CellCode == cellCode).FirstOrDefault();
            if (cell != null && cell.Storages.FirstOrDefault() != null)
            {
                if (cell.Storages.FirstOrDefault().OutFrozenQuantity > 0)
                {
                    cell.Storages.FirstOrDefault().OutFrozenQuantity = 0;
                    cell.Storages.FirstOrDefault().Quantity = 0;
                    cell.Storages.FirstOrDefault().StorageSequence = 0;
                    CellRepository.SaveChanges();
                }
            }
            return true;
        }
    }
}
