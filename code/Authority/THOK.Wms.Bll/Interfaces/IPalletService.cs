using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPalletService : IService<Pallet>
    {
        string Add(Pallet palletAdd);
    }
}
