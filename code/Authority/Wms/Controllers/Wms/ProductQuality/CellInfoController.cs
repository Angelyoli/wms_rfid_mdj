using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Wms.Controllers.Wms.ProductQuality
{
    public class CellInfoController : Controller
    {
        //
        // GET: /CellInfo/

        [Dependency]
        public IProductWarningService ProductWarningService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Detail()
        {
            var productWarn = ProductWarningService.GetCell();
            return Json(productWarn, "text", JsonRequestBehavior.AllowGet);
        }

    }
}
