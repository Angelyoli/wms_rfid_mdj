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
            string CompanyID = collection["CompanyID"] ?? "";
            var systems = DepartmentService.GetDetails(page, rows, DepartmentCode, DepartmentName, DepartmentLeaderID, CompanyID);
            return Json(systems, "text", JsonRequestBehavior.AllowGet);
        }

       
        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            bool bResult = DepartmentService.Add(department);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        
       
        //
        // POST: /Department/Edit/

        public ActionResult Edit(Department department)
        {
            bool bResult = DepartmentService.Save(department);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        

        //
        // POST: /Department/Delete/

        [HttpPost]
        public ActionResult Delete(string departId)
        {
            string error = null;
            bool bResult = false;
            try
            {
                bResult = DepartmentService.Delete(departId);
            }
            catch (Exception e)
            {
                error = "存在主外键约束";
                Response.Write("<script>alert('"+ error + e +"')</script>");
            }
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, error), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Department/GetDepartment/
        public ActionResult GetDepartment(int page,int rows,string queryString,string value)
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
    }
}
