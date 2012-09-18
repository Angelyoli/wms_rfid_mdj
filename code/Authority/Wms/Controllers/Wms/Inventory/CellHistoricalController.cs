using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;


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

            System.Data.DataTable dt1 = CellHistoricalService.GetCellHistory(page, rows, beginDate, endDate, type, id);

            string headText1 = "货位历史明细";
            string headFont = "微软雅黑"; short headSize = 20;
            string colHeadFont = "Arial"; short colHeadSize = 10;
            
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt1, null, headText1, null, headFont, headSize, colHeadFont, colHeadSize);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
