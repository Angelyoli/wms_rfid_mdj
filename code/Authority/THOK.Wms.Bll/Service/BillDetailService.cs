using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class BillDetailService : ServiceBase<BillDetail>, IBillDetailService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(BillDetail billDetail,out string strResult)
        {
            bool result = false;
            strResult = string.Empty;
            return result;
        }
    }
}
