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
    public class ProfitLossBillMasterHistoryService : ServiceBase<ProfitLossBillMaster>, IProfitLossBillMasterHistoryService
    {
        [Dependency]
        public IProfitLossBillMasterRepository ProfitLossBillMasterRepository { get; set; }
        [Dependency]
        public IProfitLossBillMasterHistoryRepository ProfitLossBillMasterHistoryRepository { get; set; }
        [Dependency]
        public IProfitLossBillDetailRepository ProfitLossBillDetailRepository { get; set; }
        [Dependency]
        public IProfitLossBillDetailHistoryRepository ProfitLossBillDetailHistoryRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(DateTime datetime, out string masterResult, out string detailResult, out string deleteResult)
        {
            bool result = false;
            masterResult = string.Empty;
            detailResult = string.Empty;
            deleteResult = string.Empty;

            var profitLossBillMaster = ProfitLossBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            var profitLossBillDetail = ProfitLossBillDetailRepository.GetQueryable().Where(i => i.ProfitLossBillMaster.BillDate <= datetime);

            if (profitLossBillMaster != null)
            {
                #region 主表移入历史表
                try
                {
                    foreach (var item in profitLossBillMaster.ToArray())
                    {
                        ProfitLossBillMasterHistory history = new ProfitLossBillMasterHistory();
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

                        ProfitLossBillMasterHistoryRepository.Add(history);
                    }
                    ProfitLossBillMasterHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    masterResult = e.InnerException.ToString();
                    result = false;
                }
                #endregion

                if (profitLossBillDetail != null)
                {
                    #region 细表移入历史表
                    try
                    {
                        foreach (var item in profitLossBillDetail.ToArray())
                        {
                            ProfitLossBillDetailHistory history = new ProfitLossBillDetailHistory();
                            history.BillNo = item.BillNo;
                            history.CellCode = item.CellCode;
                            history.StorageCode = item.StorageCode;
                            history.ProductCode = item.ProductCode;
                            history.UnitCode = item.UnitCode;
                            history.Price = item.Price;
                            history.Quantity = item.Quantity;
                            history.Description = item.Description;

                            ProfitLossBillDetailHistoryRepository.Add(history);
                        }
                        ProfitLossBillDetailHistoryRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        detailResult = e.InnerException.ToString(); ;
                    }
                    #endregion
                }
            }
            #region 删除主细分配表
            if (profitLossBillMaster != null)
            {
                foreach (var item in profitLossBillMaster.ToList())
                {
                    try
                    {
                        Del(ProfitLossBillDetailRepository, item.ProfitLossBillDetails);
                        ProfitLossBillMasterRepository.Delete(item);
                        ProfitLossBillMasterRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        deleteResult = "删除失败，原因：" + ex.Message;
                    }
                }
            }
            else
            {
                deleteResult = "删除失败！未找到当前需要删除的数据！";
            }
            #endregion

            return result;
        }
    }
}
