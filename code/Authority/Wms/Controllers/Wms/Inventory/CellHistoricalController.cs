using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Common.WebUtil;


namespace Wms.Controllers.Wms.Inventory
{
    public class CellHistoricalController : Controller
    {
        //
        // GET: /CellHistorical/

        [Dependency]
        public ICellHistoricalService CellHistoricalService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /CellHistorical/Details/

        public ActionResult Details(int page, int rows, string beginDate, string endDate, string type, string id)
        {
            var storage = CellHistoricalService.GetCellDetails(page, rows, beginDate, endDate, type, id);

            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        #region
        // GET: /CellHistorical/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string beginDate = "null01";
            string endDate = "null02";
            string type = Request.QueryString["type"];
            string id = Request.QueryString["id"];

            THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
            ep.DT1 = CellHistoricalService.GetCellHistory(page, rows, beginDate, endDate, type, id);
            ep.HeadTitle1 = "货位历史明细";
            System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
