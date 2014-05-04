using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Security;
using THOK.SMS.DbModel;
using Microsoft.Practices.Unity;
using THOK.SMS.Bll.Interfaces;
using THOK.Common.WebUtil;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Wms.Controllers.SMS
{
    [TokenAclAuthorize]
    public class BatchController : Controller
    {
        //
        // GET: /Batch/

        [Dependency]

        public IBatchService BatchInfoService { get; set; }
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            //ViewBag.hasAudit = true;
            //ViewBag.hasAntiTrial = true;
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
        
            string BatchName = collection["BatchName"] ?? "";     
            string OrderDate = collection["OrderDate"] ?? "";
            string OperateDate = collection["OperateDate"] ?? "";
            string BatchNo = collection["BatchNo"] ?? "";

            var srmDetail = BatchInfoService.GetDetails(page, rows, BatchNo, BatchName, OrderDate, OperateDate);
            return Json(srmDetail, "text", JsonRequestBehavior.AllowGet);
               
           
        }

        public ActionResult Create(Batch batchInfo)
        {
            string strResult = string.Empty;
            bool bResult = BatchInfoService.Add(batchInfo, this.User.Identity.Name.ToString(),out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Batch batchInfo)
        {
            string strResult = string.Empty;
            bool bResult = BatchInfoService.Save(batchInfo, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int batchId)
        {
            string strResult = string.Empty;
            bool bResult = BatchInfoService.Delete(batchId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
