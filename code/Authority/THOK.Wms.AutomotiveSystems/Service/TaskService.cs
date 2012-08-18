using System;
using THOK.Wms.AutomotiveSystems.Models;
using THOK.Wms.AutomotiveSystems.Interfaces;
using THOK.Wms.Dal.Interfaces;
using Microsoft.Practices.Unity;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

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

        public void GetBillDetail(BillMaster[] billMasters, string productCode, string OperateType, string Operator, Result result)
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
                            var inBillDetails = InBillAllotRepository.GetQueryable()
                                .Where(i => i.BillNo == billNo
                                    && (i.ProductCode == productCode || productCode == string.Empty)                                    
                                    && (i.Status == "0" || (i.Status == "1"&& i.Operator == Operator)))
                                .ToArray()
                                .Select(i => new BillDetail() { 
                                    BillNo = i.BillNo, 
                                    BillType = "1" ,

                                    DetailID = i.ID.ToString(),
                                    StorageName = i.Cell.CellName,
                                    StorageRfid = i.Cell.Rfid,
                                    TargetStorageName = "",
                                    TargetStorageRfid = "",

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity =(int) Math.Floor(i.AllotQuantity/i.Product.UnitList.Unit01.Count),
                                    BarQuantity = (int) Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = (int)Math.Floor(i.AllotQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = (int)Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                    OperatorCode = string.Empty,
                                    Operator = i.Operator,
                                    Status = i.Status,
                                })
                                .ToArray();
                            billDetails = billDetails.Concat(inBillDetails).ToArray();
                            break;
                        case "2"://出库单
                            var outBillDetails = OutBillAllotRepository.GetQueryable()
                                .Where(i => i.BillNo == billNo
                                    && (i.CanRealOperate == "1" || OperateType != "Real")
                                    && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator)))
                                .ToArray()
                                .Select(i => new BillDetail()
                                {
                                    BillNo = i.BillNo,
                                    BillType = "2",

                                    DetailID = i.ID.ToString(),
                                    StorageName = i.Cell.CellName,
                                    StorageRfid = i.Cell.Rfid,
                                    TargetStorageName = "",
                                    TargetStorageRfid = "",

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity = (int)Math.Floor(i.AllotQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = (int)Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = (int)Math.Floor(i.AllotQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = (int)Math.Floor((i.AllotQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

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
                                            && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator)))
                                        .ToArray()
                                        .Select(i => new BillDetail()
                                        {
                                            BillNo = i.BillNo,
                                            BillType = "3",

                                            DetailID = i.ID.ToString(),
                                            StorageName = i.OutCell.CellName,
                                            StorageRfid = i.OutCell.Rfid,
                                            TargetStorageName = i.InCell.CellName,
                                            TargetStorageRfid = i.InCell.Rfid,

                                            ProductCode = i.ProductCode,
                                            ProductName = i.Product.ProductName,

                                            PieceQuantity = (int)Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                            BarQuantity = (int)Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                            OperatePieceQuantity = (int)Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                            OperateBarQuantity = (int)Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                            OperatorCode = string.Empty,
                                            Operator = i.Operator,
                                            Status = i.Status,
                                        })
                                        .ToArray();
                                billDetails = billDetails.Concat(moveBillDetailss).ToArray();
                            }
                            break;                           
                        case "3"://移库单
                           var moveBillDetails = MoveBillDetailRepository.GetQueryable()
                                .Where(i => i.BillNo == billNo
                                    && (i.CanRealOperate == "1" || OperateType != "Real")
                                    && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator)))
                                .ToArray()
                                .Select(i => new BillDetail()
                                {
                                    BillNo = i.BillNo,
                                    BillType = "3",

                                    DetailID = i.ID.ToString(),
                                    StorageName = i.OutCell.CellName,
                                    StorageRfid = i.OutCell.Rfid,
                                    TargetStorageName = i.InCell.CellName,
                                    TargetStorageRfid = i.InCell.Rfid,

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity = (int)Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = (int)Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = (int)Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = (int)Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

                                    OperatorCode = string.Empty,
                                    Operator = i.Operator,
                                    Status = i.Status,
                                })
                                .ToArray();
                            billDetails = billDetails.Concat(moveBillDetails).ToArray();
                            break;
                        case "4"://盘点单
                            var checkBillDetails = CheckBillDetailRepository.GetQueryable()
                                .Where(i => i.BillNo == billNo
                                    && (i.Status == "0" || (i.Status == "1" && i.Operator == Operator)))
                                .ToArray()
                                .Select(i => new BillDetail()
                                {
                                    BillNo = i.BillNo,
                                    BillType = "4",

                                    DetailID = i.ID.ToString(),
                                    StorageName = i.Cell.CellName,
                                    StorageRfid = i.Cell.Rfid,
                                    TargetStorageName = "",
                                    TargetStorageRfid = "",

                                    ProductCode = i.ProductCode,
                                    ProductName = i.Product.ProductName,

                                    PieceQuantity = (int)Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    BarQuantity = (int)Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),
                                    OperatePieceQuantity = (int)Math.Floor(i.RealQuantity / i.Product.UnitList.Unit01.Count),
                                    OperateBarQuantity = (int)Math.Floor((i.RealQuantity % i.Product.UnitList.Unit01.Count) / i.Product.UnitList.Unit02.Count),

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
                result.BillDetails = billDetails.OrderBy(b=>b.StorageName).ToArray();
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "调用服务器服务查询订单细表失败，详情：" + e.Message;
            }
        }

        public void Apply(BillDetail[] billDetail, Result result)
        {
            throw new NotImplementedException();
        }

        public void Cancel(BillDetail[] billDetail, Result result)
        {
            throw new NotImplementedException();
        }

        public void Execute(BillDetail[] billDetail, Result result)
        {
            throw new NotImplementedException();
        }
    }
}
