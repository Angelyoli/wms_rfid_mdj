using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface IBatchService:IService<Batch>
    {
        object GetDetails(int page, int rows, Batch batchInfo);
        bool Add(Batch bathInfo, out string strResult);
        bool Save(Batch bathInfoCode, out string strResult);
        bool Delete(string bathCode, out string strResult);
        System.Data.DataTable GetBathInfo(int page, int rows, Batch batchInfo);

    }
}
