using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.SMS.DbModel;
using Microsoft.Practices.Unity;
using THOK.SMS.Bll.Interfaces;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Wms.Controllers.SMS
{
    public class SortOrderAllotController : Controller
    {
        [Dependency]
        public ISortOrderAllotMasterService SortOrderAllotMaster { get; set; }

        [Dependency]
        public ISortOrderAllotDetailService SortOrderAllotDetailService { get; set; }

        //
        // GET: /SortOrderAllot/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        public ActionResult OrderMaster(int page, int rows, string batchNo, string orderId, string status)
        {
            batchNo = batchNo ?? "";
            orderId = orderId ?? "";
            status = status ?? "";
            var sortOrderMaster = SortOrderAllotMaster.GetDetails(page, rows, batchNo, orderId, status);
            return Json(sortOrderMaster, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderDetails(int page, int rows, string orderMasterCode)
        {
            var sortOrderDetail = SortOrderAllotDetailService.GetDetails(page, rows, orderMasterCode);
            return Json(sortOrderDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //打印
        public FileStreamResult CreateExcelToClient()
        {
            int page;
            Int32.TryParse(Request.QueryString["page"], out page);
            int rows;
            Int32.TryParse(Request.QueryString["rows"], out rows);
            string batchNo = Request.QueryString["batchNo"] ?? "";
            string orderId = Request.QueryString["orderId"] ?? "";
            string status = Request.QueryString["status"] ?? "";

            ExportParam ep = new ExportParam();
            ep.DT1 = SortOrderAllotMaster.GetSortOrderAllotMaster(page,rows,batchNo, orderId, status);
            ep.HeadTitle1 = "烟道信息";
            return PrintService.Print(ep);
        }
    }
}
