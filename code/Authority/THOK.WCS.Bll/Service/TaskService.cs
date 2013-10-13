using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.Bll.Interfaces;
using THOK.WCS.Dal.Interfaces;
using THOK.Wms.Dal.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.DbModel;
using THOK.WCS.DbModel;
using THOK.Authority.Dal.Interfaces;
using THOK.WCS.Bll.Models;
using System.Data.SqlClient;
using System.Configuration;
using THOK.Wms.SignalR.Common;

namespace THOK.WCS.Bll.Service
{
    public class TaskService:ITaskService
    {
        [Dependency]
        public ITaskRepository TaskRepository { get; set; }
        [Dependency]
        public ITaskHistoryRepository TaskHistoryRepository { get; set; }

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

        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }

        [Dependency]
        public IStorageLocker Locker { get; set; }

        [Dependency]
        public IMoveBillCreater MoveBillCreater { get; set; }

        private void InitValue(Task task)
        {
            if (task.TaskType == "" || task.TaskType == null)
                task.TaskType = "01";
            if (task.TaskLevel.ToString() == "" || task.TaskLevel.ToString() == null)
                task.TaskLevel = 0;
            if (task.ProductCode == "" || task.ProductCode == null)
                task.ProductCode = "";
            if (task.ProductName == "" || task.ProductName == null)
                task.ProductName = "";
            if (task.CurrentPositionState == "" || task.CurrentPositionState == null)
                task.CurrentPositionState = "01";
            if (task.State == "" || task.State == null)
                task.State = "01";
            if (task.TagState == "" || task.TagState == null)
                task.TagState = "01";
            if (task.Quantity.ToString() == "" || task.Quantity.ToString() == null)
                task.Quantity = 0;
            if (task.TaskQuantity.ToString() == "" || task.TaskQuantity.ToString() == null)
                task.TaskQuantity = 0;
            if (task.OperateQuantity.ToString() == "" || task.OperateQuantity.ToString() == null)
                task.OperateQuantity = 0;
            if (task.OrderID == "" || task.OrderID == null)
                task.OrderID = "";
            if (task.OrderType == "" || task.OrderType == null)
                task.OrderType = "01";
            if (task.AllotID.ToString() == "" || task.AllotID.ToString() == null)
                task.AllotID = 0;
            if (task.DownloadState == "" || task.DownloadState == null)
                task.DownloadState = "0";
        }

        public object GetDetails(int page, int rows, Task task)
        {
            IQueryable<Task> taskQuery = TaskRepository.GetQueryable();
            IQueryable<Path> pathQuery = PathRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            var taskQuery1 = taskQuery;
            if (task.PathID != 0 && task.PathID.ToString() != null)
            {
                taskQuery1 = taskQuery.Where(a => a.PathID == task.PathID);
            }
            var taskQuery2 = taskQuery1;
            if (task.OriginPositionID != 0 && task.OriginPositionID.ToString() != null)
            {
                taskQuery2 = taskQuery1.Where(a => a.OriginPositionID == task.OriginPositionID);
            }
            var taskQuery3 = taskQuery2;
            if (task.TargetPositionID != 0 && task.TargetPositionID.ToString() != null)
            {
                taskQuery3 = taskQuery2.Where(a => a.TargetPositionID == task.TargetPositionID);
            }
            var taskQuery4 = taskQuery3;
            if (task.CurrentPositionID != 0 && task.CurrentPositionID.ToString() != null)
            {
                taskQuery4 = taskQuery3.Where(a => a.CurrentPositionID == task.CurrentPositionID);
            }

