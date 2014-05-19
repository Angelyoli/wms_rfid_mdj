﻿using System;
using THOK.SMS.DbModel;
using THOK.SMS.Dal.Interfaces;
using THOK.SMS.Bll.Interfaces;
using System.Linq;
using THOK.Wms.SignalR.Common;
using Microsoft.Practices.Unity;
using THOK.Common.Entity;

namespace THOK.SMS.Bll.Service
{
    public class DeliverLineAllotService : ServiceBase<DeliverLineAllot>, IDeliverLineAllotService
    {
        [Dependency]
        public IDeliverLineAllotRepository DeliverLineAllotRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public object GetDetails(int page, int rows, DeliverLineAllot DeliverLineAllot)
        {
            //
            //   未实现。。
            //
            return DeliverLineAllot;
        }
    }
}