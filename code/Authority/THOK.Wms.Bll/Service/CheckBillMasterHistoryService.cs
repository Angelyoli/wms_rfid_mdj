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
    public class CheckBillMasterHistoryService : ServiceBase<CheckBillMaster>, ICheckBillMasterHistoryService
    {
        [Dependency]
        public ICheckBillMasterRepository CheckBillMasterRepository { get; set; }
        [Dependency]
        public ICheckBillMasterHistoryRepository CheckBillMasterHistoryRepository { get; set; }
        [Dependency]
        public ICheckBillDetailRepository CheckBillDetailRepository { get; set; }
        [Dependency]
        public ICheckBillDetailHistoryRepository CheckBillDetailHistoryRepository { get; set; }

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

            var checkBillMaster = CheckBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            var checkBillDetail = CheckBillDetailRepository.GetQueryable().Where(i => i.CheckBillMaster.BillDate <= datetime);

            if (checkBillMaster != null)
            {
                #region 主表移入历史表
                try
                {
                    foreach (var item in checkBillMaster.ToArray())
                    {
                        CheckBillMasterHistory history = new CheckBillMasterHistory();

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

                        CheckBillMasterHistoryRepository.Add(history);
                    }
                    CheckBillMasterHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    masterResult = e.InnerException.ToString();
                    result = false;
                }
                #endregion

                if (checkBillDetail != null)
                {
                    #region 细表移入历史表
                    try
                    {
                        foreach (var item in checkBillDetail.ToArray())
                        {
                            CheckBillDetailHistory history = new CheckBillDetailHistory();
                            history.BillNo = item.BillNo;
                            history.CellCode = item.CellCode;
                            history.StorageCode = item.StorageCode;
                            history.ProductCode = item.ProductCode;
                            history.UnitCode = item.UnitCode;
                            history.Quantity = item.Quantity;
                            history.RealProductCode = item.ProductCode;
                            history.RealUnitCode = item.RealUnitCode;
                            history.RealQuantity = item.Quantity;
                            history.Status = item.Status;

                            CheckBillDetailHistoryRepository.Add(history);
                        }
                        CheckBillDetailHistoryRepository.SaveChanges();
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
            if (checkBillMaster != null)
            {
                foreach (var item in checkBillMaster.ToList())
                {
                    try
                    {
                        Del(CheckBillDetailRepository, item.CheckBillDetails);
                        CheckBillMasterRepository.Delete(item);
                        CheckBillMasterRepository.SaveChanges();
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
