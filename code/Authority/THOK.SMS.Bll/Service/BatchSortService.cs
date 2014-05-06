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

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public object GetDetails(int page, int rows, BatchSort BatchSort)
        {

            IQueryable<BatchSort> bathquery = BatchSortRepository.GetQueryable();

            var BatchSorts = bathquery.Where(a => a.BatchSortId.Equals(BatchSort.BatchSortId)).OrderBy(a => a.BatchSortId);

            int total = BatchSorts.Count();
            var BatchSortsRow = BatchSorts.Skip((page - 1) * rows).Take(rows);
            var BatchSortsSelect = BatchSortsRow.ToArray().Select(a => new
            {

                a.BatchSortId,
                a.BatchId,
                a.batch.BatchName,
                a.SortingLineCode,
                a.State
            });

            return new { total, rows = BatchSortsSelect.ToArray() };
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
                    //
                    //   未更新。。
                    //
                    result = true;
                }
                catch (Exception e)
                {

                    strResult = "原因:" + e.Message;
                }
            }
            else
            {

                strResult = "原因:批次号已存在";
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
                //
                //   未更新。。
                //
                result = true;
            }
            else
            {

                strResult = "原因:找不到相应数据";
            }
            return result;
        }

        public bool Delete(int BatchSortCode, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var BatchSort = BatchSortRepository.GetQueryable().FirstOrDefault(a => a.BatchSortId.Equals(BatchSortCode));
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
