using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface IBatchService:IService<Batch>
    {

        object GetDetails(int page, int rows, string BatchNo, string operateDate);
        //object GetDetails(int page, int rows, string BatchNo, string BatchName, string orderDate, string operateDate);


        //object GetDetails(int page, int rows, Batch batchInfo);

        bool Add(Batch batchInfo, string userName, out string strResult);
        bool Save(Batch batchInfo, out string strResult);
        bool Delete(int batchId, out string strResult);
      

    }
}
