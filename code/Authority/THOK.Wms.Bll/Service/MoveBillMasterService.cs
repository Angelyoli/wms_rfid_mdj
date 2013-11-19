﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR;
using THOK.Wms.SignalR.Common;
using System.Transactions;

namespace THOK.Wms.Bll.Service
{
    public class MoveBillMasterService : ServiceBase<MoveBillMaster>, IMoveBillMasterService
    {
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IBillTypeRepository BillTypeRepository { get; set; }
        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public IMoveBillCreater MoveBillCreater { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public string resultStr = "";//错误信息字符串

        #region //移库主单增、删、改、查、生成单号方法
        /// <summary>
        /// 判断处理状态
        /// </summary>
        /// <param name="status">数据库查询出来的状态值</param>
        /// <returns></returns>
        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "1":
                    statusStr = "已录入";
                    break;
                case "2":
                    statusStr = "已审核";
                    break;
                case "3":
                    statusStr = "执行中";
                    break;
                case "4":
                    statusStr = "已结单";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string BillNo, string WareHouseCode, string beginDate, string endDate, string OperatePersonCode, string CheckPersonCode, string Status, string IsActive)
        {
            IQueryable<MoveBillMaster> moveBillMasterQuery = MoveBillMasterRepository.GetQueryable();
            var moveBillMaster = moveBillMasterQuery.Where(i => i.BillNo.Contains(BillNo)
                    && i.Status != "4"
                    && i.WarehouseCode.Contains(WareHouseCode)
                    && i.OperatePerson.EmployeeCode.Contains(OperatePersonCode)
                    && i.Status.Contains(Status))
                .OrderByDescending(t => t.BillDate)
                .OrderByDescending(t => t.BillNo)
                .Select(i => i);

            if (!beginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(beginDate);
                moveBillMaster = moveBillMaster.Where(i => i.BillDate >= begin);
            }

            if (!endDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(endDate).AddDays(1);
                moveBillMaster = moveBillMaster.Where(i => i.BillDate <= end);
            }

            if (!CheckPersonCode.Equals(string.Empty))
            {
                moveBillMaster = moveBillMaster.Where(i => i.VerifyPerson.EmployeeCode == CheckPersonCode);
            }
            int total = moveBillMaster.Count();
            moveBillMaster = moveBillMaster.Skip((page - 1) * rows).Take(rows);

            var temp = moveBillMaster.ToArray().AsEnumerable().Select(i => new
            {
                i.BillNo,
                BillDate = i.BillDate.ToString("yyyy-MM-dd HH:mm:ss"),
                i.OperatePersonID,
                i.WarehouseCode,
                i.BillTypeCode,
                i.BillType.BillTypeName,
                i.Warehouse.WarehouseName,
                OperatePersonCode = i.OperatePerson.EmployeeCode,
                OperatePersonName = i.OperatePerson.EmployeeName,
                VerifyPersonID = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeCode,
                VerifyPersonName = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeName,
                SumQuantity = i.MoveBillDetails.Sum(s => s.RealQuantity / s.Product.Unit.Count),
                VerifyDate = (i.VerifyDate == null ? "" : ((DateTime)i.VerifyDate).ToString("yyyy-MM-dd HH:mm:ss")),
                Status = WhatStatus(i.Status),
                IsActive = i.IsActive == "1" ? "可用" : "不可用",
                Description = i.Description,
                UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
            return new { total, rows = temp.ToArray() };
        }

        public bool Add(MoveBillMaster moveBillMaster, string userName, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var mbm = new MoveBillMaster();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (employee != null)
            {
                try
                {
                    mbm.BillNo = moveBillMaster.BillNo;
                    mbm.BillDate = moveBillMaster.BillDate;
                    mbm.BillTypeCode = moveBillMaster.BillTypeCode;
                    mbm.WarehouseCode = moveBillMaster.WarehouseCode;
                    mbm.OperatePersonID = employee.ID;
                    mbm.Status = "1";
                    mbm.VerifyPersonID = moveBillMaster.VerifyPersonID;
                    mbm.VerifyDate = moveBillMaster.VerifyDate;
                    mbm.Description = moveBillMaster.Description;
                    //mbm.IsActive = moveBillMaster.IsActive;
                    mbm.IsActive = "1";
                    mbm.UpdateTime = DateTime.Now;
                    mbm.Origin = "1";

                    MoveBillMasterRepository.Add(mbm);
                    MoveBillMasterRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "新增失败，原因：" + ex.Message;
                }
            }
            else
            {
                strResult = "找不到当前登陆用户！请重新登陆！";
            }
            return result;
        }

        public bool Delete(string BillNo, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var mbm = MoveBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            if (LockBillMaster(BillNo))
            {
                if (mbm != null)
                {
                    try
                    {
                        MoveBillCreater.DeleteMoveBillDetail(mbm);
                        Del(MoveBillDetailRepository, mbm.MoveBillDetails);
                        MoveBillMasterRepository.Delete(mbm);
                        MoveBillMasterRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "删除失败，原因：" + ex.Message;
                    }
                }
                else
                {
                    strResult = "删除失败！未找到当前需要删除的数据！";
                    result = false;
                }
            }
            else
            {
                strResult = resultStr;
                result = false;
            }
            return result;
        }

        public bool Save(MoveBillMaster moveBillMaster, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var mbm = MoveBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == moveBillMaster.BillNo && i.Status == "1");
            if (LockBillMaster(moveBillMaster.BillNo))
            {
                if (mbm != null)
                {
                    try
                    {
                        mbm.BillDate = moveBillMaster.BillDate;
                        mbm.BillTypeCode = moveBillMaster.BillTypeCode;
                        mbm.WarehouseCode = moveBillMaster.WarehouseCode;
                        mbm.OperatePersonID = moveBillMaster.OperatePersonID;
                        mbm.Status = "1";
                        mbm.VerifyPersonID = moveBillMaster.VerifyPersonID;
                        mbm.VerifyDate = moveBillMaster.VerifyDate;
                        mbm.Description = moveBillMaster.Description;
                        //mbm.IsActive = moveBillMaster.IsActive;
                        mbm.IsActive = "1";
                        mbm.Origin = "1";
                        mbm.UpdateTime = DateTime.Now;

                        mbm.LockTag = string.Empty;
                        MoveBillMasterRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "保存失败，原因：" + ex.Message;
                    }
                }
                else
                {
                    strResult = "保存失败，未找到该条数据！";
                    result = false;
                }
            }
            else
            {
                strResult = resultStr;
                result = false;
            }
            return result;
        }

        public object GenMoveBillNo(string userName)
        {
            IQueryable<MoveBillMaster> moveBillMasterQuery = MoveBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            string billNo = "";
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            var moveBillMaster = moveBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).ToArray().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            if (moveBillMaster.Count() == 0)
            {
                billNo = System.DateTime.Now.ToString("yyMMdd") + "0001" + "MO";
            }
            else
            {
                string billNoStr = moveBillMaster.Last(b => b.Contains(sysTime));
                int i = Convert.ToInt32(billNoStr.ToString().Substring(6, 4));
                i++;
                string newcode = i.ToString();
                for (int j = 0; j < 4 - i.ToString().Length; j++)
                {
                    newcode = "0" + newcode;
                }
                billNo = System.DateTime.Now.ToString("yyMMdd") + newcode + "MO";
            }
            var findBillInfo = new
            {
                BillNo = billNo,
                billNoDate = DateTime.Now.ToString("yyyy-MM-dd"),
                employeeID = employee == null ? "" : employee.ID.ToString(),
                employeeCode = employee == null ? "" : employee.EmployeeCode.ToString(),
                employeeName = employee == null ? "" : employee.EmployeeName.ToString()
            };
            return findBillInfo;
        }

