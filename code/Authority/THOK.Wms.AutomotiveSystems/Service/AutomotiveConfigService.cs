using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;
using THOK.Wms.AutomotiveSystems.Models;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.AutomotiveSystems.Service
{
    public class AutomotiveConfigService : ServiceBase<AutomotiveConfig>, IAutomotiveConfigService
    {
        [Dependency]
        public IAutomotiveConfigRepository AutomotiveConfigRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetAutomotiveConfig()
        {
            var automotiveConfig = AutomotiveConfigRepository.GetQueryable()
                                    .Select(s => s);
            return automotiveConfig;
        }
    }
}
