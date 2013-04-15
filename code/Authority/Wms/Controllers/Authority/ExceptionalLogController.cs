using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Security;

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

            System.Data.DataTable dt = ExceptionalLogService.GetExceptionalLog(page, rows, catchTime, moduleName, functionName);
            string headText = "错误日志信息";
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
