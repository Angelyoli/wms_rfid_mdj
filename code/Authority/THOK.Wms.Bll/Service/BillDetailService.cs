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

        public bool Add(BillDetail billDetail, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var b = new BillDetail();

            if (b != null)
            {
                try
                {
                    b.ID = Guid.NewGuid();
                    b.MasterID = billDetail.MasterID;
                    b.PieceCigarCode = billDetail.PieceCigarCode;
                    b.BoxCigarCode = billDetail.BoxCigarCode;
                    b.BillQuantity = billDetail.BillQuantity;
                    b.FixedQuantity = billDetail.FixedQuantity;
                    b.RealQuantity = billDetail.RealQuantity;

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
            return result;
        }
        public bool Save(BillDetail billDetail, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var b = BillDetailRepository.GetQueryable().FirstOrDefault(c => c.BillMaster.UUID == billDetail.BillMaster.UUID);
            if (b != null)
            {
                try
                {
                    b.PieceCigarCode = billDetail.PieceCigarCode;
                    b.BoxCigarCode = billDetail.BoxCigarCode;
                    b.BillQuantity = billDetail.BillQuantity;
                    b.FixedQuantity = billDetail.FixedQuantity;
                    b.RealQuantity = billDetail.RealQuantity;

                    BillDetailRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            return result;
        }
    }
}
