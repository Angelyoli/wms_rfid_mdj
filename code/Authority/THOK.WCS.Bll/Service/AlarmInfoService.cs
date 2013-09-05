using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.DbModel;
using THOK.WCS.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WCS.Dal.Interfaces;

namespace THOK.WCS.Bll.Service
{
    public class AlarmInfoService : ServiceBase<AlarmInfo>, IAlarmInfoService
    {
        [Dependency]
        public IAlarmInfoRepository AlarmInfoRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, AlarmInfo alarmInfo)
        {
            IQueryable<AlarmInfo> alarmInfoQuery = AlarmInfoRepository.GetQueryable();
            var alarmInfos = alarmInfoQuery
                .Where(a =>a.AlarmCode.Contains(alarmInfo.AlarmCode) && a.Description.Contains(alarmInfo.Description))
                .OrderBy(a => a.AlarmCode);
            int total = alarmInfos.Count();
            var alarmInfoRow = alarmInfos.Skip((page - 1) * rows).Take(rows);
            var alarmInfoSelect = alarmInfoRow.ToArray().Select(a => new
            {
               
                a.AlarmCode,
                a.Description
            });
            return new { total, rows = alarmInfoSelect.ToArray() };
        }

        public bool Add(AlarmInfo alarmInfo,out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var al = AlarmInfoRepository.GetQueryable().FirstOrDefault(a => a.AlarmCode == alarmInfo.AlarmCode);
            if (al == null)
            {
                AlarmInfo alarm = new AlarmInfo();
                try
                {
                    alarm.AlarmCode = alarmInfo.AlarmCode;
                    alarm.Description = alarmInfo.Description;
                    AlarmInfoRepository.Add(alarm);
                    AlarmInfoRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    strResult = "原因：" + e.Message;
                }
            }
            else
            {
                strResult = "原因：报警编码已存在";
            }
            return result;
        }

        public bool Save(AlarmInfo alarmInfo, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var alarmInfos = AlarmInfoRepository.GetQueryable().FirstOrDefault(s => s.AlarmCode == alarmInfo.AlarmCode);
            if (alarmInfos != null)
            {
                alarmInfos.Description = alarmInfo.Description;
                AlarmInfoRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因：找不到相应数据";
            }
            return result;
        }

        public bool Delete(string code, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var alarmInfo = AlarmInfoRepository.GetQueryable().FirstOrDefault(a => a.AlarmCode == code);
            if (alarmInfo != null)
            {
                AlarmInfoRepository.Delete(alarmInfo);
                AlarmInfoRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因：没有找到相应数据";
            }
            return result;
        }

        public System.Data.DataTable GetAlarmInfo(int page, int rows, AlarmInfo alarmInfo)
        {
            IQueryable<AlarmInfo> alarmInfoQuery = AlarmInfoRepository.GetQueryable();

            var alarmInfoDetail = alarmInfoQuery.Where(a =>
                a.AlarmCode.Contains(alarmInfo.AlarmCode)
                && a.Description.Contains(alarmInfo.Description))
                .OrderBy(a => a.AlarmCode);
            var alarmInfo_Detail = alarmInfoDetail.ToArray().Select(a => new
            {
                a.AlarmCode,
                a.Description
            });

            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("报警编码", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            foreach (var s in alarmInfo_Detail)
            {
                dt.Rows.Add
                    (
                        s.AlarmCode,
                        s.Description
                    );
            }
            return dt;
        }
    }
}
