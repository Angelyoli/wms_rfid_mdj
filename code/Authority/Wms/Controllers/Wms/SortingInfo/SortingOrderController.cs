using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.SortingInfo
{
    public class SortingOrderController : Controller
    {
        [Dependency]
        public ISortOrderService SortOrderService { get; set; }
        [Dependency]
        public ISortOrderDetailService SortOrderDetailService { get; set; }
        [Dependency]
        public IDeliverLineService DeliverLineService { get; set; }
        [Dependency]
        public ICustomerService CustomerService { get; set; }
        //
        // GET: /SortingOrder/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasDownload = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //查询主单
        // GET: /SortingOrder/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string OrderID = collection["OrderID"] ?? "";
            string orderDate = collection["orderDate"] ?? "";
            var sortOrder = SortOrderService.GetDetails(page, rows, OrderID, orderDate);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /SortingOrder/sortOrderDetails/
        public ActionResult sortOrderDetails(int page, int rows, string OrderID)
        {
            var SortOrderDetail = SortOrderDetailService.GetDetails(page, rows, OrderID);
            return Json(SortOrderDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //根据时间分组查询主单
        // GET: /SortingOrder/GetOrderMaster/
        public ActionResult GetOrderMaster(string orderDate)
        {
            var sortOrder = SortOrderService.GetDetails(orderDate);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingOrder/DownSortOrder/
        public ActionResult DownSortOrder(string beginDate, string endDate)
        {
            string errorInfo = string.Empty;
            string lineErrorInfo = string.Empty;
            string custErrorInfo = string.Empty;
            bool lineResult = DeliverLineService.DownDeliverLine(out lineErrorInfo);
            bool custResult = CustomerService.DownDeliverLine(out custErrorInfo);           
            bool bResult = SortOrderService.DownSortOrder(beginDate, endDate,out errorInfo);
            string info = "线路：" + lineErrorInfo + "。客户：" + custErrorInfo + "。分拣" + errorInfo;
            string msg = bResult ? "下载成功" : "下载失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, info), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
