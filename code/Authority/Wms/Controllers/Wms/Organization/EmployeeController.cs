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
    public class EmployeeController : Controller
    {
        [Dependency]
        public IEmployeeService EmployeeService { get; set; }

        //
        // GET: /Employee/

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
        // GET: /Employee/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string EmployeeCode = collection["EmployeeCode"] ?? "";
            string EmployeeName = collection["EmployeeName"] ?? "";
            string DepartmentID = collection["DepartmentID"] ?? "";
            string JobID = collection["JobID"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var systems = EmployeeService.GetDetails(page, rows, EmployeeCode, EmployeeName, DepartmentID, JobID, Status, IsActive);
            return Json(systems, "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Employee/Create

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            bool bResult = EmployeeService.Add(employee);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Employee/Edit/5

        public ActionResult Edit(Employee employee)
        {
            bool bResult = EmployeeService.Save(employee);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Employee/Delete/

        [HttpPost]
        public ActionResult Delete(string demployeeId)
        {
            string error = null;
            bool bResult = false;
            try
            {
                bResult = EmployeeService.Delete(demployeeId);
            }
            catch (Exception e)
            {
                error = "存在主外键约束";
                Response.Write("<script>alert('"+ error + e +"')</script>");
            }
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, error), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Employee/GetEmployee/
        public ActionResult GetEmployee(int page , int rows, string queryString,string value)
        {
            if (queryString == null)
            {
                queryString = "EmployeeCode";
            }
            if (value == null)
            {
                value = "";
            }
            var employee = EmployeeService.GetEmployee(page, rows, queryString, value);
            return Json(employee, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
