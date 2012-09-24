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

        public ActionResult AddPage()
        {
            return View();
        }

        public ActionResult SearchPage()
        {
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
            string strResult = string.Empty;
            bool bResult = EmployeeService.Add(employee, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Employee/Edit/5

        public ActionResult Edit(Employee employee)
        {
            string strResult = string.Empty;
            bool bResult = EmployeeService.Save(employee, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Employee/Delete/

        [HttpPost]
        public ActionResult Delete(string demployeeId)
        {
            string strResult = string.Empty;
            bool bResult = EmployeeService.Delete(demployeeId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Employee/GetEmployee/
        public ActionResult GetEmployee(int page, int rows, string queryString, string value)
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

        #region /Employee/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string employeeCode = Request.QueryString["employeeCode"];
            string employeeName = Request.QueryString["employeeName"];
            string departmentId = Request.QueryString["departmentId"];
            string jobId = Request.QueryString["jobId"];
            string status = Request.QueryString["status"];
            string isActive = Request.QueryString["isActive"];

            System.Data.DataTable dt = EmployeeService.GetEmployee(page, rows, employeeCode, employeeName, departmentId, jobId, status, isActive);
            string headText = "员工信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize
                , 0, true, colHeadFont, colHeadSize, 0, true, 0, HeaderFooder);
            return new FileStreamResult(ms, "application/ms-excel");
        } 
        #endregion
    }
}
