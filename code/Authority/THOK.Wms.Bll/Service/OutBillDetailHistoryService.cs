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
    public class OutBillDetailHistoryService : IOutBillDetailHistoryService
    {
        [Dependency]
        public IOutBillDetailRepository outBillDetailRepository { get; set; }
        [Dependency]
        public IOutBillDetailHistoryRepository outBillDetailHistoryRepository { get; set; }

        public bool Add(DateTime datetime)
        {
            bool result = false;
            var outBillDetail = outBillDetailRepository.GetQueryable().Where(i => i.OutBillMaster.BillDate <= datetime);
            foreach (var item in outBillDetail.ToArray())
            {
                OutBillDetailHistory history = new OutBillDetailHistory();
                try
                {
                    history.BillNo = item.BillNo;
                    history.ProductCode = item.ProductCode;
                    history.UnitCode = item.UnitCode;
                    history.Price = item.Price;
                    history.BillQuantity = item.BillQuantity;
                    history.AllotQuantity = item.AllotQuantity;
                    history.RealQuantity = item.RealQuantity;
                    history.Description = item.Description;

                    outBillDetailHistoryRepository.Add(history);
                    outBillDetailHistoryRepository.SaveChanges();
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
