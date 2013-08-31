﻿using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using THOK.Wms.SignalR.Common;
using THOK.Common.Entity;

namespace THOK.Wms.Allot.Service
{
    public class OutBillAllotService : ServiceBase<OutBillAllot>, IOutBillAllotService
    {
        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
        [Dependency]
        public IAreaRepository AreaRepository { get; set; }
        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IMoveBillCreater MoveBillCreater { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IOutBillAllotService 成员
        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "0":
                    statusStr = "未开始";
                    break;
                case "1":
                    statusStr = "已申请";
                    break;
                case "2":
                    statusStr = "已完成";
                    break;
            }
            return statusStr;
        }

        public object Search(string billNo, int page, int rows)
        {
            var allotQuery = OutBillAllotRepository.GetQueryable();
            var query = allotQuery.Where(a => a.BillNo == billNo).OrderBy(a => a.ID).Select(i => i);
            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);

            var temp = query.ToArray().Select(a => new
                                        {
                                            a.ID,
                                            a.BillNo,
                                            a.ProductCode,
                                            a.Product.ProductName,
                                            a.CellCode,
                                            a.Cell.CellName,
                                            a.StorageCode,
                                            a.UnitCode,
                                            a.Unit.UnitName,
                                            AllotQuantity = a.AllotQuantity / a.Unit.Count,
                                            RealQuantity = a.RealQuantity / a.Unit.Count,
                                            a.OperatePersonID,
                                            StartTime = a.StartTime == null ? "" : ((DateTime)a.StartTime).ToString("yyyy-MM-dd"),
                                            FinishTime = a.FinishTime == null ? "" : ((DateTime)a.FinishTime).ToString("yyyy-MM-dd"),
                                            Status = WhatStatus(a.Status)
                                        });

