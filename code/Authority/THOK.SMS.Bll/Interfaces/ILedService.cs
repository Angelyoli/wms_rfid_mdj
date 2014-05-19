using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface ILedService : IService<Led>
    {
        object GetDetails(int page, int rows, string Status, string LedName, string LedGroupCode, string LedType,string LedCode);
      

        object GetLedGroupCode(int page, int rows, string QueryString, string Value);

        bool Add(Led ledInfo,  out string strResult);
        bool Save(Led ledInfo, out string strResult);
        bool Delete(string ledCode, out string strResult);
    }
}
