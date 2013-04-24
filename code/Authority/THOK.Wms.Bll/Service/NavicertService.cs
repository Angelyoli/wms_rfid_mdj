﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class NavicertService : ServiceBase<Navicert>, INavicertService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(Navicert navicert, out string strResult)
        {
            strResult = string.Empty;
            return true;
        }
    }
}
