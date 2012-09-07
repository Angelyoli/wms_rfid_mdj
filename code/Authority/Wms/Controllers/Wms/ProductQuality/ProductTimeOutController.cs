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
    public class ProductTimeOutController : Controller
    {
        [Dependency]
        public IProductWarningService ProductWarningService { get; set; }
        //
        // GET: /ProductTimeOut/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        [HttpPost]
        public ActionResult GetProductDetails(int page, int rows, FormCollection collection)
        {
            string productCode = collection["ProductCode"] ?? "";
            decimal assemblyTime=180;
            if (collection["AssemblyTime"] != null && collection["AssemblyTime"] != "")
            {
               assemblyTime= decimal.Parse(collection["AssemblyTime"]);
            }
            var product = ProductWarningService.GetProductDetails(page, rows, productCode, assemblyTime);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

    }
}
