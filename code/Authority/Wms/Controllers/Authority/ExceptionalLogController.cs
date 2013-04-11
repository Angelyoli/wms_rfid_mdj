using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;

namespace Authority.Controllers.Authority
{
    public class ExceptionalLogController : Controller
    {
        [Dependency]
        public IExceptionalLogService ExceptionalLogService { get; set; }

        //
        // GET: /ExceptionalLog/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult CreateExceptionalLog(string ModuleNam, string FunctionName, string ExceptionalType, string ExceptionalDescription, string State)
        {
            bool bResult = ExceptionalLogService.CreateExceptionLog(ModuleNam, FunctionName, ExceptionalType, ExceptionalDescription, State);
            return Json(bResult, JsonRequestBehavior.AllowGet);
        } 
    }
}
