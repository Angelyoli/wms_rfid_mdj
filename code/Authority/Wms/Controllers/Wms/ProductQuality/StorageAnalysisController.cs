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
    public class StorageAnalysisController : Controller
    {
        [Dependency]
        public IProductWarningService ProductWarningService { get; set; }
        //
        // GET: /StorageAnalysis/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Details()
        {
            var productWarn = ProductWarningService.GetStorageByTime();
            return Json(productWarn, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
