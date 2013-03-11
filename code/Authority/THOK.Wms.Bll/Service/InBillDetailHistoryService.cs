using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class InBillDetailHistoryService:IInBillDetailHistoryService
    {
        [Dependency]
        public IInBillDetailRepository inBillDetailRepository { get; set; }

        #region IInBillDetailHistoryService 成员

        public bool Add(DateTime datetime)
        {
            bool result = false;
            //var inBillDetail = inBillDetailRepository.GetQueryable().Where(i => i.BillDate <= datetime);
            //foreach (var item in inBillDetail.ToArray())
            //{
            //    InBillDetailHistory history = new InBillDetailHistory();
            //    try
            //    {
            //        history.BillNo = inBillDetail.BillNo;
            //        history.ProductCode = inBillDetail.ProductCode;
            //        history.UnitCode = inBillDetail.UnitCode;
            //        history.Price = inBillDetail.Price;
            //        history.BillQuantity = inBillDetail.BillQuantity * unit.Count;
            //        history.AllotQuantity = 0;
            //        history.RealQuantity = 0;
            //        history.Description = inBillDetail.Description;

            //        InBillDetailRepository.Add(history);
            //        InBillDetailRepository.SaveChanges();
            //        result = true;
            //    }
            //    catch (Exception)
            //    {
            //        result = false;
            //    }
            //}
            return result;
        }

        #endregion
    }
}
