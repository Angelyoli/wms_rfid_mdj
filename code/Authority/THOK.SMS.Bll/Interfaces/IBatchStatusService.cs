using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface IBatchStatusService : IService<BatchStatus>
    {
        object GetDetails(int page, int rows, BatchStatus BatchStatus);
        bool Add(BatchStatus BatchStatus, out string strResult);
        bool Save(BatchStatus BatchStatus, out string strResult);
        bool Delete(int BatchStatusId, out string strResult);
        System.Data.DataTable GetBatchStatus(int page, int rows, BatchStatus BatchStatus);
    }
}
