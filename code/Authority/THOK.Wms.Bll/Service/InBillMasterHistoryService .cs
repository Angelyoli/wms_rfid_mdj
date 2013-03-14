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
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillMasterHistoryRepository InBillMasterHistoryRepository { get; set; }
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }
        [Dependency]
        public IInBillDetailHistoryRepository InBillDetailHistoryRepository { get; set; }
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IInBillAllotHistoryRepository InBillAllotHistoryRepository { get; set; }

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

            var inBillMaster = InBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            var inBillDetail = InBillDetailRepository.GetQueryable().Where(i => i.InBillMaster.BillDate <= datetime);
            var inBillAllot = InBillAllotRepository.GetQueryable().Where(i => i.InBillMaster.BillDate <= datetime);

            if (inBillMaster != null)
            {
                var inBillMasterHistory = InBillMasterHistoryRepository.GetQueryable().Where(i => i.BillDate <= datetime);

                #region 主表移入历史表
                try
                {
                    foreach (var item in inBillMaster.ToArray())
                    {
                        InBillMasterHistory history = new InBillMasterHistory();
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
                        history.TargetCellCode = item.TargetCellCode;

                        InBillMasterHistoryRepository.Add(history);
                    }
                    InBillMasterHistoryRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    masterResult = "原因：" + e.InnerException;
                    result = false;
                }
                #endregion

                if (inBillDetail != null)
                {
                    var inBillDetailHistory = InBillDetailHistoryRepository.GetQueryable().Where(i => i.InBillMasterHistory.BillDate <= datetime);

                    #region 细表移入历史表
                    try
                    {
                        foreach (var item2 in inBillDetail.ToArray())
                        {
                            InBillDetailHistory history2 = new InBillDetailHistory();
                            history2.BillNo = item2.BillNo;
                            history2.ProductCode = item2.ProductCode;
                            history2.UnitCode = item2.UnitCode;
                            history2.Price = item2.Price;
                            history2.BillQuantity = item2.BillQuantity;
                            history2.AllotQuantity = item2.AllotQuantity;
                            history2.RealQuantity = item2.RealQuantity;
                            history2.Description = item2.Description;

                            InBillDetailHistoryRepository.Add(history2);
                        }
                        InBillDetailHistoryRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        detailResult = "原因：" + e.InnerException;
                        result = false;
                    }
                    #endregion

                    if (inBillAllot != null)
                    {
                        //var inBillAllotHistory = InBillAllotHistoryRepository.GetQueryable().Where(i => i.InBillMasterHistory.BillDate <= datetime);

                        #region 分配表移入历史表
                        foreach (var itemDetailHistory in inBillDetailHistory.ToArray())
                        {
                            try
                            {
                                foreach (var item3 in inBillAllot.ToArray())
                                {
                                    InBillAllotHistory history3 = new InBillAllotHistory();
                                    history3.BillNo = item3.BillNo;
                                    history3.ProductCode = item3.ProductCode;
                                    history3.InBillDetailId = itemDetailHistory.ID;
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

                                    InBillAllotHistoryRepository.Add(history3);
                                }
                                InBillAllotHistoryRepository.SaveChanges();
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
            if (result == true)
            {
                #region 删除主细分配表
                if (inBillMaster != null)
                {
                    foreach (var item in inBillMaster.ToList())
                    {
                        try
                        {
                            Del(InBillAllotRepository, item.InBillAllots);
                            Del(InBillDetailRepository, item.InBillDetails);
                            InBillMasterRepository.Delete(item);
                            InBillMasterRepository.SaveChanges();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            deleteResult = "删除失败，原因：" + ex.Message;
                            result = false;
                        }
                    }
                }
                else
                {
                    deleteResult = "删除失败！未找到当前需要删除的数据！";
                    result = false;
                }
                #endregion
            }
            else
            {
                deleteResult = "删除失败";
            }
            return result;
        }

        #region Test
        public bool Add2(DateTime datetime, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var inBillMaster = InBillMasterRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            var inBillDetail = InBillDetailRepository.GetQueryable().Where(i => i.InBillMaster.BillDate <= datetime);

            try
            {
                using (var scope = new System.Transactions.TransactionScope())
                {
                    Console.WriteLine("start");
                    var sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    foreach (var item in inBillMaster.ToArray())
                    {
                        if (inBillDetail.Any())
                        {
                            InBillMasterHistory history = new InBillMasterHistory();
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
                            history.TargetCellCode = item.TargetCellCode;
                            InBillMasterHistoryRepository.Add(history);
                            inBillDetail.AsParallel().ForAll(i =>
                            {
                                InBillDetailHistory history2 = new InBillDetailHistory();
                                history2.BillNo = i.BillNo;
                                history2.ProductCode = i.ProductCode;
                                history2.UnitCode = i.UnitCode;
                                history2.Price = i.Price;
                                history2.BillQuantity = i.BillQuantity;
                                history2.AllotQuantity = i.AllotQuantity;
                                history2.RealQuantity = i.RealQuantity;
                                history2.Description = i.Description;
                                lock (history.InBillDetailHistorys)
                                {
                                    history.InBillDetailHistorys.Add(history2);
                                }
                            });
                            result = true;
                        }
                    }
                    sw.Stop();
                    Console.WriteLine(sw.ElapsedMilliseconds);

                    sw.Restart();
                    InBillMasterHistoryRepository.SaveChanges();
                    sw.Stop();
                    Console.WriteLine(sw.ElapsedMilliseconds);
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                strResult = e.Message;
            }
            return result;
        }
        #endregion
    }
}
