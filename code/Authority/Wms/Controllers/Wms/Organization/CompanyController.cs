using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
using THOK.Wms.DbModel;

namespace Authority.Controllers.Organization
{
    public class CompanyController : Controller
    {
        [Dependency]
        public ICompanyService CompanyService { get; set; }
        //
        // GET: /Company/

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

        //
        // GET: /Company/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string CompanyCode = collection["CompanyCode"] ?? "";
            string CompanyName = collection["CompanyName"] ?? "";
            string CompanyType = collection["CompanyType"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var systems = CompanyService.GetDetails(page, rows, CompanyCode, CompanyName, CompanyType, IsActive);
            return Json(systems, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Company/Create
        [HttpPost]
        public ActionResult Create(Company company)
        {
            string strResult = string.Empty;
            bool bResult = CompanyService.Add(company, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Company/Edit/
        public ActionResult Edit(Company company)
        {
            string strResult = string.Empty;
            bool bResult = CompanyService.Save(company, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Company/Delete/
        [HttpPost]
        public ActionResult Delete(string companyID)
        {
            string strResult = string.Empty;
            bool bResult = CompanyService.Delete(companyID, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Company/GetParentName/
        public ActionResult GetParentName(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "CompanyCode";
            }
            if (value == null)
            {
                value = "";
            }
            var company = CompanyService.GetParentName(page, rows, queryString, value);
            return Json(company, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Company/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string companyCode = Request.QueryString["companyCode"];
            string companyName = Request.QueryString["companyName"];
            string companyType = Request.QueryString["companyType"];
            string isActive = Request.QueryString["isActive"];
            
            System.Data.DataTable dt = CompanyService.GetCompany(page, rows, companyCode, companyName, companyType, isActive);
            string headText = "公司信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10; Int16 colHeadWidth = 300;
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            this.GetResponse(headText);

            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize,
                colHeadFont, colHeadSize, colHeadWidth, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion

        #region this.GetResponse
        private void GetResponse(string headText)
        {
            string filename = headText + DateTime.Now.ToString("yyMMdd-HHmm-ss");
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";
        }        
        #endregion
    }
}
