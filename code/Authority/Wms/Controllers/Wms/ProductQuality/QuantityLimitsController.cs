using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using THOK.Common.WebUtil;
using THOK.Security;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Wms.Controllers.Wms.ProductQuality
{
    [TokenAclAuthorize]
    public class QuantityLimitsController : Controller
    {
        //
        // GET: /QuantityLimits/
        [Dependency]
        public IStorageService StorageService { get; set; }

        [Dependency]
        public IProductWarningService ProductWarningService { get; set; }

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
            string unitCode = collection["UnitCode"] ?? "";
            decimal minLimited =100000;
            decimal maxLimited=100000;
            if (collection["MinLimited"] != null && collection["MinLimited"] != "")
            {
                minLimited = decimal.Parse(collection["MinLimited"]);
            }
            if (collection["MaxLimited"] != null && collection["MaxLimited"] != "")
            {
                maxLimited = decimal.Parse(collection["MaxLimited"]);
            }
            var productWarn = ProductWarningService.GetQtyLimitsDetail(page, rows, productCode, minLimited, maxLimited, unitCode);
            return Json(productWarn, "text", JsonRequestBehavior.AllowGet);
        }

        #region /QuantityLimits/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string productCode = Request.QueryString["productCode"];
            string unitCode = Request.QueryString["unitCode"];
            decimal minLimited = 100000;
            decimal maxLimited = 100000;

            ExportParam ep = new ExportParam();
            ep.DT1 = ProductWarningService.GetQuantityLimitsDetail(page, rows, productCode, minLimited, maxLimited,unitCode);
            ep.HeadTitle1 = "产品超储短缺查询";
            return PrintService.Print(ep);
        }
        #endregion
    }
}
