using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Service
{
    public class InBillMasterHistoryService : IInBillMasterHistoryService
    {
        [Dependency]
        public IInBillMasterRepository inBillMasterRepository { get; set; }
        [Dependency]
        public IInBillMasterHistoryRepository inBillMasterHistoryRepository { get; set; }

        public bool Add(DateTime datetime)
        {
            bool result = false;
            var inBillMaster = inBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime );
            foreach (var item in inBillMaster.ToArray())
            {
                InBillMasterHistory history = new InBillMasterHistory();
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
                    history.UpdateTime = DateTime.Now;
                    history.TargetCellCode = item.TargetCellCode;

                    inBillMasterHistoryRepository.Add(history);
                    inBillMasterHistoryRepository.SaveChanges();
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
