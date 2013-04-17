using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
using THOK.Wms.DbModel;
#region
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;
using THOK.Common;
using THOK.Security;
#endregion

namespace Authority.Controllers.Organization
{
    [TokenAclAuthorize]
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

        public ActionResult SearchPage()
        {
            return View();
        }

        public ActionResult AddPage()
        {
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
            string companyCode = Request.QueryString["companyCode"] ?? "";
            string companyName = Request.QueryString["companyName"] ?? "";
            string companyType = Request.QueryString["companyType"] ?? "";
            string isActive = Request.QueryString["isActive"] ?? "";
            
            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = CompanyService.GetCompany(page, rows, companyCode, companyName, companyType, isActive);
            ep.HeadTitle1 = "公司信息";
            ep.BigHeadColor=NPOI.HSSF.Util.HSSFColor.BLACK.index;
            ep.ColHeadColor=NPOI.HSSF.Util.HSSFColor.BLACK.index;
            ep.ContentColor=NPOI.HSSF.Util.HSSFColor.BLACK.index;
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
