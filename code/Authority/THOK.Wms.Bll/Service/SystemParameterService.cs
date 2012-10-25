using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class SystemParameterService : ServiceBase<SystemParameter>, ISystemParameter
    {
        [Dependency]
        public ISystemParameterRepository SystemParameterRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetSystemParameter()
        {
            var query = SystemParameterRepository.GetQueryable()
                                    .Select(s => s);
            return query;
        }
        public bool SetSystemParameter(SystemParameter sp)
        {
            var query = SystemParameterRepository.GetQueryable().FirstOrDefault(a => a.Id == sp.Id);
            query.Id = sp.Id;

            return true;
        }
    }
}
