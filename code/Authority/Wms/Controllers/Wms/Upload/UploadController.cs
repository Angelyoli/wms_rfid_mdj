using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Upload;
using THOK.WMS.Upload.Bll;
using THOK.WebUtil;

namespace Wms.Controllers.Wms.Upload
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasUpload = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            return View();
        }
        public ActionResult UploadDetails(FormCollection collection)
        {

            string beginDate = collection["BeginDate"] ?? "";
            string endDate = collection["EndDate"] ?? "";
            UploadBll upload = new UploadBll();
            var result = upload.QuerySynchroInfo(beginDate, endDate);
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Upload()
        {
            UploadDate upload = new UploadDate();
            bool result = upload.UploadInfoData();
            string msg = result ? "上传成功" : "上传失败";
            return Json(JsonMessageHelper.getJsonMessage(result, msg), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
