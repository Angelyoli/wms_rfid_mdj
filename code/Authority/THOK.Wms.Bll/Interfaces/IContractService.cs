using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IContractService : IService<Contract>
    {
        bool Add(Contract contract,out string strResult);

        bool Save(Contract contract, out string strResult);
    }
}
