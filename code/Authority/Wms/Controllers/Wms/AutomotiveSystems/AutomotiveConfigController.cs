using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;

namespace Wms.Controllers.Wms.AutomotiveSystems
{
    public class AutomotiveConfigController : Controller
    {
        //
        // GET: /ParameterConfig/

        [Dependency]
        public IAutomotiveConfigService AutomotiveConfigService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.ModuleID = moduleID;
            return View();
        }
        //GO: /AutomotiveConfig/GetAutomotiveConfig/
        public ActionResult GetAutomotiveConfig()
        {
            var result = AutomotiveConfigService.GetAutomotiveConfig();
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
