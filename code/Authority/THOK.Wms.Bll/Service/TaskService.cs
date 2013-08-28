using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;
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

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region 作业任务管理
        /// <summary>查询</summary>
        public object GetDetails(int page, int rows)
        {
            var task = TaskRepository.GetQueryable().OrderBy(t => t.ID).Select(t => t);
            int total = task.Count();
            task = task.Skip((page - 1) * rows).Take(rows);
            var temp = task.ToArray().Select(t => new
            {
                t.ID
                ,
                t.TaskType
                ,
                t.TaskLevel
                ,
                PathName = t.Path.PathName
                ,
                t.ProductCode
                ,
                t.ProductName
                ,
                t.OriginStorageCode
                ,
                t.TargetStorageCode
                ,
                OriginPositionName = t.OriginPosition.PositionName
                ,
                TargetPositionName = t.TargetPosition.PositionName
                ,
                CurrentPositionName = t.CurrentPosition.PositionName
                ,
                CurrentPositionState = t.CurrentPositionState == "01" ? "未达到" : "已到达"
                ,
                State = t.State == "01" ? "等待中" : t.State == "02" ? "执行中" : t.State == "03" ? "拣选中" : t.State == "04" ? "已完成" : "异常"
                ,
                TagState = t.TagState == "01" ? "未点亮" : "已点亮"
                ,
                t.Quantity
                ,
                t.TaskQuantity
                ,
                t.OperateQuantity
                ,
                t.OrderID
                ,
                OrderType = t.OrderType == "01" ? "入库单" : t.OrderType == "02" ? "移库单" : t.OrderType == "03" ? "出库单" : t.OrderType == "04" ? "盘点单" : "异常"
                ,
                t.AllotID
                ,
                DownloadState = t.DownloadState == "0" ? "未下载" : "已下载"
            });
            return new { total, rows = temp.ToArray() };
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
                            var targetCellPosition = CellPositionRepository.GetQueryable().FirstOrDefault(c => c.CellCode == outItem.CellCode);
                            if (targetCellPosition != null)
                            {
                                //根据“起始位置的参数”查找“起始的位置信息”
                                var originPosition = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == targetCellPosition.StockOutPositionID);
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
                                            outTask.OriginPositionID = Convert.ToInt32(originPositionSystem.ParameterValue);
                                            outTask.TargetPositionID = targetPosition.ID;
                                            outTask.CurrentPositionID = Convert.ToInt32(originPositionSystem.ParameterValue);
                                            outTask.CurrentPositionState = "01";
                                            outTask.State = "01";
                                            outTask.TagState = "01";
                                            outTask.Quantity = Convert.ToInt32(outItem.AllotQuantity / outItem.Product.Unit.Count);
                                            outTask.TaskQuantity = Convert.ToInt32(outItem.AllotQuantity / outItem.Product.Unit.Count);
                                            outTask.OperateQuantity = 0;
                                            outTask.OrderID = outItem.BillNo;
                                            outTask.OrderType = "02";
                                            outTask.AllotID = outItem.ID;
                                            outTask.DownloadState = "0";
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
                                        errorInfo = "未找到【位置信息】目标货位位置：" + targetCellPosition.StockOutPosition.PositionName;
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
                                            moveTask.CurrentPositionState = "01";
                                            moveTask.State = "01";
                                            moveTask.TagState = "01";
                                            moveTask.Quantity = Convert.ToInt32(moveItem.RealQuantity / moveItem.Product.Unit.Count);
                                            moveTask.TaskQuantity = Convert.ToInt32(moveItem.RealQuantity / moveItem.Product.Unit.Count);
                                            moveTask.OperateQuantity = Convert.ToInt32(moveItem.RealQuantity);
                                            moveTask.OrderID = moveItem.BillNo;
                                            moveTask.OrderType = "03";
                                            moveTask.AllotID = moveItem.ID;
                                            moveTask.DownloadState = "0";
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
                             * Status == 1:已录入 2:已审核 3:已分配 4:已确认 5:执行中 6:已结单 */
                            var moveBillMaster = MoveBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "4");
                            if (moveBillMaster != null)
                            {
                                moveBillMaster.Status = "5";
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
