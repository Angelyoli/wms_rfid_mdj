using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;
using THOK.WCS.Dal.Interfaces;
using THOK.WCS.DbModel;
using THOK.Authority.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class TaskService : ServiceBase<Task>, ITaskService
    {
        [Dependency]
        public ITaskRepository TaskRepository { get; set; }
        [Dependency]
        public ICellPositionRepository CellPositionRepository { get; set; }
        [Dependency]
        public IPositionRepository PositionRepository { get; set; }
        [Dependency]
        public IPathRepository PathRepository { get; set; }
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }
        [Dependency]
        public IRegionRepository RegionRepository { get; set; }
        [Dependency]
        public ISortWorkDispatchRepository SortWorkDispatchRepository { get; set; }
        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region 初始化值
        public void InitValue(Task task)
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
        #endregion

        #region 查询
        /// <summary>查询</summary>
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
        #endregion

        #region 添加
        /// <summary>添加</summary>
        public bool Add(Task task, out string strResult)
        {
            bool bResult = false;
            strResult = string.Empty;

            InitValue(task);

            var cellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode == task.OriginStorageCode || a.CellCode == task.TargetStorageCode);
            if (cellPosition != null)
            {
                //起始位置为货位位置的出库位置
                var originPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == cellPosition.StockOutPositionID);
                if (originPosition != null)
                {
                    //目标位置为货位位置的入库位置
                    var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == cellPosition.StockInPositionID);
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
                                t.OriginPositionID = cellPosition.StockOutPositionID;
                                t.TargetPositionID = cellPosition.StockInPositionID;
                                t.CurrentPositionID = cellPosition.StockOutPositionID;
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
                strResult = "未找到【货位信息】起始货位或目标货位！";
            }
            return bResult;
        }
        #endregion

        #region 保存
        /// <summary>保存</summary>
        public bool Save(Task task, out string strResult)
        {
            strResult = string.Empty;
            bool bResult = false;

            InitValue(task);

            var t = TaskRepository.GetQueryable().FirstOrDefault(a => a.ID == task.ID);
            if (t != null)
            {
                var cellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(a => a.CellCode == task.OriginStorageCode || a.CellCode == task.TargetStorageCode);
                if (cellPosition != null)
                {
                    //起始位置为货位位置的出库位置
                    var originPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == cellPosition.StockOutPositionID);
                    if (originPosition != null)
                    {
                        //目标位置为货位位置的入库位置
                        var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(a => a.ID == cellPosition.StockInPositionID);
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
                                    t.OriginPositionID = cellPosition.StockOutPositionID;
                                    t.TargetPositionID = cellPosition.StockInPositionID;
                                    t.CurrentPositionID = cellPosition.StockOutPositionID;
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
                    strResult = "未找到【货位信息】起始货位或目标货位！";
                }
            }
            else
            {
                strResult = "未找到当前需要修改的数据！";
            }
            return bResult;
        }
        #endregion

        #region 删除
        /// <summary>删除</summary>
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
        #endregion

        #region 入库单据作业
        /// <summary>
        /// 入库单据作业
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="errInfo">错误消息</param>
        /// <returns></returns>
        public bool InBillTask(string billNo, out string errorInfo)
        {
            bool result = true;
            errorInfo = string.Empty;
            try
            {
                //查询“起始位置参数”
                var originPositionSystem = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName == "InBillPositionId");
                if (originPositionSystem != null)
                {
                    //入库分配信息
                    var inBillAllot = InBillAllotRepository.GetQueryable().Where(i => i.BillNo == billNo);
                    if (inBillAllot.Any())
                    {
                        foreach (var inItem in inBillAllot.ToArray())
                        {
                            //根据“入库货位编码”查找“目标货位位置”
                            var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == inItem.CellCode);
                            if (targetCellPosition != null)
                            {
                                //根据“起始位置参数”查找“起始位置信息”
                                int paramterValue = Convert.ToInt32(originPositionSystem.ParameterValue);
                                var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == paramterValue);
                                if (originPosition != null)
                                {
                                    //根据“货位位置中的入库位置ID”查找“目标位置信息”
                                    var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                                    if (targetPosition != null)
                                    {
                                        //根据“入库（目标和起始）位置信息的区域ID”查找“路径信息”
                                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                                        if (path != null)
                                        {
                                            var inTask = new Task();
                                            inTask.TaskType = "01";
                                            inTask.TaskLevel = 0;
                                            inTask.PathID = path.ID;
                                            inTask.ProductCode = inItem.Product.ProductCode;
                                            inTask.ProductName = inItem.Product.ProductName;
                                            inTask.OriginStorageCode = inItem.CellCode;
                                            inTask.TargetStorageCode = inItem.CellCode;
                                            inTask.OriginPositionID = Convert.ToInt32(originPositionSystem.ParameterValue);
                                            inTask.TargetPositionID = targetPosition.ID;
                                            inTask.CurrentPositionID = Convert.ToInt32(originPositionSystem.ParameterValue);
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
                                        else
                                        {
                                            errorInfo = "未找到【路径信息】起始位置：" + originPosition.Region.RegionName + "，目标位置：" + targetPosition.Region.RegionName;
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        errorInfo = "未找到【位置信息】目标货位位置：" + targetCellPosition.StockInPosition.PositionName;
                                        result = false;
                                    }
                                }
                                else
                                {
                                    errorInfo = "未找到【位置信息】的起始位置！";
                                    result = false;
                                }
                            }
                            else
                            {
                                errorInfo = "未找到【货位位置】入库货位：" + inItem.Cell.CellName;
                                result = false;
                            }
                        }
                        using (var scope = new System.Transactions.TransactionScope())
                        {
                            try
                            {
                                /* 作业生成后 修改入库订单状态=5 
                                 * Status == 1:已录入 2:已审核 3:已分配 4:已确认 5:执行中 6:已结单 */
                                var inBillMaster = InBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "4");
                                if (inBillMaster != null)
                                {
                                    inBillMaster.Status = "5";
                                    InBillMasterRepository.SaveChanges();

                                    TaskRepository.SaveChanges();
                                }
                                else
                                {
                                    errorInfo = "未获取订单号";
                                    result = false;
                                }
                                if (result == true)
                                    scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                errorInfo = "事务异常：" + ex.Message;
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        errorInfo = "当前选择订单没有分配数据，请重新选择！";
                    }
                }
                else
                {
                    errorInfo = "请检查【系统参数】，未找到起始位置InBillPosition！";
                    result = false;
                }
            }
            catch (Exception e)
            {
                errorInfo = e.Message;
                result = false;
            }
            return result;
        }
        #endregion

        #region 出库单据作业
        /// <summary>
        /// 出库单据作业
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="errInfo">错误消息</param>
        /// <returns></returns>
        public bool OutBillTask(string billNo, out string errorInfo)
        {
            bool result = true;
            errorInfo = string.Empty;
            try
            {
                //查询“起始位置的参数”
                var originPositionSystem = SystemParameterRepository.GetQueryable().FirstOrDefault(s => s.ParameterName == "OutBillPositionId");
                if (originPositionSystem != null)
                {
                    int paramterValue = Convert.ToInt32(originPositionSystem.ParameterValue);
                    //出库分配信息
                    var outBillAllot = OutBillAllotRepository.GetQueryable().Where(i => i.BillNo == billNo);
                    if (outBillAllot.Any())
                    {
                        foreach (var outItem in outBillAllot.ToArray())
                        {
                            //根据“出库货位编码”查找“货位位置”
                            var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == outItem.CellCode);
                            if (originCellPosition != null)
                            {
                                //根据“起始位置的参数”查找“起始的位置信息”
                                var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                                if (originPosition != null)
                                {
                                    //根据“货位位置中的出库位置ID”查找“目标的位置信息”
                                    var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == paramterValue);
                                    if (targetPosition != null)
                                    {
                                        //根据“出库（目标和起始）位置信息的区域ID”查找“路径信息”
                                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                                        if (path != null)
                                        {
                                            var outTask = new Task();
                                            outTask.TaskType = "01";
                                            outTask.TaskLevel = 0;
                                            outTask.PathID = path.ID;
                                            outTask.ProductCode = outItem.Product.ProductCode;
                                            outTask.ProductName = outItem.Product.ProductName;
                                            outTask.OriginStorageCode = outItem.CellCode;
                                            outTask.TargetStorageCode = outItem.CellCode;
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
                                            outTask.OrderType = "02";
                                            outTask.AllotID = outItem.ID;
                                            outTask.DownloadState = "0";
                                            outTask.StorageSequence = outItem.Storage.StorageSequence;
                                            TaskRepository.Add(outTask);
                                        }
                                        else
                                        {
                                            errorInfo = "未找到【路径信息】起始位置：" + originPosition.Region.RegionName + "，目标位置：" + targetPosition.Region.RegionName;
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        errorInfo = "未找到【位置信息】目标货位位置：" + originCellPosition.StockOutPosition.PositionName;
                                        result = false;
                                    }
                                }
                                else
                                {
                                    errorInfo = "未找到【位置信息】的起始位置！";
                                    result = false;
                                }
                            }
                            else
                            {
                                errorInfo = "未找到【货位位置】出库货位：" + outItem.Cell.CellName;
                                result = false;
                            }
                        }
                        using (var scope = new System.Transactions.TransactionScope())
                        {
                            try
                            {
                                /* 作业生成后 修改入库订单状态=5 
                                 * Status == 1:已录入 2:已审核 3:已分配 4:已确认 5:执行中 6:已结单 */
                                var outBillMaster = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "4");
                                if (outBillMaster != null)
                                {
                                    outBillMaster.Status = "5";
                                    OutBillMasterRepository.SaveChanges();

                                    TaskRepository.SaveChanges();
                                }
                                else
                                {
                                    errorInfo = "未获取订单号";
                                    result = false;
                                }
                                if (result == true)
                                    scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                errorInfo = "事务异常：" + ex.Message;
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        errorInfo = "当前选择订单没有分配数据，请重新选择！";
                    }
                }
                else
                {
                    errorInfo = "请检查【系统参数】，未找到起始位置OutBillPosition！";
                    result = false;
                }
            }
            catch (Exception e)
            {
                errorInfo = e.Message;
                result = false;
            }
            return result;
        }
        #endregion

        #region 移库单据作业
        /// <summary>
        /// 移库单据作业
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="errInfo">错误消息</param>
        /// <returns></returns>
        public bool MoveBillTask(string billNo, out string errorInfo)
        {
            bool result = true;
            errorInfo = string.Empty;
            try
            {
                var moveQuery = MoveBillDetailRepository.GetQueryable().Where(i => i.BillNo == billNo);
                if (moveQuery.Any())
                {
                    foreach (var moveItem in moveQuery.ToArray())
                    {
                        //根据“移出的货位信息的编码”查找“起始的位置信息”
                        var originCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveItem.OutCellCode);
                        if (originCellPosition != null)
                        {
                            //根据“移入的货位信息”查找“目标货位位置”
                            var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveItem.InCellCode);
                            if (targetCellPosition != null)
                            {
                                //根据“移出的位置信息ID”去找“起始位置的位置信息”
                                var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == originCellPosition.StockOutPositionID);
                                if (originPosition != null)
                                {
                                    //根据“移入位置ID”去找“目标位置的区域ID”信息
                                    var targetPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockInPositionID);
                                    if (targetPosition != null)
                                    {
                                        //根据“入库的目标位置信息的区域ID”和"起始的位置信息的区域ID"去找"路径信息"
                                        var path = PathRepository.GetQueryable().FirstOrDefault(p => p.OriginRegionID == originPosition.RegionID && p.TargetRegionID == targetPosition.RegionID);
                                        if (path != null)
                                        {
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
                                            moveTask.OrderType = "03";
                                            moveTask.AllotID = moveItem.ID;
                                            moveTask.DownloadState = "0";
                                            moveTask.StorageSequence = moveItem.OutStorage.StorageSequence;
                                            TaskRepository.Add(moveTask);
                                        }
                                        else
                                        {
                                            errorInfo = "未找到【路径信息】起始位置：" + originPosition.Region.RegionName + "，目标位置：" + targetPosition.Region.RegionName;
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        errorInfo = "未找到目标【位置信息】移入位置：" + targetCellPosition.StockInPosition.PositionName;
                                        result = false;
                                    }
                                }
                                else
                                {
                                    errorInfo = "未找到起始【位置信息】移出位置：" + originCellPosition.StockOutPosition.PositionName;
                                    result = false;
                                }
                            }
                            else
                            {
                                errorInfo = "未找到目标【货位位置】移入货位：" + moveItem.InCell.CellName;
                                result = false;
                            }
                        }
                        else
                        {
                            errorInfo = "未找到起始【货位位置】移出货位：" + moveItem.OutCell.CellName;
                            result = false;
                        }
                    }
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        try
                        {
                            /* 作业生成后 修改入库订单状态=5 
                             * Status == 1:已录入 2:已审核  3:执行中 4:已结单 */
                            var moveBillMaster = MoveBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "2");
                            if (moveBillMaster != null)
                            {
                                moveBillMaster.Status = "3";
                                MoveBillMasterRepository.SaveChanges();

                                TaskRepository.SaveChanges();
                            }
                            else
                            {
                                errorInfo = "未获取订单号";
                                result = false;
                            }
                            if (result == true)
                                scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            errorInfo = "事务异常：" + ex.Message;
                            result = false;
                        }
                    }
                }
                else
                {
                    errorInfo = "当前选择订单没有移库细表数据，请重新选择！";
                }
            }
            catch (Exception e)
            {
                result = false;
                errorInfo = e.Message;
            }
            return result;
        }
        #endregion

    }
}
