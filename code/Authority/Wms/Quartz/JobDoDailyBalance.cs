using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using THOK.Security;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Quartz
{
    public class JobDoDailyBalance : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string errorInfo = string.Empty;
            ServiceFactory ServiceFactory = new ServiceFactory();
            ServiceFactory.GetService<IDailyBalanceService>().DoDailyBalance(null, System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), ref errorInfo);
        }
    }
}