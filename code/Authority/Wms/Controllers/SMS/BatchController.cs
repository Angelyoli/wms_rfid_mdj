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
            Batch batchInfo = new Batch();
            batchInfo.BatchId = Convert.ToInt32(collection["BatchID"]);
            batchInfo.BatchName = collection["BatchName"] ?? "";
            batchInfo.BatchNo = Convert.ToInt32(collection["BatchNo"]);
            batchInfo.Description = collection["Description"] ?? "";
            var srmDetail = BatchInfoService.GetDetails(page, rows, batchInfo);
            return Json(srmDetail, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(Batch batchInfo)
        {
            string strResult = string.Empty;
            bool bResult = BatchInfoService.Add(batchInfo, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Batch batchInfoCode)
        {
            string strResult = string.Empty;
            bool bResult = BatchInfoService.Save(batchInfoCode, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string batchcode)
        {
            string strResult = string.Empty;
            bool bResult = BatchInfoService.Delete(batchcode, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }     
    }
}
