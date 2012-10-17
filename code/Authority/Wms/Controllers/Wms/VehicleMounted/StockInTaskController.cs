using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Allot.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.Wms.VehicleMounted
{
    public class StockInTaskController : Controller
    {
        //
        // GET: /StockInTask/

        [Dependency]
        public IInBillAllotService InBillAllotService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasApply = true;
            ViewBag.hasCancel = true;
            ViewBag.hasFinish = true;
            ViewBag.hasBatch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        public ActionResult Search(string billNo, int page, int rows)
        {
            string status0 = "0";
            string status1 = "1";
            var result = InBillAllotService.SearchInBillAllot(billNo, status0, status1, page, rows);
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Apply(int id, string status)
        {
            string operator1 = "admin";
            string strResult = string.Empty;
            bool bResult = InBillAllotService.EditAllot(id, status, operator1, out strResult);
            string msg = bResult ? "申请成功" : "申请失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Cancel(int id, string status)
        {
            string operator1 = null;
            string strResult = string.Empty;
            bool bResult = InBillAllotService.EditAllot(id, status, operator1, out strResult);
            string msg = bResult ? "取消成功" : "取消失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
