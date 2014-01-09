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
using THOK.Common.Entity;
using System.Transactions;

namespace THOK.WCS.Bll.Service
{
    public class TaskService : ITaskService
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
        public ISortingLineRepository SortingLineRepository { get; set; }

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
                task.OrderType = "00";
            if (task.AllotID.ToString() == "" || task.AllotID.ToString() == null)
                task.AllotID = 0;
            if (task.DownloadState == "" || task.DownloadState == null)
                task.DownloadState = "0";
            if (task.StorageSequence == 0 || task.StorageSequence == null)
                task.StorageSequence = 0;
            if (task.OriginStorageCode == "" || task.OriginStorageCode == null)
                task.OriginStorageCode = "";
            if (task.TargetStorageCode == "" || task.TargetStorageCode == null)
                task.TargetStorageCode = "";
        }

        public object GetDetails(int page, int rows, Task task)
        {
            IQueryable<Task> taskQuery = TaskRepository.GetQueryable();
            IQueryable<Path> pathQuery = PathRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            var taskQuery0 = taskQuery;
            if (task.ID != 0 && task.ID.ToString() != null)
            {
                taskQuery0 = taskQuery.Where(a => a.ID == task.ID);
            }
            var taskQuery1 = taskQuery0;
            if (task.PathID != 0 && task.PathID.ToString() != null)
            {
                taskQuery1 = taskQuery0.Where(a => a.PathID == task.PathID);
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
            var taskQuery5 = taskQuery4;
            if (task.AllotID != 0 && task.AllotID.ToString() != null)
            {
                taskQuery5 = taskQuery4.Where(a => a.AllotID == task.AllotID);
            }

            #region taskQuery6
            var taskQuery6 = taskQuery5.Join(pathQuery, t => t.PathID, p => p.ID, (t, p) => new
            {
                t.ID,
                t.TaskType,
                t.TaskLevel,
                t.PathID,
                t.ProductCode,
                t.ProductName,
                t.OriginCellCode,
                t.TargetCellCode,
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
                t.StorageSequence,
                t.CreateTime,
                p.PathName,
                OriginRegionName = p.OriginRegion.RegionName,
                TargetRegionName = p.TargetRegion.RegionName
            })
            .Join(cellQuery, t => t.OriginCellCode, c => c.CellCode, (t, c) => new
            {
                t.ID,
                t.TaskType,
                t.TaskLevel,
                t.PathID,
                t.ProductCode,
                t.ProductName,
                t.OriginCellCode,
                t.TargetCellCode,
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
                t.StorageSequence,
                t.CreateTime,
                t.OriginRegionName,
                t.TargetRegionName,
                t.PathName,
                OriginCellName = c.CellName
            })
            .Join(cellQuery, t => t.TargetCellCode, c => c.CellCode, (t, c) => new
            {
                t.ID,
                t.TaskType,
                t.TaskLevel,
                t.PathID,
                t.ProductCode,
                t.ProductName,
                t.OriginCellCode,
                t.TargetCellCode,
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
                t.StorageSequence,
                t.CreateTime,
                t.OriginRegionName,
                t.TargetRegionName,
                t.PathName,
                t.OriginCellName,
                TargetCellName = c.CellName
            })
            .Join(positionQuery, t => t.OriginPositionID, p => p.ID, (t, p) => new
            {
                t.ID,
                t.TaskType,
                t.TaskLevel,
                t.PathID,
                t.ProductCode,
                t.ProductName,
                t.OriginCellCode,
                t.TargetCellCode,
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
                t.StorageSequence,
                t.CreateTime,
                t.OriginRegionName,
                t.TargetRegionName,
                t.PathName,
                t.OriginCellName,
                t.TargetCellName,
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
                t.OriginCellCode,
                t.TargetCellCode,
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
                t.StorageSequence,
                t.CreateTime,
                t.OriginRegionName,
                t.TargetRegionName,
                t.PathName,
                t.OriginCellName,
                t.TargetCellName,
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
                t.OriginCellCode,
                t.TargetCellCode,
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
                t.StorageSequence,
                t.CreateTime,
                t.OriginRegionName,
                t.TargetRegionName,
                t.PathName,
                t.OriginCellName,
                t.TargetCellName,
                t.OriginPositionName,
                t.TargetPositionName,
                CurrentPositionName = p.PositionName
            })
            .Where(t => t.TaskType.Contains(task.TaskType)
                        && t.ProductCode.Contains(task.ProductCode)
                        && t.ProductName.Contains(task.ProductName)
                        && t.OriginCellCode.Contains(task.OriginCellCode)
                        && t.TargetCellCode.Contains(task.TargetCellCode)
                        && t.CurrentPositionState.Contains(task.CurrentPositionState)
                        && t.State.Contains(task.State)
                        && t.TagState.Contains(task.TagState)
                        && t.OrderID.Contains(task.OrderID)
                        && t.OrderType.Contains(task.OrderType)
                        && t.DownloadState.Contains(task.DownloadState)
                        )
            .OrderByDescending(t => t.ID)
            .Select(t => t);
            #endregion

            int total = taskQuery6.Count();
            taskQuery6 = taskQuery6.Skip((page - 1) * rows).Take(rows);

            var taskQuery7 = taskQuery6.ToArray().Select(t => new
                      {
                          t.ID,
                          t.PathID,
                          t.OriginPositionID,
                          t.TargetPositionID,
                          t.CurrentPositionID,
                          TaskType = t.TaskType == "01" ? "正常任务" : t.TaskType == "02" ? "叠空托盘" : t.TaskType == "03" ? "自动移库" : "异常",
                          t.TaskLevel,
                          t.OriginRegionName,
                          t.TargetRegionName,
                          t.ProductCode,
                          t.ProductName,
                          t.PathName,
                          t.OriginCellCode,
                          t.OriginCellName,
                          t.TargetCellCode,
                          t.TargetCellName,
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
                          OrderType = t.OrderType == "00" ? "无" : t.OrderType == "01" ? "入库单" : t.OrderType == "02" ? "移库单" : t.OrderType == "03" ? "出库单" : t.OrderType == "04" ? "盘点单" : t.OrderType == "05" ? "叠空托盘" : t.OrderType == "06" ? "补空托盘" : t.OrderType == "07" ? "移库余烟反库" : t.OrderType == "08" ? "出库余烟返库" : t.OrderType == "09" ? "盘点余烟返库" : "异常",
                          t.AllotID,
                          DownloadState = t.DownloadState == "0" ? "未下载" : t.DownloadState == "1" ? "已下载" : "异常",
                          t.StorageSequence,
                          CreateTime = t.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                      });
            return new { total, rows = taskQuery7 };
        }
        public bool Add(Task task, out string strResult)
        {
            bool bResult = false;
            strResult = string.Empty;

            InitValue(task);

            var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.OriginCellCode));
            var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.TargetCellCode));
            if (originCellPosition != null && targetCellPosition != null)
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
                                t.OriginCellCode = task.OriginCellCode;
                                t.TargetCellCode = task.TargetCellCode;
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
                                t.StorageSequence = task.StorageSequence;
                                t.CreateTime = System.DateTime.Now;

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
                var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.OriginCellCode));
                var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode.Contains(task.TargetCellCode));
                if (originCellPosition != null && targetCellPosition != null)
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
                                    t.OriginCellCode = task.OriginCellCode;
                                    t.TargetCellCode = task.TargetCellCode;
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
                                    t.StorageSequence = task.StorageSequence;

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

        public bool ClearTask(out string errorInfo)
        {
            bool result = true; errorInfo = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var tasks = TaskRepository.GetQueryable()
                        .Where(t => t.State == "04"
                            || (t.OrderType == "01" && t.CurrentPositionID == t.OriginPositionID && t.CurrentPositionState == "01")
                            || (t.OrderType != "01" && t.OrderType != "05"
                                && t.OrderType != "06" && t.OrderType != "07"
                                && t.OrderType != "08" && t.OrderType != "09"
                                && t.CurrentPositionID == t.OriginPositionID && t.State == "01"));

                    foreach (var task in tasks)
                    {
                        AddTaskHistorys(task);
                    }

                    TaskHistoryRepository.SaveChanges();

                    TaskRepository.GetObjectSet().DeleteEntity(t => t.State == "04"
                            || (t.OrderType == "01" && t.CurrentPositionID == t.OriginPositionID && t.CurrentPositionState == "01")
                            || (t.OrderType != "01" && t.OrderType != "05"
                                && t.OrderType != "06" && t.OrderType != "07"
                                && t.OrderType != "08" && t.OrderType != "09"
                                && t.CurrentPositionID == t.OriginPositionID && t.State == "01"));

                    if (TaskRepository.GetQueryable().Count() == 0)
                    {
                        TaskRepository.GetObjectContext().ExecuteStoreCommand("truncate table wcs_task");
                    }

                    scope.Complete();
                    return result;
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        public bool ClearTask(string orderID, out string errorInfo)
        {
            bool result = true; errorInfo = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var tasks = TaskRepository.GetQueryable()
                        .Where(t => t.OrderID == orderID
                            && (t.State == "04"
                                || (t.OrderType == "01" && t.CurrentPositionID == t.OriginPositionID && t.CurrentPositionState == "01")
                                || (t.OrderType != "01" && t.OrderType != "05"
                                    && t.OrderType != "06" && t.OrderType != "07"
                                    && t.OrderType != "08" && t.OrderType != "09"
                                    && t.CurrentPositionID == t.OriginPositionID && t.State == "01")));

                    foreach (var task in tasks)
                    {
                        AddTaskHistorys(task);
                    }

                    TaskHistoryRepository.SaveChanges();

                    TaskRepository.GetObjectSet().DeleteEntity(t => t.OrderID == orderID
                        && (t.State == "04"
                            || (t.OrderType == "01" && t.CurrentPositionID == t.OriginPositionID && t.CurrentPositionState == "01")
                            || (t.OrderType != "01" && t.OrderType != "05"
                                && t.OrderType != "06" && t.OrderType != "07"
                                && t.OrderType != "08" && t.OrderType != "09"
                                && t.CurrentPositionID == t.OriginPositionID && t.State == "01")));

                    if (TaskRepository.GetQueryable().Where(t => t.OrderID == orderID).Count() != 0)
                    {
                        result = false;
                        errorInfo = "当前有任务正在执行中或者未执行完毕！";
                    }

                    scope.Complete();
                    return result;
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        private void AddTaskHistorys(Task task)
        {
            TaskHistory taskHistory = new TaskHistory();
            taskHistory.TaskID = task.ID;
            taskHistory.TaskType = task.TaskType;
            taskHistory.TaskLevel = task.TaskLevel;
            taskHistory.PathID = task.PathID;
            taskHistory.ProductCode = task.ProductCode;
            taskHistory.ProductName = task.ProductName;
            taskHistory.OriginCellCode = task.OriginCellCode;
            taskHistory.TargetCellCode = task.TargetCellCode;
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
            taskHistory.StorageSequence = task.StorageSequence;
            taskHistory.CreateTime = task.CreateTime;
            taskHistory.ClearTime = System.DateTime.Now;

            TaskHistoryRepository.Add(taskHistory);
        }

        public bool CreateInBillTask(string billNo, out string errorInfo)
        {
            errorInfo = string.Empty;

            if (TaskRepository.GetQueryable().Where(t => t.OrderID == billNo).Count() > 0)
            {
                errorInfo = "当前订单已作业！";
                return false;
            }

            var originSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName == "InBillPositionId");
            if (originSystemParam == null) { errorInfo = "请检查系统参数，未找到参数InBillPosition！"; return false; }
            int paramValue = Convert.ToInt32(originSystemParam.ParameterValue);

            var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == paramValue);
            if (originPosition == null) { errorInfo = "未找到起始位置ID（移出位置）：" + paramValue; return false; }
            var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(i => i.StockOutPositionID == originPosition.ID);
            if (originCellPosition == null) { errorInfo = "未找到起始货位位置的起始位置：" + originPosition.PositionName; return false; }

            var originCell = CellRepository.GetQueryable().FirstOrDefault(i => i.CellCode == originCellPosition.CellCode);
            if (originCell == null) { errorInfo = "未找到起始货位编码：" + originCellPosition.CellCode; return false; }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var inBillMaster = InBillMasterRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && (i.Status == "4" || i.Status == "5"))
                        .FirstOrDefault();
                    if (inBillMaster == null)
                    {
                        errorInfo = "当前订单不存在，或不可作业！";
                        return false;
                    }

                    var inBillAllots = InBillAllotRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && i.Status == "0");

                    foreach (var inBillAllot in inBillAllots.ToArray())
                    {
                        var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == inBillAllot.CellCode);
                        if (targetCellPosition == null) { errorInfo = "未找到货位位置的入库货位：" + inBillAllot.Cell.CellName; return false; }

                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                        if (targetPosition == null) { errorInfo = "未找到目标位置（移入位置）：" + targetCellPosition.StockInPosition.PositionName; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.Region.RegionName + "，和目标位置：" + targetPosition.Region.RegionName; return false; }

                        Task inTask = new Task();
                        inTask.TaskType = "01";
                        inTask.TaskLevel = 0;
                        inTask.PathID = path.ID;
                        inTask.ProductCode = inBillAllot.Product.ProductCode;
                        inTask.ProductName = inBillAllot.Product.ProductName;
                        inTask.OriginCellCode = originCell.CellCode;
                        inTask.TargetCellCode = inBillAllot.CellCode;
                        inTask.OriginStorageCode = "";
                        inTask.TargetStorageCode = "";
                        inTask.OriginPositionID = Convert.ToInt32(originSystemParam.ParameterValue);
                        inTask.TargetPositionID = targetPosition.ID;
                        inTask.CurrentPositionID = Convert.ToInt32(originSystemParam.ParameterValue);
                        inTask.CurrentPositionState = "01";
                        inTask.State = "01";
                        inTask.TagState = "01";
                        inTask.Quantity = Convert.ToInt32(inBillAllot.Storage.Quantity / inBillAllot.Product.Unit.Count);
                        inTask.TaskQuantity = Convert.ToInt32(inBillAllot.AllotQuantity / inBillAllot.Product.Unit.Count);
                        inTask.OperateQuantity = 0;
                        inTask.OrderID = inBillAllot.BillNo;
                        inTask.OrderType = "01";
                        inTask.AllotID = inBillAllot.ID;
                        inTask.DownloadState = "0";
                        inTask.StorageSequence = 0;
                        inTask.CreateTime = System.DateTime.Now;

                        TaskRepository.Add(inTask);
                    }

                    inBillMaster.Status = "5";
                    TaskRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        public bool CreateOutBillTask(string billNo, out string errorInfo)
        {
            errorInfo = string.Empty;

            if (TaskRepository.GetQueryable().Where(t => t.OrderID == billNo).Count() > 0)
            {
                errorInfo = "当前订单已作业！";
                return false;
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var outBillMaster = OutBillMasterRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && (i.Status == "4" || i.Status == "5"))
                        .FirstOrDefault();
                    if (outBillMaster == null)
                    {
                        errorInfo = "当前订单不存在，或不可作业！";
                        return false;
                    }

                    var outBillAllots = OutBillAllotRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && i.Status == "0");

                    foreach (var outBillAllot in outBillAllots.ToArray())
                    {
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == outBillAllot.CellCode);
                        if (originCellPosition == null) { errorInfo = "未找到货位位置的起始货位位置：" + outBillAllot.Cell.CellName; return false; }
                        var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                        if (originPosition == null) { errorInfo = "未找到起始货位位置：" + originCellPosition.StockOutPosition.PositionName; return false; }

                        var targetSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName.Contains(originPosition.SRMName) && s.ParameterName.Contains("StockOutAndCheckPositionID"));
                        if (targetSystemParam == null) { errorInfo = "请检查系统参数，未找到目标位置OutBillPosition！"; return false; }
                        int targetPositionID = Convert.ToInt32(targetSystemParam.ParameterValue);

                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetPositionID);
                        if (targetPosition == null) { errorInfo = "未找到目标位置（移入位置）：" + targetPosition.PositionName; return false; }

                        var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(i => i.StockInPositionID == targetPositionID);
                        if (targetCellPosition == null) { errorInfo = "未找到货位位置的目标位置：" + targetCellPosition.StockInPosition.PositionName; return false; }

                        var targetCell = CellRepository.GetQueryable().FirstOrDefault(i => i.CellCode == targetCellPosition.CellCode);
                        if (targetCell == null) { errorInfo = "未找到目标货位编码：" + targetCellPosition.CellCode; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.PositionName + "，目标位置：" + targetPosition.PositionName; return false; }

                        Task outTask = new Task();
                        outTask.TaskType = "01";
                        outTask.TaskLevel = 0;
                        outTask.PathID = path.ID;
                        outTask.ProductCode = outBillAllot.Product.ProductCode;
                        outTask.ProductName = outBillAllot.Product.ProductName;
                        outTask.OriginCellCode = outBillAllot.CellCode;
                        outTask.TargetCellCode = targetCell.CellCode;
                        outTask.OriginStorageCode = "";
                        outTask.TargetStorageCode = "";
                        outTask.OriginPositionID = originPosition.ID;
                        outTask.TargetPositionID = targetPosition.ID;
                        outTask.CurrentPositionID = originPosition.ID;
                        outTask.CurrentPositionState = "02";
                        outTask.State = "01";
                        outTask.TagState = "01";
                        outTask.Quantity = Convert.ToInt32(outBillAllot.Storage.Quantity / outBillAllot.Product.Unit.Count);
                        outTask.TaskQuantity = Convert.ToInt32(outBillAllot.AllotQuantity / outBillAllot.Product.Unit.Count);
                        outTask.OperateQuantity = 0;
                        outTask.OrderID = outBillAllot.BillNo;
                        outTask.OrderType = "03";
                        outTask.AllotID = outBillAllot.ID;
                        outTask.DownloadState = "0";
                        outTask.StorageSequence = outBillAllot.Storage.StorageSequence;
                        outTask.CreateTime = System.DateTime.Now;

                        TaskRepository.Add(outTask);
                    }

                    outBillMaster.Status = "5";
                    TaskRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        public bool CreateMoveBillTask(string billNo, int taskLevel, out string errorInfo)
        {
            errorInfo = string.Empty;

            if (TaskRepository.GetQueryable().Where(t => t.OrderID == billNo).Count() > 0)
            {
                errorInfo = "当前订单已作业！";
                return false;
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var moveBillMaster = MoveBillMasterRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && (i.Status == "2" || i.Status == "3"))
                        .FirstOrDefault();
                    if (moveBillMaster == null)
                    {
                        errorInfo = "当前订单不存在，或不可作业！";
                        return false;
                    }

                    var moveBillDetails = MoveBillDetailRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && i.Status == "0");

                    foreach (var moveBillDetail in moveBillDetails.ToArray())
                    {
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.OutCellCode);
                        if (originCellPosition == null) { errorInfo = "未找到货位位置的起始货位位置：" + moveBillDetail.OutCell.CellName; return false; }
                        var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                        if (originPosition == null) { errorInfo = "未找到起始货位位置：" + originCellPosition.StockOutPosition.PositionName; return false; }

                        int targetPositionID = 0;
                        if (SortingLineRepository.GetQueryable().Where(s => s.CellCode == moveBillDetail.InCellCode).Count() > 0)
                        {
                            var targetSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName.Contains(originPosition.SRMName) && s.ParameterName.Contains("StockOutAndCheckPositionID"));
                            if (targetSystemParam == null) { errorInfo = "请检查系统参数，未找到目标位置OutBillPosition！"; return false; }
                            targetPositionID = Convert.ToInt32(targetSystemParam.ParameterValue);
                        }

                        var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => (targetPositionID > 0 && c.StockInPositionID == targetPositionID) || (targetPositionID == 0 && c.CellCode == moveBillDetail.InCellCode));
                        if (targetCellPosition == null) { errorInfo = "未找到货位位置的目标货位位置：" + moveBillDetail.InCell.CellName; return false; }
                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                        if (targetPosition == null) { errorInfo = "未找到目标货位位置：" + targetCellPosition.StockInPosition.PositionName; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.PositionName + "，目标位置：" + targetPosition.PositionName; return false; }

                        var moveTask = new Task();
                        moveTask.TaskType = "01";
                        moveTask.TaskLevel = taskLevel;
                        moveTask.PathID = path.ID;
                        moveTask.ProductCode = moveBillDetail.Product.ProductCode;
                        moveTask.ProductName = moveBillDetail.Product.ProductName;
                        moveTask.OriginCellCode = moveBillDetail.OutCellCode;
                        moveTask.TargetCellCode = moveBillDetail.InCellCode;
                        moveTask.OriginStorageCode = "";
                        moveTask.TargetStorageCode = "";
                        moveTask.OriginPositionID = originPosition.ID;
                        moveTask.TargetPositionID = targetPosition.ID;
                        moveTask.CurrentPositionID = originPosition.ID;
                        moveTask.CurrentPositionState = "02";
                        moveTask.State = "01";
                        moveTask.TagState = "01";
                        moveTask.Quantity = Convert.ToInt32(moveBillDetail.OutStorage.Quantity / moveBillDetail.Product.Unit.Count);
                        moveTask.TaskQuantity = Convert.ToInt32(moveBillDetail.RealQuantity / moveBillDetail.Product.Unit.Count);
                        moveTask.OperateQuantity = 0;
                        moveTask.OrderID = moveBillDetail.BillNo;
                        moveTask.OrderType = "02";
                        moveTask.AllotID = moveBillDetail.ID;
                        moveTask.DownloadState = "0";
                        moveTask.StorageSequence = moveBillDetail.OutStorage.StorageSequence;
                        moveTask.CreateTime = System.DateTime.Now;

                        TaskRepository.Add(moveTask);
                    }

                    moveBillMaster.Status = "3";
                    TaskRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        public bool CreateAutoMoveBillTask(string billNo, int taskLevel, out string errorInfo)
        {
            errorInfo = string.Empty;

            if (TaskRepository.GetQueryable().Where(t => t.OrderID == billNo).Count() > 0)
            {
                errorInfo = "当前订单已作业！";
                return false;
            }

            try
            {
                var moveBillMaster = MoveBillMasterRepository.GetQueryable()
                    .Where(i => i.BillNo == billNo && (i.Status == "2" || i.Status == "3"))
                    .FirstOrDefault();
                if (moveBillMaster == null)
                {
                    errorInfo = "当前订单不存在，或不可作业！";
                    return false;
                }

                var moveBillDetails = MoveBillDetailRepository.GetQueryable()
                    .Where(i => i.BillNo == billNo && i.Status == "0");

                foreach (var moveBillDetail in moveBillDetails.ToArray())
                {
                    var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.OutCellCode);
                    if (originCellPosition == null) { errorInfo = "未找到货位位置的起始货位位置：" + moveBillDetail.OutCell.CellName; return false; }
                    var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                    if (originPosition == null) { errorInfo = "未找到起始货位位置：" + originCellPosition.StockOutPosition.PositionName; return false; }

                    var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.InCellCode);
                    if (targetCellPosition == null) { errorInfo = "未找到货位位置的目标货位位置：" + moveBillDetail.InCell.CellName; return false; }
                    var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                    if (targetPosition == null) { errorInfo = "未找到目标货位位置：" + targetCellPosition.StockInPosition.PositionName; return false; }

                    var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                    if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.PositionName + "，目标位置：" + targetPosition.PositionName; return false; }

                    var moveTask = new Task();
                    moveTask.TaskType = "03";
                    moveTask.TaskLevel = taskLevel;
                    moveTask.PathID = path.ID;
                    moveTask.ProductCode = moveBillDetail.Product.ProductCode;
                    moveTask.ProductName = moveBillDetail.Product.ProductName;
                    moveTask.OriginCellCode = moveBillDetail.OutCellCode;
                    moveTask.TargetCellCode = moveBillDetail.InCellCode;
                    moveTask.OriginStorageCode = "";
                    moveTask.TargetStorageCode = "";
                    moveTask.OriginPositionID = originPosition.ID;
                    moveTask.TargetPositionID = targetPosition.ID;
                    moveTask.CurrentPositionID = originPosition.ID;
                    moveTask.CurrentPositionState = "02";
                    moveTask.State = "01";
                    moveTask.TagState = "01";
                    moveTask.Quantity = Convert.ToInt32(moveBillDetail.OutStorage.Quantity / moveBillDetail.Product.Unit.Count);
                    moveTask.TaskQuantity = Convert.ToInt32(moveBillDetail.RealQuantity / moveBillDetail.Product.Unit.Count);
                    moveTask.OperateQuantity = 0;
                    moveTask.OrderID = moveBillDetail.BillNo;
                    moveTask.OrderType = "02";
                    moveTask.AllotID = moveBillDetail.ID;
                    moveTask.DownloadState = "0";
                    moveTask.StorageSequence = moveBillDetail.OutStorage.StorageSequence;
                    moveTask.CreateTime = System.DateTime.Now;

                    TaskRepository.Add(moveTask);
                }

                moveBillMaster.Status = "3";
                TaskRepository.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        public bool CreateCheckBillTask(string billNo, out string errorInfo)
        {
            errorInfo = string.Empty;

            if (CheckBillMasterRepository.GetQueryable()
                .Where(i => i.BillNo == billNo
                    && i.Warehouse.WarehouseType == "3"
                    && i.BillTypeCode == "4003"
                ).Count() != 0)
            {
                errorInfo = "该仓库是密集库，无法使用异动盘点进行作业！请打印单子进行盘点！";
                return false;
            }

            if (TaskRepository.GetQueryable().Where(t => t.OrderID == billNo).Count() > 0)
            {
                errorInfo = "当前订单已作业！";
                return false;
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var checkBillMaster = CheckBillMasterRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && (i.Status == "2" || i.Status == "3"))
                        .FirstOrDefault();
                    if (checkBillMaster == null)
                    {
                        errorInfo = "当前订单不存在，或不可作业！";
                        return false;
                    }

                    var checkBillDetails = CheckBillDetailRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && i.Status == "0");

                    foreach (var checkItem in checkBillDetails.ToArray())
                    {
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == checkItem.CellCode);
                        if (originCellPosition == null) { errorInfo = "未找到货位位置的起始货位位置：" + checkItem.Cell.CellName; return false; }
                        var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                        if (originPosition == null) { errorInfo = "未找到起始货位位置：" + originCellPosition.StockOutPosition.PositionName; return false; }

                        var targetSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName.Contains(originPosition.SRMName) && s.ParameterName.Contains("StockOutAndCheckPositionID"));
                        if (targetSystemParam == null) { errorInfo = "请检查系统参数，未找到目标位置OutBillPosition！"; return false; }
                        int targetPositionID = Convert.ToInt32(targetSystemParam.ParameterValue);

                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetPositionID);
                        if (targetPosition == null) { errorInfo = "未找到目标位置（移入位置）：" + targetPosition.PositionName; return false; }

                        var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(i => i.StockInPositionID == targetPositionID);
                        if (targetCellPosition == null) { errorInfo = "未找到货位位置的目标位置：" + targetCellPosition.StockInPosition.PositionName; return false; }

                        var targetCell = CellRepository.GetQueryable().FirstOrDefault(i => i.CellCode == targetCellPosition.CellCode);
                        if (targetCell == null) { errorInfo = "未找到目标货位编码：" + targetCellPosition.CellCode; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.PositionName + "，目标位置：" + targetPosition.PositionName; return false; }

                        Task checkTask = new Task();
                        checkTask.TaskType = "01";
                        checkTask.TaskLevel = 0;
                        checkTask.PathID = path.ID;
                        checkTask.ProductCode = checkItem.Product.ProductCode;
                        checkTask.ProductName = checkItem.Product.ProductName;
                        checkTask.OriginCellCode = checkItem.CellCode;
                        checkTask.TargetCellCode = targetCell.CellCode;
                        checkTask.OriginStorageCode = "";
                        checkTask.TargetStorageCode = "";
                        checkTask.OriginPositionID = originPosition.ID;
                        checkTask.TargetPositionID = targetPosition.ID;
                        checkTask.CurrentPositionID = originPosition.ID;
                        checkTask.CurrentPositionState = "02";
                        checkTask.State = "01";
                        checkTask.TagState = "01";
                        checkTask.Quantity = Convert.ToInt32(checkItem.Storage.Quantity / checkItem.Product.Unit.Count);
                        checkTask.TaskQuantity = Convert.ToInt32(checkItem.RealQuantity / checkItem.Product.Unit.Count);
                        checkTask.OperateQuantity = 0;
                        checkTask.OrderID = checkItem.BillNo;
                        checkTask.OrderType = "04";
                        checkTask.AllotID = checkItem.ID;
                        checkTask.DownloadState = "0";
                        checkTask.StorageSequence = checkItem.Storage.StorageSequence;
                        checkTask.CreateTime = System.DateTime.Now;

                        TaskRepository.Add(checkTask);
                    }

                    checkBillMaster.Status = "3";
                    TaskRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        public bool CreateSortWorkDispatchTask(string billNo, out string errorInfo)
        {
            errorInfo = string.Empty;

            if (TaskRepository.GetQueryable().Where(t => t.OrderID == billNo).Count() > 0)
            {
                errorInfo = "当前订单已作业！";
                return false;
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var moveBillMaster = MoveBillMasterRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && (i.Status == "2" || i.Status == "3"))
                        .FirstOrDefault();
                    if (moveBillMaster == null)
                    {
                        errorInfo = "当前订单不存在，或不可作业！";
                        return false;
                    }

                    var moveBillDetails = MoveBillDetailRepository.GetQueryable()
                        .Where(i => i.BillNo == billNo && i.Status == "0");

                    foreach (var moveBillDetail in moveBillDetails.ToArray())
                    {
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.OutCellCode);
                        if (originCellPosition == null) { errorInfo = "未找到货位位置的起始货位位置：" + moveBillDetail.OutCell.CellName; return false; }
                        var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                        if (originPosition == null) { errorInfo = "未找到起始货位位置：" + originCellPosition.StockOutPosition.PositionName; return false; }

                        var targetSystemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName.Contains(originPosition.SRMName) && s.ParameterName.Contains("StockOutAndCheckPositionID"));
                        if (targetSystemParam == null) { errorInfo = "请检查系统参数，未找到目标位置OutBillPosition！"; return false; }
                        int targetPositionID = Convert.ToInt32(targetSystemParam.ParameterValue);

                        var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.StockInPositionID == targetPositionID);
                        if (targetCellPosition == null) { errorInfo = "未找到货位位置的目标货位位置：" + moveBillDetail.InCell.CellName; return false; }
                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                        if (targetPosition == null) { errorInfo = "未找到目标货位位置：" + targetCellPosition.StockInPosition.PositionName; return false; }

                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                        if (path == null) { errorInfo = "未找到路径信息的起始位置：" + originPosition.PositionName + "，目标位置：" + targetPosition.PositionName; return false; }

                        var moveTask = new Task();
                        moveTask.TaskType = "01";
                        moveTask.TaskLevel = 12;
                        moveTask.PathID = path.ID;
                        moveTask.ProductCode = moveBillDetail.Product.ProductCode;
                        moveTask.ProductName = moveBillDetail.Product.ProductName;
                        moveTask.OriginCellCode = moveBillDetail.OutCellCode;
                        moveTask.TargetCellCode = moveBillDetail.InCellCode;
                        moveTask.OriginStorageCode = "";
                        moveTask.TargetStorageCode = "";
                        moveTask.OriginPositionID = originPosition.ID;
                        moveTask.TargetPositionID = targetPosition.ID;
                        moveTask.CurrentPositionID = originPosition.ID;
                        moveTask.CurrentPositionState = "02";
                        moveTask.State = "01";
                        moveTask.TagState = "01";
                        moveTask.Quantity = Convert.ToInt32(moveBillDetail.OutStorage.Quantity / moveBillDetail.Product.Unit.Count);
                        moveTask.TaskQuantity = Convert.ToInt32(moveBillDetail.RealQuantity / moveBillDetail.Product.Unit.Count);
                        moveTask.OperateQuantity = 0;
                        moveTask.OrderID = moveBillDetail.BillNo;
                        moveTask.OrderType = "02";
                        moveTask.AllotID = moveBillDetail.ID;
                        moveTask.DownloadState = "0";
                        moveTask.StorageSequence = moveBillDetail.OutStorage.StorageSequence;
                        moveTask.CreateTime = System.DateTime.Now;

                        TaskRepository.Add(moveTask);
                    }
                    moveBillMaster.Status = "3";
                    TaskRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }

        public bool CreateNewTaskForEmptyPalletStack(int positionID, string positionName, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var systemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(i => i.ParameterName == "EmptyPallet");
            if (systemParam == null)
            {
                errorInfo = "请检查：系统参数是否存在参数名：EmptyPallet，并且该值是否存在卷烟信息表中！";
                return false;
            }
            string palletCode = systemParam.ParameterValue;

            var position = PositionRepository.GetQueryable().Where(i => (i.ID == positionID || i.PositionName == positionName) && (i.PositionType == "02" || i.PositionType == "03" || i.PositionType == "04")).FirstOrDefault();
            if (position == null && !position.HasGoods)
            {
                errorInfo = "请检查：位置信息不能为空，位置上是否有空托盘，并且位置类型必须是大品种出库位、小品种出库位、异型烟出库位！";
                return false;
            }
            var task = TaskRepository.GetQueryable().Where(t => t.State != "04" && t.OrderType == "05" && t.OriginPositionID == position.ID).FirstOrDefault();
            if (task != null)
            {
                errorInfo = string.Format("已生成一个任务号[{0}]，位置[{1}]已有任务在执行中！", task.ID, position.PositionName);
                return true;
            }
            var positionQuery = PositionRepository.GetQueryable().Where(i => i.SRMName == position.SRMName && i.AbleStockInPallet && i.ID != position.ID);
            if (positionQuery == null)
            {
                errorInfo = string.Format("请检查：堆垛机[{0}]区域的位置[{1}]，必须允许叠空托盘！", position.SRMName, position.PositionName);
                return false;
            }
            var cellPositionQuery = CellPositionRepository.GetQueryable().Where(i => i.StockOutPositionID != position.ID && positionQuery.Contains(i.StockInPosition));
            if (cellPositionQuery == null)
            {
                errorInfo = string.Format("请检查：位置[{0}]必须在货位位置表中存在！", position.PositionName);
                return false;
            }
            CellPosition originCellPosition = CellPositionRepository.GetQueryable().Where(i => i.StockOutPositionID == position.ID).FirstOrDefault();
            Cell originCell = CellRepository.GetQueryable().Where(i => i.CellCode == originCellPosition.CellCode).FirstOrDefault();

            var cellQuery = CellRepository.GetQueryable().Where(i => i.IsSingle == "1"
                    && i.IsActive == "1"
                    && cellPositionQuery.Any(p => p.CellCode == i.CellCode)
                    && (i.Storages.Any(s => s.ProductCode == palletCode
                        && s.Quantity + s.InFrozenQuantity < ((s.Cell.MaxQuantity / 5) * 2) && s.OutFrozenQuantity == 0)));
            if (!cellQuery.Any())
            {
                cellQuery = CellRepository.GetQueryable().Where(i => i.IsSingle == "1"
                    && i.IsActive == "1"
                    && cellPositionQuery.Any(p => p.CellCode == i.CellCode)
                    && (i.Storages.Count == 0 || i.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                        && s.Quantity == 0 && s.InFrozenQuantity == 0)));
            }
            if (cellQuery == null)
            {
                errorInfo = "叠空托盘的目标货位以及个数，请检查：该货位必须是单一货位，" + palletCode + "是否存在于卷烟信息表中。"
                          + "分析引导："
                          + "1.此货位的数量+入库冻结量<托盘的最大托盘个数((最大放入量/5)*2)，并且出库冻结量必须=0；"
                          + "2.LockTag必须未锁定，库存数量和入库冻结量必须=0";
                return false;
            }

            var cell = cellQuery.ToArray().OrderBy(c => Math.Abs(c.Col - c.Shelf.CellCols / 2)).FirstOrDefault();
            if (cell == null)
            {
                errorInfo = "请检查：货位表的字段储位列号Col和货架列数CellCols，计算：Math.Abs(Col - CellCols / 2)是否正确！";
                return false;
            }
            var cellPosition = CellPositionRepository.GetQueryable().Where(cp => cp.CellCode == cell.CellCode).FirstOrDefault();
            if (cellPosition == null)
            {
                errorInfo = string.Format("未找到[{0}]的货位位置！", cell.CellName);
            }

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
            if (targetStorage == null)
            {
                errorInfo = "未找到目标货位的库存或者货位位置不存在！";
            }
            var path = PathRepository.GetQueryable()
                .Where(p => p.OriginRegion.ID == cellPosition.StockOutPosition.Region.ID
                    && p.TargetRegion.ID == position.Region.ID)
                    .FirstOrDefault();
            if (path == null)
            {
                errorInfo = string.Format("从 [{0}] 到 [{1}] 未找到路径!", cellPosition.StockOutPosition.PositionName, position.PositionName);
                return false;
            }
            try
            {
                targetStorage.ProductCode = palletCode;
                targetStorage.InFrozenQuantity += 1;
                var newTask = new Task();
                newTask.TaskType = "02";
                newTask.TaskLevel = 9;
                newTask.PathID = path.ID;
                newTask.ProductCode = palletCode;
                newTask.ProductName = "空托盘";
                newTask.OriginCellCode = originCell.CellCode;
                newTask.TargetCellCode = cell.CellCode;
                newTask.OriginStorageCode = "";
                newTask.TargetStorageCode = targetStorage.StorageCode;
                newTask.OriginPositionID = position.ID;
                newTask.TargetPositionID = cellPosition.StockInPositionID;
                newTask.CurrentPositionID = position.ID;
                newTask.CurrentPositionState = "02";
                newTask.State = "01";
                newTask.TagState = "01";//拟不使用
                newTask.Quantity = 1;
                newTask.TaskQuantity = 1;
                newTask.OperateQuantity = 0;
                newTask.OrderID = "";
                newTask.OrderType = "05";
                newTask.DownloadState = "0";
                newTask.StorageSequence = 0;
                newTask.CreateTime = System.DateTime.Now;

                TaskRepository.Add(newTask);
                TaskRepository.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
            }
            return result;
        }
        public bool CreateNewTaskForEmptyPalletSupply(int positionID, string positionName, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var systemParam = SystemParameterRepository.GetQueryable().FirstOrDefault(i => i.ParameterName == "EmptyPallet");
            if (systemParam == null)
            {
                errorInfo = "请检查：系统参数是否存在参数名：EmptyPallet，并且该值是否存在卷烟信息表中！";
                return false;
            }
            string palletCode = systemParam.ParameterValue;
            int palletCount = 10;

            var storageQuery = StorageRepository.GetQueryable()
                .Where(i => i.ProductCode == palletCode
                    && i.Quantity - i.OutFrozenQuantity >= palletCount
                    && i.OutFrozenQuantity == 0
                    && i.InFrozenQuantity == 0)
                .OrderByDescending(i => i.StorageTime);
            if (!storageQuery.Any())
            {
                storageQuery = StorageRepository.GetQueryable()
                    .Where(i => i.ProductCode == palletCode
                        && i.Quantity - i.OutFrozenQuantity > 0
                        && i.OutFrozenQuantity == 0
                        && i.InFrozenQuantity == 0)
                    .OrderByDescending(i => i.Quantity);
            }
            if (storageQuery == null)
            {
                errorInfo = "请检查：该货位必须是单一货位，" + palletCode + "是否存在于卷烟信息表中。"
                             + "分析引导："
                             + "1.此货位的数量-出库冻结量>=10(托盘数量)，并且入库和出库冻结量必须=0；"
                             + "2.此货位的数量-出库冻结量>0，并且并且入库和出库冻结量必须=0";
                return false;
            }
            var storage = storageQuery.FirstOrDefault();
            if (storage == null)
            {
                errorInfo = "未找到库存！";
                return false;
            }
            var position = PositionRepository.GetQueryable()
                .Where(i => (i.ID == positionID || i.PositionName == positionName)).FirstOrDefault();
            if (position == null || position.PositionType != "05")
            {
                errorInfo = string.Format("未找到空托盘出库位[{0}]的位置信息", positionName);
                return false;
            }
            var positionCell = CellPositionRepository.GetQueryable().Where(i => i.StockInPositionID == position.ID).FirstOrDefault();

            var task = TaskRepository.GetQueryable().Where(t => t.State != "04" && t.OrderType == "06").FirstOrDefault();
            if (task != null)
            {
                errorInfo = string.Format("已生成一个空托盘出库任务号[{0}]正在执行中，未到达空托盘缓存区[{1}]！", task.ID, positionName);
                return false;
            }
            var cellPosition = CellPositionRepository.GetQueryable()
                .Where(cp => cp.CellCode == storage.CellCode).FirstOrDefault();

            if (cellPosition == null)
            {
                errorInfo = string.Format("请检查：未找到货位[{0}]的位置！", storage.Cell.CellName);
                return false;
            }
            var path = PathRepository.GetQueryable()
                .Where(p => p.OriginRegion.ID == cellPosition.StockOutPosition.Region.ID
                    && p.TargetRegion.ID == position.Region.ID)
                    .FirstOrDefault();
            if (path == null)
            {
                errorInfo = string.Format("从 [{0}] 到 [{1}] 未找到路径!", cellPosition.StockOutPosition.PositionName, position.PositionName);
                return false;
            }
            try
            {
                var quantity = storage.Quantity - storage.OutFrozenQuantity;
                storage.OutFrozenQuantity += quantity;
                var newTask = new Task();
                newTask.TaskType = "01";
                newTask.TaskLevel = 20;
                newTask.PathID = path.ID;
                newTask.ProductCode = palletCode;
                newTask.ProductName = "空托盘";
                newTask.OriginCellCode = storage.CellCode;
                newTask.TargetCellCode = positionCell != null ? positionCell.CellCode : "";
                newTask.OriginStorageCode = storage.StorageCode;
                newTask.TargetStorageCode = "";
                newTask.OriginPositionID = cellPosition.StockOutPositionID;
                newTask.TargetPositionID = position.ID;
                newTask.CurrentPositionID = cellPosition.StockOutPositionID;
                newTask.CurrentPositionState = "02";
                newTask.State = "01";
                newTask.TagState = "01";//拟不使用
                newTask.Quantity = Convert.ToInt32(storage.Quantity);
                newTask.TaskQuantity = Convert.ToInt32(quantity);
                newTask.OperateQuantity = 0;
                newTask.OrderID = "";
                newTask.OrderType = "06";
                newTask.DownloadState = "1";
                newTask.StorageSequence = 0;
                newTask.CreateTime = System.DateTime.Now;

                TaskRepository.Add(newTask);
                TaskRepository.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
            }
            return result;
        }
        public bool CreateNewTaskForMoveBackRemain(int taskID, out string errorInfo)
        {
            return CreateNewTaskForMoveBackRemainAndReturnTaskID(taskID, out errorInfo) > 0;
        }
        private int CreateNewTaskForMoveBackRemainAndReturnTaskID(int taskID, out string errorInfo)
        {
            errorInfo = "";
            var task = TaskRepository.GetQueryable().Where(i => i.ID == taskID).FirstOrDefault();
            if (task != null)
            {
                var cellPosition = CellPositionRepository.GetQueryable()
                    .Where(i => i.StockOutPositionID == task.OriginPositionID)
                    .FirstOrDefault();
                if (cellPosition == null) return 0;

                var currentPosition = PositionRepository.GetQueryable()
                   .Where(i => i.ID == task.CurrentPositionID)
                   .FirstOrDefault();
                if (currentPosition == null) return 0;

                var targetPosition = PositionRepository.GetQueryable()
                   .Where(i => i.ID == cellPosition.StockInPositionID)
                   .FirstOrDefault();
                if (targetPosition == null) return 0;

                var path = PathRepository.GetQueryable()
                    .Where(p => p.OriginRegion.ID == currentPosition.Region.ID
                        && p.TargetRegion.ID == targetPosition.Region.ID)
                    .FirstOrDefault();
                if (path == null) return 0;

                var newTask = new Task();
                newTask.TaskType = "01";
                newTask.TaskLevel = 11;
                newTask.PathID = path.ID;
                newTask.ProductCode = task.ProductCode;
                newTask.ProductName = task.ProductName;
                newTask.OriginCellCode = task.TargetCellCode;
                newTask.TargetCellCode = task.OriginCellCode;
                newTask.OriginStorageCode = "";
                newTask.TargetStorageCode = "";
                newTask.OriginPositionID = task.CurrentPositionID;
                newTask.TargetPositionID = cellPosition.StockInPositionID;
                newTask.CurrentPositionID = task.CurrentPositionID;
                newTask.CurrentPositionState = "02";
                newTask.State = "01";
                newTask.TagState = "01";//拟不使用
                newTask.Quantity = task.OrderType == "04" ? task.Quantity : task.Quantity - task.TaskQuantity;
                newTask.TaskQuantity = task.OrderType == "04" ? task.Quantity : task.Quantity - task.TaskQuantity;
                newTask.OperateQuantity = 0;
                newTask.OrderID = task.OrderID;
                newTask.OrderType = task.OrderType == "02" ? "07" : (task.OrderType == "03" ? "08" : "09");
                newTask.AllotID = task.AllotID;
                newTask.DownloadState = "1";
                newTask.StorageSequence = 0;
                newTask.CreateTime = System.DateTime.Now;

                TaskRepository.Add(newTask);
                TaskRepository.SaveChanges();
                return newTask.ID;
            }
            return 0;
        }

        public bool FinishTask(int taskID, out string errorInfo)
        {
            errorInfo = string.Empty;
            var task = TaskRepository.GetQueryable().Where(i => i.ID == taskID).FirstOrDefault();
            if (task != null && task.State == "04")
            {
                return FinishTask(task.ID, task.OrderType, task.OrderID, task.AllotID, task.OriginCellCode, task.TargetCellCode, task.OriginStorageCode, task.TargetStorageCode, out errorInfo);
            }
            else
            {
                errorInfo = "当前任务号[" + taskID + "]未完成！";
                return false;
            }
        }
        public bool FinishTask(int taskID, string orderType, string orderID, int allotID, string originCellCode, string targetCellCode, string originStorageCode, string targetStorageCode, out string errorInfo)
        {
            errorInfo = string.Empty;
            switch (orderType)
            {
                case "01": return FinishInBillTask(orderID, allotID, out errorInfo);
                case "02": return FinishMoveBillTask(orderID, allotID, out errorInfo);
                case "03": return FinishOutBillTask(orderID, allotID, out errorInfo);
                case "04": return FinishCheckBillTask(orderID, allotID, out errorInfo);
                case "05": return FinishEmptyPalletStackTask(targetCellCode, targetStorageCode, out errorInfo);
                case "06": return FinishEmptyPalletSupplyTask(originCellCode, originStorageCode, out errorInfo);
                case "07": return FinishMoveRemainMoveBackTask(orderID, allotID);
                case "08": return FinishStockOutRemainMoveBackTask(orderID, allotID);
                case "09": return FinishInventoryRemainMoveBackTask(orderID, allotID);
                default: return true;
            }
        }

        private bool FinishInBillTask(string orderID, int allotID, out string errorInfo)
        {
            errorInfo = string.Empty;

            try
            {
                if (InBillAllotRepository.GetQueryable().Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "2").Count() == 1)
                {
                    return true;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    var inAllot = InBillAllotRepository.GetQueryable()
                        .Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "0")
                        .FirstOrDefault();

                    if (inAllot != null && (inAllot.InBillMaster.Status == "4" || inAllot.InBillMaster.Status == "5"))
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
                            }

                            InBillAllotRepository.SaveChanges();

                            #region 反馈给浪潮的xml数据信息

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

                                MdjInspurWmsService.LwmWarehouseWorkServiceService LWWSS = new MdjInspurWmsService.LwmWarehouseWorkServiceService();
                                LWWSS.lwmStroeInProgFeedback(inspurService.BillProgressFeedback(inspur, "in"));
                                if (inAllot.InBillDetail.RealQuantity == inAllot.InBillDetail.AllotQuantity)
                                {
                                    LWWSS.lwmStoreInComplete(inspurService.BillFinished(inspur, "in"));
                                }
                            }
                            catch (Exception ex)
                            {
                                errorInfo = "Into Storage progress feedback to Inspur Failed！" + ex.Message;
                            }

                            #endregion 反馈给浪潮的xml数据信息

                            scope.Complete();
                            return true;
                        }
                        else
                        {
                            errorInfo = "需确认入库的数据别人在操作或完成的数量不对，完成出错！";
                            return false;
                        }
                    }
                    else
                    {
                        errorInfo = "需确认入库的数据查询为空或者主单状态不对，完成出错！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        private bool FinishOutBillTask(string orderID, int allotID, out string errorInfo)
        {
            errorInfo = string.Empty;

            try
            {
                if (OutBillAllotRepository.GetQueryable().Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "2").Count() == 1)
                {
                    return true;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    var outAllot = OutBillAllotRepository.GetQueryable()
                        .Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "0")
                        .FirstOrDefault();

                    if (outAllot != null && (outAllot.OutBillMaster.Status == "4" || outAllot.OutBillMaster.Status == "5"))
                    {
                        decimal quantity = outAllot.AllotQuantity;
                        if (string.IsNullOrEmpty(outAllot.Storage.LockTag)
                            && outAllot.AllotQuantity >= quantity
                            && outAllot.Storage.OutFrozenQuantity >= quantity)
                        {
                            outAllot.Status = "2";
                            outAllot.RealQuantity += quantity;
                            outAllot.Storage.Quantity -= quantity;
                            outAllot.Storage.OutFrozenQuantity -= quantity;
                            if (outAllot.Storage.Quantity == 0)
                            {
                                outAllot.Storage.Rfid = "";
                                outAllot.Storage.ProductCode = null;
                                outAllot.Storage.StorageSequence = 0;
                            }
                            else
                            {
                                if (outAllot.Storage.Cell.FirstInFirstOut) outAllot.Storage.StorageSequence = outAllot.Storage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                                if (!outAllot.Storage.Cell.FirstInFirstOut) outAllot.Storage.StorageSequence = outAllot.Storage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                            }
                            outAllot.Storage.Cell.StorageTime = outAllot.Storage.Cell.Storages.Where(s => s.Quantity > 0).Count() > 0
                                ? outAllot.Storage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime) : DateTime.Now;
                            outAllot.OutBillDetail.RealQuantity += quantity;
                            outAllot.OutBillMaster.Status = "5";
                            outAllot.FinishTime = DateTime.Now;
                            if (outAllot.OutBillMaster.OutBillAllots.All(c => c.Status == "2"))
                            {
                                outAllot.OutBillMaster.Status = "6";
                            }

                            OutBillAllotRepository.SaveChanges();

                            #region 反馈给浪潮的xml数据信息

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

                                MdjInspurWmsService.LwmWarehouseWorkServiceService LWWSS = new MdjInspurWmsService.LwmWarehouseWorkServiceService();
                                LWWSS.lwmStoreOutProgFeedback(inspurService.BillProgressFeedback(inspur, "out"));
                                if (outAllot.OutBillDetail.RealQuantity == outAllot.OutBillDetail.AllotQuantity)
                                {
                                    LWWSS.lwmStoreOutComplete(inspurService.BillFinished(inspur, "out"));
                                }
                            }
                            catch (Exception ex)
                            {
                                errorInfo = "Out Storage progress feedback to Inspur Failed！" + ex.Message;
                            }

                            #endregion 反馈给浪潮的xml数据信息

                            scope.Complete();
                            return true;
                        }
                        else
                        {
                            errorInfo = "需确认出库的数据别人在操作或完成的数量不对，完成出错！";
                            return false;
                        }
                    }
                    else
                    {
                        errorInfo = "需确认出库的数据查询为空或者主单状态不对，完成出错！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        private bool FinishMoveBillTask(string orderID, int allotID, out string errorInfo)
        {
            errorInfo = string.Empty;

            try
            {
                if (MoveBillDetailRepository.GetQueryable().Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "2").Count() == 1)
                {
                    return true;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    var moveDetail = MoveBillDetailRepository.GetQueryable()
                        .Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "0")
                        .FirstOrDefault();

                    if (moveDetail != null && (moveDetail.MoveBillMaster.Status == "2" || moveDetail.MoveBillMaster.Status == "3"))
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
                            if (moveDetail.OutStorage.Quantity == 0)
                            {
                                moveDetail.OutStorage.Rfid = "";
                                moveDetail.OutStorage.StorageSequence = 0;
                                moveDetail.OutStorage.ProductCode = null;
                            }
                            moveDetail.OutStorage.Cell.StorageTime = moveDetail.OutStorage.Cell.Storages.Where(s => s.Quantity > 0).Count() > 0
                                ? moveDetail.OutStorage.Cell.Storages.Where(s => s.Quantity > 0).Min(s => s.StorageTime) : DateTime.Now;

                            if (moveDetail.InStorage.Quantity - moveDetail.RealQuantity == 0)
                            {
                                moveDetail.InStorage.StorageTime = moveDetail.OutStorage.StorageTime;
                            }
                            else
                            {
                                if (DateTime.Compare(moveDetail.OutStorage.StorageTime, moveDetail.InStorage.StorageTime) == -1)
                                    moveDetail.InStorage.StorageTime = moveDetail.OutStorage.StorageTime;
                            }

                            moveDetail.MoveBillMaster.Status = "3";
                            moveDetail.FinishTime = DateTime.Now;
                            var sortwork = SortWorkDispatchRepository.GetQueryable()
                                .Where(s => s.MoveBillMaster.BillNo == moveDetail.MoveBillMaster.BillNo && s.DispatchStatus == "2")
                                .FirstOrDefault();

                            if (sortwork != null)
                            {
                                sortwork.DispatchStatus = "3";
                            }
                            if (moveDetail.MoveBillMaster.MoveBillDetails.All(c => c.Status == "2"))
                            {
                                moveDetail.MoveBillMaster.Status = "4";
                            }
                            MoveBillDetailRepository.SaveChanges();

                            scope.Complete();
                            return true;
                        }
                        else
                        {
                            errorInfo = "需确认移库的数据别人在操作或者完成的数量不对，完成出错！";
                            return false;
                        }
                    }
                    else
                    {
                        errorInfo = "需确认移库的数据查询为空或者主单状态不对，完成出错！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        private bool FinishCheckBillTask(string orderID, int allotID, out string errorInfo)
        {
            errorInfo = string.Empty;

            try
            {
                if (CheckBillDetailRepository.GetQueryable().Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "2").Count() == 1)
                {
                    return true;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    var checkDetail = CheckBillDetailRepository.GetQueryable()
                        .Where(i => i.BillNo == orderID && i.ID == allotID && i.Status == "0")
                        .FirstOrDefault();

                    if (checkDetail != null && (checkDetail.CheckBillMaster.Status == "2" || checkDetail.CheckBillMaster.Status == "3"))
                    {
                        decimal quantity = checkDetail.Quantity;
                        checkDetail.Status = "2";
                        checkDetail.RealQuantity = quantity;
                        checkDetail.Storage.IsLock = "0";
                        checkDetail.CheckBillMaster.Status = "3";
                        checkDetail.FinishTime = DateTime.Now;

                        if (checkDetail.Storage.Cell.FirstInFirstOut) checkDetail.Storage.StorageSequence = checkDetail.Storage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                        if (!checkDetail.Storage.Cell.FirstInFirstOut) checkDetail.Storage.StorageSequence = checkDetail.Storage.Cell.Storages.Min(s => s.StorageSequence) - 1;

                        if (checkDetail.CheckBillMaster.CheckBillDetails.All(c => c.Status == "2"))
                        {
                            checkDetail.CheckBillMaster.Status = "4";
                        }
                        CheckBillDetailRepository.SaveChanges();

                        scope.Complete();
                        return true;
                    }
                    else
                    {
                        errorInfo = "需确认盘点的数据查询为空或者主单状态不对，完成出错！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        private bool FinishEmptyPalletStackTask(string cellCode, string storageCode, out string errorInfo)
        {
            errorInfo = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var cell = CellRepository.GetQueryable().Where(i => i.CellCode == cellCode).FirstOrDefault();
                    if (cell != null)
                    {
                        var storage = cell.Storages.Where(s => s.StorageCode == storageCode || string.IsNullOrEmpty(storageCode)).FirstOrDefault();
                        if (storage.InFrozenQuantity >= 1)
                        {
                            storage.InFrozenQuantity -= 1;
                            storage.Quantity += 1;
                            storage.StorageSequence = 0;
                            storage.StorageTime = DateTime.Now;
                            CellRepository.SaveChanges();

                            scope.Complete();
                            return true;
                        }
                        else
                        {
                            errorInfo = "未找到货位库存信息或者货位入库冻结量 < 1";
                            return false;
                        }
                    }
                    else
                    {
                        errorInfo = "未找到货位库存信息！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        private bool FinishEmptyPalletSupplyTask(string cellCode, string storageCode, out string errorInfo)
        {
            errorInfo = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var cell = CellRepository.GetQueryable().Where(i => i.CellCode == cellCode).FirstOrDefault();
                    if (cell != null)
                    {
                        var storage = cell.Storages.Where(s => (s.StorageCode == storageCode || string.IsNullOrEmpty(storageCode))
                            && s.OutFrozenQuantity > 0).FirstOrDefault();
                        if (storage != null && storage.OutFrozenQuantity > 0)
                        {
                            storage.ProductCode = null;
                            storage.OutFrozenQuantity = 0;
                            storage.Quantity = 0;
                            storage.StorageSequence = 0;
                            CellRepository.SaveChanges();

                            scope.Complete();
                            return true;
                        }
                        else
                        {
                            errorInfo = "库存不存在或补空托盘货位的出库冻结量 <=0";
                            return false;
                        }
                    }
                    else
                    {
                        errorInfo = "未找到货位库存信息！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }
        private bool FinishMoveRemainMoveBackTask(string orderID, int allotID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var moveBillDetail = MoveBillDetailRepository.GetQueryable()
                        .Where(i => i.BillNo == orderID && i.ID == allotID)
                        .FirstOrDefault();

                    if (moveBillDetail != null)
                    {
                        if (moveBillDetail.OutStorage.Cell.FirstInFirstOut) moveBillDetail.OutStorage.StorageSequence = moveBillDetail.OutStorage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                        if (!moveBillDetail.OutStorage.Cell.FirstInFirstOut) moveBillDetail.OutStorage.StorageSequence = moveBillDetail.OutStorage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                        OutBillAllotRepository.SaveChanges();

                        scope.Complete();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool FinishStockOutRemainMoveBackTask(string orderID, int allotID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var outBillDetail = OutBillAllotRepository.GetQueryable()
                        .Where(i => i.BillNo == orderID && i.ID == allotID)
                        .FirstOrDefault();

                    if (outBillDetail != null)
                    {
                        if (outBillDetail.Storage.Cell.FirstInFirstOut) outBillDetail.Storage.StorageSequence = outBillDetail.Storage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                        if (!outBillDetail.Storage.Cell.FirstInFirstOut) outBillDetail.Storage.StorageSequence = outBillDetail.Storage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                        OutBillAllotRepository.SaveChanges();

                        scope.Complete();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool FinishInventoryRemainMoveBackTask(string orderID, int allotID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var checkBillDetail = CheckBillDetailRepository.GetQueryable()
                        .Where(i => i.BillNo == orderID && i.ID == allotID)
                        .FirstOrDefault();

                    if (checkBillDetail != null)
                    {
                        if (checkBillDetail.Storage.Cell.FirstInFirstOut) checkBillDetail.Storage.StorageSequence = checkBillDetail.Storage.Cell.Storages.Max(s => s.StorageSequence) + 1;
                        if (!checkBillDetail.Storage.Cell.FirstInFirstOut) checkBillDetail.Storage.StorageSequence = checkBillDetail.Storage.Cell.Storages.Min(s => s.StorageSequence) - 1;
                        CheckBillDetailRepository.SaveChanges();

                        scope.Complete();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int FinishStockOutTask(int taskID, int stockOutQuantity, out string errorInfo)
        {
            int newTaskID = 0; errorInfo = string.Empty;

            var task = TaskRepository.GetQueryable()
                .Where(i => i.ID == taskID).FirstOrDefault();

            if (task != null && FinishOutBillTask(task.OrderID, task.AllotID, out errorInfo))
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        if (task.Quantity > task.TaskQuantity)
                        {
                            newTaskID = CreateNewTaskForMoveBackRemainAndReturnTaskID(taskID, out errorInfo);
                        }
                        else
                        {
                            return -1;
                        }

                        task.CurrentPositionID = task.TargetPositionID;
                        task.State = "04";
                        TaskRepository.SaveChanges();

                        if (newTaskID > 0)
                        {
                            scope.Complete();
                            return newTaskID;
                        }
                        else
                        {
                            return newTaskID;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorInfo = ex.Message;
                    return newTaskID;
                }
            }
            else
            {
                return newTaskID;
            }
        }
        public int FinishInventoryTask(int taskID, int realQuantity, out string errorInfo)
        {
            int newTaskID = 0; errorInfo = string.Empty;

            var task = TaskRepository.GetQueryable()
                .Where(i => i.ID == taskID).FirstOrDefault();

            if (task != null && FinishCheckBillTask(task.OrderID, task.AllotID, out errorInfo))
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        newTaskID = CreateNewTaskForMoveBackRemainAndReturnTaskID(taskID, out errorInfo);

                        task.CurrentPositionID = task.TargetPositionID;
                        task.State = "04";
                        TaskRepository.SaveChanges();

                        if (newTaskID > 0)
                        {
                            scope.Complete();
                            return newTaskID;
                        }
                        else
                        {
                            return newTaskID;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorInfo = ex.Message;
                    return newTaskID;
                }
            }
            else
            {
                return newTaskID;
            }
        }

        private static object locker = new object();
        public bool CreateAutoMoveBill(out string errorInfo)
        {
            errorInfo = string.Empty;

            lock (locker)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        var positions = PositionRepository.GetQueryable()
                            .Where(i => (i.PositionType == "02" || i.PositionType == "03" || i.PositionType == "04")
                                && !i.HasGoods);

                        var cellPositions = CellPositionRepository.GetQueryable()
                            .Where(i => positions.Contains(i.StockInPosition));

                        var cells = CellRepository.GetQueryable()
                            .Where(i => cellPositions.Any(p => p.CellCode == i.CellCode)
                                && !string.IsNullOrEmpty(i.DefaultProductCode))
                            .ToArray();

                        if (cells.Count() > 0)
                        {
                            var systemParamWare = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.ParameterName == "WarehouseCode");
                            var systemParamMove = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.ParameterName == "MoveBillTypeCode");
                            var systemParamPerson = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.ParameterName == "OperatePersonID");

                            string warehouseCode = systemParamWare.ParameterValue;
                            string moveBillTypeCode = systemParamMove.ParameterValue;
                            string operatePersonID = systemParamPerson.ParameterValue;

                            MoveBillMaster moveBillMaster = MoveBillCreater.CreateMoveBillMaster(warehouseCode, moveBillTypeCode, operatePersonID);
                            moveBillMaster.Origin = "2";
                            moveBillMaster.Description = "系统自动生成补固定拆盘位移库单！";
                            moveBillMaster.Status = "2";
                            moveBillMaster.VerifyPersonID = Guid.Parse(operatePersonID);
                            moveBillMaster.VerifyDate = DateTime.Now;

                            foreach (var cell in cells)
                            {
                                var task = TaskRepository.GetQueryable()
                                    .Where(t => t.State != "04" && t.TargetCellCode == cell.CellCode)
                                    .FirstOrDefault();
                                if (task == null)
                                {
                                    AlltoMoveBill(moveBillMaster, cell.Product, cell);
                                }
                            }

                            if (moveBillMaster.MoveBillDetails.Count > 0)
                            {
                                CellRepository.SaveChanges();
                                if (CreateAutoMoveBillTask(moveBillMaster.BillNo, 10, out errorInfo))
                                {
                                    scope.Complete();
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        private void AlltoMoveBill(MoveBillMaster moveBillMaster, Product product, Cell cell)
        {
            //选择当前订单操作目标仓库；
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable()
                .Where(s => s.Cell.WarehouseCode == moveBillMaster.WarehouseCode
                    && s.Quantity > 0
                    && s.OutFrozenQuantity == 0
                    && s.Cell.Area.AllotOutOrder > 0
                    && s.Cell.IsActive == "1"
                    && s.IsLock == "0");


            //分配整托盘烟；固定拆盘位； 
            var storages = storageQuery.Where(s => product.PointAreaCodes.Contains(s.Cell.AreaCode)
                                    && s.ProductCode == product.ProductCode)
                              .OrderBy(s => new { s.Cell.StorageTime, s.Cell.Area.AllotOutOrder, s.CellCode, s.StorageSequence, s.Quantity });
            AllotPallet(moveBillMaster, storages, cell);
        }
        private void AllotPallet(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> storages, Cell cell)
        {
            foreach (var s in storages.ToArray())
            {
                int storageSequence = s.Cell.Storages.Where(t => t.Quantity > 0 && t.OutFrozenQuantity == 0).Min(t => t.StorageSequence);
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

        public void GetOutTask(string positionType, string orderType, RestReturn result)
        {
            try
            {
                RestTask[] RestTask = new RestTask[] { };

                var taskQuery = TaskRepository.GetQueryable().Where(a => a.OrderType == orderType && a.State != "04");
                var positionQuery = PositionRepository.GetQueryable().Where(a => a.PositionType == positionType);

                var outBillAllotQuery = OutBillAllotRepository.GetQueryable();
                var moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
                var checkBillDetailQuery = CheckBillDetailRepository.GetQueryable();
                var cellPosition = CellPositionRepository.GetQueryable();
                var cell = CellRepository.GetQueryable();

                if (orderType == "03")
                {
                    #region 出库
                    RestTask = taskQuery
                                .Join(outBillAllotQuery, a => a.AllotID, o => o.ID, (a, o) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    o.Product,
                                    o.AllotQuantity,
                                    OriginCellCode = o.Cell.CellCode,
                                    OriginCellName = o.Cell.CellName
                                })
                                .Join(cellPosition, a => a.CurrentPositionID, c => c.StockInPositionID, (a, c) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.Product,
                                    a.AllotQuantity,
                                    c.CellCode
                                })
                                .Join(cell, a => a.CellCode, c => c.CellCode, (a, c) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.Product,
                                    a.AllotQuantity,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    TargetCellCode = a.CellCode,
                                    TargetCellName = c.CellName
                                })
                                .Join(positionQuery, a => a.CurrentPositionID, p => p.ID, (a, p) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.AllotQuantity,
                                    a.Product,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    a.TargetCellCode,
                                    a.TargetCellName
                                })
                                .Select(i => new RestTask()
                                {
                                    TaskID = i.ID,
                                    OriginCellCode = i.OriginCellCode,
                                    OriginCellName = i.OriginCellName,
                                    TargetCellCode = i.TargetCellCode,
                                    TargetCellName = i.TargetCellName,
                                    ProductCode = i.ProductCode,
                                    ProductName = i.ProductName,
                                    OrderID = i.OrderID,
                                    OrderType = i.OrderType == "02" ? "移库" : i.OrderType == "03" ? "出库" : "盘点",
                                    Quantity = i.Quantity,
                                    TaskQuantity = i.AllotQuantity / i.Product.Unit.Count,
                                    PieceQuantity = Math.Floor(i.AllotQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    Status = i.State == "01" ? "等待中" : i.State == "02" ? " 执行中" : i.State == "03" ? "拣选中" : "已完成"
                                })
                                .OrderBy(t => t.TargetCellCode)
                                .ToArray();
                    #endregion}
                }
                else if (orderType == "02")
                {
                    #region 移库
                    RestTask = taskQuery
                                .Join(moveBillDetailQuery, a => a.AllotID, m => m.ID, (a, m) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    m.RealQuantity,
                                    m.Product,
                                    OriginCellCode = m.OutCell.CellCode,
                                    OriginCellName = m.OutCell.CellName,
                                })
                                .Join(cellPosition, a => a.CurrentPositionID, c => c.StockInPositionID, (a, c) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.Product,
                                    a.RealQuantity,
                                    c.CellCode
                                })
                                .Join(cell, a => a.CellCode, c => c.CellCode, (a, c) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.Product,
                                    a.RealQuantity,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    TargetCellCode = a.CellCode,
                                    TargetCellName = c.CellName
                                })
                                .Join(positionQuery, a => a.CurrentPositionID, p => p.ID, (a, p) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.RealQuantity,
                                    a.Product,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    a.TargetCellCode,
                                    a.TargetCellName
                                })
                                .Select(i => new RestTask()
                                {
                                    TaskID = i.ID,
                                    OriginCellCode = i.OriginCellCode,
                                    OriginCellName = i.OriginCellName,
                                    TargetCellCode = i.TargetCellCode,
                                    TargetCellName = i.TargetCellName,
                                    ProductCode = i.ProductCode,
                                    ProductName = i.ProductName,
                                    OrderID = i.OrderID,
                                    OrderType = i.OrderType == "02" ? "移库" : i.OrderType == "03" ? "出库" : "盘点",
                                    Quantity = i.Quantity,
                                    TaskQuantity = i.RealQuantity / i.Product.Unit.Count,
                                    PieceQuantity = Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    Status = i.State == "01" ? "等待中" : i.State == "02" ? " 执行中" : i.State == "03" ? "拣选中" : "已完成"
                                })
                                .OrderBy(t => t.TargetCellCode)
                                .ToArray();
                    #endregion
                }
                else if (orderType == "04")
                {
                    #region 盘点
                    RestTask = taskQuery
                                .Join(checkBillDetailQuery, a => a.AllotID, o => o.ID, (a, o) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    o.Product,
                                    checkQuantity = o.Quantity,
                                    OriginCellCode = o.Cell.CellCode,
                                    OriginCellName = o.Cell.CellName,
                                })
                                .Join(cellPosition, a => a.CurrentPositionID, c => c.StockInPositionID, (a, c) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.Product,
                                    a.checkQuantity,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    c.CellCode
                                })
                                .Join(cell, a => a.CellCode, c => c.CellCode, (a, c) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.CurrentPositionID,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.Product,
                                    a.checkQuantity,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    TargetCellCode = a.CellCode,
                                    TargetCellName = c.CellName
                                })
                                .Join(positionQuery, a => a.CurrentPositionID, p => p.ID, (a, p) => new
                                {
                                    a.ID,
                                    a.TaskType,
                                    a.OrderID,
                                    a.OrderType,
                                    a.ProductCode,
                                    a.ProductName,
                                    a.Quantity,
                                    a.TaskQuantity,
                                    a.State,
                                    a.checkQuantity,
                                    a.Product,
                                    a.OriginCellCode,
                                    a.OriginCellName,
                                    a.TargetCellCode,
                                    a.TargetCellName
                                })
                                .Select(i => new RestTask()
                                {
                                    TaskID = i.ID,
                                    OriginCellCode = i.OriginCellCode,
                                    OriginCellName = i.OriginCellName,
                                    TargetCellCode = i.TargetCellCode,
                                    TargetCellName = i.TargetCellName,
                                    ProductCode = i.ProductCode,
                                    ProductName = i.ProductName,
                                    OrderID = i.OrderID,
                                    OrderType = i.OrderType == "02" ? "移库" : i.OrderType == "03" ? "出库" : "盘点",
                                    Quantity = i.Quantity,
                                    TaskQuantity = i.checkQuantity / i.Product.Unit.Count,
                                    PieceQuantity = Math.Floor(i.checkQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = Math.Floor((i.checkQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    Status = i.State == "01" ? "等待中" : i.State == "02" ? " 执行中" : i.State == "03" ? "拣选中" : "已完成"
                                })
                                .OrderBy(t => t.TargetCellCode)
                                .ToArray();
                    #endregion
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "查询的订单类型不存在，请检查！";
                }
                result.IsSuccess = true;
                result.RestTasks = RestTask;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = e.Message;
            }
        }
        public void FinishTask(string taskID, RestReturn result)
        {
            string errorInfo = string.Empty;

            try
            {
                int tid = Convert.ToInt32(taskID);
                var task = TaskRepository.GetQueryable().FirstOrDefault(a => a.ID == tid);
                var position = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == task.CurrentPositionID);

                if (FinishTask(task.ID, task.OrderType, task.OrderID, task.AllotID, task.OriginCellCode, task.TargetCellCode, task.OriginStorageCode, task.TargetStorageCode, out errorInfo))
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        if (task.Quantity == task.TaskQuantity && task.OrderType != "04")
                        {
                            if (task.TaskType != "03")
                            {
                                if (CreateNewTaskForEmptyPalletStack(0, position.PositionName, out errorInfo))
                                {
                                    task.CurrentPositionID = task.TargetPositionID;
                                    task.State = "04";
                                    TaskRepository.SaveChanges();

                                    scope.Complete();
                                    result.IsSuccess = true;
                                }
                                else
                                {
                                    result.IsSuccess = false;
                                    result.Message = string.Format("{0} 生成空托盘叠垛任务失败！", position.PositionName);
                                }
                            }
                            else
                            {
                                task.CurrentPositionID = task.TargetPositionID;
                                task.State = "04";
                                TaskRepository.SaveChanges();

                                scope.Complete();
                                result.IsSuccess = true;
                            }
                        }
                        else
                        {
                            if (task.CurrentPositionID != task.OriginPositionID)
                            {
                                if (CreateNewTaskForMoveBackRemain(task.ID, out errorInfo))
                                {
                                    task.CurrentPositionID = task.TargetPositionID;
                                    task.State = "04";
                                    TaskRepository.SaveChanges();

                                    scope.Complete();
                                    result.IsSuccess = true;
                                }
                                else
                                {
                                    result.IsSuccess = false;
                                    result.Message = string.Format("{0} 生成余烟回库任务失败！", position.PositionName);
                                }
                            }
                            else
                            {
                                task.CurrentPositionID = task.TargetPositionID;
                                task.State = "04";
                                TaskRepository.SaveChanges();

                                scope.Complete();
                                result.IsSuccess = true;
                            }
                        }
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = string.Format("{0} 完成任务失败！", task.ID);
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + errorInfo;
            }
        }

        public bool CreateAutoMoveCell(out string errorInfo)
        {
            errorInfo = string.Empty;

            lock (locker)
            {
                string[] areaCodes = new string[] { "001-01", "001-02" };
                IQueryable<Storage> storageQuery = StorageRepository.GetQueryable()
                                        .Where(s => ((s.Quantity > 0 && s.OutFrozenQuantity == 0) || (s.Quantity == 0 && s.InFrozenQuantity > 0))
                                        && s.Cell.IsActive == "1"
                                        && areaCodes.Any(a => a == s.Cell.AreaCode)
                                        && s.IsLock == "0"
                                        && s.IsActive == "1");
                try
                {
                    //需要移动的品牌
                    var products = storageQuery.GroupBy(s => new { s.Product, s.Cell })
                                        .Select(s => new
                                        {
                                            maxPallet = s.Key.Cell.MaxPalletQuantity,
                                            s.Key.Product,
                                            HasPalletCount = s.Count(),
                                            CanMovePalletConut = s.Key.Cell.MaxPalletQuantity - s.Count()
                                        }).Where(a => a.HasPalletCount < a.maxPallet && a.CanMovePalletConut >= 1)
                                        .GroupBy(s => new { s.Product })
                                        .Select(s => new
                                        {
                                            prpduct = s.Key.Product,
                                            cellCount = s.Count()
                                        }).Where(s => s.cellCount >= 2).OrderByDescending(s => s.cellCount);
                    //遍历品牌
                    foreach (var product in products.ToArray())
                    {
                        AutoMoveCell(storageQuery, product.prpduct.ProductCode, out errorInfo);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    errorInfo += e.Message;
                    return false;
                }
            }
        }
        private void AutoMoveCell(IQueryable<Storage> storageQuery, string productCode, out string errorInfo)
        {
            errorInfo = string.Empty;
            var storages = storageQuery.Where(s => s.ProductCode == productCode).ToArray();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var systemParamWare = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.ParameterName == "WarehouseCode");
                    var systemParamMove = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.ParameterName == "MoveBillTypeCode");
                    var systemParamPerson = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.ParameterName == "OperatePersonID");

                    string warehouseCode = systemParamWare.ParameterValue;
                    string moveBillTypeCode = systemParamMove.ParameterValue;
                    string operatePersonID = systemParamPerson.ParameterValue;

                    MoveBillMaster moveBillMaster = MoveBillCreater.CreateMoveBillMaster(warehouseCode, moveBillTypeCode, operatePersonID);
                    moveBillMaster.Origin = "2";
                    moveBillMaster.Description = "系统自动生成同品牌合并货位移库单！";
                    moveBillMaster.Status = "2";
                    moveBillMaster.VerifyPersonID = Guid.Parse(operatePersonID);
                    moveBillMaster.VerifyDate = DateTime.Now;

                    //需要合并的货位
                    var cells = storages.Where(s => (s.Quantity > 0 && s.OutFrozenQuantity == 0) || (s.Quantity == 0 && s.InFrozenQuantity > 0))
                                       .GroupBy(s => new { s.Cell })
                                       .Select(s => new
                                       {
                                           s.Key.Cell,
                                           HasPalletCount = s.Count(),
                                           CanMovePalletConut = s.Key.Cell.MaxPalletQuantity - s.Count()
                                       })
                                       .Where(a => a.HasPalletCount < a.Cell.MaxPalletQuantity && a.CanMovePalletConut >= 1);

                    //遍历货位
                    foreach (var cell in cells.OrderByDescending(s => s.HasPalletCount).ThenByDescending(s => s.Cell.CellCode).ToArray())
                    {
                        if (cell.Cell.Storages.Any(s => s.OutFrozenQuantity > 0))
                        {
                            continue;
                        }
                        for (int i = 0; i < cell.CanMovePalletConut; i++)
                        {
                            //查询需要移出的数据
                            var sourceCell = cells.OrderBy(s => s.HasPalletCount).ThenBy(s => s.Cell.CellCode).FirstOrDefault().Cell.CellCode;
                            if (cell.Cell.MaxPalletQuantity - cell.Cell.Storages.Where(s => s.Quantity > 0 && s.OutFrozenQuantity == 0).Count() > 0 && sourceCell != cell.Cell.CellCode)
                            {
                                var sourceStorage = storages.Where(s => s.Cell.CellCode == sourceCell)
                                                           .Where(s => s.Quantity > 0 && s.OutFrozenQuantity == 0)
                                                           .AsQueryable()
                                                           .OrderBy(s => s.StorageSequence);
                                AllotPallet(moveBillMaster, sourceStorage, cell.Cell);
                            }
                        }
                    }

                    if (moveBillMaster.MoveBillDetails.Count > 0)
                    {
                        CellRepository.SaveChanges();
                        if (CreateAutoMoveBillTask(moveBillMaster.BillNo, 1, out errorInfo))
                        {
                            scope.Complete();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorInfo += e.Message;
            }
        }

    }
}
