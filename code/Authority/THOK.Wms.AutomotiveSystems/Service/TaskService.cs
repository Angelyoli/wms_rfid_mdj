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
                            var inBillMaster = InBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "4" || i.Status == "5")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "1" })
                                .ToArray();
                            billMasters.Concat(inBillMaster);
                            break;
                        case "2"://出库单
                            var outBillMaster = OutBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "4" || i.Status == "5")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "2" })
                                .ToArray();
                            billMasters.Concat(outBillMaster);
                            break;
                        case "3"://移库单
                            var moveBillMaster = MoveBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "2" || i.Status == "3")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "3" })
                                .ToArray();
                            billMasters.Concat(moveBillMaster);
                            break;
                        case "4"://盘点单
                            var checkBillMaster = CheckBillMasterRepository.GetQueryable()
                                .Where(i => i.Status == "2" || i.Status == "3")
                                .Select(i => new BillMaster() { BillNo = i.BillNo, BillType = "4" })
                                .ToArray();
                            billMasters.Concat(checkBillMaster);
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

        public void GetBillDetail(BillMaster[] billMaster, Result result)
        {
            throw new NotImplementedException();
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
