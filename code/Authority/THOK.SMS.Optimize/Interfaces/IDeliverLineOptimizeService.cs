using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Optimize.Interfaces
{
     public interface IDeliverLineOptimizeService
    {
         bool OptimizeAllot(int batchNo, out string strResult);
    }
}