        #endregion

        #region//移库主单审核、反审、结单方法
        public bool Audit(string BillNo, string userName, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var mbm = MoveBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (LockBillMaster(BillNo))
            {
                if (mbm != null)
                {
                    try
                    {
                        mbm.Status = "2";
                        mbm.VerifyDate = DateTime.Now;
                        mbm.UpdateTime = DateTime.Now;
                        mbm.VerifyPersonID = employee.ID;
                        mbm.LockTag = string.Empty;
                        MoveBillMasterRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "审核失败，原因：" + ex.Message;
                    }
                }
                else
                {
                    strResult = "审核失败，未找到该条数据！";
                    result = false;
                }
            }
            else
            {
                strResult = resultStr;
                result = false;
            }
            return result;
        }

        public bool AntiTrial(string BillNo, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var mbm = MoveBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "2");
            if (LockBillMaster(BillNo))
            {
                if (mbm != null)
                {
                    try
                    {
                        mbm.Status = "1";
                        mbm.VerifyDate = null;
                        mbm.UpdateTime = DateTime.Now;
                        mbm.VerifyPersonID = null;
                        mbm.LockTag = string.Empty;
                        MoveBillMasterRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "反审失败，原因：" + ex.Message;
                    }
                }
                else
                {
                    strResult = "反审失败，未找到该条数据！";
                    result = false;
                }
            }
            else
            {
                strResult = resultStr;
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 移库结单
        /// </summary>
        /// <param name="BillNo">单据号</param>
        /// <param name="strResult">提示信息</param>
        /// <returns></returns>
        public bool Settle(string BillNo, out string strResult)
        {
            bool result = false;
            strResult = string.Empty;
            var mbm = MoveBillMasterRepository.GetQueryable().FirstOrDefault(m => m.BillNo == BillNo);
            if (mbm != null && mbm.Status == "3")
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        //结单移库单,修改冻结量
                        var moveDetail = MoveBillDetailRepository.GetQueryable()
                                                                     .Where(m => m.BillNo == BillNo
                                                                         && m.Status != "2");
                        var sourceStorages = moveDetail.Select(m => m.OutStorage).ToArray();
                        var targetStorages = moveDetail.Select(m => m.InStorage).ToArray();
                        if (!Locker.Lock(sourceStorages) || !Locker.Lock(targetStorages))
                        {
                            strResult = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                            return false;
                        }
                        moveDetail.AsParallel().ForAll(
                            (Action<MoveBillDetail>)delegate(MoveBillDetail m)
                            {
                                if (m.InStorage.ProductCode == m.ProductCode
                                    && m.OutStorage.ProductCode == m.ProductCode
                                    && m.InStorage.InFrozenQuantity >= m.RealQuantity
                                    && m.OutStorage.OutFrozenQuantity >= m.RealQuantity)
                                {
                                    m.InStorage.InFrozenQuantity -= m.RealQuantity;
                                    m.InStorage.ProductCode = null;
                                    m.InStorage.StorageSequence = 0;
                                    m.OutStorage.OutFrozenQuantity -= m.RealQuantity;
                                    m.InStorage.LockTag = string.Empty;
                                    m.OutStorage.LockTag = string.Empty;
                                }
                                else
                                {
                                    throw new Exception("储位的卷烟或入库冻结量与当前分配不符，信息可能被异常修改，不能结单！");
                                }
                            }
                        );
                        MoveBillDetailRepository.SaveChanges();
                        mbm.Status = "4";
                        mbm.UpdateTime = DateTime.Now;
                        MoveBillMasterRepository.SaveChanges();

                        scope.Complete();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        strResult = "移库单结单出错！原因：" + e.Message;
                        return false;
                    }
                }
            }
            return result;
        }
        #endregion

        #region//移库主单获取单据类型和仓库信息方法
        public object GetBillTypeDetail(string BillClass, string IsActive)
        {
            throw new NotImplementedException();
        }

        public object GetWareHouseDetail(string IsActive)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 对移库单进行加锁
        /// </summary>
        /// <param name="BillNo">移库单号</param>
        /// <returns></returns>
        public bool LockBillMaster(string BillNo)
        {
            bool result = false;
            var pbm = MoveBillMasterRepository.GetQueryable().FirstOrDefault(p => p.BillNo == BillNo);
            if (pbm != null)
            {
                if (string.IsNullOrEmpty(pbm.LockTag))
                {
                    pbm.LockTag = BillNo;
                    MoveBillMasterRepository.SaveChanges();
                    result = true;
                }
                else
                {
                    resultStr = "当前订单其他人正在操作，请稍候重试！";
                    result = false;
                }
            }
            else
            {
                resultStr = "当前单据的状态不是已录入状态或者该单据已被删除无法编辑，请刷新页面！";
                result = false;
            }
            return result;
        }


        public bool GeneratePalletTag(string billNo, ref string strResult)
        {
            bool result = false;
            var mbms = MoveBillMasterRepository.GetQueryable().Where(p => billNo.Contains(p.BillNo));
            IEnumerable<MoveBillDetail> tempDetails = new MoveBillDetail[] { };
            foreach (var mbm in mbms)
            {
                tempDetails = tempDetails.Concat(mbm.MoveBillDetails);
            }

            decimal i = 0;
            int j = 1;

            var details = tempDetails.Where(d => (d.Product.AbcTypeCode == "2" || d.Product.AbcTypeCode == "3")
                                                    && d.RealQuantity != (d.InCell.MaxQuantity * d.Unit.Count)
                                                    && d.InCell.Area.AreaType != "3")
                                    .OrderBy(d => d.OutCellCode)
                                    .ToArray();

            foreach (var detail in details)
            {
                if (detail.PalletTag == null)
                {
                    if (detail.RealQuantity + i < 300000)
                    {
                        detail.PalletTag = j;
                        i += detail.RealQuantity;
                    }
                    else
                    {
                        detail.PalletTag = ++j;
                        i = detail.RealQuantity;
                    }
                }
                else
                {
                    strResult = "当前订单已组盘！";
                    return true;
                }
            }

            MoveBillMasterRepository.SaveChanges();
            result = true;
            return result;
        }
    }
}
