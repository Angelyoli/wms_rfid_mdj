using System;
using THOK.SMS.DbModel;
using THOK.SMS.Dal.Interfaces;
using THOK.SMS.Bll.Interfaces;
using System.Linq;
using THOK.Wms.SignalR.Common;
using Microsoft.Practices.Unity;
using THOK.Common.Entity;

using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.SMS.Bll.Service
{
    public class BatchService : ServiceBase<Batch>, IBatchService
    {
        [Dependency]
        public IBatchRepository BatchRepository { get; set; }

        [Dependency]
        public IUserRepository UserRepository { get; set; }

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
                    statusStr = "初始化";
                    break;
                case "02":
                    statusStr = "已下载";
                    break;
                case "03":
                    statusStr = "已优化";
                    break;
                case "04":
                    statusStr = "已上传";
                    break;
                case "05":
                    statusStr = "已结单";
                    break;
            }
            return statusStr;
        }

        //判断优化进度
        public string WhatOptimizeSchedule(string optimizeschedule)
        {
            string optimizescheduleStr = "";
            switch (optimizeschedule)
            {
                case "1":
                    optimizescheduleStr = "分拣线优化";
                    break;
                case "2":
                    optimizescheduleStr = "分拣线路优化";
                    break;
                case "3":
                    optimizescheduleStr = "烟道优化";
                    break;
                case "4":
                    optimizescheduleStr = "";
                    break;
                case "5":
                    optimizescheduleStr = "";
                    break;
            }
            return optimizescheduleStr;
        }


        public object GetDetails(int page, int rows, string BatchNo, string operateDate)
        {

            IQueryable<Batch> bathquery = BatchRepository.GetQueryable();
            IQueryable<User> userquery = UserRepository.GetQueryable();

            var batchs = bathquery.Join(userquery, a => a.OperatePersonId, u => u.UserID, (a, u) => new
            {
                a.BatchId,
                a.BatchName,
                a.BatchNo,
                a.Description,
                a.OrderDate,
                a.OperatePersonId,
                OperatePersonName = u.UserName,
                a.OptimizeSchedule,
                a.OperateDate,
                a.ProjectBatchNo,
                a.Status,
                a.VerifyPersonId

            })
            
            //.GroupJoin(userquery, a => a.VerifyPersonId, uu => uu.UserID, (a, uu) => new
            //{
            //    a.BatchId,
            //    a.BatchName,
            //    a.BatchNo,
            //    a.Description,
            //    a.OrderDate,
            //    a.OperatePersonId,
            //    a.OperatePersonName,
            //    a.OptimizeSchedule,
            //    a.OperateDate,
            //    a.ProjectBatchNo,
            //    a.State,
            //    a.VerifyPersonId,
            //    VerifyPersonName=uu

            //});




            .Join(userquery, a => a.VerifyPersonId, uu => uu.UserID, (a, uu) => new
            {
                a.BatchId,
                a.BatchName,
                a.BatchNo,
                a.Description,
                a.OrderDate,
                a.OperatePersonId,
                a.OperatePersonName,
                a.OptimizeSchedule,
                a.OperateDate,
                a.ProjectBatchNo,
                a.Status,
                a.VerifyPersonId,
                VerifyPersonName = uu.UserName


            });
            //.Where(a => a.BatchName.Contains(BatchName));

            if (BatchNo != "")
            {
                int batchNo = 0;
                int.TryParse(BatchNo, out batchNo);
                if (batchNo > 0)
                {
                    batchs = batchs.Where(a => a.BatchNo == batchNo);
                }
            }

            if (operateDate != string.Empty && operateDate != null)
            {
                DateTime opdate = Convert.ToDateTime(operateDate);
                batchs = batchs.Where(a => a.OperateDate >= opdate);
            }



            var batch = batchs.OrderByDescending(a => a.BatchId).ToArray()//.AsEnumerable()
                 .Select(a =>
                 new
                 {
                     a.BatchId,
                     a.BatchName,
                     a.BatchNo,
                     a.Description,
                     OrderDate = a.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                     a.ProjectBatchNo,
                     State = WhatStatus(a.Status),
                     a.VerifyPersonId,
                     OperateDate = a.OperateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                     OptimizeSchedule = WhatOptimizeSchedule(a.OptimizeSchedule.ToString()),
                     a.OperatePersonName,
                     a.OperatePersonId,
                     VerifyPersonName = a.VerifyPersonName

                 });

            int total = batch.Count();
            var batchsRow = batch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = batch.ToArray() };
        }



        public bool Add(Batch batchInfo, string userName, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var al = UserRepository.GetQueryable().FirstOrDefault(a => a.UserName == userName);
            if (al != null)
            {

                Batch batchs = new Batch();
                try
                {
                    batchs.BatchName = batchInfo.BatchName;
                    batchs.BatchNo = batchInfo.BatchNo;
                    batchs.Description = batchInfo.Description;
                    batchs.OperateDate = batchInfo.OperateDate;
                    batchs.OptimizeSchedule = 0;
                    batchs.OrderDate = batchInfo.OrderDate;
                    batchs.ProjectBatchNo = 0;
                    batchs.Status = "01";
                    batchs.OperatePersonId = al.UserID;
                    batchs.VerifyPersonId = al.UserID;

                    BatchRepository.Add(batchs);
                    BatchRepository.SaveChanges();

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

        public bool Save(Batch batchInfo, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var batchs = BatchRepository.GetQueryable().FirstOrDefault(a => a.BatchId == batchInfo.BatchId);
            if (batchs != null)
            {
                batchs.BatchName = batchInfo.BatchName;
                batchs.BatchNo = batchInfo.BatchNo;
                batchs.Description = batchInfo.Description;
                batchs.OperateDate = batchInfo.OperateDate;
                batchs.OptimizeSchedule = batchInfo.OptimizeSchedule;
                batchs.OrderDate = batchInfo.OrderDate;
                batchs.ProjectBatchNo = batchInfo.ProjectBatchNo;
                batchs.Status = batchInfo.Status;
                batchs.OperatePersonId = batchInfo.OperatePersonId;
                batchInfo.VerifyPersonId = batchInfo.VerifyPersonId;

                BatchRepository.SaveChanges();
                result = true;
            }
            else
            {

                strResult = "原因:找不到相应数据";
            }
            return result;
        }

        public bool Delete(int batchId, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var batchInfo = BatchRepository.GetQueryable().FirstOrDefault(a => a.BatchId.Equals(batchId));
            if (batchInfo != null)
            {

                BatchRepository.Delete(batchInfo);
                BatchRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因:没有找到相应数据";

            }
            return result;
        }
    }
}
