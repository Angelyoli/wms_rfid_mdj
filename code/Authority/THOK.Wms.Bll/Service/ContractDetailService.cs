using System;
using System.Linq;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Data;

namespace THOK.Wms.Bll.Service
{
    public class ContractDetailService : ServiceBase<ContractDetail>, IContractDetailService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(ContractDetail contractDetail, out string strResult)
        {
            bool result=false;
            strResult = string.Empty;
            return result;
        }
    }
}
