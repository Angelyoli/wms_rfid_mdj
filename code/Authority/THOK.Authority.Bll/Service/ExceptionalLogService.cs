using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class ExceptionalLogService : ServiceBase<ExceptionalLog>,IExceptionalLogService
    {
        [Dependency]
        public IExceptionalLogRepository ExceptionalLogRepository { get; set; }
        [Dependency]
        public IModuleRepository ModuleRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool CreateExceptionLog(string ModuleNam, string FunctionName, string ExceptionalType, string ExceptionalDescription, string State)
        {
            var moduleRepository = ModuleRepository.GetQueryable().Where(m => m.ModuleURL == ModuleNam).Select(m => new {modulname=m.ModuleName }).ToArray();
            var ExceptionalLogs = new ExceptionalLog()
            {
                ExceptionalLogID = Guid.NewGuid(),
                CatchTime = DateTime.Now.ToString(),
                ModuleName = moduleRepository[0].modulname,
                FunctionName = FunctionName,
                ExceptionalType = ExceptionalType,
                ExceptionalDescription = ExceptionalDescription,
                State = State,
            };
            ExceptionalLogRepository.Add(ExceptionalLogs);
            ExceptionalLogRepository.SaveChanges();
            return true;
        }
    }
}
