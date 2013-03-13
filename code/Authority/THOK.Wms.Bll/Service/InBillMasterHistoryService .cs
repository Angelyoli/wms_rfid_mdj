using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Wms.SignalR.Common;

namespace THOK.Wms.Bll.Service
{
    public class InBillMasterHistoryService : ServiceBase<InBillMaster>, IInBillMasterHistoryService
    {
        [Dependency]
        public IInBillMasterRepository inBillMasterRepository { get; set; }
        [Dependency]
        public IInBillAllotRepository inBillAllotRepository { get; set; }
        [Dependency]
        public IInBillMasterHistoryRepository inBillMasterHistoryRepository { get; set; }
        [Dependency]
        public IInBillDetailRepository inBillDetailRepository { get; set; }
        [Dependency]
        public IInBillDetailHistoryRepository inBillDetailHistoryRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(DateTime datetime, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            
            #region 主表
            var inBillMaster = inBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
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
                catch (Exception e)
                {
                    strResult = "提示" + e.Message;
                }
            } 
            #endregion

            #region 细表
            var inBillDetail = inBillDetailRepository.GetQueryable().Where(i => i.InBillMaster.BillDate <= datetime);
            foreach (var item2 in inBillDetail.ToArray())
            {
                InBillDetailHistory history2 = new InBillDetailHistory();
                try
                {
                    history2.BillNo = item2.BillNo;
                    history2.ProductCode = item2.ProductCode;
                    history2.UnitCode = item2.UnitCode;
                    history2.Price = item2.Price;
                    history2.BillQuantity = item2.BillQuantity;
                    history2.AllotQuantity = item2.AllotQuantity;
                    history2.RealQuantity = item2.RealQuantity;
                    history2.Description = item2.Description;

                    inBillDetailHistoryRepository.Add(history2);
                    inBillDetailHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            } 
            #endregion

            return result;
        }
        public bool DeleteMaster(DateTime datetime, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var inBillMaster = inBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);            

            if (inBillMaster != null)
            {
                foreach (var item in inBillMaster.ToList())
                {
                    try
                    {
                        Del(inBillAllotRepository, item.InBillAllots);
                        Del(inBillDetailRepository,item.InBillDetails);
                        inBillMasterRepository.Delete(item);
                        inBillMasterRepository.SaveChanges();
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
            return result;
        }
    }
}
