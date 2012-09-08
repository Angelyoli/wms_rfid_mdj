using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using THOK.WebUtil;

namespace Wms.Controllers.Wms.ProductQuality
{
    public class QuantityLimitsController : Controller
    {
        //
        // GET: /QuantityLimits/
        [Dependency]
        public IStorageService StorageServer { get; set; }

        [Dependency]
        public IProductWarningService ProductWarningServer { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string productCode = collection["ProductCode"] ?? "";
            decimal minLimited = decimal.Parse(collection["MinLimited"]);
            decimal maxLimited = decimal.Parse(collection["MaxLimited"]);
            string unitType = collection["UnitType"] ?? "";
            var productWarn = ProductWarningServer.GetQtyLimitsDetail(page, rows, productCode, minLimited, maxLimited, unitType);
            return Json(productWarn, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
