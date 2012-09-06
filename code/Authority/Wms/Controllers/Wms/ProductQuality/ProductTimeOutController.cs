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
        public IProductWarningService ProductWarningServer { get; set; }
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
        //public ActionResult Details(int page, int rows, string productCode)
        //{
        //    var productTimeOut = ProductWarningServer.GetTimeOut(page, rows, productCode);
        //    return Json(productTimeOut, "text", JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult GetProductDetails(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "ProductCode";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = ProductWarningServer.GetProductDetails(page, rows, QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

    }
}
