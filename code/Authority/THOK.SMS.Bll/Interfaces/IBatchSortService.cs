using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface IBatchSortService : IService<BatchSort>
    {
        //object GetDetails(int page, int rows, BatchSort BatchSort);


        object GetDetails(int page, int rows, string Status, string BatchNo, string BatchName, string OperateDate);

        object GetBatch(int page, int rows, string queryString, string value);

        bool Add(BatchSort BatchSort, out string strResult);
        bool Save(BatchSort BatchSort, out string strResult);
        bool Delete(int BatchSortId, out string strResult);
        System.Data.DataTable GetBatchSort(int page, int rows, BatchSort BatchSort);
    }
}
