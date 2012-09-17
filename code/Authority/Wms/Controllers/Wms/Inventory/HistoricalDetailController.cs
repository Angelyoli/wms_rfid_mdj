using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.Inventory
{
    public class HistoricalDetailController : Controller
    {
        //
        // GET: /HistoricalDetail/

        [Dependency]
        public IHistoricalDetailService HistoricalDetailService { get; set; }

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
            string warehouseCode = collection["WarehouseCode"] ?? "";
            string productCode = collection["ProductCode"] ?? "";
            string beginDate = collection["BeginDate"] ?? "";
            string endDate = collection["EndDate"] ?? "";
            var HistoricalDetail = HistoricalDetailService.GetDetails(page, rows, warehouseCode, productCode, beginDate, endDate);
            return Json(HistoricalDetail, "text", JsonRequestBehavior.AllowGet);
        }

        #region
        // GET: /HistoricalDetail/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string warehouseCode = Request.QueryString["warehouseCode"];
            string productCode = Request.QueryString["productCode"];
            string beginDate = Request.QueryString["beginDate"];
            string endDate = Request.QueryString["endDate"];

            System.Data.DataTable dt1 = HistoricalDetailService.GetHistoryDetail(page, rows, warehouseCode, productCode, beginDate,endDate);
            string headText1 = "库存历史明细";
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

