using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.Inventory
{
    public class CurrentStockController : Controller
    {
        //
        // GET: /CurrentStock/

        [Dependency]
        public ICurrentStockService CurrentStockService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /CurrentStock/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string productCode = collection["ProductCode"] ?? "";
            string ware = collection["Ware"] ?? "";
            string area = collection["Area"] ?? "";
            string unitType = collection["UnitType"] ?? "";
            var storage = CurrentStockService.GetCellDetails(page, rows, productCode, ware, area, unitType);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        // /CurrentStock/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string productCode = Request.QueryString["productCode"];
            string ware = Request.QueryString["ware"];
            string area = Request.QueryString["area"];
            string unitType = Request.QueryString["unitType"];
            string areaName;
            System.Data.DataTable dt = CurrentStockService.GetCurrentStock(page, rows, productCode, ware, area, unitType, out areaName);
            string headText = "当前库存--" + areaName;
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
    }
}