            return new { total, rows = temp.ToArray() };
        }

        public bool AllotCancel(string billNo, out string strResult)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            var outAllot = OutBillAllotRepository.GetQueryable()
                                                                 .Where(o => o.BillNo == ibm.BillNo)
                                                                 .ToArray();

                            var storages = outAllot.Select(i => i.Storage).ToArray();

                            if (!Locker.Lock(storages))
                            {
                                strResult = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                                return false;
                            }

                            outAllot.AsParallel().ForAll(
                                (Action<OutBillAllot>)delegate(OutBillAllot o)
                                {
                                    if (o.Storage.ProductCode == o.ProductCode
                                        && o.Storage.OutFrozenQuantity >= o.AllotQuantity)
                                    {
                                        lock (o.OutBillDetail)
                                        {
                                            o.OutBillDetail.AllotQuantity -= o.AllotQuantity;
                                        }
                                        o.Storage.OutFrozenQuantity -= o.AllotQuantity;
                                        o.Storage.LockTag = string.Empty;
                                    }
                                    else
                                    {
                                        throw new Exception("储位的卷烟或出库冻结量与当前分配不符，信息可能被异常修改，不能取消当前出库分配！");
                                    }
                                }
                            );

                            OutBillAllotRepository.SaveChanges();

                            OutBillAllotRepository.GetObjectSet()
                                .DeleteEntity(i => i.BillNo == ibm.BillNo);

                            ibm.Status = "2";
                            ibm.UpdateTime = DateTime.Now;
                            OutBillMasterRepository.SaveChanges();
                            result = true;
                            strResult = "取消成功";

                            scope.Complete();
                        }
                    }
                    catch (Exception e)
                    {
                        strResult = "取消分配失败，详情：" + e.Message;
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }

        public bool AllotAdd(string billNo, long id, string productCode, string cellCode, decimal allotQuantity, out string strResult)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            var cell = CellRepository.GetQueryable().Single(c => c.CellCode == cellCode);
            var obm = OutBillDetailRepository.GetQueryable().FirstOrDefault(o => o.ID == id);
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    Storage storage = Locker.LockNoEmpty(cell, obm.Product);
                    if (storage != null && allotQuantity > 0)
                    {
                        OutBillAllot billAllot = null;
                        decimal q1 = obm.BillQuantity - obm.AllotQuantity;
                        decimal q2 = allotQuantity * obm.Unit.Count;
                        decimal q3 = storage.Quantity - storage.OutFrozenQuantity;
                        if (q1 >= q2 && q2 <= q3)
                        {
                            try
                            {
                                billAllot = new OutBillAllot()
                                {
                                    BillNo = billNo,
                                    OutBillDetailId = obm.ID,
                                    ProductCode = obm.ProductCode,
                                    CellCode = storage.CellCode,
                                    StorageCode = storage.StorageCode,
                                    UnitCode = obm.UnitCode,
                                    AllotQuantity = q2,
                                    RealQuantity = 0,
                                    Status = "0"
                                };
                                obm.AllotQuantity += q2;
                                storage.OutFrozenQuantity += q2;
                                ibm.OutBillAllots.Add(billAllot);
                                ibm.Status = "3";
                                storage.LockTag = string.Empty;
                                StorageRepository.SaveChanges();
                                strResult = "保存修改成功！";
                                result = true;
                            }
                            catch (Exception)
                            {
                                strResult = "保存添加失败，订单或储位其他人正在操作！";
                            }
                        }
                        else
                        {
                            strResult = "分配数量超过订单数量,或者当前储位库存量不足！";
                        }
                    }
                    else
                    {
                        strResult = "当前选择的储位不可用，其他人正在操作或没有库存！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }
        #region 手动分配出库
        public bool AllotAdd(string billNo, long id, string cellCode, string productName, out string strResult, out decimal allotQuantity)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            var cell = CellRepository.GetQueryable().Single(c => c.CellCode == cellCode);
            var obm = OutBillDetailRepository.GetQueryable().FirstOrDefault(o => o.ID == id);
            var stor=StorageRepository.GetQueryable().Single(c=>c.CellCode==cellCode);
            decimal q1 = obm.BillQuantity - obm.AllotQuantity;
            allotQuantity = 0;
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    Storage storage = Locker.LockNoEmpty(cell, obm.Product);
                    if (storage != null)
                    {
                        if (stor.Product.ProductName == productName)
                        {
                            if (q1 > 0)
                            {
                                OutBillAllot billAllot = null;
                                decimal q3 = storage.Quantity - storage.OutFrozenQuantity;
                                if (q1 <= q3)
                                {
                                    try
                                    {
                                        billAllot = new OutBillAllot()
                                        {
                                            BillNo = billNo,
                                            OutBillDetailId = obm.ID,
                                            ProductCode = obm.ProductCode,
                                            CellCode = storage.CellCode,
                                            StorageCode = storage.StorageCode,
                                            UnitCode = obm.UnitCode,
                                            AllotQuantity = q1,
                                            RealQuantity = 0,
                                            Status = "0"
                                        };
                                        allotQuantity = (q3 - q1) / storage.Product.Unit.Count;
                                        obm.AllotQuantity += q1;
                                        storage.OutFrozenQuantity += q1;
                                        ibm.OutBillAllots.Add(billAllot);
                                        ibm.Status = "3";
                                        storage.LockTag = string.Empty;
                                        StorageRepository.SaveChanges();
                                        strResult = "保存修改成功！";
                                        result = true;
                                    }
                                    catch (Exception)
                                    {
                                        strResult = "保存添加失败，订单或储位其他人正在操作！";
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        billAllot = new OutBillAllot()
                                        {
                                            BillNo = billNo,
                                            OutBillDetailId = obm.ID,
                                            ProductCode = obm.ProductCode,
                                            CellCode = storage.CellCode,
                                            StorageCode = storage.StorageCode,
                                            UnitCode = obm.UnitCode,
                                            AllotQuantity = q3,
                                            RealQuantity = 0,
                                            Status = "0"
                                        };
                                        obm.AllotQuantity += q3;
                                        storage.OutFrozenQuantity += q3;
                                        ibm.OutBillAllots.Add(billAllot);
                                        ibm.Status = "3";
                                        storage.LockTag = string.Empty;
                                        StorageRepository.SaveChanges();
                                        strResult = "保存修改成功！";
                                        result = true;
                                    }
                                    catch (Exception)
                                    {
                                        strResult = "保存添加失败，订单或储位其他人正在操作！";
                                    }
                                }
                            }
                            else
                            {
                                strResult = "该产品已无分配任务！";
                            }
                        }
                        else
                        {
                            strResult = "该卷烟没有出库任务！";
                        }
                    }
                    else
                    {
                        strResult = "当前选择的储位不可用，其他人正在操作或没有库存！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }
        #endregion
        public bool AllotDelete(string billNo, long id, out string strResult)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    var allotDetail = ibm.OutBillAllots.Single(a => a.ID == (int)id);
                    if (string.IsNullOrEmpty(allotDetail.Storage.LockTag))
                    {
                        try
                        {
                            allotDetail.OutBillDetail.AllotQuantity -= allotDetail.AllotQuantity;
                            allotDetail.Storage.OutFrozenQuantity -= allotDetail.AllotQuantity;
                            allotDetail.Storage.LockTag = string.Empty;
                            ibm.OutBillAllots.Remove(allotDetail);
                            OutBillAllotRepository.Delete(allotDetail);
                            if (ibm.OutBillAllots.Count == 0)
                            {
                                ibm.Status = "2";
                                ibm.UpdateTime = DateTime.Now;
                            }
                            OutBillAllotRepository.SaveChanges();
                            strResult = "";
                            result = true;
                        }
                        catch (Exception)
                        {
                            strResult = "当前储位或订单其他人正在操作，请稍候重试！";
                        }
                    }
                    else
                    {
                        strResult = "当前储位其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }

        public bool AllotEdit(string billNo, long id, string cellCode, decimal allotQuantity, out string strResult)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            var cell = CellRepository.GetQueryable().Single(c => c.CellCode == cellCode);
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    var allotDetail = ibm.OutBillAllots.Single(a => a.ID == (int)id);
                    if (string.IsNullOrEmpty(allotDetail.Storage.LockTag))
                    {
                        Storage storage;
                        if (allotDetail.CellCode == cellCode)
                        {
                            storage = allotDetail.Storage;
                            storage.OutFrozenQuantity -= allotDetail.AllotQuantity;
                        }
                        else
                        {
                            storage = Locker.LockNoEmpty(cell, allotDetail.Product);
                            allotDetail.Storage.OutFrozenQuantity -= allotDetail.AllotQuantity;
                        }
                        if (storage != null)
                        {
                            decimal q1 = allotDetail.OutBillDetail.BillQuantity - allotDetail.OutBillDetail.AllotQuantity;
                            decimal q2 = allotQuantity * allotDetail.Unit.Count;
                            decimal q3 = storage.Quantity - storage.OutFrozenQuantity;
                            if (q1 >= q2 && q2 <= q3)
                            {
                                try
                                {
                                    allotDetail.OutBillDetail.AllotQuantity -= allotDetail.AllotQuantity;
                                    allotDetail.OutBillDetail.AllotQuantity += q2;
                                    storage.OutFrozenQuantity += q2;
                                    storage.LockTag = string.Empty;
                                    allotDetail.CellCode = storage.Cell.CellCode;
                                    allotDetail.StorageCode = storage.StorageCode;
                                    allotDetail.AllotQuantity = q2;
                                    OutBillAllotRepository.SaveChanges();
                                    strResult = "保存修改成功！";
                                    result = true;
                                }
                                catch (Exception)
                                {
                                    strResult = "保存修改失败，订单或储位其他人正在操作！";
                                }
                            }
                            else
                            {
                                strResult = "分配数量超过订单数量,或者当前储位库存量不足！";
                            }
                        }
                        else
                        {
                            strResult = "当前选择的储位不可用，其他人正在操作或没有库存！";
                        }
                    }
                    else
                    {
                        strResult = "当前储位其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }

        public bool AllotConfirm(string billNo, string userName, ref string errorInfo)
        {
            try
            {
                var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
                var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);

                if (employee == null)
                {
                    errorInfo = "当前用户不存在或不可用，未能审核！";
                    return false;
                }
                if (ibm == null)
                {
                    errorInfo = "当前订单状态不是已分配，或当前订单不存在！";
                    return false;
                }
                if (!(ibm.OutBillDetails.All(b => b.BillQuantity == b.AllotQuantity)
                    && ibm.OutBillDetails.Sum(b => b.BillQuantity) == ibm.OutBillAllots.Sum(a => a.AllotQuantity)))
                {
                    errorInfo = "当前订单分配未完成或分配结果不正确！";
                    return false;
                }
                if (!string.IsNullOrEmpty(ibm.LockTag))
                {
                    errorInfo = "当前订单其他人正在操作，请稍候重试！";
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (MoveBillCreater.CheckIsNeedSyncMoveBill(ibm.WarehouseCode))
                    {
                        var moveBillMaster = MoveBillCreater.CreateMoveBillMaster(ibm.WarehouseCode, ibm.BillTypeCode, employee.ID.ToString());
                        MoveBillCreater.CreateSyncMoveBillDetail(moveBillMaster);
                        moveBillMaster.Status = "2";
                        moveBillMaster.Description = "出库生成同步移库单！";
                        moveBillMaster.VerifyDate = DateTime.Now;
                        moveBillMaster.VerifyPersonID = employee.ID;
                        if (MoveBillCreater.CheckIsNeedSyncMoveBill(ibm.WarehouseCode))
                        {
                            errorInfo = "生成同步移库单不完整，请重新确认！";
                            return false;
                        }
                        else
                        {
                            ibm.MoveBillMasterBillNo = moveBillMaster.BillNo;
                        }
                    }

                    ibm.Status = "4";
                    ibm.UpdateTime = DateTime.Now;
                    OutBillMasterRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorInfo = "确认分配失败，详情：" + e.Message;
                return false;
            }
        }

        public bool AllotCancelConfirm(string billNo, out string strResult)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "4");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            if (ibm.MoveBillMaster != null)
                            {
                                try
                                {
                                    MoveBillCreater.DeleteMoveBillDetail(ibm.MoveBillMaster);
                                    MoveBillMasterRepository.Delete(ibm.MoveBillMaster);
                                    MoveBillMasterRepository.SaveChanges();
                                }
                                catch (Exception)
                                {
                                    strResult = "删除同步移库单失败，请重新取消！";
                                    return false;
                                }

                            }

                            ibm.Status = "3";
                            ibm.UpdateTime = DateTime.Now;
                            OutBillMasterRepository.SaveChanges();
                            result = true;
                            strResult = "取消成功";

                            scope.Complete();
                        }
                    }
                    catch (Exception)
                    {
                        strResult = "当前订单其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已确认，或当前订单不存在！";
            }
            return result;
        }
        #endregion

        #region IOutBillAllotService 成员
        public System.Data.DataTable AllotSearch(int page, int rows, string billNo)
        {
            System.Data.DataTable dt = null;
            var allotQuery = OutBillAllotRepository.GetQueryable();
            if (allotQuery != null)
            {
                var query = allotQuery.Where(a => a.BillNo == billNo).OrderBy(a => a.ID).Select(a => new
                {
                    a.ProductCode,
                    a.Product.ProductName,
                    a.Cell.CellName,
                    a.Unit.UnitName,
                    AllotQuantity = a.AllotQuantity / a.Unit.Count,
                    RealQuantity = a.RealQuantity / a.Unit.Count,
                    a.OperatePersonID,
                    a.StartTime,
                    a.FinishTime,
                    Status = a.Status == "0" ? "未开始" : a.Status == "1" ? "已申请" : a.Status == "2" ? "已完成" : "空"
                });
                dt = new System.Data.DataTable();
                dt.Columns.Add("商品编码", typeof(string));
                dt.Columns.Add("商品名称", typeof(string));
                dt.Columns.Add("储位名称", typeof(string));
                dt.Columns.Add("单位名称", typeof(string));
                dt.Columns.Add("分配数量", typeof(decimal));
                dt.Columns.Add("实际数量", typeof(decimal));
                dt.Columns.Add("作业人员", typeof(string));
                dt.Columns.Add("开始时间", typeof(string));
                dt.Columns.Add("完成时间", typeof(string));
                dt.Columns.Add("作业状态", typeof(string));
                foreach (var q in query)
                {
                    dt.Rows.Add
                        (
                            q.ProductCode,
                            q.ProductName,
                            q.CellName,
                            q.UnitName,
                            q.AllotQuantity,
                            q.RealQuantity,
                            q.OperatePersonID,
                            q.StartTime,
                            q.FinishTime,
                            q.Status
                        );
                }
                if (query.Count() > 0)
                {
                    dt.Rows.Add(
                        null, null, null, "总数：",
                        query.Sum(m => m.AllotQuantity),
                        query.Sum(m => m.RealQuantity),
                        null, null, null, null);
                }
            }
            return dt;
        }
        #endregion

        #region 车载系统
        public object GetOutBillMaster()
        {
            var outBillMaster = OutBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "4" || i.Status == "5")
                                .Select(i => new { BillNo = i.BillNo, BillType = "2" })
                                .ToArray();
            return outBillMaster;
        }
        public object SearchOutBillAllot(string billNo, int page, int rows)
        {
            var query = OutBillAllotRepository.GetQueryable()
                                .Where(a => a.BillNo == billNo && a.Status != "2")
                                .OrderByDescending(a => a.Status == "1")
                                .Select(i => i);
            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);

            var temp = query.ToArray().Select(a => new
            {
                a.ID,
                a.BillNo,
                a.ProductCode,
                a.Product.ProductName,
                a.CellCode,
                a.Cell.CellName,
                BillType = "出库",
                a.StorageCode,
                a.UnitCode,
                a.Unit.UnitName,
                PieceQuantity = Math.Floor(a.AllotQuantity / a.Product.UnitList.Unit01.Count),
                BarQuantity = Math.Floor(a.AllotQuantity % a.Product.UnitList.Unit01.Count / a.Product.UnitList.Unit02.Count),
                Status = WhatStatus(a.Status),
                a.Operator
            });
            return new { total, rows = temp.ToArray() };
        }
        public bool EditAllot(string id, string status, string operater, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            string[] ids = id.Split(',');
            string strId = "";
            OutBillAllot allot = null;

            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.UserName == operater);

            for (int i = 0; i < ids.Length; i++)
            {
                strId = ids[i].ToString();
                allot = OutBillAllotRepository.GetQueryable().ToArray().FirstOrDefault(a => strId == a.ID.ToString());
                if (allot != null)
                {
                    if (allot.Status == "0" && status == "1"
                     || allot.Status == "1" && status == "0"
                     || allot.Status == "1" && status == "2")
                    {
                        try
                        {
                            allot.Status = status;
                            if (operater != "")
                            {
                                allot.Operator = employee.EmployeeName;
                            }
                            else
                            {
                                allot.Operator = "";
                            }
                            OutBillAllotRepository.SaveChanges();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            strResult = "原因：" + ex.Message;
                        }
                    }
                    else
                    {
                        strResult = "原因：操作错误！";
                    }
                }
                else
                {
                    strResult = "原因：未找到该记录！";
                }
            }
            return result;
        }
        #endregion
    }
}
