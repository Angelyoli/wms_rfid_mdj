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
    public class MoveBillMasterHistoryService : ServiceBase<MoveBillMaster>, IMoveBillMasterHistoryService
    {
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillMasterHistoryRepository MoveBillMasterHistoryRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public IMoveBillDetailHistoryRepository MoveBillDetailHistoryRepository { get; set; }

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

            var moveBillMaster = MoveBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            var moveBillDetail = MoveBillDetailRepository.GetQueryable().Where(i => i.MoveBillMaster.BillDate <= datetime);

            if (moveBillMaster != null)
            {
                #region 主表移入历史表
                try
                {
                    foreach (var item in moveBillMaster.ToArray())
                    {
                        MoveBillMasterHistory history = new MoveBillMasterHistory();
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
                        MoveBillMasterHistoryRepository.Add(history);
                    }
                    MoveBillMasterHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    masterResult = e.InnerException.ToString();
                    result = false;
                }
                #endregion

                if (moveBillDetail != null)
                {
                    #region 细表移入历史表
                    try
                    {
                        foreach (var item in moveBillDetail.ToArray())
                        {
                            MoveBillDetailHistory history = new MoveBillDetailHistory();
                            history.BillNo = item.BillNo;
                            history.ProductCode = item.ProductCode;
                            history.OutCellCode = item.OutCellCode;
                            history.OutStorageCode = item.OutStorageCode;
                            history.InCellCode = item.InCellCode;
                            history.InStorageCode = item.InStorageCode;
                            history.UnitCode = item.UnitCode;
                            history.RealQuantity = item.RealQuantity;
                            history.OperatePersonID = item.OperatePersonID;
                            history.StartTime = item.StartTime;
                            history.FinishTime = item.FinishTime;
                            history.Status = item.Status;
                            history.Operator = item.Operator;
                            history.CanRealOperate = item.CanRealOperate;
                            history.PalletTag = item.PalletTag;
                            MoveBillDetailHistoryRepository.Add(history);
                        }
                        MoveBillDetailHistoryRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        detailResult = e.InnerException.ToString(); ;
                    }
                    #endregion
                }
                if (result == true)
                {
                    #region 删除主细分配表
                    try
                    {
                        foreach (var item in moveBillMaster.ToList())
                        {
                            Del(MoveBillDetailRepository, item.MoveBillDetails);
                            MoveBillMasterRepository.Delete(item);
                            MoveBillMasterRepository.SaveChanges();
                            result = true;
                        }
                    }
                    catch (Exception e)
                    {
                        deleteResult = e.InnerException.ToString();
                    }
                    #endregion
                }
            }
            return result;
        }
    }
}
