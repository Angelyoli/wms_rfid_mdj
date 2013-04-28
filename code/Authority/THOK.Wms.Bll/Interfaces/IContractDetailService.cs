using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IContractDetailService : IService<ContractDetail>
    {
        bool Add(ContractDetail contractDetail, out string strResult);

        bool Save(ContractDetail contractDetail, out string strResult);
    }
}
