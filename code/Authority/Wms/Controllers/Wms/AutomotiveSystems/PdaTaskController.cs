using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.WCS.Bll.Models;
using THOK.WCS.Bll.Service;
using THOK.WCS.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Common.WebUtil;

namespace Wms.Controllers.WMS.AutomotiveSystems
{
    public class PdaTaskController : Controller
    {
        [Dependency]
        public ITaskService TaskService { get; set; }
        //
        // GET: /PdaTask/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            //ViewBag.hasApply = true;
            //ViewBag.hasCancel = true;
            ViewBag.hasFinish = true;
            //ViewBag.hasBatch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        
        //PDA读取数据
        // GET: /PdaTask/GetOutTask
        public ActionResult GetOutTask(string positionType, string orderType)
        {
            RestReturn restReturn = new RestReturn();
            TaskService.GetOutTask(positionType, orderType, restReturn);
            return Json(restReturn.RestTasks, "text", JsonRequestBehavior.AllowGet);
        }

        //PDA完成数据
        // GET: /PdaTask/FinishOutTask
        public ActionResult FinishOutTask(string taskID)
        {
            RestReturn restReturn = new RestReturn();
            TaskService.FinishTask(taskID, restReturn);
            return Json(JsonMessageHelper.getJsonMessage(restReturn.IsSuccess, restReturn.IsSuccess ? "完成成功！" : "完成失败！", restReturn.Message), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
