using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;

namespace Wms.Controllers.Wms.AutomotiveSystems
{
    public class SystemConfigController : Controller
    {
        //
        // GET: /SystemConfig/

        [Dependency]
        public ISystemConfig SystemConfig { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.ModuleID = moduleID;
            return View();
        }
        //GO: /SystemConfig/GetSystemConfig/
        public ActionResult GetSystemConfig()
        {
            var result = SystemConfig.GetSystemConfig();
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
