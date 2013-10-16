using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WCS.DbModel;
using THOK.Common.WebUtil;
using THOK.WCS.Bll.Interfaces;

namespace Wms.Controllers.WCS
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
            ViewBag.hasEmpty = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /TaskManage/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            Task task = new Task();
            task.TaskType = collection["TaskType"] ?? "";
            task.ProductCode = collection["ProductCode"] ?? "";
            task.ProductName = collection["ProductName"] ?? "";
            task.OriginStorageCode = collection["OriginStorageCode"] ?? "";
            task.TargetStorageCode = collection["TargetStorageCode"] ?? "";
            task.CurrentPositionState = collection["CurrentPositionState"] ?? "";
            task.State = collection["State"] ?? "";
            task.TagState = collection["TagState"] ?? "";
            task.OrderID = collection["OrderID"] ?? "";
            task.OrderType = collection["OrderType"] ?? "";
            task.DownloadState = collection["DownloadState"] ?? "";
            string pathID = collection["PathID"] ?? "";
            string originPositionID = collection["OriginPositionID"] ?? "";
            string targetPositionID = collection["TargetPositionID"] ?? "";
            string currentPositionID = collection["CurrentPositionID"] ?? "";
            if (pathID != null && pathID != "") 
                task.PathID = Convert.ToInt32(pathID);
            if (originPositionID != null && originPositionID != "") 
                task.OriginPositionID = Convert.ToInt32(originPositionID);
            if (targetPositionID != null && targetPositionID != "") 
                task.TargetPositionID = Convert.ToInt32(targetPositionID);
            if (currentPositionID != null && currentPositionID != "") 
                task.CurrentPositionID = Convert.ToInt32(currentPositionID);

            var detail = TaskService.GetDetails(page, rows, task);
            return Json(detail, "text", JsonRequestBehavior.AllowGet);
        }
        // GET: /TaskManage/Create/
        public ActionResult Create(Task task)
        {
            string strResult = string.Empty;
            var bResult = TaskService.Add(task, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
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
        // GET: /TaskManage/Clear/
        public ActionResult Clear()
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.ClearTask(out errorInfo);
            string msg = bResult ? "清空成功" : "清空失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
