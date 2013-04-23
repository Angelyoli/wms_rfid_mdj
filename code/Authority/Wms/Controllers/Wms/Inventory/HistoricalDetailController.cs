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
            string warehouseCode = Request.QueryString["warehouseCode"] ?? "";
            string productCode = Request.QueryString["productCode"] ?? "";
            string beginDate = Request.QueryString["beginDate"] ?? "";
            string endDate = Request.QueryString["endDate"] ?? "";

            THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
            ep.DT1 = HistoricalDetailService.GetHistoryDetail(page, rows, warehouseCode, productCode, beginDate, endDate);
            ep.HeadTitle1 = "库存历史明细";
            System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}

