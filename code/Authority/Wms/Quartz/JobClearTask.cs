using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using THOK.Security;
using THOK.Wms.Bll.Interfaces;
using THOK.WCS.Bll.Interfaces;

namespace Wms.Quartz
{
    public class JobClearTask : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string errorInfo = string.Empty;
            ServiceFactory ServiceFactory = new ServiceFactory();
            ServiceFactory.GetService<ITaskService>().ClearTask(out errorInfo);
        }
    }
}