            #region taskQuery5
            var taskQuery5 = taskQuery4.Join(pathQuery, t => t.PathID, p => p.ID, (t, p) => new
            {
                t.ID,
                t.TaskType,
                t.TaskLevel,
                t.PathID,
                t.ProductCode,
                t.ProductName,
                t.OriginStorageCode,
                t.TargetStorageCode,
                t.OriginPositionID,
                t.TargetPositionID,
                t.CurrentPositionID,
                t.CurrentPositionState,
                t.State,
                t.TagState,
                t.Quantity,
                t.TaskQuantity,
                t.OperateQuantity,
                t.OrderID,
                t.OrderType,
                t.AllotID,
                t.DownloadState,
                p.PathName,
                OriginRegionName = p.OriginRegion.RegionName,
                TargetRegionName = p.TargetRegion.RegionName
            })
                    .Join(cellQuery, t => t.OriginStorageCode, c => c.CellCode, (t, c) => new
                    {
                        t.ID,
                        t.TaskType,
                        t.TaskLevel,
                        t.PathID,
                        t.ProductCode,
                        t.ProductName,
                        t.OriginStorageCode,
                        t.TargetStorageCode,
                        t.OriginPositionID,
                        t.TargetPositionID,
                        t.CurrentPositionID,
                        t.CurrentPositionState,
                        t.State,
                        t.TagState,
                        t.Quantity,
                        t.TaskQuantity,
                        t.OperateQuantity,
                        t.OrderID,
                        t.OrderType,
                        t.AllotID,
                        t.DownloadState,
                        t.OriginRegionName,
                        t.TargetRegionName,
                        t.PathName,
                        OriginStorageName = c.CellName
                    })
                    .Join(cellQuery, t => t.TargetStorageCode, c => c.CellCode, (t, c) => new
                    {
                        t.ID,
                        t.TaskType,
                        t.TaskLevel,
                        t.PathID,
                        t.ProductCode,
                        t.ProductName,
                        t.OriginStorageCode,
                        t.TargetStorageCode,
                        t.OriginPositionID,
                        t.TargetPositionID,
                        t.CurrentPositionID,
                        t.CurrentPositionState,
                        t.State,
                        t.TagState,
                        t.Quantity,
                        t.TaskQuantity,
                        t.OperateQuantity,
                        t.OrderID,
                        t.OrderType,
                        t.AllotID,
                        t.DownloadState,
                        t.OriginRegionName,
                        t.TargetRegionName,
                        t.PathName,
                        t.OriginStorageName,
                        TargetStorageName = c.CellName
                    })
                    .Join(positionQuery, t => t.OriginPositionID, p => p.ID, (t, p) => new
                    {
                        t.ID,
                        t.TaskType,
                        t.TaskLevel,
                        t.PathID,
                        t.ProductCode,
                        t.ProductName,
                        t.OriginStorageCode,
                        t.TargetStorageCode,
                        t.OriginPositionID,
                        t.TargetPositionID,
                        t.CurrentPositionID,
                        t.CurrentPositionState,
                        t.State,
                        t.TagState,
                        t.Quantity,
                        t.TaskQuantity,
                        t.OperateQuantity,
                        t.OrderID,
                        t.OrderType,
                        t.AllotID,
                        t.DownloadState,
                        t.OriginRegionName,
                        t.TargetRegionName,
                        t.PathName,
                        t.OriginStorageName,
                        t.TargetStorageName,
                        OriginPositionName = p.PositionName
                    })
                    .Join(positionQuery, t => t.TargetPositionID, p => p.ID, (t, p) => new
                    {
                        t.ID,
                        t.TaskType,
                        t.TaskLevel,
                        t.PathID,
                        t.ProductCode,
                        t.ProductName,
                        t.OriginStorageCode,
                        t.TargetStorageCode,
                        t.OriginPositionID,
                        t.TargetPositionID,
                        t.CurrentPositionID,
                        t.CurrentPositionState,
                        t.State,
                        t.TagState,
                        t.Quantity,
                        t.TaskQuantity,
                        t.OperateQuantity,
                        t.OrderID,
                        t.OrderType,
                        t.AllotID,
                        t.DownloadState,
                        t.OriginRegionName,
                        t.TargetRegionName,
                        t.PathName,
                        t.OriginStorageName,
                        t.TargetStorageName,
                        t.OriginPositionName,
                        TargetPositionName = p.PositionName
                    })
                    .Join(positionQuery, t => t.CurrentPositionID, p => p.ID, (t, p) => new
                    {
                        t.ID,
                        t.TaskType,
                        t.TaskLevel,
                        t.PathID,
                        t.ProductCode,
                        t.ProductName,
                        t.OriginStorageCode,
                        t.TargetStorageCode,
                        t.OriginPositionID,
                        t.TargetPositionID,
                        t.CurrentPositionID,
                        t.CurrentPositionState,
                        t.State,
                        t.TagState,
                        t.Quantity,
                        t.TaskQuantity,
                        t.OperateQuantity,
                        t.OrderID,
                        t.OrderType,
                        t.AllotID,
                        t.DownloadState,
                        t.OriginRegionName,
                        t.TargetRegionName,
                        t.PathName,
                        t.OriginStorageName,
                        t.TargetStorageName,
                        t.OriginPositionName,
                        t.TargetPositionName,
                        CurrentPositionName = p.PositionName
                    })
                    .Where(t => t.TaskType.Contains(task.TaskType)
                        ///路径的 区域
                               && t.ProductCode.Contains(task.ProductCode)
                               && t.ProductName.Contains(task.ProductName)
                               && t.OriginStorageCode.Contains(task.OriginStorageCode)
                               && t.TargetStorageCode.Contains(task.TargetStorageCode)
                        ///起始位置，目标位置，当前位置
                               && t.CurrentPositionState.Contains(task.CurrentPositionState)
                               && t.State.Contains(task.State)
                               && t.TagState.Contains(task.TagState)
                               && t.OrderID.Contains(task.OrderID)
                               && t.OrderType.Contains(task.OrderType)
                               && t.DownloadState.Contains(task.DownloadState)
                             )
                      .OrderByDescending(t => t.ID)
                      .Select(t => new
                      {
                          t.ID,
                          t.PathID,
                          t.OriginPositionID,
                          t.TargetPositionID,
                          t.CurrentPositionID,
                          TaskType = t.TaskType == "01" ? "正常任务" : t.TaskType == "02" ? "叠空托盘" : "异常",
                          t.TaskLevel,
                          t.OriginRegionName,
                          t.TargetRegionName,
                          t.ProductCode,
                          t.ProductName,
                          t.PathName,
                          t.OriginStorageCode,
                          t.OriginStorageName,
                          t.TargetStorageCode,
                          t.TargetStorageName,
                          t.OriginPositionName,
                          t.TargetPositionName,
                          t.CurrentPositionName,
                          CurrentPositionState = t.CurrentPositionState == "01" ? "未达到" : t.CurrentPositionState == "02" ? "已到达" : "异常",
                          State = t.State == "01" ? "等待中" : t.State == "02" ? "执行中" : t.State == "03" ? "拣选中" : t.State == "04" ? "已完成" : "异常",
                          TagState = t.TagState == "01" ? "未点亮" : t.TagState == "02" ? "已点亮" : "异常",
                          t.Quantity,
                          t.TaskQuantity,
                          t.OperateQuantity,
                          t.OrderID,
                          OrderType = t.OrderType == "01" ? "入库单" : t.OrderType == "02" ? "移库单" : t.OrderType == "03" ? "出库单" : t.OrderType == "04" ? "盘点单" : "异常",
                          t.AllotID,
                          DownloadState = t.DownloadState == "0" ? "未下载" : t.DownloadState == "1" ? "已下载" : "异常"
                      });
            #endregion

