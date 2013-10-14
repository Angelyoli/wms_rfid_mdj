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

        public ActionResult CreateNewTaskForEmptyPalletStack(string positionName)
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.CreateNewTaskForEmptyPalletStack(0,positionName,out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = "todo" }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewTaskForEmptyPalletSupply(string positionName)
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.CreateNewTaskForEmptyPalletSupply(0,positionName,out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = "todo" }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewTaskForMoveBackRemain(int taskID)
        {
            bool bResult = TaskService.CreateNewTaskForMoveBackRemain(taskID);
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

        public ActionResult FinishStockOutTask(int taskID,int stockOutQuantity)
        {
            int iResult = TaskService.FinishStockOutTask(taskID, stockOutQuantity);
            return Json(new RestReturn() { IsSuccess = true, Message = "todo", Data = iResult }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishInventoryTask(int taskID,int realQuantity)
        {
            int iResult = TaskService.FinishInventoryTask(taskID, realQuantity);
            return Json(new RestReturn() { IsSuccess = true, Message = "todo" , Data = iResult}, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
