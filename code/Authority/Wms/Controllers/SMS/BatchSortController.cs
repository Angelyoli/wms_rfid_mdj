using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using THOK.SMS.DbModel;
using THOK.Common.WebUtil;
using THOK.Security;
using Microsoft.Practices.Unity;
using THOK.SMS.Bll.Interfaces;

namespace Wms.Controllers.SMS
{
     [TokenAclAuthorize]
    public class BatchSortController : Controller
    {
        //
        // GET: /BatchSort/

        [Dependency]

        public IBatchSortService BatchSortService { get; set; }

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

        public ActionResult Details(int page, int rows, string Status, string BatchNo, string BatchName,string OperateDate)
        {
            var srmDetail = BatchSortService.GetDetails(page, rows, Status, BatchNo, BatchName, OperateDate);
            return Json(srmDetail, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBatch(int page, int rows, string queryString, string value)
        {

            if (queryString == null)
            {
                queryString = "BatchNo";
            }
            if (value == null)
            {
                value = "";
            }
            var batch =BatchSortService.GetBatch(page, rows, queryString, value);
            return Json(batch, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(BatchSort BatchSort)
        {
            string strResult = string.Empty;
            bool bResult = BatchSortService.Add(BatchSort, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(BatchSort BatchSort)
        {
            string strResult = string.Empty;
            bool bResult = BatchSortService.Save(BatchSort, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int BatchSortId)
        {
            string strResult = string.Empty;
            bool bResult = BatchSortService.Delete(BatchSortId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
