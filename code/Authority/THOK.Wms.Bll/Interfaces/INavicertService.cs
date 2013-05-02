using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface INavicertService : IService<Navicert>
    {
        bool Add(Navicert navicert, out string strResult);

        bool Save(Navicert navicert, out string strResult);
    }
}
