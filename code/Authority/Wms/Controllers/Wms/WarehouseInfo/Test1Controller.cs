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
        //
        // GET: /Test1/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.moduleID = moduleID;
            return View();
        }

        [Dependency]
        public IInBillMasterHistoryService inBillMasterHistoryService { get; set; }

        public ActionResult Test1_delete()
        {
            string strResult = string.Empty;
            DateTime datetime = Convert.ToDateTime("2013-01-03");
            bool bResult = inBillMasterHistoryService.DeleteMaster(datetime, out strResult);
            string msg = bResult ? "转移成功" : "转移失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Test1()
        {
            string strResult=string.Empty;
            DateTime datetime = Convert.ToDateTime("2013-01-03");
            bool bResult = inBillMasterHistoryService.Add(datetime, out strResult);
            string msg = bResult ? "转移成功" : "转移失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        
        [Dependency]
        public IOutBillMasterHistoryService outBillMasterHistoryService { get; set; }  
        public ActionResult Test3()
        {
            DateTime datetime = Convert.ToDateTime("2013-01-03");
            bool bResult = outBillMasterHistoryService.Add(datetime);
            string msg = bResult ? "转移成功" : "转移失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        [Dependency]
        public IOutBillMasterHistoryService outBillDetailHistoryService { get; set; }
        public ActionResult Test4()
        {
            DateTime datetime = Convert.ToDateTime("2013-01-03");
            bool bResult = outBillDetailHistoryService.Add(datetime);
            string msg = bResult ? "转移成功" : "转移失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        [Dependency]
        public IDailyBalanceHistoryService dailyBalanceHistoryService { get; set; }
        public ActionResult Test5()
        {
            DateTime datetime = Convert.ToDateTime("2013-01-03");
            bool bResult = dailyBalanceHistoryService.Add(datetime);
            string msg = bResult ? "转移成功" : "转移失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
