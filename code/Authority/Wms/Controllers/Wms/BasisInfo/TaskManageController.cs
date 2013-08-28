using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.WMS.BasisInfo
{
    public class TaskManageController : Controller
    {
        [Dependency]
        public ITaskService TaskService { get; set; }

        //
        // GET: /TaskManage/

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
        // GET: /TaskManage/Details/
        public ActionResult Details(int page, int rows)
        {
            var detail = TaskService.GetDetails(page, rows);
            return Json(detail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
