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
            bool isAbnormity =Convert.ToBoolean(Request.QueryString["isAbnormity"]);
            string areaName;
            
            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = CurrentStockService.GetCurrentStock(page, rows, productCode, ware, area, unitType, out areaName, isAbnormity);
            ep.DT2 = null;
            ep.HeadTitle1 = "当前库存" + areaName;
            ep.HeadTitle2 = "";
            ep.ContentModule = null;
            ep.ContentModuleColor = 0;
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
    }
}
