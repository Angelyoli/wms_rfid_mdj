using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Service
{
    public class DayEndService : ServiceBase<DayEnd>, IDayEndService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
    }
}
