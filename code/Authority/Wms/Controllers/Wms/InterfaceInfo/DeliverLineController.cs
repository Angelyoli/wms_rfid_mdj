using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;

namespace Wms.Controllers.Wms.InterfaceInfo
{
    public class DeliverLineController : Controller
    {
        [Dependency]
        public IDeliverLineService DeliverLineService { get; set; }

        //
        // GET: /DeliverLine/
        public ActionResult Index()
        {
            return View();
        }


        //下载线路信息
        // GET: /DeliverLine/DownloadDeliverLineSave/
        public ActionResult DownloadDeliverLineSave()
        {
            string errorInfo = string.Empty;
            bool bResult = DeliverLineService.DownDeliverLine(out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
