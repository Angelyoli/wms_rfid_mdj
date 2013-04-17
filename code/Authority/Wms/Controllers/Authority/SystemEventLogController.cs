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
    public class SystemEventLogController : Controller
    {
        //
        // GET: /SystemEventLog/
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

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string eventname = collection["EventName"] ?? "";
            string operateuser = collection["OperateUser"] ?? "";
            string targetsystem = collection["TargetSystem"] ?? "";
            var users = SystemEventLogService.GetDetails(page, rows,eventname, operateuser, targetsystem);
            return Json(users, "text", JsonRequestBehavior.AllowGet);

        }

        //
        // POST: /SystemEventLog/Delete/
        [HttpPost]
        public ActionResult Delete(string eventLogId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = SystemEventLogService.Delete(eventLogId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SystemEventLog/Emptys/
        [HttpPost]
        public ActionResult Emptys()
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = SystemEventLogService.Emptys(out strResult);
            string msg = bResult ? "清空成功" : "清空失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //  /SystemEventLog/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string eventLogTime = Request.QueryString["eventLogTime"];
            string eventName = Request.QueryString["eventName"];
            string fromPC = Request.QueryString["fromPC"];
            string operateUser = Request.QueryString["operateUser"];
            string targetSystem = Request.QueryString["targetSystem"];

            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = SystemEventLogService.GetSystemEventLog(page, rows, eventLogTime, eventName, fromPC, operateUser, targetSystem);
            ep.HeadTitle1 = "业务日志信息";
            ep.BigHeadColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            ep.ColHeadColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            ep.ContentColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }  

        public ActionResult CreateEventLog(string EventName, string EventDescription, string OperateUser, Guid TargetSystem)
        {
            bool result=SystemEventLogService.CreateEventLog(EventName,EventDescription,OperateUser,TargetSystem);
            return Json(result);
        }

    }
}

