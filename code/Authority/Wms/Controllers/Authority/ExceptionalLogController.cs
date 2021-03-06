﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Security;
using THOK.Common.WebUtil;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Authority.Controllers.Authority
{
    [TokenAclAuthorize]
    public class ExceptionalLogController : Controller
    {
        [Dependency]
        public IExceptionalLogService ExceptionalLogService { get; set; }

        //
        // GET: /ExceptionalLog/

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
        public ActionResult CreateExceptionalLog(string ModuleNam, string FunctionName, string ExceptionalType, string ExceptionalDescription, string State)
        {
            bool bResult = ExceptionalLogService.CreateExceptionLog(ModuleNam, FunctionName, ExceptionalType, ExceptionalDescription, State);
            return Json(bResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string moduleName = collection["ModuleName"] ?? "";
            string functionName = collection["FunctionName"] ?? "";
            var users = ExceptionalLogService.GetDetails(page, rows, moduleName, functionName);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ExceptionalLog/Delete/
        [HttpPost]
        public ActionResult Delete(string exceptionalLogId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = ExceptionalLogService.Delete(exceptionalLogId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ExceptionalLog/Emptys/
        public ActionResult Emptys()
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = ExceptionalLogService.Emptys(out strResult);
            string msg = bResult ? "清空成功" : "清空失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //  /ExceptionalLog/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string catchTime = Request.QueryString["catchTime"];
            string moduleName = Request.QueryString["moduleName"];
            string functionName = Request.QueryString["functionName"];

            ExportParam ep = new ExportParam();
            ep.DT1 = ExceptionalLogService.GetExceptionalLog(page, rows, catchTime, moduleName, functionName);
            ep.HeadTitle1 = "错误日志信息";
            return PrintService.Print(ep);
        } 
    }
}
