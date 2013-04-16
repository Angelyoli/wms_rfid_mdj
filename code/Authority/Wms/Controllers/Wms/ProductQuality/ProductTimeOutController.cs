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
using THOK.Security;

namespace Wms.Controllers.Wms.ProductQuality
{
    [TokenAclAuthorize]
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
            decimal assemblyTime=360;
            if (collection["AssemblyTime"] != null && collection["AssemblyTime"] != "")
            {
               assemblyTime= decimal.Parse(collection["AssemblyTime"]);
            }
            var product = ProductWarningService.GetProductDetails(page, rows, productCode, assemblyTime);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        #region /ProductTimeOut/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string productCode = Request.QueryString["productCode"]??"";
            decimal assemblyTime = 360;
            if (Request.QueryString["assemblyTime"] != null && Request.QueryString["assemblyTime"] != "")
            {
                assemblyTime = decimal.Parse(Request.QueryString["assemblyTime"]);
            }
            
            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = ProductWarningService.GetProductTimeOut(page, rows, productCode, assemblyTime);
            ep.DT2 = null;
            ep.HeadTitle1 = "产品预警信息设置";
            ep.HeadTitle2 = "";
            ep.ContentModule = null;
            ep.ContentModuleColor = 0;
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
