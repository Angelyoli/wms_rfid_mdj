using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.WebUtil;

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
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            Task task = new Task();
            task.TaskType = collection["TaskType"] ?? "";
            task.CurrentPositionState = collection["CurrentPositionState"] ?? "";
            task.State = collection["Status"] ?? "";
            task.OrderType = collection["OrderType"] ?? "";
            task.DownloadState = collection["DownloadState"] ?? "";

            var detail = TaskService.GetDetails(page, rows, task);
            return Json(detail, "text", JsonRequestBehavior.AllowGet);
        }
        // GET: /TaskManage/Create/
        public ActionResult Create(Task task)
        {
            string strResult = string.Empty;
            var bResult = TaskService.Add(task, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        // GET: /TaskManage/Edit/
        public ActionResult Edit(Task task)
        {
            string strResult = string.Empty;
            bool bResult = TaskService.Save(task, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        // GET: /TaskManage/Delete/
        public ActionResult Delete(string taskId)
        {
            string strResult = string.Empty;
            bool bResult = TaskService.Delete(taskId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
