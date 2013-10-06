using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.Common.WebUtil;
using THOK.WCS.Bll.Models;

namespace Wms.Controllers.WCS
{
    public class TaskRestController : Controller
    {
        [Dependency]
        public ITaskService TaskService { get; set; }

        public ActionResult CreateNewTaskForEmptyPalletStack(int positionID,string positionName)
        {
            bool bResult = TaskService.CreateNewTaskForEmptyPalletStack(positionID,positionName);
            return Json(new RestReturn() { IsSuccess = bResult, Message = "todo" }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewTaskForEmptyPalletSupply(int positionID,string positionName)
        {
            bool bResult = TaskService.CreateNewTaskForEmptyPalletSupply(positionID,positionName);
            return Json(new RestReturn() { IsSuccess = bResult, Message = "todo" }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewTaskForMoveBackRemain(int positionID)
        {
            bool bResult = TaskService.CreateNewTaskForMoveBackRemain(positionID);
            return Json(new RestReturn() { IsSuccess = bResult, Message = "todo" }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishTaskUseTaskID(int taskID)
        {
            bool bResult = TaskService.FinishTask(taskID);
            return Json(new RestReturn() { IsSuccess = bResult, Message = "todo" }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishTask(int taskID, string orderType, string orderID, int allotID, string originStorageCode, string targetStorageCode)
        {
            bool bResult = TaskService.FinishTask(taskID, orderType, orderID, allotID,originStorageCode,targetStorageCode);
            return Json(new RestReturn() { IsSuccess = bResult, Message = "todo" }, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
