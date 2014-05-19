using System;
using THOK.SMS.DbModel;
using THOK.SMS.Dal.Interfaces;
using THOK.SMS.Bll.Interfaces;
using System.Linq;
using THOK.Wms.SignalR.Common;
using Microsoft.Practices.Unity;
using THOK.Common.Entity;
using System.Data;

namespace THOK.SMS.Bll.Service
{
    public class SortOrderAllotDetailService : ServiceBase<SortOrderAllotDetail>, ISortOrderAllotDetailService
    {
        [Dependency]
        public ISortOrderAllotDetailRepository SortOrderAllotDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public object GetDetails(int page, int rows, string orderMasterCode)
        {
            IQueryable<SortOrderAllotDetail> sortOrderAllotDetailsQuery = SortOrderAllotDetailRepository.GetQueryable();
            var sortOrderAllotDetails = sortOrderAllotDetailsQuery.Where(s => s.OrderMasterCode == orderMasterCode);
            int total = sortOrderAllotDetails.Count();
            var sortOrderAllotDetailsArray = sortOrderAllotDetails.OrderBy(s=>s.OrderDetailCode).Skip((page - 1) * rows).Take(rows)
                .Select(s => new
            {
                s.OrderDetailCode,
                s.OrderMasterCode,
                s.ProductCode,
                s.ProductName,
                s.ChannelCode,
                s.Quantity
            }).ToArray();
            return new { total, rows = sortOrderAllotDetailsArray };
        }

        public DataTable GetDetailsByOrderMasterCode(string orderMasterCode)
        {
            IQueryable<SortOrderAllotDetail> sortOrderAllotDetailsQuery = SortOrderAllotDetailRepository.GetQueryable();
            var sortOrderAllotDetails = sortOrderAllotDetailsQuery.Where(s => s.OrderMasterCode == orderMasterCode);
            var sortOrderAllotDetailsArray = sortOrderAllotDetails.OrderBy(s => s.OrderDetailCode)
                .Select(s => new
                {
                    s.OrderDetailCode,
                    s.OrderMasterCode,
                    s.ProductCode,
                    s.ProductName,
                    s.ChannelCode,
                    s.Quantity
                }).ToArray();
            DataTable dt = new DataTable();
            dt.Columns.Add("主单代码", typeof(string));
            dt.Columns.Add("订单日期", typeof(string));
            dt.Columns.Add("批次号", typeof(string));
            dt.Columns.Add("烟包包号", typeof(string));
            dt.Columns.Add("订单编码", typeof(string));
            dt.Columns.Add("客户顺序", typeof(string));
            dt.Columns.Add("配送顺序", typeof(string));
            dt.Columns.Add("烟包数量", typeof(string));
            dt.Columns.Add("开始时间", typeof(string));
            dt.Columns.Add("完成时间", typeof(string));
            dt.Columns.Add("任务状态", typeof(string));
            foreach (var item in sortOrderAllotDetailsArray)
            {
                dt.Rows.Add("", "", "",
                    item.OrderDetailCode,
                    item.OrderMasterCode,
                    item.ProductCode,
                    item.ProductName,
                    item.ChannelCode,
                    item.Quantity, "", "");
            }
            return dt;
        }
    }
}
