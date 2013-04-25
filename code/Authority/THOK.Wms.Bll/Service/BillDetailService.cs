using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class BillDetailService : ServiceBase<BillDetail>, IBillDetailService
    {
        [Dependency]
        public IBillDetailRepository BillDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(BillDetail billDetail,out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var billDetails = BillDetailRepository.GetQueryable().FirstOrDefault(d => d.MasterID == billDetail.BillMaster.ID);
            var b = new BillDetail();
            if (billDetails == null)
            {
                if (b != null)
                {
                    try
                    {
                        b.ID = Guid.NewGuid();
                        b.MasterID = billDetails.MasterID;
                        b.PieceCigarCode = billDetails.PieceCigarCode;
                        b.BoxCigarCode = billDetails.BoxCigarCode;
                        b.BillQuantity = billDetails.BillQuantity;
                        b.FixedQuantity = billDetails.FixedQuantity;
                        b.RealQuantity = billDetails.RealQuantity;

                        BillDetailRepository.Add(b);
                        BillDetailRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "原因：" + ex.ToString();
                        result = false;
                    }
                }
                else
                {
                    strResult = "原因：找不到当前登陆用户！请重新登陆！";
                    result = false;
                }
            }
            else
            {
                strResult = "原因：该编号已存在！";
                result = false;
            }
            return result;
        }
    }
}
