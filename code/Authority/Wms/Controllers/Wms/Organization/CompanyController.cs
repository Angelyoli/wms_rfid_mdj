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
#endregion

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
            DataTable dt = CompanyService.GetCompany(page, rows, companyCode, companyName, companyType, isActive);

            string headText = "公司信息";
            string headFont = "微软雅黑"; 
            Int16 headSize = 20;
            Int16 headColor = NPOI.HSSF.Util.HSSFColor.RED.index;
            string colHeadFont = "宋体"; 
            Int16 colHeadSize = 10;
            Int16 colHeadColor = NPOI.HSSF.Util.HSSFColor.BLUE.index;
            Int16 contentColor = NPOI.HSSF.Util.HSSFColor.GREEN.index;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            MemoryStream ms = ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize
                ,headColor,false, colHeadFont, colHeadSize, colHeadColor, true, contentColor, HeaderFooder);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion

    }
}
