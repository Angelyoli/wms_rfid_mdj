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

        public ActionResult Test1()
        {
            DateTime datetime = Convert.ToDateTime("2013-01-03");
            bool bResult = inBillMasterHistoryService.Add(datetime);
            string msg = bResult ? "转移成功" : "转移失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
