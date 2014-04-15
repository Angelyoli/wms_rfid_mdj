using System;
using THOK.SMS.DbModel;
using THOK.SMS.Dal.Interfaces;
using THOK.SMS.Bll.Interfaces;
using System.Linq;
using THOK.Wms.SignalR.Common;
using Microsoft.Practices.Unity;
using THOK.Common.Entity;

namespace THOK.SMS.Bll.Servive
{
    public class BatchService : ServiceBase<Batch>, IBatchService
    {
        [Dependency]
        public IBatchRepository BatchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
    }
}
