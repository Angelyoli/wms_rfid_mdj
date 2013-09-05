using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.DbModel;

namespace THOK.WCS.Bll.Interfaces
{
    public interface IAlarmInfoService : IService<AlarmInfo>
    {
        object GetDetails(int page, int rows, AlarmInfo alarmInfo);

        bool Add(AlarmInfo alarmInfo,out string strResult);

        bool Save(AlarmInfo alarmInfoCode, out string strResult);

        bool Delete(string code, out string strResult);

        System.Data.DataTable GetAlarmInfo(int page, int rows, AlarmInfo alarmInfo);
    }
}
