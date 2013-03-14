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
        [Dependency]
        public IOutBillAllotHistoryRepository OutBillAllotHistoryRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(DateTime datetime, out string masterResult, out string detailResult, out string allotResult, out string deleteResult)
        {
            bool result = false;
            masterResult = string.Empty;
            detailResult = string.Empty;
            allotResult = string.Empty;
            deleteResult = string.Empty;

            var outBillMaster = OutBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            var outBillDetail = OutBillDetailRepository.GetQueryable().Where(i => i.OutBillMaster.BillDate <= datetime);
            var outBillAllot = OutBillAllotRepository.GetQueryable().Where(i => i.OutBillMaster.BillDate <= datetime);

            if (outBillMaster != null)
            {
                var outBillMasterHistory = OutBillMasterHistoryRepository.GetQueryable().Where(i => i.BillDate <= datetime);

                #region 主表移入历史表
                try
                {
                    foreach (var item in outBillMaster.ToArray())
                    {
                        OutBillMasterHistory history = new OutBillMasterHistory();

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
                    }
                    OutBillMasterHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    masterResult = e.InnerException.ToString();
                    result = false;
                }
                #endregion

                if (outBillDetail != null)
                {
                    var outBillDetailHistory = OutBillDetailHistoryRepository.GetQueryable().Where(i => i.OutBillMasterHistory.BillDate == datetime);

                    #region 细表移入历史表
                    try
                    {
                        foreach (var item in outBillDetail.ToArray())
                        {
                            OutBillDetailHistory history = new OutBillDetailHistory();

                            history.BillNo = item.BillNo;
                            history.ProductCode = item.ProductCode;
                            history.UnitCode = item.UnitCode;
                            history.Price = item.Price;
                            history.BillQuantity = item.BillQuantity;
                            history.AllotQuantity = item.AllotQuantity;
                            history.RealQuantity = item.RealQuantity;
                            history.Description = item.Description;

                            OutBillDetailHistoryRepository.Add(history);
                            OutBillDetailHistoryRepository.SaveChanges();
                            result = true;
                        }
                    }
                    catch (Exception e)
                    {
                        detailResult = e.Message;
                    }
                    #endregion

                    if (outBillAllot != null)
                    {
                        #region 分配表移入历史表
                        foreach (var itemDetailHistory in outBillDetailHistory.ToArray())
                        {
                            try
                            {
                                foreach (var item3 in outBillAllot.ToArray())
                                {
                                    OutBillAllotHistory history3 = new OutBillAllotHistory();
                                    history3.BillNo = item3.BillNo;
                                    history3.ProductCode = item3.ProductCode;
                                    history3.OutBillDetailId = itemDetailHistory.ID;
                                    history3.CellCode = item3.CellCode;
                                    history3.StorageCode = item3.StorageCode;
                                    history3.UnitCode = item3.UnitCode;
                                    history3.AllotQuantity = item3.AllotQuantity;
                                    history3.RealQuantity = item3.RealQuantity;
                                    history3.OperatePersonID = item3.OperatePersonID;
                                    history3.Operator = item3.Operator;
                                    history3.StartTime = item3.StartTime;
                                    history3.FinishTime = item3.FinishTime;
                                    history3.Status = item3.Status;

                                    OutBillAllotHistoryRepository.Add(history3);
                                }
                                OutBillAllotHistoryRepository.SaveChanges();
                                result = true;
                            }
                            catch (Exception e)
                            {
                                //throw e.InnerException;
                                allotResult = "原因：" + e.InnerException;
                                result = false;
                            }
                        }
                        #endregion
                    }
                }
            }
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
                        OutBillMasterRepository.SaveChanges();
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
