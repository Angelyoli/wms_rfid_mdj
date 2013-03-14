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
    public class OutBillMasterHistoryService : ServiceBase<OutBillMaster>, IOutBillMasterHistoryService
    {
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillMasterHistoryRepository OutBillMasterHistoryRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
        [Dependency]
        public IOutBillDetailHistoryRepository OutBillDetailHistoryRepository { get; set; }
        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(DateTime datetime, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var outBillMaster = OutBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            var outBillDetail = OutBillDetailRepository.GetQueryable().Where(i => i.OutBillMaster.BillDate <= datetime);

            #region 主表移入历史表
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

                    OutBillMasterHistoryRepository.Add(history);                    
                    result = true;
                }
                catch (Exception e)
                {
                    strResult = e.Message;
                    result = false;
                }
            }
            
            #endregion

            #region 细表移入历史表
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

                    OutBillDetailHistoryRepository.Add(history);
                    
                    result = true;
                }
                catch (Exception e)
                {
                    strResult = e.Message;
                }
            }
            #endregion

            #region 删除主细分配表
            if (outBillMaster != null)
            {
                foreach (var item in outBillMaster.ToList())
                {
                    try
                    {
                        Del(OutBillAllotRepository, item.OutBillAllots);
                        Del(OutBillDetailRepository, item.OutBillDetails);
                        OutBillMasterRepository.Delete(item);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "删除失败，原因：" + ex.Message;
                    }
                }
            }
            else
            {
                strResult = "删除失败！未找到当前需要删除的数据！";
            }
            #endregion

            try
            {
                OutBillMasterHistoryRepository.SaveChanges();
            }
            catch (Exception e)
            {
                strResult = e.Message;
            }

            return result;
        }
    }
}
