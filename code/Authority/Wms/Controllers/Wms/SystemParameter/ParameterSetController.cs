using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;

namespace Wms.Controllers.Wms.SystemParameter
{
    public class ParameterSetController : Controller
    {
        //
        // GET: /ParamterSet/

        public ActionResult Index(string moduleID)
        {
            ViewBag.ModuleID = moduleID;
            return View();
        }
    }
}
