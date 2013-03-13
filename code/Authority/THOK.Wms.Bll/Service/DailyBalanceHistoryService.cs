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
    public class DailyBalanceHistoryService : IDailyBalanceHistoryService
    {
        [Dependency]
        public IDailyBalanceRepository inBillDetailRepository { get; set; }
        [Dependency]
        public IDailyBalanceHistoryRepository dailyBalanceHistoryRepository { get; set; }

        #region IDailyBalanceHistoryService 成员

        public bool Add(DateTime datetime)
        {
            bool result = false;
            var inBillDetail = inBillDetailRepository.GetQueryable().Where(i => i.SettleDate <= datetime);
            foreach (var item in inBillDetail.ToArray())
            {
                DailyBalanceHistory history = new DailyBalanceHistory();
                try
                {
                    history.SettleDate = item.SettleDate;
                    history.WarehouseCode = item.WarehouseCode;
                    history.ProductCode = item.ProductCode;
                    history.UnitCode = item.UnitCode;
                    history.Beginning = item.Beginning;
                    history.EntryAmount = item.EntryAmount;
                    history.DeliveryAmount = item.DeliveryAmount;
                    history.ProfitAmount = item.ProfitAmount;
                    history.LossAmount = item.LossAmount;
                    history.Ending = item.Ending;

                    dailyBalanceHistoryRepository.Add(history);
                    dailyBalanceHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        #endregion
    }
}
