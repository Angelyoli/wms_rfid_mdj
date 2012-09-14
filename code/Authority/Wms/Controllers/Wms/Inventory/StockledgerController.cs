using System.Web.Mvc;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;

namespace Authority.Controllers.Wms.Inventory
{
    public class StockledgerController : Controller
    {
        //
        // GET: /Stockledger/

        [Dependency]
        public IStockledgerService StockledgerService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /Stockledger/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string warehouseCode = collection["WarehouseCode"] ?? "";
            string productCode = collection["ProductCode"] ?? "";
            string beginDate = collection["BeginDate"] ?? "";
            string endDate = collection["EndDate"] ?? "";
            string unitType = collection["UnitType"] ?? "";
            var Stockledger = StockledgerService.GetDetails(page, rows, warehouseCode, productCode, beginDate, endDate, unitType);
            return Json(Stockledger, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET:/Stockledger/InfoDetails/
        public ActionResult InfoDetails(int page, int rows, string warehouseCode, string productCode, string settleDate)
        {
            var StockledgerDetails = StockledgerService.GetInfoDetails(page, rows, warehouseCode, productCode, settleDate);
            return Json(StockledgerDetails, "text", JsonRequestBehavior.AllowGet);
        }

        #region
        // GET: /Stockledger/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string warehouseCode = Request.QueryString["warehouseCode"];
            string productCode = Request.QueryString["productCode"];
            string settleDate = Request.QueryString["settleDate"];

            System.Data.DataTable dt1 = StockledgerService.GetInfoDetail(page, rows, warehouseCode, productCode, settleDate);
            string headText1 = "库存历史总账明细";
            string headFont = "微软雅黑"; short headSize = 20;
            string colHeadFont = "Arial"; short colHeadSize = 10; short colHeadWidth = 300;
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            string filename = headText1 + System.DateTime.Now.ToString("yyMMdd-HHmm-ss");

            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";

            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt1, null, headText1, null, headFont, headSize,
                colHeadFont, colHeadSize, colHeadWidth, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
