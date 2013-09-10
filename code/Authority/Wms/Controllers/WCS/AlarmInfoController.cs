using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Security;
using THOK.WCS.DbModel;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.Common.WebUtil;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Wms.Controllers.WCS
{
    [TokenAclAuthorize]
    public class AlarmInfoController : Controller
    {
        [Dependency]
        public IAlarmInfoService AlarmInfoService { get; set; }
        //
        // GET: /AlarmInfo/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        public ActionResult AddPage()
        {
            return View();
        }

        public ActionResult SearchPage() 
        {
            return View();
        }

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.AlarmCode = collection["AlarmCode"] ?? "";
            alarmInfo.Description = collection["Description"] ?? "";
            var srmDetail = AlarmInfoService.GetDetails(page, rows, alarmInfo);
            return Json(srmDetail, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(AlarmInfo alarmInfo)
        {
            string strResult = string.Empty;
            bool bResult = AlarmInfoService.Add(alarmInfo, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(AlarmInfo alarmInfoCode)
        {
            string strResult = string.Empty;
            bool bResult = AlarmInfoService.Save(alarmInfoCode,out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string code)
        {
            string strResult = string.Empty;
            bool bResult = AlarmInfoService.Delete(code, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string AlarmCode = Request.QueryString["alarmCode"];
            string Description = Request.QueryString["description"];
            AlarmInfo alarmInfo=new AlarmInfo ();
            alarmInfo.AlarmCode = AlarmCode;
            alarmInfo.Description = Description;

            ExportParam ep = new ExportParam();
            ep.DT1 = AlarmInfoService.GetAlarmInfo(page, rows, alarmInfo);
            ep.HeadTitle1 = "报警信息";
            return PrintService.Print(ep);
        }  
    }
}
