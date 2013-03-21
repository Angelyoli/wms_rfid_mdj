using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Service
{
    public class PositionService : ServiceBase<Position>, IPositionService
    {
        [Dependency]
        public IPositionRepository PositionRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
    }
}
