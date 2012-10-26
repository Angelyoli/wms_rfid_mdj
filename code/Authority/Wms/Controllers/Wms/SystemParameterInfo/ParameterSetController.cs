using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
using THOK.Wms.DbModel;

namespace Wms.Controllers.Wms.SystemParameterInfo
{
    public class ParameterSetController : Controller
    {
        //
        // GET: /ParameterSet/

        [Dependency]
        public ISystemParameterService SystemParameterService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        // GET: /ParameterSet/GetSystemParameter/
        public ActionResult GetSystemParameter(FormCollection collection)
        {
            string parameterName = collection["ParameterName"] ?? "";
            var result = SystemParameterService.GetSystemParameter(parameterName);
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
        // GET: /ParameterSet/AddSystemParameter/
        public ActionResult AddSystemParameter(SystemParameter systemParameter)
        {
            string error = string.Empty;
            bool bResult = SystemParameterService.AddSystemParameter(systemParameter, out error);
            string msg = bResult ? "添加成功" : "添加失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, error), "text", JsonRequestBehavior.AllowGet);
        }
        // GET: /ParameterSet/SetSystemParameter/
        public ActionResult SetSystemParameter(SystemParameter systemParameter)
        {
            string error = string.Empty;
            bool bResult = SystemParameterService.SetSystemParameter(systemParameter, out error);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, error), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
