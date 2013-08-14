﻿using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Allot.Interfaces;
using System.Linq;
using System.Collections.Generic;
using THOK.Wms.SignalR.Connection;
using System.Threading;
using THOK.Wms.SignalR.Model;
using THOK.Wms.SignalR.Common;
using THOK.Common.Entity;
using THOK.Authority;
using THOK.Authority.Dal.Interfaces;
namespace THOK.Wms.SignalR.Allot.Service
{
    public class InBillAllotService : Notifier<AllotStockInConnection>, IInBillAllotService
    {
        [Dependency]
        public IStorageLocker Locker { get; set; }
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }

        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }        
         
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

        public void Allot(string connectionId,ProgressState ps, CancellationToken cancellationToken,string billNo, string[] areaCodes)
        {
            Locker.LockKey = billNo;
            ConnectionId = connectionId;
            ps.State = StateType.Start;
            ps.Messages.Add("开始分配!");
            NotifyConnection(ps.Clone());
            IQueryable<THOK.Authority.DbModel.SystemParameter> systemParQuery = SystemParameterRepository.GetQueryable();
            IQueryable<InBillMaster> inBillMasterQuery = InBillMasterRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetObjectSet()
                .Include("Warehouse").Include("Area").Include("Storages");

            InBillMaster billMaster = inBillMasterQuery.Single(b => b.BillNo == billNo);
            if (!CheckAndLock(billMaster, ps)){return;}

            var isDefaultProduct = systemParQuery.FirstOrDefault(s => s.ParameterName == "IsDefaultProduct");//查询预设的卷烟其他卷烟是否可入 0 否 1是
            var InMantissaIsPiece = systemParQuery.FirstOrDefault(s => s.ParameterName == "InMantissaIsPiece");//查询入库的尾数是否放入件烟区  0 否 1是

            //选择未分配的细单数组；
            var billDetails = billMaster.InBillDetails
                                        .Where(b => (b.BillQuantity - b.AllotQuantity) > 0)
                                        .ToArray();


            //选择当前订单操作目标仓库；
            var cells = cellQuery.Where(c => c.WarehouseCode == billMaster.WarehouseCode
                                            && c.Warehouse.IsActive == "1"
                                            && c.Area.IsActive == "1"
                                            && c.IsActive == "1"
                                            && (areaCodes.Any(a => a == c.AreaCode)
                                                || (!areaCodes.Any() && c.Area.AllotInOrder > 0)))
                                 .ToArray();  

            //1：主库区；2：件烟区；
            //3；条烟区；4：暂存区；
            //5：备货区；6：残烟区；
            //7：罚烟区；8：虚拟区；
            //9：其他区；

            //排除 件烟区,条烟区 货位是单一存储的空货位；
            string [] areaTypes = new string []{"2","3"};
            var cells1 = cells.Where(c => areaTypes.All(a => a != c.Area.AreaType)
                                    && c.IsSingle == "1"
                                    && (c.Storages.Count == 0
                                            || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                                                && s.Quantity == 0
                                                && s.InFrozenQuantity == 0)));

            //条烟区 货位是单一存储的货位（不必是空货位，因为条烟会多次存储到同一个货位）；
            areaTypes = new string[] {"3"};
            var cell2 = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                        && c.IsSingle == "1");

            //件烟区 货位是单一存储的空货位； 
            areaTypes = new string[] {"2","4"};
            var cell3 = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                        && c.IsSingle == "1"
                                        && c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                                            && s.Product != null
                                            && (s.Quantity > 0 || s.InFrozenQuantity > 0)
                                            && (c.MaxQuantity >= s.Product.CellMaxProductQuantity ? s.Product.CellMaxProductQuantity : c.MaxQuantity * s.Product.Unit.Count) > s.Quantity + s.InFrozenQuantity));

            //件烟区 货位是单一存储的空货位； 
            areaTypes = new string[] { "2" };
            var cell4 = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                         && c.IsSingle == "1"
                                            && (c.Storages.Count == 0
                                                    || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0
                                                    )
                                                )
                                    );

            //非货位管理区
            var cell5 = cells.Where(c => c.IsSingle == "0");


            //排除 件烟区，条烟区
            var cellQueryFromList1 = cells1.Where(c => c.Storages.Count == 0
                                                || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                                                    && s.Quantity == 0
                                                    && s.InFrozenQuantity == 0))
                                              .OrderBy(c => c.Area.AllotInOrder);
            //条烟区
            var cellQueryFromList2 = cell2.OrderBy(c => c.Area.AllotInOrder);

            //件烟区 --入库尾数可以放入件烟区
            var cellQueryFromList3 = cell3.OrderBy(c => c.Area.AllotInOrder);
            if (InMantissaIsPiece.ParameterValue != "0")
            {
                cellQueryFromList3 = cell3.Where(c => c.Storages.Any(s => (c.MaxQuantity >= s.Product.CellMaxProductQuantity ? s.Product.CellMaxProductQuantity : c.MaxQuantity) * s.Product.Unit.Count > s.Quantity + s.InFrozenQuantity))
                                              .OrderBy(c => c.Area.AllotInOrder);
            }
            //件烟区 --入库尾数不放入件烟区
            var cellQueryFromList4 = cell4.Where(c => c.Storages.Count == 0
                                                || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag)
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0))
                                              .OrderBy(c => c.Area.AllotInOrder);

            //非货位管理区
            var cellQueryFromList5 = cell5.OrderBy(c => c.Area.AllotInOrder % 10000);
            
            foreach (var billDetail in billDetails)
            {
                //分配预设当前卷烟的货位；
                var cs = cellQueryFromList1.Where(c => c.DefaultProductCode == billDetail.ProductCode);
                AllotPallet(billMaster, billDetail, cs, cancellationToken, ps);

                //分配没预设卷烟的货位；
                cs = cellQueryFromList1.Where(c => string.IsNullOrEmpty(c.DefaultProductCode)
                    && (string.IsNullOrEmpty(billDetail.Product.PointAreaCodes) || billDetail.Product.PointAreaCodes.Contains(c.AreaCode)));
                AllotPallet(billMaster, billDetail, cs, cancellationToken, ps);
                
                if (isDefaultProduct.ParameterValue != "0")//判断预设卷烟后不能放入其他烟
                {
                    //分配预设其他卷烟的货位；
                    cs = cellQueryFromList1.Where(c => c.DefaultProductCode != billDetail.ProductCode
                                                        && !string.IsNullOrEmpty(c.DefaultProductCode)
                                                        && (string.IsNullOrEmpty(billDetail.Product.PointAreaCodes) || billDetail.Product.PointAreaCodes.Contains(c.AreaCode)));
                    AllotPallet(billMaster, billDetail, cs, cancellationToken, ps);
                }

                //分配条烟到条烟区；
                cs = cellQueryFromList2.Where(c => c.DefaultProductCode == billDetail.ProductCode
                                                        || (c.Storages.Count == 1
                                                            && c.Storages.First().ProductCode == billDetail.ProductCode) 
                                                        );
                AllotBar(billMaster, billDetail, cs, cancellationToken, ps);
                //分配条烟到条烟区；
                if (!cs.Any())
                {
                    cs = cellQueryFromList2.Where(c => (isDefaultProduct.ParameterValue == "0" ? string.IsNullOrEmpty(c.DefaultProductCode) : true));
                    AllotBar(billMaster, billDetail, cs, cancellationToken, ps);
                }

                //分配未满一托盘的卷烟到件烟区；
                cs = cellQueryFromList3.Where(c => (isDefaultProduct.ParameterValue == "0" ? string.IsNullOrEmpty(c.DefaultProductCode) : true) || c.DefaultProductCode == billDetail.ProductCode);
                if (cellQueryFromList2.Any())
                {
                    if (InMantissaIsPiece.ParameterValue != "0")
                        AllotPiece(billMaster, billDetail, cs, cancellationToken, ps);
                    cs = cellQueryFromList4.Where(c => (isDefaultProduct.ParameterValue == "0" ? string.IsNullOrEmpty(c.DefaultProductCode) : true));
                    AllotPiece(billMaster, billDetail, cs, cancellationToken, ps);
                }
                else
                {
                    if (InMantissaIsPiece.ParameterValue != "0")
                        AllotPieceAndBar(billMaster, billDetail, cs, cancellationToken, ps);
                    cs = cellQueryFromList4.Where(c => (isDefaultProduct.ParameterValue == "0" ? string.IsNullOrEmpty(c.DefaultProductCode) : true));
                    AllotPieceAndBar(billMaster, billDetail, cs, cancellationToken, ps);
                }    

                //分配未满一托盘的卷烟到下层货架；
                cs = cellQueryFromList1.Where(c => c.Layer == 1 &&(isDefaultProduct.ParameterValue == "0" ? string.IsNullOrEmpty(c.DefaultProductCode) : true)
                    && (string.IsNullOrEmpty(billDetail.Product.PointAreaCodes) || billDetail.Product.PointAreaCodes.Contains(c.AreaCode)));
                if (cellQueryFromList2.Count() > 0)
                {
                    AllotPiece(billMaster, billDetail, cs, cancellationToken, ps);
                }
                else
                {
                    AllotPieceAndBar(billMaster, billDetail, cs, cancellationToken, ps);
                }

                //分配未分配卷烟到其他库区；
                cs = cellQueryFromList1.Where(c => (isDefaultProduct.ParameterValue == "0" ? string.IsNullOrEmpty(c.DefaultProductCode) : true)
                    && (string.IsNullOrEmpty(billDetail.Product.PointAreaCodes) || billDetail.Product.PointAreaCodes.Contains(c.AreaCode)));
                AllotPiece(billMaster, billDetail, cs, cancellationToken, ps);

                //分配未分配卷烟到其他非货位管理货位；
                while (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    var c = cellQueryFromList5.Where(i => !i.Storages.Any()
                                                        || i.Storages.Count() < i.MaxPalletQuantity
                                                            || i.Storages.Any(s=> string.IsNullOrEmpty(s.LockTag)
                                                                && s.Quantity == 0
                                                                && s.InFrozenQuantity == 0))
                                              .Where(i=>string.IsNullOrEmpty(billDetail.Product.PointAreaCodes) || billDetail.Product.PointAreaCodes.Contains(i.AreaCode))
                                              .FirstOrDefault();

                    if (c != null)
                    {
                        lock (c)
                        {
                            decimal allotQuantity = (c.MaxQuantity >= billDetail.Product.CellMaxProductQuantity ? billDetail.Product.CellMaxProductQuantity : c.MaxQuantity) * billDetail.Product.Unit.Count;
                            decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                            allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                            var targetStorage = Locker.LockStorage(c);
                            if (targetStorage != null
                                && targetStorage.Quantity == 0
                                && targetStorage.InFrozenQuantity == 0)
                            {
                                Allot(billMaster, billDetail, c, targetStorage, allotQuantity, ps);
                                Locker.UnLockStorage(targetStorage);
                            }
                        }
                    }
                    else break;                   
                }
            }

            string billno = billMaster.BillNo;
            if (billMaster.InBillDetails.Any(i => i.BillQuantity - i.AllotQuantity > 0))
            {
                ps.State = StateType.Warning;
                ps.Errors.Add("分配未全部完成，没有储位可分配！");
                NotifyConnection(ps.Clone());
                
                InBillMasterRepository.GetObjectSet()
                    .UpdateEntity(i => i.BillNo == billno,
                    i => new InBillMaster() { LockTag = ""});
            }
            else
            {
                ps.State = StateType.Info;
                ps.Messages.Add("分配完成,开始保存请稍候!");
                NotifyConnection(ps.Clone());                

                billMaster.Status = "3";
                try
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        billMaster.LockTag = string.Empty;
                        CellRepository.SaveChanges();
                        ps.State = StateType.Info;
                        ps.Messages.Clear();
                        ps.Messages.Add("分配成功!");
                        NotifyConnection(ps.Clone());
                    }
                }
                catch (Exception e)
                {
                    ps.State = StateType.Error;
                    ps.Messages.Add("保存失败，详情：" + e.Message);
                    NotifyConnection(ps.Clone());
                }
                finally
                {
                    InBillMasterRepository.GetObjectSet()
                        .UpdateEntity(i => i.BillNo == billno,
                        i => new InBillMaster() { LockTag = "" });
                }
            }
        }

        private void Allot(InBillMaster billMaster, InBillDetail billDetail, Cell cell, Storage storage, decimal allotQuantity, ProgressState ps)
        {
            if (storage != null && allotQuantity > 0)
            {
                InBillAllot billAllot = null;
                billDetail.AllotQuantity += allotQuantity;
                storage.ProductCode = billDetail.ProductCode;
                storage.LockTag = billDetail.BillNo;
                storage.InFrozenQuantity += allotQuantity;

                billAllot = new InBillAllot()
                {
                    BillNo = billMaster.BillNo,
                    InBillDetailId = billDetail.ID,
                    ProductCode = billDetail.ProductCode,
                    CellCode = cell.CellCode,
                    StorageCode = storage.StorageCode,
                    UnitCode = billDetail.UnitCode,
                    AllotQuantity = allotQuantity,
                    RealQuantity = 0,
                    Status = "0"
                };
                billMaster.InBillAllots.Add(billAllot);

                decimal sumBillQuantity = billMaster.InBillDetails.Sum(d => d.BillQuantity);
                decimal sumAllotQuantity = billMaster.InBillDetails.Sum(d => d.AllotQuantity);

                decimal sumBillProductQuantity = billMaster.InBillDetails.Where(d => d.ProductCode == billDetail.ProductCode)
                                                                         .Sum(d => d.BillQuantity);
                decimal sumAllotProductQuantity = billMaster.InBillDetails.Where(d => d.ProductCode == billDetail.ProductCode)
                                                                          .Sum(d => d.AllotQuantity);

                ps.State = StateType.Processing;
                ps.TotalProgressName = "分配入库单：" + billMaster.BillNo;
                ps.TotalProgressValue = (int)(sumAllotQuantity / sumBillQuantity * 100);
                ps.CurrentProgressName = "分配卷烟：" + billDetail.Product.ProductName;
                ps.CurrentProgressValue = (int)(sumAllotProductQuantity / sumBillProductQuantity * 100);
                NotifyConnection(ps.Clone());
            }
        }

        //分配件烟
        private void AllotPiece(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var c in cs)
            {
                lock (c)
                {
                    if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                    {
                        var targetStorage = Locker.LockStorage(c);                       
                        if (targetStorage != null
                            && (string.IsNullOrEmpty(targetStorage.ProductCode)
                                || targetStorage.ProductCode == billDetail.ProductCode
                                || (targetStorage.Quantity == 0
                                    && targetStorage.InFrozenQuantity == 0)))
                        {
                            decimal allotQuantity = (c.MaxQuantity >= billDetail.Product.CellMaxProductQuantity ? billDetail.Product.CellMaxProductQuantity : c.MaxQuantity) * billDetail.Product.Unit.Count 
                                                        - targetStorage.Quantity 
                                                        - targetStorage.InFrozenQuantity;
                            decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                                / billDetail.Product.Unit.Count)
                                * billDetail.Product.Unit.Count;
                            allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity; 
                            Allot(billMaster, billDetail, c, targetStorage, allotQuantity, ps);
                            Locker.UnLockStorage(targetStorage);
                        }
                    }
                    else break;
                }
            }
        }

        //分配件烟，条烟
        private void AllotPieceAndBar(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var c in cs)
            {
                lock (c)
                {
                    if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                    {
                        var targetStorage = Locker.LockStorage(c);
                        if (targetStorage != null
                            && (string.IsNullOrEmpty(targetStorage.ProductCode)
                                || targetStorage.ProductCode == billDetail.ProductCode
                                || (targetStorage.Quantity == 0
                                    && targetStorage.InFrozenQuantity == 0)))
                        {
                            decimal allotQuantity = (c.MaxQuantity >= billDetail.Product.CellMaxProductQuantity ? billDetail.Product.CellMaxProductQuantity : c.MaxQuantity) * billDetail.Product.Unit.Count
                                                        - targetStorage.Quantity
                                                        - targetStorage.InFrozenQuantity; 
                            decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                            allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                            Allot(billMaster, billDetail, c, targetStorage, allotQuantity, ps);
                            Locker.UnLockStorage(targetStorage);
                        }
                    }
                    else break;
                }
            }
        }

        //分配条烟
        private void AllotBar(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var c in cs)
            {
                lock (c)
                {
                    if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                    {
                        decimal billQuantity = (billDetail.BillQuantity - billDetail.AllotQuantity) % billDetail.Product.Unit.Count;
                        if (billQuantity > 0)
                        {
                            var targetStorage = Locker.LockStorage(c);
                            if (targetStorage != null
                                && (string.IsNullOrEmpty(targetStorage.ProductCode)
                                || targetStorage.ProductCode == billDetail.ProductCode
                                || (targetStorage.Quantity == 0
                                    && targetStorage.InFrozenQuantity == 0)))
                            {
                                Allot(billMaster, billDetail, c, targetStorage, billQuantity, ps);
                                Locker.UnLockStorage(targetStorage);
                            }
                        }
                        else break;
                    }
                    else break;
                }
            }
        }

        //分配整盘
        private void AllotPallet(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var c in cs)
            {
                lock (c)
                {
                    if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                    {
                        decimal allotQuantity = (c.MaxQuantity >= billDetail.Product.CellMaxProductQuantity ? billDetail.Product.CellMaxProductQuantity : c.MaxQuantity) * billDetail.Product.Unit.Count;
                        decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                            / billDetail.Product.Unit.Count)
                            * billDetail.Product.Unit.Count;
                        if (billQuantity >= allotQuantity)
                        {
                            var targetStorage = Locker.LockStorage(c);
                            if (targetStorage != null
                                && targetStorage.Quantity == 0
                                && targetStorage.InFrozenQuantity == 0)
                            {
                                Allot(billMaster, billDetail, c,targetStorage, allotQuantity, ps);
                                Locker.UnLockStorage(targetStorage);
                            }
                        }
                        else break;
                    }
                    else break;
                }
            }
        }

        //检查主表并加锁
        private bool CheckAndLock(InBillMaster billMaster, ProgressState ps)
        {
            if ((new string[] { "1" }).Any(s => s == billMaster.Status))
            {
                ps.State = StateType.Info;
                ps.Messages.Add("当前订单未审核，不可以进行分配！");
                NotifyConnection(ps.Clone());
                return false;
            }

            if ((new string[] { "4", "5", "6" }).Any(s => s == billMaster.Status))
            {
                ps.State = StateType.Info;
                ps.Messages.Add("分配已确认生效不能再分配！");
                NotifyConnection(ps.Clone());
                return false;
            }

            if (!string.IsNullOrEmpty(billMaster.LockTag))
            {
                ps.State = StateType.Error;
                ps.Errors.Add("当前订单被锁定不可以进行分配！");
                NotifyConnection(ps.Clone());
                return false;
            }
            else
            {
                try
                {
                    billMaster.LockTag = ConnectionId;
                    InBillMasterRepository.SaveChanges();
                    ps.Messages.Add("锁定当前订单成功！");
                    NotifyConnection(ps.Clone());
                    return true;
                }
                catch (Exception)
                {
                    ps.State = StateType.Error;
                    ps.Errors.Add("锁定当前订单失败不可以进行分配！");
                    NotifyConnection(ps.Clone());
                    return false;
                }
            }
        }        
    }
}
