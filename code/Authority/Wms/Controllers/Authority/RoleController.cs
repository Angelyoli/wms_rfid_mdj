﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Common.WebUtil;
using THOK.Authority.Bll.Interfaces;
using THOK.Security;
using System;
using THOK.Security;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Authority.Controllers.Authority
{
    [TokenAclAuthorize]
    public class RoleController : Controller
    {
        [Dependency]
        public IRoleService RoleService { get; set; }

        // GET: /Role/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.hasPermissionAdmin = true;
            ViewBag.hasUserAdmin = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /Role/Details/
        public ActionResult Details(int page, int rows,FormCollection collection)
        {
            string roleName = collection["RoleName"] ?? "";
            string description = collection["Description"] ?? "";
            string status = collection["Status"] ?? "";
            var roles = RoleService.GetDetails(page, rows, roleName, description, status);
            return Json(roles,"text",JsonRequestBehavior.AllowGet);
        }    

        // POST: /Role/Create/
        [HttpPost]
        public ActionResult Create(string roleName, string description, bool status)
        {
            bool bResult = RoleService.Add(roleName, description, status);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/Edit/
        [HttpPost]
        public ActionResult Edit(string roleID, string roleName, string description, bool status)
        {
            bool bResult = RoleService.Save(roleID, roleName, description, status);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/Delete/
        [HttpPost]
        public ActionResult Delete(string roleID)
        {
            bool bResult = RoleService.Delete(roleID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/GetRoleUser/
        [HttpPost]
        public ActionResult GetRoleUser(string RoleID)
        {
            var users = RoleService.GetRoleUser(RoleID);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/GetUserInfo/
        [HttpPost]
        public ActionResult GetUserInfo(string RoleID)
        {
            var users = RoleService.GetUserInfo(RoleID);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/DeleteRoleUser/
        [HttpPost]
        public ActionResult DeleteRoleUser(string roleUserIdStr)
        {
            bool bResult = RoleService.DeleteRoleUser(roleUserIdStr);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/AddRoleUser/
        [HttpPost]
        public ActionResult AddRoleUser(string roleId, string userIDstr)
        {
            bool bResult = RoleService.AddRoleUser(roleId, userIDstr);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        #region /Role/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string roleName = Request.QueryString["roleName"] ?? "";
            string meMo = Request.QueryString["meMo"] ?? "";
            string isLock = Request.QueryString["isLock"] ?? "";

            ExportParam ep = new ExportParam();
            ep.DT1 = RoleService.GetRoleConten(page, rows, roleName, meMo, isLock);
            ep.HeadTitle1 = "角色信息";
            return PrintService.Print(ep);
        }
        #endregion
    }
}
