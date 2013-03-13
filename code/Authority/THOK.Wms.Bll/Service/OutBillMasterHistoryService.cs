using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Service
{
    public class OutBillMasterHistoryService : IOutBillMasterHistoryService
    {
        [Dependency]
        public IOutBillMasterRepository outBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillMasterHistoryRepository outBillMasterHistoryRepository { get; set; }

        public bool Add(DateTime datetime)
        {
            bool result = false;
            var outBillMaster = outBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            foreach (var item in outBillMaster.ToArray())
            {
                OutBillMasterHistory history = new OutBillMasterHistory();
                try
                {
                    history.BillNo = item.BillNo;
                    history.BillDate = item.BillDate;
                    history.BillTypeCode = item.BillTypeCode;
                    history.WarehouseCode = item.WarehouseCode;
                    history.OperatePersonID = item.OperatePersonID;
                    history.Status = item.Status;
                    history.VerifyPersonID = item.VerifyPersonID;
                    history.VerifyDate = item.VerifyDate;
                    history.Description = item.Description;
                    history.IsActive = item.IsActive;
                    history.UpdateTime = item.UpdateTime;
                    history.Origin = item.Origin;
                    history.TargetCellCode = item.TargetCellCode;

                    outBillMasterHistoryRepository.Add(history);
                    outBillMasterHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
