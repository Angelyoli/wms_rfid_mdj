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
        public IDailyBalanceRepository DailyBalanceRepository { get; set; }
        [Dependency]
        public IDailyBalanceHistoryRepository DailyBalanceHistoryRepository { get; set; }

        public bool Add(DateTime datetime, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var dailyBalancel = DailyBalanceRepository.GetQueryable().Where(i => i.SettleDate <= datetime);

            if (dailyBalancel != null)
            {
                #region 日结表移入历史表
                try
                {
                    foreach (var item in dailyBalancel.ToArray())
                    {
                        DailyBalanceHistory history = new DailyBalanceHistory();
                        history.ID = item.ID;
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
                        DailyBalanceHistoryRepository.Add(history);
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    strResult = e.InnerException.ToString();
                }
                #endregion
            }
            #region 删除记录
            if (dailyBalancel != null)
            {
                foreach (var item in dailyBalancel.ToList())
                {
                    try
                    {
                        DailyBalanceRepository.Delete(item);
                        DailyBalanceRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        strResult = e.InnerException.ToString();
                    }
                }
                DailyBalanceHistoryRepository.SaveChanges();
            }
            else
            {
                strResult = "删除失败！未找到当前需要删除的数据！";
            }
            #endregion

            return result;
        }
    }
}
