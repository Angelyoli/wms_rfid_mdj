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

            System.Data.DataTable dt1 = HistoricalDetailService.GetHistoryDetail(page, rows, warehouseCode, productCode, beginDate,endDate);

            string headText1 = "库存历史明细";
            string headFont = "微软雅黑"; short headSize = 20;
            string colHeadFont = "Arial"; short colHeadSize = 10;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt1, null, headText1, null, headFont, headSize, 0, true,
                colHeadFont, colHeadSize, 0, true, 0, HeaderFooder);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}

