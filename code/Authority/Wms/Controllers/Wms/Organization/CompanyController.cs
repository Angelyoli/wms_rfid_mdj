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
            bool bResult = CompanyService.Add(company);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Company/Edit/
        public ActionResult Edit(Company company)
        {
            bool bResult = CompanyService.Save(company);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Company/Delete/
        [HttpPost]
        public ActionResult Delete(string companyID)
        {
            string error = null;
            bool bResult = false;
            try
            {
                bResult = CompanyService.Delete(companyID);
            }
            catch (Exception e)
            {
                error = "存在主外键约束";
                Response.Write("<script>alert('"+ error + e +"')</script>");
            }
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, error), "text", JsonRequestBehavior.AllowGet);
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
    }
}
