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
    public class BatchStatusService : ServiceBase<BatchStatus>, IBatchStatusService
    {
        [Dependency]
        public IBatchStatusRepository BatchStatusRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public object GetDetails(int page, int rows, BatchStatus batchStatus)
        {

            IQueryable<BatchStatus> bathquery = BatchStatusRepository.GetQueryable();

            var batchStatuss = bathquery.Where(a => a.BatchStatusId.Equals(batchStatus.BatchStatusId)).OrderBy(a => a.BatchStatusId);

            int total = batchStatuss.Count();
            var batchStatussRow = batchStatuss.Skip((page - 1) * rows).Take(rows);
            var batchStatussSelect = batchStatussRow.ToArray().Select(a => new
            {

                a.BatchStatusId,
                a.BatchId,
                a.batch.BatchName,
                a.SortingLineCode,
                a.State
            });

            return new { total, rows = batchStatussSelect.ToArray() };
        }



        public bool Add(BatchStatus batchStatus, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var al = BatchStatusRepository.GetQueryable().FirstOrDefault(a => a.BatchStatusId == batchStatus.BatchStatusId);
            if (al == null)
            {

                BatchStatus batchStatuss = new BatchStatus();
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



        public bool Save(BatchStatus batchStatus, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var batchStatuss = BatchStatusRepository.GetQueryable().FirstOrDefault(a => a.BatchStatusId == batchStatus.BatchStatusId);
            if (batchStatuss != null)
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

        public bool Delete(int batchStatusCode, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var batchStatus = BatchStatusRepository.GetQueryable().FirstOrDefault(a => a.BatchStatusId.Equals(batchStatusCode));
            if (batchStatus != null)
            {

                BatchStatusRepository.Delete(batchStatus);
                BatchStatusRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因:没有找到相应数据";

            }
            return result;
        }


        public System.Data.DataTable GetBatchStatus(int page, int rows, BatchStatus BatchStatusInfo)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            //
            //   未更新。。
            //
            return dt;
        }
    }
}
