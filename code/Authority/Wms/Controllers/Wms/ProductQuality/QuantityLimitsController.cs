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

            System.Data.DataTable dt = ProductWarningService.GetQuantityLimitsDetail(page, rows, productCode, minLimited, maxLimited,unitCode);
            string headText = "产品超储短缺查询";
            string headFontName = "微软雅黑"; Int16 headFontSize = 20;
            string colHeadFontName = "Arial"; Int16 colHeadFontSize = 10; Int16 colHeadWidth = 300;
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            string filename = headText + DateTime.Now.ToString("yyMMdd-HHmm-ss");

            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";

            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFontName, headFontSize, colHeadFontName, colHeadFontSize, colHeadWidth, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
