using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.WebUtil;
using THOK.Security;

namespace Authority.Controllers.Wms.Organization
{
    [TokenAclAuthorize]
    public class JobController : Controller
    {
        [Dependency]
        public IJobService JobService { get; set; }
        //
        // GET: /Job/

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

        //
        // GET: /Job/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string JobCode = collection["JobCode"] ?? "";
            string JobName = collection["JobName"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var job = JobService.GetDetails(page, rows, JobCode, JobName, IsActive);
            return Json(job, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Job/Create/

        [HttpPost]
        public ActionResult Create(Job job)
        {
            string strResult = string.Empty;
            bool bResult = JobService.Add(job, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Job/Edit/5

        public ActionResult Edit(Job job)
        {
            string strResult = string.Empty;
            bool bResult = JobService.Save(job, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Job/Delete/

        [HttpPost]
        public ActionResult Delete(string jobId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = JobService.Delete(jobId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Job/GetJob/
        public ActionResult GetJob(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "JobCode";
            }
            if (value == null)
            {
                value = "";
            }
            var job = JobService.GetJob(page, rows, queryString, value);
            return Json(job, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Job/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string jobCode = Request.QueryString["jobCode"];
            string jobName = Request.QueryString["jobName"];
            string isActive = Request.QueryString["isActive"];

            THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
            ep.DT1 = JobService.GetJob(page, rows, jobCode, jobName, isActive);
            ep.HeadTitle1 = "岗位信息";
            System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }  
        #endregion
    }
}
