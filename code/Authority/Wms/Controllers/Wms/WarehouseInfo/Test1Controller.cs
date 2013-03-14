using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.Wms.WarehouseInfo
{
    public class Test1Controller : Controller
    {
        [Dependency]
        public IInBillMasterHistoryService InBillMasterHistoryService { get; set; }
        [Dependency]
        public IOutBillMasterHistoryService OutBillMasterHistoryService { get; set; }
        [Dependency]
        public IDailyBalanceHistoryService DailyBalanceHistoryService { get; set; }

        //
        // GET: /Test1/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.moduleID = moduleID;
            return View();
        }
        public ActionResult InBillMasterHistory(string datetime)
        {
            string masterResult = string.Empty;
            string detailResult = string.Empty;
            string allotResult = string.Empty;
            bool bResult = InBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out masterResult, out detailResult, out allotResult);
            string msg = bResult ? "成功！" : "失败！";
            string result = "-------------------------------------------------【主表：" + masterResult 
                        + "】-------------------------------------------------【细表：" + detailResult 
                        + "】-------------------------------------------------【分配表：" + allotResult + "】";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult InBillMasterHistory2(string datetime)
        {
            string strResult = string.Empty;
            bool bResult = InBillMasterHistoryService.Add2(Convert.ToDateTime(datetime), out strResult);
            string msg = bResult ? "成功！" : "失败！";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult OutBillMasterHistory(DateTime datetime)
        {
            string strResult = string.Empty;
            bool bResult = OutBillMasterHistoryService.Add(datetime, out strResult);
            string msg = bResult ? "成功！" : "失败！";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DailyBalanceHistory(DateTime datetime)
        {
            string strResult = string.Empty;
            bool bResult = DailyBalanceHistoryService.Add(datetime, out strResult);
            string msg = bResult ? "成功！" : "失败！";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
