using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.DbModel;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class BillMasterService : ServiceBase<BillMaster>, IBillMasterService
    {
        [Dependency]
        public IBillMasterRepository BillMasterRepository { get; set; }
        [Dependency]
        public IBillDetailRepository BillDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(BillMaster billMaster, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            //var billMasters = BillMasterRepository.GetQueryable().FirstOrDefault(c => c.UUID == billMaster.UUID);
            var b = new BillMaster();
            //if (billMasters == null)
            //{
                if (b != null)
                {
                    try
                    {
                        b.ID = billMaster.ID;
                        b.UUID = billMaster.UUID;
                        b.BillType = billMaster.BillType;
                        b.BillDate = billMaster.BillDate;
                        b.MakerName = billMaster.MakerName;
                        b.OperateDate = billMaster.OperateDate;
                        b.CigaretteType = billMaster.CigaretteType;
                        b.BillCompanyCode = billMaster.BillCompanyCode;
                        b.SupplierCode = billMaster.SupplierCode;
                        b.SupplierType = billMaster.SupplierType;
                        b.State = billMaster.State;
                        
                        BillMasterRepository.Add(b);
                        BillMasterRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "原因：" + ex.ToString(); 
                        result = false;
                    }
                //}
                //else
                //{
                //    strResult = "原因：找不到当前登陆用户！请重新登陆！"; 
                //    result = false;
                //}
            }
            else
            {
                strResult = "原因：该编号已存在！"; 
                result = false;
            }
            return result;
        }
        public bool Save(BillMaster billMaster, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var b = BillMasterRepository.GetQueryable().FirstOrDefault(c => c.UUID == billMaster.UUID);
            if (b != null)
            {
                try
                {
                    b.UUID = billMaster.UUID;
                    b.BillType = billMaster.BillType;
                    b.BillDate = billMaster.BillDate;
                    b.MakerName = billMaster.MakerName;
                    b.OperateDate = billMaster.OperateDate;
                    b.CigaretteType = billMaster.CigaretteType;
                    b.BillCompanyCode = billMaster.BillCompanyCode;
                    b.SupplierCode = billMaster.SupplierCode;
                    b.SupplierType = billMaster.SupplierType;
                    b.State = billMaster.State;

                    BillMasterRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            return result;
        }
        public bool Delete(string uuid, string strResult)
        {
            bool result = false;
            strResult = string.Empty;

            var billMaster = BillMasterRepository.GetQueryable().Where(i => i.UUID == uuid);
            var billDetail = BillDetailRepository.GetQueryable().Where(i => i.BillMaster.UUID == uuid);
            
            if (result == true)
            {
                #region 删除主细分配表
                try
                {
                    foreach (var item in billMaster.ToList())
                    {
                        //Del(BillDetailRepository, item.InBillAllots);
                        //Del(InBillDetailRepository, item.InBillDetails);
                        //InBillMasterRepository.Delete(item);
                        //result = true;
                    }
                }
                catch (Exception e)
                {
                    strResult = "删除操作时：" + e.InnerException.ToString();
                    result = false;
                }
                BillMasterRepository.SaveChanges();
                #endregion
            }
            return result;
        }
    }
}
