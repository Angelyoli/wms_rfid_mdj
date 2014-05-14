using System;
using THOK.SMS.DbModel;
using THOK.SMS.Dal.Interfaces;
using THOK.SMS.Bll.Interfaces;
using System.Linq;
using THOK.Wms.SignalR.Common;
using Microsoft.Practices.Unity;
using THOK.Common.Entity;

namespace THOK.SMS.Bll.Service
{
    public class BatchSortService : ServiceBase<BatchSort>, IBatchSortService
    {
        [Dependency]
        public IBatchSortRepository BatchSortRepository { get; set; }

        [Dependency]
        public IBatchRepository BatchRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        //判断其状态
        public string WhatStatus(string state)
        {
            string statusStr = "";
            switch (state)
            {
                case "01":
                    statusStr = "未下载";
                    break;
                case "02":
                    statusStr = "已下载";
                    break;
                case "03":
                    statusStr = "已挂起";
                    break;
                case "04":
                    statusStr = "已结单";
                    break;
                case "05":
                    statusStr = "";
                    break;
            }
            return statusStr;
        }



        public string WhatState(string Status)
        {
            string optimizescheduleStr = "";
            switch (Status)
            {
                case "未下载":
                    optimizescheduleStr = "01";
                    break;
                case "已下载":
                    optimizescheduleStr = "02";
                    break;
                case "已挂起":
                    optimizescheduleStr = "03";
                    break;
                case "已结单":
                    optimizescheduleStr = "04";
                    break;
                case "":
                    optimizescheduleStr = "";
                    break;
            }
            return optimizescheduleStr;
        }


        public object GetDetails(int page, int rows, string Status, string BatchNo, string BatchName, string OperateDate)
        {

            IQueryable<BatchSort> batchsortquery = BatchSortRepository.GetQueryable();
            IQueryable<Batch> batchquery = BatchRepository.GetQueryable();

            //未实行关联    分拣线信息表【SortingLine】
            var batchsort = batchsortquery.Join(batchquery, a => a.BatchId, u => u.BatchId, (a, u) => new
            {
                a.BatchId,
                a.BatchSortId,
                BatchName=u.BatchName,
                BatchNo=u.BatchNo,
                OperateDate=u.OperateDate,

                a.SortingLineCode,
                a.Status

            });
               
            if (BatchNo != "")
            {
                int batchNo = 0;
                int.TryParse(BatchNo, out batchNo);
                if (batchNo > 0)
                {
                    batchsort = batchsort.Where(a => a.BatchNo == batchNo);
                }
            }

            if (OperateDate != string.Empty && OperateDate != null)
            {
                DateTime opdate = Convert.ToDateTime(OperateDate);
                batchsort = batchsort.Where(a => a.OperateDate >= opdate);
            }

            if (Status != string.Empty && Status != null)
            {              
                batchsort = batchsort.Where(a => a.Status==Status);
            }
            if (BatchName != string.Empty && BatchName != null)
            {
                batchsort = batchsort.Where(a => a.BatchName==BatchName);
            }

            var batch = batchsort.OrderByDescending(a => a.BatchSortId).ToArray()
                 .Select(a =>
                 new
                 {
                     a.BatchSortId,
                     a.BatchId,
                     a.BatchName,
                     a.BatchNo,

                     Status = WhatStatus(a.Status),
                     a.SortingLineCode,
                     OperateDate = a.OperateDate.ToString("yyyy-MM-dd HH:mm:ss")
               

                 });

            int total = batch.Count();
            var batchsRow = batch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = batch.ToArray() };
        }

        public object GetBatch(int page, int rows, string queryString, string value)
        {
            string batchno = "", batchname = "";

            if (queryString == "BatchNo")
            {
                batchno = value;
            }
            else
            {
                batchname = value;
            }
            IQueryable<Batch> batchQuery = BatchRepository.GetQueryable();
            var batchs = batchQuery.Where(e =>e.BatchName.Contains(batchname));
              
              
            if (batchno != "")
            {
                int batchNo = 0;
                int.TryParse(batchno, out batchNo);
                if (batchNo > 0)
                {
                    batchs = batchs.Where(a => a.BatchNo == batchNo);
                }
            }

            var batch = batchs.OrderByDescending(a => a.BatchId).OrderBy(e => e.BatchId).AsEnumerable().Select(a => new
            {
                a.BatchId,
                a.BatchName,
                a.BatchNo,  
                OperateDate = a.OperateDate.ToString("yyyy-MM-dd HH:mm:ss")           
            });

            int total = batch.Count();
            batch = batch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = batch.ToArray() };
        }


        public bool Add(BatchSort BatchSort, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var al = BatchSortRepository.GetQueryable().FirstOrDefault(a => a.BatchSortId == BatchSort.BatchSortId);
            if (al == null)
            {

                BatchSort BatchSorts = new BatchSort();
                try
                {
                    BatchSorts.BatchId = BatchSort.BatchId;
                    BatchSorts.SortingLineCode = "1"; // 未实现 给默认值
                    BatchSorts.Status = BatchSort.Status;
                    BatchSortRepository.Add(BatchSorts);
                    BatchSortRepository.SaveChanges();

                    result = true;
                }
                catch (Exception e)
                {

                    strResult = "原因:" + e.Message;
                }
            }
            else
            {
                strResult = "原因:批次分拣已存在";
            }
            return result;
        }



        public bool Save(BatchSort BatchSort, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var BatchSorts = BatchSortRepository.GetQueryable().FirstOrDefault(a => a.BatchSortId == BatchSort.BatchSortId);
            if (BatchSorts != null)
            {
                BatchSorts.BatchId=BatchSort.BatchId;
                BatchSorts.SortingLineCode=BatchSort.SortingLineCode;
                BatchSorts.Status = BatchSort.Status;// WhatState(BatchSort.Status);
                BatchSortRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因:找不到相应数据";
            }
            return result;
        }

        public bool Delete(int BatchSortId, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var BatchSort = BatchSortRepository.GetQueryable().FirstOrDefault(a => a.BatchSortId.Equals(BatchSortId));
            if (BatchSort != null)
            {

                BatchSortRepository.Delete(BatchSort);
                BatchSortRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因:没有找到相应数据";

            }
            return result;
        }


        public System.Data.DataTable GetBatchSort(int page, int rows, BatchSort BatchSortInfo)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            //
            //   未更新。。
            //
            return dt;
        }
    }
}
