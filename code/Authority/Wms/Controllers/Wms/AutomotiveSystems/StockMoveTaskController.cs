using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Allot.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Common.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Security;

namespace Wms.Controllers.Wms.AutomotiveSystems
{
    [TokenAclAuthorize]
    public class StockMoveTaskController : Controller
    {
        //
        // GET: /StockMoveTask/

        [Dependency]
        public IMoveBillDetailService MoveBillDetailService { get; set; }

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
        //GO: /StockMoveTask/GetBillNo/
        public ActionResult GetBillNo()
        {
            var result = MoveBillDetailService.GetMoveBillMaster();
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
        //GO: /StockMoveTask/Search/
        public ActionResult Search(string billNo, int page, int rows)
        {
            var result = MoveBillDetailService.SearchMoveBillDetail(billNo, page, rows);
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
        //GO: /StockMoveTask/Operate/
        public ActionResult Operate(string id, string status)
        {
            string strResult = string.Empty;
            string operater = string.Empty;
            string msg = string.Empty;
            operater = this.User.Identity.Name.ToString();            
            bool bResult = MoveBillDetailService.EditAllot(id, status, operater, out strResult);
            if (status == "0") msg = bResult ? "取消成功" : "取消失败";
            if (status == "1") msg = bResult ? "申请成功" : "申请失败";
            if (status == "2") msg = bResult ? "操作成功" : "操作失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
