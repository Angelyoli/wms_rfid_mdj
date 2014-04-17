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
    public class BatchService : ServiceBase<Batch>, IBatchService
    {
        [Dependency]
        public IBatchRepository BatchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public object GetDetails(int page, int rows, Batch bathInfo)
        {

            IQueryable<Batch> bathquery = BatchRepository.GetQueryable();

            var baths = bathquery.Where(a => a.BatchId.Equals(bathInfo.BatchId)).OrderBy(a => a.BatchId);

            int total = baths.Count();
            var bathsRow = baths.Skip((page - 1) * rows).Take(rows);
            var bathsSelect = bathsRow.ToArray().Select(a => new
            {

                a.BatchId,
                a.BatchName,
                a.BatchNo,
                a.Description
            });

            return new { total, rows = bathsSelect.ToArray() };
        }



        public bool Add(Batch bathInfo, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var al = BatchRepository.GetQueryable().FirstOrDefault(a => a.BatchId == bathInfo.BatchId);
            if (al == null)
            {

                Batch baths = new Batch();
                try
                {
                    baths.BatchId = bathInfo.BatchId;
                    baths.BatchName = bathInfo.BatchName;
                    baths.BatchNo = bathInfo.BatchNo;
                    baths.Description = bathInfo.Description;
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



        public bool Save(Batch bathInfo, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var baths = BatchRepository.GetQueryable().FirstOrDefault(a => a.BatchId == bathInfo.BatchId);
            if (baths != null)
            {

                baths.BatchName = bathInfo.BatchName;
                baths.BatchNo = bathInfo.BatchNo;
                baths.Description = bathInfo.Description;
                result = true;
            }
            else
            {

                strResult = "原因:找不到相应数据";
            }
            return result;
        }

        public bool Delete(string bathCode, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var bachInfo = BatchRepository.GetQueryable().FirstOrDefault(a => a.BatchId.Equals(bathCode));
            if (bachInfo != null)
            {

                BatchRepository.Delete(bachInfo);
                BatchRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因:没有找到相应数据";

            }
            return result;
        }

      
        public System.Data.DataTable GetBathInfo(int page, int rows, Batch batchInfo)
        {

            IQueryable<Batch> batchquery = BatchRepository.GetQueryable();
            var batchInfoDetail = batchquery.Where(a => a.BatchId.Equals(batchInfo.BatchId) &&
                a.Description.Contains(batchInfo.Description)).OrderBy(a => a.BatchId);
            var batchInfo_Detail = batchInfoDetail.ToArray().Select(a => new
            {

                a.BatchId,
                a.BatchName,
                a.BatchNo,
                a.Description
            });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("批次ID", typeof(int));
            dt.Columns.Add("批次名称", typeof(string));
            dt.Columns.Add("批次编号", typeof(int));
            dt.Columns.Add("批次描述", typeof(string));
            foreach (var s in batchInfo_Detail)
            {
                dt.Rows.Add(
                    s.BatchId,
                    s.BatchName,
                    s.BatchNo,
                    s.Description

                    );
            }
            return dt;
        }          
    }
}
