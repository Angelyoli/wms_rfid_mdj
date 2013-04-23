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

            THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
            ep.DT1 = StockledgerService.GetInfoDetail(page, rows, warehouseCode, productCode, settleDate);
            ep.HeadTitle1 = "库存历史总账明细";
            System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
