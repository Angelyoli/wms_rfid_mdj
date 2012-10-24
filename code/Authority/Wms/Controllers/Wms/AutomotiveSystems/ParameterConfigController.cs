using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wms.Controllers.Wms.AutomotiveSystems
{
    public class ParameterConfigController : Controller
    {
        //
        // GET: /ParameterConfig/

        public ActionResult Index(string moduleID)
        {
            ViewBag.ModuleID = moduleID;
            return View();
        }

    }
}
