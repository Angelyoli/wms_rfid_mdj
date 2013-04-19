using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.Bll.Interfaces;
using System;
using THOK.Security;

namespace Authority.Controllers.Authority
{
    [TokenAclAuthorize]
    public class LoginLogController : Controller
    {
        //
        // GET: /LoginLog/
        [Dependency]
        public ILoginLogService LoginLogService { get; set; }
        [Dependency]
        public ISystemEventLogService SystemEventLogService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasDelete = true;
            ViewBag.hasEmpty = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /LoginLog/Details/5

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string userName = collection["UserName"] ?? "";
            string systemName = collection["SystemName"] ?? "";
            var users = LoginLogService.GetDetails(page, rows, userName, systemName);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /LoginLog/Create/
        public ActionResult Create(string login_time,string user_name, string system_ID)
        {
            string localip = this.ControllerContext.HttpContext.Request.UserHostAddress;
            login_time = DateTime.Now.ToString();
            bool bResult = LoginLogService.CreateLoginLog(login_time, user_name, Guid.Parse(system_ID), localip);
            return Json(bResult, JsonRequestBehavior.AllowGet);
        }
 
        //
        // POST: /LoginLog/Edit/5
        public ActionResult Edit(string user_name, string logout_time)
        {
            logout_time = DateTime.Now.ToString();
            bool bResult = LoginLogService.UpdateLoginLog(user_name, logout_time);
            return Json(bResult,JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /LoginLog/Delete/
        public ActionResult Delete(string loginLogId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = LoginLogService.Delete(loginLogId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /LoginLog/Emptys/
        [HttpPost]
        public ActionResult Emptys()
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = LoginLogService.Emptys(out strResult);
            string msg = bResult ? "清空成功" : "清空失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //  /LoginLog/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string loginPC = Request.QueryString["loginPC"];
            string loginTime = Request.QueryString["loginTime"];
            string logoutTime = Request.QueryString["logoutTime"];

            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = LoginLogService.GetLoginLog(page, rows, loginPC, loginTime, logoutTime);
            ep.HeadTitle1 = "登录日志信息";
            ep.BigHeadColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            ep.ColHeadColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            ep.ContentColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }  
    }
}
