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
    public class DepartmentController : Controller
    {
        [Dependency]
        public IDepartmentService DepartmentService { get; set; }

        //
        // GET: /Department/

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
        // GET: /Department/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string DepartmentCode = collection["DepartmentCode"] ?? "";
            string DepartmentName = collection["DepartmentName"] ?? "";
            string DepartmentLeaderID = collection["DepartmentLeaderID"] ?? "";
            string CompanyID =  collection["CompanyID"] ?? "";
            var systems = DepartmentService.GetDetails(page, rows, DepartmentCode, DepartmentName, DepartmentLeaderID, CompanyID);
            return Json(systems, "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            string strResult = string.Empty;
            bool bResult = DepartmentService.Add(department, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Department/Edit/

        public ActionResult Edit(Department department)
        {
            string strResult = string.Empty;
            bool bResult = DepartmentService.Save(department, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Department/Delete/

        [HttpPost]
        public ActionResult Delete(string departId)
        {
            string strResult = string.Empty;
            bool bResult = DepartmentService.Delete(departId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Department/GetDepartment/
        public ActionResult GetDepartment(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "EmployeeCode";
            }
            if (value == null)
            {
                value = "";
            }
            var department = DepartmentService.GetDepartment(page, rows, queryString, value);
            return Json(department, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Department/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string departmentCode = Request.QueryString["departmentCode"];
            string departmentName = Request.QueryString["departmentName"];
            string departmentLeaderId = Request.QueryString["departmentType"];
            string companyId = Request.QueryString["companyId"];

            System.Data.DataTable dt = DepartmentService.GetDepartment(page, rows, departmentCode, departmentName, departmentLeaderId, companyId);
            string headText = "部门信息";
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
