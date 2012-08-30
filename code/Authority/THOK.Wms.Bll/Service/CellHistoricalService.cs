using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class CellHistoricalService : ServiceBase<Storage>, ICellHistoricalService
    {

        #region ICellHistoricalService 成员

        public object GetCellDetails(int page, int rows, string type, string id)
        {
            throw new NotImplementedException();
        }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #endregion
    }
}
