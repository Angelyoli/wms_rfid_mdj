using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DownloadWms.Bll;

namespace Wms.Controllers.Wms.Inventory
{
    public class DailyBalanceController : Controller
    {
        //
        // GET: /DailyBalance/

        [Dependency]
        public IDailyBalanceService DailyBalanceService { get; set; }

        //
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasBalance = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /DailyBalance/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string beginDate = collection["BeginDate"] ?? "";
            string endDate = collection["EndDate"] ?? "";
            string warehouseCode = collection["WarehouseCode"] ?? "";
            string unitType = collection["UnitType"] ?? "";
            var DailyBalance = DailyBalanceService.GetDetails(page, rows, beginDate, endDate, warehouseCode, unitType);
            return Json(DailyBalance, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /DailyBalance/InfoDetails/
        public ActionResult InfoDetails(int page, int rows, string warehouseCode, string settleDate,string unitType)
        {
            var DailyBalanceInfo = DailyBalanceService.GetInfoDetails(page, rows, warehouseCode, settleDate, unitType);
            return Json(DailyBalanceInfo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /DailyBalance/InfoCheck/
        public ActionResult InfoCheck(int page, int rows, string warehouseCode, string settleDate, string unitType)
        {
            var DailyBalanceInfo = DailyBalanceService.GetInfoCheck(page, rows, warehouseCode, settleDate, unitType);
            return Json(DailyBalanceInfo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /DailyBalance/DoDailyBalance/
        public ActionResult DoDailyBalance(string warehouseCode, string settleDate)
        {
            DownBusinessSystemsDailyBalanceBll dbll = new DownBusinessSystemsDailyBalanceBll();
            dbll.DownDayEndInfo(settleDate);
            string errorInfo = string.Empty;
            bool bResult = DailyBalanceService.DoDailyBalance(warehouseCode, settleDate,ref errorInfo);
            string msg = bResult ? "日结成功！" : "日结失败！";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        #region
        // GET: /DailyBalance/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string warehouseCode = Request.QueryString["warehouseCode"];
            string settleDate = Request.QueryString["settleDate"];
            string unitType = Request.QueryString["unitType"];

            System.Data.DataTable dt1 = DailyBalanceService.GetInfoDetail(page, rows, warehouseCode,settleDate,unitType);
            System.Data.DataTable dt2 = DailyBalanceService.GetInfoChecking(page, rows, warehouseCode,settleDate,unitType);
            string headText1 = "仓库库存日结明细";
            string headText2 = "仓库库存日结核对";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10; Int16 colHeadWidth = 300;
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            string filename = headText1 + DateTime.Now.ToString("yyMMdd-HHmm-ss");

            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";

            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt1, dt2, headText1, headText2, headFont, headSize,
                colHeadFont, colHeadSize, colHeadWidth, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion

        //
        // GET: /DailyBalance/DailyBalanceInfos/
        //public ActionResult DailyBalanceInfos(int page, int rows, string warehouseCode, string settleDate)
        //{
        //    var DailyBalanceInfo = DailyBalanceService.GetDailyBalanceInfos(page, rows, warehouseCode, settleDate);
        //    return Json(DailyBalanceInfo, "text", JsonRequestBehavior.AllowGet);
        //}
    }
}
