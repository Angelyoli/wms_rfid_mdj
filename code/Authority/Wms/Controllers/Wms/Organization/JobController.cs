using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.Organization
{
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

            System.Data.DataTable dt = JobService.GetJob(page, rows, jobCode, jobName, isActive);
            string headText = "岗位信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10; Int16 colHeadWidth = 300;
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            string filename = headText + DateTime.Now.ToString("yyMMdd-HHmm-ss");

            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";

            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize,
                colHeadFont, colHeadSize, colHeadWidth, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }  
        #endregion
    }
}
