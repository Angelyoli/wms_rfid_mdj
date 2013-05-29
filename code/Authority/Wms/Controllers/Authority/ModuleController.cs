﻿using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Web.Script.Serialization;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Common.WebUtil;
using THOK.Authority.Bll.Interfaces;
using THOK.Security;
using THOK.Authority.DbModel;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;
using System;

namespace Authority.Controllers.Authority
{
    [TokenAclAuthorize]
    public class ModuleController : Controller
    {
        [Dependency]
        public IModuleService ModuleService { get; set; }

        // GET: /Module/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasFunctionAdmin = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /Module/Details/
        public ActionResult Details(string systemId)
        {
            var modules = ModuleService.GetDetails(systemId);
            return Json(modules, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details2(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "SystemName";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = ModuleService.GetDetails2(page, rows, QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /Module/Create/
        [HttpPost]
        public ActionResult Create(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemId,string parentModuleID)
        {
            bool bResult = ModuleService.Add(moduleName, showOrder, moduleUrl, indicateImage, desktopImage, systemId, parentModuleID ?? null);
            string msg = bResult ? "新增成功" : "新增失败" ;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Edit/
        [HttpPost]
        public ActionResult Edit(string moduleID, string moduleName, int showOrder, string moduleUrl, string indicateImage, string deskTopImage)
        {
            bool bResult = ModuleService.Save(moduleID, moduleName, showOrder, moduleUrl, indicateImage, deskTopImage);
            string msg = bResult ? "修改成功" : "修改失败" ;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Delete/
        [HttpPost]
        public ActionResult Delete(string moduleId)
        {
            bool bResult = ModuleService.Delete(moduleId);
            string msg = bResult ? "删除成功" : "删除失败" ;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // GET: /Module/InitRoleSystem/
        public ActionResult InitRoleSystem(string roleId,string cityId,string systemId)
        {
            ModuleService.InitRoleSys(roleId,cityId,systemId);
            return Json(1, "text", JsonRequestBehavior.AllowGet);
        }

        // GET: /Module/GetRoleSystemDetails/
        public ActionResult GetRoleSystemDetails(string roleId,string cityId,string systemId)
        {
            var modules = ModuleService.GetRoleSystemDetails(roleId,cityId,systemId);
            return Json(modules, "text", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProcessRolePermissionStr(string rolePermissionStr)
        {
            bool bResult = ModuleService.ProcessRolePermissionStr(rolePermissionStr);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/InitUserSystemInfo/
        [HttpPost]
        public ActionResult InitUserSystemInfo(string userID, string cityID, string systemID)
        {
            bool bResult = ModuleService.InitUserSystemInfo(userID, cityID, systemID);
            string msg = bResult ? "初始化成功" : "初始化失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // GET: /Module/GetUserSystemDetails/
        public ActionResult GetUserSystemDetails(string userID,string cityID,string systemID)
        {
            var modules = ModuleService.GetUserSystemDetails(userID, cityID, systemID);
            return Json(modules, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/ProcessUserPermissionStr/
        [HttpPost]
        public ActionResult ProcessUserPermissionStr(string userPermissionStr)
        {
            bool bResult = ModuleService.ProcessUserPermissionStr(userPermissionStr);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //  /Module/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            bool systemIdIsNull = true;
            Module module = new Module();
            if (Request.QueryString["systemId"] != null && Request.QueryString["systemId"] != "")
            {
                systemIdIsNull = false;
                module.System_SystemID = Guid.Parse(Request.QueryString["systemId"]);
            }

            ExportParam ep = new ExportParam();
            ep.DT1 = ModuleService.GetModules(page, rows, module,systemIdIsNull);
            ep.HeadTitle1 = "模块信息";
            return PrintService.Print(ep);
        }  
    }
}
