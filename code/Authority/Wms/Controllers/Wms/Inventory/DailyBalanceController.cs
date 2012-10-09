using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DownloadWms.Bll;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using THOK.WMS.Upload.Bll;

namespace Wms.Controllers.Wms.Inventory
{
    public class DailyBalanceController : Controller
    {
        //
        // GET: /DailyBalance/

        [Dependency]
        public IDailyBalanceService DailyBalanceService { get; set; }
        [Dependency]
        public ICellService CellService { get; set; }
        [Dependency]
        public IStorageService StorageService { get; set; }
        [Dependency]
        public IInBillMasterService InBillMasterService { get; set; }
        [Dependency]
        public IOutBillMasterService OutBillMasterService { get; set; }

        UploadBll upload = new UploadBll();

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
        public ActionResult InfoDetails(int page, int rows, string warehouseCode, string settleDate, string unitType)
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
            bool bResult = DailyBalanceService.DoDailyBalance(warehouseCode, settleDate, ref errorInfo);
            string msg = bResult ? "日结成功！" : "日结失败！";
            //if (bResult)
            //{
            //    if (!CellService.uploadCell())
            //    {
            //        msg = msg + "上报仓储属性表失败！";
            //    }
            //    if (!StorageService.uploadStorage())
            //    {
            //        msg = msg + "上报库存表失败！";
            //    }
            //    if (!StorageService.uploadBusiStorage())
            //    {
            //        msg = msg + "上报业务库存表失败！";
            //    }
            //    if (!InBillMasterService.uploadInBill())
            //    {
            //        msg = msg + "上报入库信息失败！";
            //    }
            //    if (!OutBillMasterService.uploadOutBill())
            //    {
            //        msg = msg + "上报出库信息失败！";
            //    }
            //    upload.InsertSynchro();
            //}
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        #region 打印
        // GET: /DailyBalance/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string warehouseCode = Request.QueryString["warehouseCode"];
            string settleDate = Request.QueryString["settleDate"];
            string unitType = Request.QueryString["unitType"];

            System.Data.DataTable dt1 = DailyBalanceService.GetInfoDetail(page, rows, warehouseCode, settleDate, unitType);
            System.Data.DataTable dt2 = DailyBalanceService.GetInfoChecking(page, rows, warehouseCode, settleDate, unitType);
            string headText1 = "仓库库存日结明细";
            string headText2 = "仓库库存日结核对";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10;
            string contentChangeColColorFrom = "DailyBalance";
            short contentChangeColColor = NPOI.HSSF.Util.HSSFColor.RED.index;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt1, dt2, headText1, headText2, headFont, headSize
                , 0, true, colHeadFont, colHeadSize, 0, true, 0, HeaderFooder
                , contentChangeColColorFrom, contentChangeColColor);
            return new FileStreamResult(ms, "application/ms-excel");
        }

        // GET: /DailyBalance/ExportFromTemplate/
        public FileStreamResult ExportFromTemplate()
        {
            int page = 0, rows = 0;
            string warehouseCode = Request.QueryString["warehouseCode"];
            string settleDate = Request.QueryString["settleDate"];
            string unitType = Request.QueryString["unitType"];

            System.Data.DataTable dt1 = DailyBalanceService.GetInfoDetail(page, rows, warehouseCode, settleDate, unitType);
            System.Data.DataTable dt2 = DailyBalanceService.GetInfoChecking(page, rows, warehouseCode, settleDate, unitType);
            string excelTemplate = Server.MapPath("~/ExcelTemplate/DailyBalance.xls");
            string title1 = "仓库库存日结明细";
            string title2 = "仓库库存日结核对";

            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportFromTemplate(dt1, dt2, excelTemplate, title1, title2);
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