            int total = taskQuery5.Count();
            var taskQuery6 = taskQuery5.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = taskQuery6.ToArray() };
        }
        public bool Add(Task task, out string strResult)
        {
            bool bResult = false;
            strResult = string.Empty;

            InitValue(task);

            var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.OriginStorageCode));
            var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.TargetStorageCode));
            if (originCellPosition != null && targetCellPosition!=null)
            {
                //起始位置为货位位置的出库位置
                var originPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == originCellPosition.StockOutPositionID);
                if (originPosition != null)
                {
                    //目标位置为货位位置的入库位置
                    var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == targetCellPosition.StockInPositionID);
                    if (targetPosition != null)
                    {
                        //查询 路径
                        var path = PathRepository.GetQueryable().FirstOrDefault(a => a.OriginRegionID == originPosition.RegionID && a.TargetRegionID == targetPosition.RegionID);
                        if (path != null)
                        {
                            try
                            {
                                Task t = new Task();
                                t.TaskType = task.TaskType;
                                t.TaskLevel = task.TaskLevel;
                                t.PathID = path.ID;
                                t.ProductCode = task.ProductCode;
                                t.ProductName = task.ProductName;
                                t.OriginStorageCode = task.OriginStorageCode;
                                t.TargetStorageCode = task.TargetStorageCode;
                                t.OriginPositionID = originCellPosition.StockOutPositionID;
                                t.TargetPositionID = targetCellPosition.StockInPositionID;
                                if (task.CurrentPositionID == 0)
                                {
                                    t.CurrentPositionID = originCellPosition.StockOutPositionID;
                                }
                                else
                                {
                                    t.CurrentPositionID = task.CurrentPositionID;
                                }
                                t.CurrentPositionState = task.CurrentPositionState;
                                t.State = task.State;
                                t.TagState = task.TagState;
                                t.Quantity = task.Quantity;
                                t.TaskQuantity = task.TaskQuantity;
                                t.OperateQuantity = task.OperateQuantity;
                                t.OrderID = task.OrderID;
                                t.OrderType = task.OrderType;
                                t.AllotID = task.AllotID;
                                t.DownloadState = task.DownloadState;

                                TaskRepository.Add(t);
                                TaskRepository.SaveChanges();
                                bResult = true;
                            }
                            catch (Exception ex)
                            {
                                strResult = "原因：" + ex.Message;
                            }
                        }
                        else
                        {
                            strResult = "未找到【路径信息】路径编号！";
                        }
                    }
                    else
                    {
                        strResult = "未找到【位置信息】目标位置！";
                    }
                }
                else
                {
                    strResult = "未找到【位置信息】起始位置！";
                }
            }
            else
            {
                strResult = "未找到【货位位置】起始货位位置或目标货位位置！";
            }
            return bResult;
        }
        public bool Save(Task task, out string strResult)
        {
            strResult = string.Empty;
            bool bResult = false;

            InitValue(task);

            var t = TaskRepository.GetQueryable().FirstOrDefault(a => a.ID == task.ID);
            if (t != null)
            {
                var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.OriginStorageCode));
                var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.TargetStorageCode));
                if (originCellPosition != null && targetCellPosition!=null)
                {
                    //起始位置为货位位置的出库位置
                    var originPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == originCellPosition.StockOutPositionID);
                    if (originPosition != null)
                    {
                        //目标位置为货位位置的入库位置
                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == targetCellPosition.StockInPositionID);
                        if (targetPosition != null)
                        {
                            //查询 路径
                            var path = PathRepository.GetQueryable().FirstOrDefault(a => a.OriginRegionID == originPosition.RegionID && a.TargetRegionID == targetPosition.RegionID);
                            if (path != null)
                            {
                                try
                                {
                                    t.TaskType = task.TaskType;
                                    t.TaskLevel = task.TaskLevel;
                                    t.PathID = path.ID;
                                    t.ProductCode = task.ProductCode;
                                    t.ProductName = task.ProductName;
                                    t.OriginStorageCode = task.OriginStorageCode;
                                    t.TargetStorageCode = task.TargetStorageCode;
                                    t.OriginPositionID = originCellPosition.StockOutPositionID;
                                    t.TargetPositionID = targetCellPosition.StockInPositionID;
                                    if (task.CurrentPositionID == 0 || task.CurrentPositionID == null)
                                    {
                                        t.CurrentPositionID = originCellPosition.StockOutPositionID;
                                    }
                                    else
                                    {
                                        t.CurrentPositionID = task.CurrentPositionID;
                                    }
                                    t.CurrentPositionState = task.CurrentPositionState;
                                    t.State = task.State;
                                    t.TagState = task.TagState;
                                    t.Quantity = task.Quantity;
                                    t.TaskQuantity = task.TaskQuantity;
                                    t.OperateQuantity = task.OperateQuantity;
                                    t.OrderID = task.OrderID;
                                    t.OrderType = task.OrderType;
                                    t.AllotID = task.AllotID;
                                    t.DownloadState = task.DownloadState;

                                    TaskRepository.SaveChanges();
                                    bResult = true;
                                }
                                catch (Exception ex)
                                {
                                    strResult = "原因：" + ex.Message;
                                }
                            }
                            else
                            {
                                strResult = "未找到【路径信息】路径编号！";
                            }
                        }
                        else
                        {
                            strResult = "未找到【位置信息】目标位置！";
                        }
                    }
                    else
                    {
                        strResult = "未找到【位置信息】起始位置！";
                    }
                }
                else
                {
                    strResult = "未找到【货位位置】起始货位位置或目标货位位置！";
                }
            }
            else
            {
                strResult = "未找到当前需要修改的数据！";
            }
            return bResult;
        }
        public bool Delete(string taskID, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            try
            {
                IQueryable<Task> TaskQuery = TaskRepository.GetQueryable();
                int intID = Convert.ToInt32(taskID);
                var t = TaskQuery.FirstOrDefault(i => i.ID == intID);
                TaskRepository.Delete(t);
                TaskRepository.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                strResult = "删除失败，原因：" + ex.Message;
            }
            return result;
        }

        public bool InBillTask(string billNo, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            try
            {
                var originSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName == "InBillPositionId");
                if (originSystemParam == null) { errorInfo = "请检查系统参数，未找到参数InBillPosition！"; return false; }
                int paramValue = Convert.ToInt32(originSystemParam.ParameterValue);

                var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == paramValue);
                if (originPosition == null) { errorInfo = "未找到起始位置ID（移出位置）：" + paramValue; return false; }
                var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(i => i.StockOutPositionID == originPosition.ID);
                if (originCellPosition == null) { errorInfo = "未找到起始货位位置的起始位置：" + originPosition.PositionName; return false; }

                var originCell = CellRepository.GetQueryable().FirstOrDefault(i => i.CellCode == originCellPosition.CellCode);
                if (originCell == null) { errorInfo = "未找到起始货位编码：" + originCellPosition.CellCode; return false; }

                var inBillAllot = InBillAllotRepository.GetQueryable().Where(i => i.BillNo == billNo);
                if (inBillAllot.Any())
                {
                    foreach (var inItem in inBillAllot.ToArray())
                    {
                        var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == inItem.CellCode);
                        if (targetCellPosition == null) { errorInfo = "未找到货位位置的入库货位：" + inItem.Cell.CellName; return false; }

                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                        if (targetPosition == null) { errorInfo = "未找到目标位置（移入位置）：" + targetCellPosition.StockInPosition.PositionName; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.Region.RegionName + "，和目标位置：" + targetPosition.Region.RegionName; return false; }

                        Task inTask = new Task();
                        inTask.TaskType = "01";
                        inTask.TaskLevel = 0;
                        inTask.PathID = path.ID;
                        inTask.ProductCode = inItem.Product.ProductCode;
                        inTask.ProductName = inItem.Product.ProductName;
                        inTask.OriginStorageCode = originCell.CellCode;
                        inTask.TargetStorageCode = inItem.CellCode;
                        inTask.OriginPositionID = Convert.ToInt32(originSystemParam.ParameterValue);
                        inTask.TargetPositionID = targetPosition.ID;
                        inTask.CurrentPositionID = Convert.ToInt32(originSystemParam.ParameterValue);
                        inTask.CurrentPositionState = "01";
                        inTask.State = "01";
                        inTask.TagState = "01";
                        inTask.Quantity = Convert.ToInt32(inItem.AllotQuantity / inItem.Product.Unit.Count);
                        inTask.TaskQuantity = Convert.ToInt32(inItem.AllotQuantity / inItem.Product.Unit.Count);
                        inTask.OperateQuantity = 0;
                        inTask.OrderID = inItem.BillNo;
                        inTask.OrderType = "01";
                        inTask.AllotID = inItem.ID;
                        inTask.DownloadState = "0";
                        inTask.StorageSequence = 0;
                        TaskRepository.Add(inTask);
                    }
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        try
                        {
                            /* 作业生成后 修改入库订单状态=5 
                             * Status == 1:已录入 2:已审核 3:已分配 4:已确认 5:执行中 6:已结单 */
                            var inBillMaster = InBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && (i.Status == "4" || i.Status == "5"));
                            if (inBillMaster != null)
                            {
                                inBillMaster.Status = "5";
                                InBillMasterRepository.SaveChanges();
                                TaskRepository.SaveChanges();

                                scope.Complete();
                                result = true;
                            }
                            else { errorInfo = "未获取订单号"; }
                        }
                        catch (Exception ex) { errorInfo = "事务异常：" + ex.Message; }
                    }

                }
                else { errorInfo = "当前选择订单没有分配数据，请重新选择！"; }
            }
            catch (Exception e) { errorInfo = e.Message; }
            return result;
        }
        public bool OutBillTask(string billNo, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            try
            {
                var targetSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName == "OutBillPositionId");
                if (targetSystemParam == null) { errorInfo = "请检查系统参数，未找到参数OutBillPosition！"; return false; }
                int paramValue = Convert.ToInt32(targetSystemParam.ParameterValue);

                var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == paramValue);
                if (targetPosition == null) { errorInfo = "未找到目标位置ID（移入位置）：" + paramValue; return false; }
                var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(i => i.StockInPositionID == paramValue);
                if (targetCellPosition == null) { errorInfo = "未找到目标货位位置的起始位置：" + targetPosition.PositionName; return false; }

                var targetCell = CellRepository.GetQueryable().FirstOrDefault(i => i.CellCode == targetCellPosition.CellCode);
                if (targetCell == null) { errorInfo = "未找到货位位置的货位编码：" + targetCellPosition.CellCode; return false; }

                var outBillAllot = OutBillAllotRepository.GetQueryable().Where(i => i.BillNo == billNo);
                if (outBillAllot.Any())
                {
                    foreach (var outItem in outBillAllot.ToArray())
                    {
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == outItem.CellCode);
                        if (originCellPosition == null) { errorInfo = "未找到起始货位：" + outItem.Cell.CellName; return false; }

                        var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                        if (originPosition == null) { errorInfo = "未找到起始位置（移出位置）：" + originCellPosition.StockOutPosition.PositionName; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.Region.RegionName + "，目标位置：" + targetPosition.Region.RegionName; return false; }

                        Task outTask = new Task();
                        outTask.TaskType = "01";
                        outTask.TaskLevel = 0;
                        outTask.PathID = path.ID;
                        outTask.ProductCode = outItem.Product.ProductCode;
                        outTask.ProductName = outItem.Product.ProductName;
                        outTask.OriginStorageCode = outItem.CellCode;
                        outTask.TargetStorageCode = targetCell.CellCode;
                        outTask.OriginPositionID = originPosition.ID;
                        outTask.TargetPositionID = targetPosition.ID;
                        outTask.CurrentPositionID = originPosition.ID;
                        outTask.CurrentPositionState = "02";
                        outTask.State = "01";
                        outTask.TagState = "01";
                        outTask.Quantity = Convert.ToInt32(outItem.AllotQuantity / outItem.Product.Unit.Count);
                        outTask.TaskQuantity = Convert.ToInt32(outItem.AllotQuantity / outItem.Product.Unit.Count);
                        outTask.OperateQuantity = 0;
                        outTask.OrderID = outItem.BillNo;
                        outTask.OrderType = "03";
                        outTask.AllotID = outItem.ID;
                        outTask.DownloadState = "0";
                        outTask.StorageSequence = outItem.Storage.StorageSequence;
                        TaskRepository.Add(outTask);
                    }
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        try
                        {
                            /* 作业生成后 修改入库订单状态=5 
                             * Status == 1:已录入 2:已审核 3:已分配 4:已确认 5:执行中 6:已结单 */
                            var outBillMaster = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && (i.Status == "4" || i.Status == "5"));
                            if (outBillMaster != null)
                            {
                                outBillMaster.Status = "5";
                                OutBillMasterRepository.SaveChanges();

                                TaskRepository.SaveChanges();
                                scope.Complete();
                                result = true;
                            }
                            else { errorInfo = "未获取订单号"; }
                        }
                        catch (Exception ex) { errorInfo = "事务异常：" + ex.Message; }
                    }
                }
                else { errorInfo = "当前选择订单没有分配数据，请重新选择！"; }
            }
            catch (Exception e) { errorInfo = e.Message; }
            return result;
        }
        public bool MoveBillTask(string billNo, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            try
            {
                var moveQuery = MoveBillDetailRepository.GetQueryable().Where(i => i.BillNo == billNo);
                if (moveQuery.Any())
                {
                    foreach (var moveItem in moveQuery.ToArray())
                    {
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveItem.OutCellCode);
                        if (originCellPosition == null) { errorInfo = "未找到货位位置的起始货位：" + moveItem.OutCell.CellName; return false; }
                        var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                        if (originPosition == null) { errorInfo = "未找到目标位置（移出位置）：" + originCellPosition.StockOutPosition.PositionName; return false; }

                        var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveItem.InCellCode);
                        if (targetCellPosition == null) { errorInfo = "未找到货位位置的目标货位：" + moveItem.InCell.CellName; return false; }
                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                        if (targetPosition == null) { errorInfo = "未找到目标位置（移入位置）：" + targetCellPosition.StockInPosition.PositionName; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.Region.RegionName + "，目标位置：" + targetPosition.Region.RegionName; return false; }
                        
                        var moveTask = new Task();
                        moveTask.TaskType = "01";
                        moveTask.TaskLevel = 0;
                        moveTask.PathID = path.ID;
                        moveTask.ProductCode = moveItem.Product.ProductCode;
                        moveTask.ProductName = moveItem.Product.ProductName;
                        moveTask.OriginStorageCode = moveItem.OutCellCode;
                        moveTask.TargetStorageCode = moveItem.InCellCode;
                        moveTask.OriginPositionID = originPosition.ID;
                        moveTask.TargetPositionID = targetPosition.ID;
                        moveTask.CurrentPositionID = originPosition.ID;
                        moveTask.CurrentPositionState = "02";
                        moveTask.State = "01";
                        moveTask.TagState = "01";
                        moveTask.Quantity = Convert.ToInt32(moveItem.RealQuantity / moveItem.Product.Unit.Count);
                        moveTask.TaskQuantity = Convert.ToInt32(moveItem.RealQuantity / moveItem.Product.Unit.Count);
                        moveTask.OperateQuantity = Convert.ToInt32(moveItem.RealQuantity);
                        moveTask.OrderID = moveItem.BillNo;
                        moveTask.OrderType = "02";
                        moveTask.AllotID = moveItem.ID;
                        moveTask.DownloadState = "0";
                        moveTask.StorageSequence = moveItem.OutStorage.StorageSequence;
                        TaskRepository.Add(moveTask);
                    }
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        try
                        {
                            /* 作业生成后 修改入库订单状态=3
                             * Status == 1:已录入 2:已审核  3:执行中 4:已结单 */
                            var moveBillMaster = MoveBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && (i.Status == "2" || i.Status == "3"));
                            if (moveBillMaster != null)
                            {
                                moveBillMaster.Status = "3";
                                MoveBillMasterRepository.SaveChanges();
                                TaskRepository.SaveChanges();
                                scope.Complete();
                                result = true;
                            }
                            else { errorInfo = "未获取订单号"; }
                        }
                        catch (Exception ex) { errorInfo = "事务异常：" + ex.Message; }
                    }
                }
                else { errorInfo = "当前选择的订单没有移库细表的数据，请重新选择！"; }
            }
            catch (Exception ex) { errorInfo = ex.Message; }
            return result;
        }
        public bool CheckBillTask(string billNo, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var taskQuery = TaskRepository.GetQueryable().Where(t => t.State == "02" && t.State == "03");//02:已到达 03:拣选中
            if (!taskQuery.Any())
            {
                var targetSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName == "OutBillPositionId");
                if (targetSystemParam != null) { errorInfo = "请检查系统参数，未找到目标位置OutBillPosition！"; return false; }
                int paramValue = Convert.ToInt32(targetSystemParam.ParameterValue);

                var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == paramValue);
                if (targetPosition != null) { errorInfo = "未找到目标位置（移入位置）：" + targetPosition.PositionName; return false; }

                var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(i => i.StockInPositionID == paramValue || i.StockOutPositionID == paramValue);
                if (targetCellPosition == null) { errorInfo = "未找到货位位置的目标位置：" + targetCellPosition.StockInPosition.PositionName; return false; }

                var targetCell = CellRepository.GetQueryable().FirstOrDefault(i => i.CellCode == targetCellPosition.CellCode);
                if (targetCell == null) { errorInfo = "未找到目标货位编码：" + targetCellPosition.CellCode; return false; }

                var checkQuery = CheckBillDetailRepository.GetQueryable().Where(i => i.BillNo == billNo);
                if (checkQuery.Any())
                {
                    foreach (var checkItem in checkQuery.ToArray())
                    {
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == checkItem.CellCode);
                        if (originCellPosition != null) { errorInfo = "未找到起始货位：" + checkItem.Cell.CellName; return false; }

                        var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);//根据“货位出库位置ID”查找“起始位置信息”
                        if (originPosition != null) { errorInfo = "未找到起始位置（移出位置）：" + originCellPosition.StockOutPosition.PositionName; return false; }

                        //根据“出库（目标和起始）位置信息的区域ID”查找“路径信息”
                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.Region.RegionName + "，目标位置：" + targetPosition.Region.RegionName; return false; }
                        
                        Task checkTask = new Task();
                        checkTask.TaskType = "01";
                        checkTask.TaskLevel = 0;
                        checkTask.PathID = path.ID;
                        checkTask.ProductCode = checkItem.Product.ProductCode;
                        checkTask.ProductName = checkItem.Product.ProductName;
                        checkTask.OriginStorageCode = checkItem.CellCode;
                        checkTask.TargetStorageCode = targetCell.CellCode;
                        checkTask.OriginPositionID = originPosition.ID;
                        checkTask.TargetPositionID = targetPosition.ID;
                        checkTask.CurrentPositionID = originPosition.ID;
                        checkTask.CurrentPositionState = "02";
                        checkTask.State = "01";
                        checkTask.TagState = "01";
                        checkTask.Quantity = Convert.ToInt32(checkItem.RealQuantity / checkItem.Product.Unit.Count);
                        checkTask.TaskQuantity = Convert.ToInt32(checkItem.RealQuantity / checkItem.Product.Unit.Count);
                        checkTask.OperateQuantity = Convert.ToInt32(checkItem.RealQuantity);
                        checkTask.OrderID = checkItem.BillNo;
                        checkTask.OrderType = "04";
                        checkTask.AllotID = checkItem.ID;
                        checkTask.DownloadState = "0";
                        checkTask.StorageSequence = checkItem.Storage.StorageSequence;
                        TaskRepository.Add(checkTask);
                    }
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        /* 作业生成后 修改入库订单状态=3 
                         * Status == 1:已录入 2:已审核 3:执行中 4:已盘点 5:已结单 */
                        var checkBillMaster = CheckBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo
                                                                                    && (i.Status == "2" || i.Status == "3"));
                        if (checkBillMaster != null)
                        {
                            checkBillMaster.Status = "3";
                            CheckBillMasterRepository.SaveChanges();
                            TaskRepository.SaveChanges();

                            scope.Complete();
                            result = true;
                        }
                        else { errorInfo = "未获取订单号"; }
                    }
                    catch (Exception ex) { errorInfo = "事务异常：" + ex.Message; }
                }
            }
            return result;
        }

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

        public bool CreateNewTaskForEmptyPalletStack(int positionID, string positionName)
        {
            var systemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(i => i.ParameterName == "EmptyPallet");
            string palletCode = systemParam.ParameterValue;
            int palletCount = 10;

            var position = PositionRepository.GetQueryable()
                .Where(i=>(i.ID == positionID || i.PositionName == positionName) &&(i.PositionType == "02" || i.PositionType == "03" || i.PositionType == "04"))
                .FirstOrDefault();

            if (position == null) return false;

            var task = TaskRepository.GetQueryable()
                .Where(t => t.State != "04" && t.OrderType == "05" && t.OriginPositionID ==  position.ID).FirstOrDefault();

            if (position != null && position.HasGoods && task == null)
            {
                var positionQuery = PositionRepository.GetQueryable()
                    .Where(i => i.SRMName == position.SRMName
                        && i.AbleStockInPallet && i.ID != position.ID);
                var cellPositionQuery = CellPositionRepository.GetQueryable()
                    .Where(i => i.StockOutPositionID != position.ID
                        && positionQuery.Contains(i.StockInPosition));
                var cellQuery = CellRepository.GetQueryable()
                    .Where(i => i.IsSingle == "1"
                        && cellPositionQuery.Any(p => p.CellCode == i.CellCode)
                        && (i.Storages.Any(s => s.ProductCode == palletCode
                                && s.Quantity + s.InFrozenQuantity < palletCount
                                && s.OutFrozenQuantity == 0)));
                if (!cellQuery.Any())
                {
                    cellQuery = CellRepository.GetQueryable()
                        .Where(i => i.IsSingle == "1"
                            && cellPositionQuery.Any(p => p.CellCode == i.CellCode)
                            && (i.Storages.Count == 0
                                || i.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                                    && s.Quantity == 0
                                    && s.InFrozenQuantity == 0)));
                }

                var cell = cellQuery.OrderBy(c=>Math.Abs(c.Col - c.Shelf.CellCols/2)).FirstOrDefault();
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
                        targetStorage.ProductCode = palletCode;
                        targetStorage.InFrozenQuantity += 1;
                        var newTask = new Task();
                        newTask.TaskType = "02";
                        newTask.TaskLevel = 0;
                        //newTask.PathID = path.ID;
                        newTask.ProductCode = palletCode;
                        newTask.ProductName = "空托盘";
                        newTask.OriginStorageCode = "";
                        newTask.TargetStorageCode = cell.CellCode;                        
                        newTask.OriginPositionID = position.ID;
                        newTask.TargetPositionID = cellPosition.StockInPositionID;
                        newTask.CurrentPositionID = position.ID;
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
        public bool CreateNewTaskForEmptyPalletSupply(int positionID, string positionName)
        {
            var systemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(i => i.ParameterName == "EmptyPallet");
            string palletCode = systemParam.ParameterValue;
            int palletCount = 10;

            var storageQuery = StorageRepository.GetQueryable()
                .Where(i => i.ProductCode == palletCode
                    && i.Quantity - i.OutFrozenQuantity >= palletCount
                    && i.OutFrozenQuantity == 0
                    && i.InFrozenQuantity == 0)
                .OrderByDescending(i=>i.StorageTime);

            if (!storageQuery.Any())
            {
                storageQuery = StorageRepository.GetQueryable()
                    .Where(i => i.ProductCode == palletCode
                        && i.Quantity - i.OutFrozenQuantity > 0
                        && i.OutFrozenQuantity == 0
                        && i.InFrozenQuantity == 0)
                    .OrderByDescending(i=>i.StorageTime);
            }

            var storage = storageQuery.FirstOrDefault();
            var position = PositionRepository.GetQueryable()
                .Where(i => (i.ID == positionID || i.PositionName == positionName)).FirstOrDefault();

            if (position == null) return false;

            var positionCell = CellPositionRepository.GetQueryable()
                .Where(i => i.StockInPositionID == position.ID).FirstOrDefault();

            var task = TaskRepository.GetQueryable()
                .Where(t => t.State != "04" && t.OrderType == "06").FirstOrDefault();

            if (storage != null && position != null && position.PositionType == "05" && task == null)
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
                        newTask.TargetPositionID = position.ID;
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
                case "01": return FinishInBillTask(orderID, allotID);
                case "02": return FinishMoveBillTask(orderID, allotID);
                case "03": return FinishOutBillTask(orderID, allotID);
                case "04": return FinishCheckBillTask(orderID, allotID);
                case "05": return FinishEmptyPalletStackTask(targetStorageCode);
                case "06": return FinishEmptyPalletSupplyTask(originStorageCode);
                case "07": break;
                case "08": return FinishStockOutRemainMoveBackTask(orderID, allotID);
                case "09": return FinishInventoryRemainMoveBackTask(orderID, allotID);
                default: break;
            }
            return false;
        }

        private bool FinishInBillTask(string orderID, int allotID)
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
                    inAllot.Storage.Cell.StorageTime = inAllot.Storage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime);
                    inAllot.InBillDetail.RealQuantity += quantity;
                    inAllot.InBillMaster.Status = "5";
                    inAllot.FinishTime = DateTime.Now;
                    if (inAllot.InBillMaster.InBillAllots.All(c => c.Status == "2"))
                    {
                        inAllot.InBillMaster.Status = "6";
                        this.ClearTask(orderID);
                    }
                    InBillAllotRepository.SaveChanges();
                    try
                    {
                        InspurService inspurService = new InspurService();
                        Inspur inspur = new Inspur();
                        inspur.Param = "";
                        inspur.User = inAllot.InBillMaster.OperatePerson.EmployeeName;
                        inspur.Time = inAllot.InBillMaster.UpdateTime.ToString();
                        inspur.BillNo = inAllot.BillNo;
                        inspur.ProductCode = inAllot.ProductCode;
                        inspur.RealQuantity = inAllot.InBillDetail.RealQuantity;
                        //反馈给浪潮的xml数据信息
                        MdjInspurWmsService.LwmWarehouseWorkServiceService LWWSS = new MdjInspurWmsService.LwmWarehouseWorkServiceService();
                        LWWSS.lwmStroeInProgFeedback(inspurService.BillProgressFeedback(inspur, "in"));
                        if (inAllot.InBillDetail.RealQuantity == inAllot.InBillDetail.AllotQuantity)
                        {
                            LWWSS.lwmStoreInComplete(inspurService.BillFinished(inspur, "in"));
                        }
                    }
                    catch (Exception) { return false; }
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
                    outAllot.Storage.Cell.StorageTime = outAllot.Storage.Cell.Storages.Where(s => s.Quantity > 0).Count() > 0
                        ? outAllot.Storage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime) : DateTime.Now;
                    outAllot.OutBillDetail.RealQuantity += quantity;
                    outAllot.OutBillMaster.Status = "5";
                    outAllot.FinishTime = DateTime.Now;
                    if (outAllot.OutBillMaster.OutBillAllots.All(c => c.Status == "2"))
                    {
                        outAllot.OutBillMaster.Status = "6";
                        this.ClearTask(orderID);
                    }
                    OutBillAllotRepository.SaveChanges();
                    try
                    {
                        InspurService inspurService = new InspurService();
                        Inspur inspur = new Inspur();
                        inspur.Param = "";
                        inspur.User = outAllot.OutBillMaster.OperatePerson.EmployeeName;
                        inspur.Time = outAllot.OutBillMaster.UpdateTime.ToString();
                        inspur.BillNo = outAllot.BillNo;
                        inspur.ProductCode = outAllot.ProductCode;
                        inspur.RealQuantity = outAllot.OutBillDetail.RealQuantity;
                        //反馈给浪潮的xml数据信息
                        MdjInspurWmsService.LwmWarehouseWorkServiceService LWWSS = new MdjInspurWmsService.LwmWarehouseWorkServiceService();
                        LWWSS.lwmStoreOutProgFeedback(inspurService.BillProgressFeedback(inspur, "out"));
                        if (outAllot.OutBillDetail.RealQuantity == outAllot.OutBillDetail.AllotQuantity)
                        {
                            LWWSS.lwmStoreOutComplete(inspurService.BillFinished(inspur, "out"));
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
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
                    moveDetail.OutStorage.Cell.StorageTime = moveDetail.OutStorage.Cell.Storages.Where(s => s.Quantity > 0).Count() > 0 
                        ? moveDetail.OutStorage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime) : DateTime.Now;
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
                        this.ClearTask(orderID);
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
                else
                {
                    return false;
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

        public bool ClearTask(out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var tasks = TaskRepository.GetQueryable();
            if (tasks.All(a => (a.CurrentPositionID == a.OriginPositionID)
                            || (a.CurrentPositionID == a.TargetPositionID)))
            {
                TaskHistory taskHistory = null;
                foreach (var task in tasks)
                {
                    taskHistory = new TaskHistory();
                    taskHistory.TaskID = task.ID;
                    taskHistory.TaskType = task.TaskType;
                    taskHistory.TaskLevel = task.TaskLevel;
                    taskHistory.PathID = task.PathID;
                    taskHistory.ProductCode = task.ProductCode;
                    taskHistory.ProductName = task.ProductName;
                    taskHistory.OriginStorageCode = task.OriginStorageCode;
                    taskHistory.TargetStorageCode = task.TargetStorageCode;
                    taskHistory.OriginPositionID = task.OriginPositionID;
                    taskHistory.TargetPositionID = task.TargetPositionID;
                    taskHistory.CurrentPositionID = task.CurrentPositionID;
                    taskHistory.CurrentPositionState = task.CurrentPositionState;
                    taskHistory.State = task.State;
                    taskHistory.TagState = task.TagState;
                    taskHistory.Quantity = task.Quantity;
                    taskHistory.TaskQuantity = task.TaskQuantity;
                    taskHistory.OperateQuantity = task.OperateQuantity;
                    taskHistory.OrderID = task.OrderID;
                    taskHistory.OrderType = task.OrderType;
                    taskHistory.AllotID = task.AllotID;
                    taskHistory.DownloadState = task.DownloadState;
                    taskHistory.ClearTime = System.DateTime.Now;
                    
                    TaskHistoryRepository.Add(taskHistory);
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    TaskHistoryRepository.SaveChanges();

                    string constr = ConfigurationManager.ConnectionStrings["AuthorizeContext"].ConnectionString;
                    SqlConnection con = new SqlConnection(constr);
                    string sql = "truncate table wcs_task";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    try
                    {
                        con.Open();
                        int i = (int)cmd.ExecuteNonQuery();
                        if (i > 0) result = true; else result = false;
                    }
                    catch (Exception ex) { errorInfo = ex.Message; }
                    finally { con.Close(); }

                    scope.Complete();
                    result = true;
                }
            }
            else
            {
                errorInfo = "当前有未执行完毕的任务！";
            }
            return result;
        }
        public bool ClearTask(string orderID)
        {
            bool result = false;
            var tasks = TaskRepository.GetQueryable().Where(a => a.OrderID == orderID);
            if (tasks.All(a => ((a.OrderID == orderID) && (a.CurrentPositionID == a.OriginPositionID || a.CurrentPositionID == a.TargetPositionID))))
            {
                TaskHistory taskHistory = null;
                foreach (var task in tasks)
                {
                    taskHistory = new TaskHistory();
                    taskHistory.TaskID = task.ID;
                    taskHistory.TaskType = task.TaskType;
                    taskHistory.TaskLevel = task.TaskLevel;
                    taskHistory.PathID = task.PathID;
                    taskHistory.ProductCode = task.ProductCode;
                    taskHistory.ProductName = task.ProductName;
                    taskHistory.OriginStorageCode = task.OriginStorageCode;
                    taskHistory.TargetStorageCode = task.TargetStorageCode;
                    taskHistory.OriginPositionID = task.OriginPositionID;
                    taskHistory.TargetPositionID = task.TargetPositionID;
                    taskHistory.CurrentPositionID = task.CurrentPositionID;
                    taskHistory.CurrentPositionState = task.CurrentPositionState;
                    taskHistory.State = task.State;
                    taskHistory.TagState = task.TagState;
                    taskHistory.Quantity = task.Quantity;
                    taskHistory.TaskQuantity = task.TaskQuantity;
                    taskHistory.OperateQuantity = task.OperateQuantity;
                    taskHistory.OrderID = task.OrderID;
                    taskHistory.OrderType = task.OrderType;
                    taskHistory.AllotID = task.AllotID;
                    taskHistory.DownloadState = task.DownloadState;
                    taskHistory.ClearTime = System.DateTime.Now;

                    TaskHistoryRepository.Add(taskHistory);
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    TaskHistoryRepository.SaveChanges();

                    string constr = ConfigurationManager.ConnectionStrings["AuthorizeContext"].ConnectionString;
                    SqlConnection con = new SqlConnection(constr);
                    string sql = string.Format("truncate table wcs_task where orderID='{0}' ", orderID);
                    SqlCommand cmd = new SqlCommand(sql, con);
                    try
                    {
                        con.Open();
                        int i = (int)cmd.ExecuteNonQuery();
                        if (i > 0) result = true; else result = false;
                    }
                    catch (Exception) {  }
                    finally { con.Close(); }

                    scope.Complete();
                    result = true;
                }
            }
            else
            {

            }
            return result;
        }

        public int FinishStockOutTask(int taskID, int stockOutQuantity)
        {
            var task = TaskRepository.GetQueryable().Where(i => i.ID == taskID).FirstOrDefault();
            if (task != null && task.State == "04")
            {
                FinishOutBillTask(task.OrderID, task.AllotID);
                if (task.Quantity > task.TaskQuantity)
                {
                    return CreateNewTaskForMoveBackRemainAndReturnTaskID(taskID);
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }
        public int FinishInventoryTask(int taskID, int realQuantity)
        {
            var task = TaskRepository.GetQueryable().Where(i => i.ID == taskID).FirstOrDefault();
            if (task != null && task.State == "04")
            {
                FinishCheckBillTask(task.OrderID, task.AllotID);

                if (task.Quantity > task.TaskQuantity)
                {
                    return CreateNewTaskForMoveBackRemainAndReturnTaskID(taskID);
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }

        private bool FinishStockOutRemainMoveBackTask(string orderID, int allotID)
        {
            var checkDetail = OutBillAllotRepository.GetQueryable()
                                    .Where(i => i.BillNo == orderID
                                        && i.ID == allotID)
                                    .FirstOrDefault();

            if (checkDetail != null)
            {
                if (checkDetail.Storage.Cell.FirstInFirstOut) checkDetail.Storage.StorageSequence = checkDetail.Storage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                if (!checkDetail.Storage.Cell.FirstInFirstOut) checkDetail.Storage.StorageSequence = checkDetail.Storage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                return true;
            }
            return false;
        }
        private bool FinishInventoryRemainMoveBackTask(string orderID, int allotID)
        {
            var outAllot = CheckBillDetailRepository.GetQueryable()
                                        .Where(i => i.BillNo == orderID
                                            && i.ID == allotID)
                                        .FirstOrDefault();
            if (outAllot != null)
            {
                if (outAllot.Storage.Cell.FirstInFirstOut) outAllot.Storage.StorageSequence = outAllot.Storage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                if (!outAllot.Storage.Cell.FirstInFirstOut) outAllot.Storage.StorageSequence = outAllot.Storage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                return true;
            }
            return false;
        }

        private int CreateNewTaskForMoveBackRemainAndReturnTaskID(int taskID)
        {
            var task = TaskRepository.GetQueryable().Where(i => i.ID == taskID).FirstOrDefault();
            if (task != null)
            {
                var currentPosition = PositionRepository.GetQueryable()
                   .Where(i => i.ID == task.CurrentPositionID).FirstOrDefault();
                if (currentPosition == null) return 0;

                var targetPosition = PositionRepository.GetQueryable()
                   .Where(i => i.ID == task.OriginPositionID).FirstOrDefault();
                if (targetPosition == null) return 0;

                var path = PathRepository.GetQueryable()
                                .Where(p => p.OriginRegion.ID == currentPosition.Region.ID
                                    && p.TargetRegion.ID == targetPosition.Region.ID)
                                    .FirstOrDefault();
                if (path == null) return 0;

                var newTask = new Task();
                newTask.TaskType = "01";
                newTask.TaskLevel = 0;
                newTask.PathID = path.ID;
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
                newTask.Quantity = task.Quantity - task.OperateQuantity;
                newTask.TaskQuantity = task.Quantity - task.OperateQuantity;
                newTask.OperateQuantity = 0;
                newTask.OrderID = task.OrderID;
                newTask.OrderType = task.OrderType == "02"?"08":"09";
                newTask.AllotID = task.AllotID;
                newTask.DownloadState = "1";
                newTask.StorageSequence = 0;
                TaskRepository.Add(newTask);
                TaskRepository.SaveChanges();
                return newTask.ID;
            }
            return 0;
        }

        public void AutoCreateMoveBill()
        {
            var positions = PositionRepository.GetQueryable()
                .Where(i => i.PositionType == "02"
                    && !i.HasGoods 
                    && !string.IsNullOrEmpty(i.ChannelCode) 
                    && i.ChannelCode != "0");

            var cellPositions= CellPositionRepository.GetQueryable()
                .Where(i => positions.Contains(i.StockInPosition));

            var cells = CellRepository.GetQueryable()
                .Where(i => cellPositions.Any(p => p.CellCode == i.CellCode)
                    && !string.IsNullOrEmpty(i.DefaultProductCode))
                .ToArray();

            if (cells.Count() > 0)
            {
                string WarehouseCode = "", MoveBillTypeCode = "", operatePersonID = "";
                MoveBillMaster moveBillMaster = MoveBillCreater.CreateMoveBillMaster(WarehouseCode, MoveBillTypeCode, operatePersonID);
                moveBillMaster.Origin = "2";
                moveBillMaster.Description = "系统自动生成补大品种拆盘位移库单！";
                moveBillMaster.Status = "2";
                moveBillMaster.VerifyDate = DateTime.Now;

                foreach (var cell in cells)
                {
                    AlltoMoveBill(moveBillMaster, cell.Product, cell);
                }

                if (moveBillMaster.MoveBillDetails.Count > 0)
                {
                    CellRepository.SaveChanges();
                    string errorInfo = string.Empty;
                    MoveBillTask(moveBillMaster.BillNo, out errorInfo);
                }
            }
        }

        private void AlltoMoveBill(MoveBillMaster moveBillMaster, Product product, Cell cell)
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
                //分配整托盘烟；大品种区 
                var storages = storageQuery.Where(s => product.PointAreaCodes.Contains(s.Cell.AreaCode)
                                        && s.ProductCode == product.ProductCode)
                                  .OrderBy(s => new { s.Cell.StorageTime, s.Cell.Area.AllotOutOrder,s.CellCode, s.StorageSequence, s.Quantity });
                AllotPallet(moveBillMaster, storages, cell);
            }
        }

        private void AllotPallet(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> storages, Cell cell)
        {
            foreach (var s in storages.ToArray())
            {
                int storageSequence = s.Cell.Storages.Where(t=>t.Quantity - t.OutFrozenQuantity > 0).Min(t => t.StorageSequence);
                decimal allotQuantity = Math.Floor((s.Quantity - s.OutFrozenQuantity) / s.Product.Unit.Count) * s.Product.Unit.Count;

                if (allotQuantity > 0 
                    && allotQuantity == s.Quantity - s.OutFrozenQuantity
                    && storageSequence == s.StorageSequence)
                {
                    var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                    var targetStorage = Locker.LockStorage(cell);
                    if (sourceStorage != null && targetStorage != null
                        && targetStorage.Quantity == 0
                        && targetStorage.InFrozenQuantity == 0)
                    {
                        MoveBillCreater.AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, allotQuantity, "0");
                        break;
                    }
                }
            }
        }
    }
}
