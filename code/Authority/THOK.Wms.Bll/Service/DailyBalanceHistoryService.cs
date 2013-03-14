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

        public bool Add(DateTime datetime,out string strResult)
        {
            strResult=string.Empty;
            bool result = false;
            var dailyBalancelHistory = DailyBalanceRepository.GetQueryable().Where(i => i.SettleDate <= datetime);
            
            #region 日结表移入历史表
            foreach (var item in dailyBalancelHistory.ToArray())
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

                    DailyBalanceHistoryRepository.Add(history);
                    DailyBalanceHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    strResult = "提示" + e.Message;
                }
            } 
            #endregion

            #region 删除记录
            //if (dailyBalancelHistory != null)
            //{
            //    foreach (var item in dailyBalancelHistory.ToList())
            //    {
            //        try
            //        {
            //            DailyBalanceRepository.Delete(item);
            //            DailyBalanceRepository.SaveChanges();
            //            result = true;
            //        }
            //        catch (Exception ex)
            //        {
            //            strResult = "删除失败，原因：" + ex.Message;
            //        }
            //    }
            //}
            //else
            //{
            //    strResult = "删除失败！未找到当前需要删除的数据！";
            //}
            #endregion

            return result;
        }
    }
}
