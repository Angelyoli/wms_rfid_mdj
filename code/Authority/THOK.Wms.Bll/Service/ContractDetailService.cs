using System;
using System.Linq;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Data;

namespace THOK.Wms.Bll.Service
{
    public class ContractDetailService : ServiceBase<ContractDetail>, IContractDetailService
    {
        [Dependency]
        public IContractDetailRepository ContractDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(ContractDetail contractDetail, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var c = new ContractDetail();
            if (c != null)
            {
                try
                {
                    c.ID = Guid.NewGuid();
                    c.ContractCode = contractDetail.ContractCode;
                    c.BrandCode = contractDetail.BrandCode;
                    c.Quantity = contractDetail.Quantity;
                    c.Price = contractDetail.Price;
                    c.Amount = contractDetail.Amount;
                    c.TaxAmount = contractDetail.TaxAmount;

                    ContractDetailRepository.Add(c);
                    ContractDetailRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.InnerException;
                }
            }
            else
            {
                strResult = "原因：找不到当前登陆用户！请重新登陆！";
            }
            return result;
        }
    }
}
