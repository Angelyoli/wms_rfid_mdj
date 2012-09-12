using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
using THOK.WMS.DownloadWms.Bll;
using THOK.Wms.DownloadWms.Bll;

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
        public ActionResult DownSortOrder(string beginDate, string endDate, string sortLineCode, bool isSortDown,string batch)
        {
            string errorInfo = string.Empty;
            string lineErrorInfo = string.Empty;
            string custErrorInfo = string.Empty;
            bool bResult = false;
            bool lineResult = false;
            if (beginDate == string.Empty || endDate == string.Empty)
            {
                beginDate = DateTime.Now.ToString("yyyyMMdd");
                endDate = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                beginDate = Convert.ToDateTime(beginDate).ToString("yyyyMMdd");
                endDate = Convert.ToDateTime(endDate).ToString("yyyyMMdd");
            }
            DownSortingInfoBll dsinfo = new DownSortingInfoBll();
            DownRouteBll bll = new DownRouteBll();
            DownSortingOrderBll sbll = new DownSortingOrderBll();
            DownCustomerBll cbll = new DownCustomerBll();
            bool custResult = cbll.DownCustomerInfo();
            if (isSortDown)
            {
                //从分拣下载分拣数据
                lineResult = bll.DownSortRouteInfo();
                bResult = dsinfo.GetSortingOrderDate(beginDate, endDate, sortLineCode, batch, out errorInfo);
            }
            else
            {
                //从营销下载分拣数据
                lineResult = bll.DownRouteInfo();                
                bResult = sbll.GetSortingOrderDate(beginDate, endDate, out errorInfo);
            }

            string info = "线路：" + lineErrorInfo + "。客户：" + custErrorInfo + "。分拣" + errorInfo;
            string msg = bResult ? "下载成功" : "下载失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
