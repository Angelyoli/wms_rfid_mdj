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

            System.Data.DataTable dt = LoginLogService.GetLoginLog(page, rows, loginPC, loginTime, logoutTime);
            string headText = "登录日志信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize
                , 0, true, colHeadFont, colHeadSize, 0, true, 0, HeaderFooder, null, 0);
            return new FileStreamResult(ms, "application/ms-excel");
        }  
    }
}
