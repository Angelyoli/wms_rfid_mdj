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
            return Json(new RestReturn() { IsSuccess = bResult, Message = errorInfo }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewTaskForEmptyPalletSupply(string positionName)
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.CreateNewTaskForEmptyPalletSupply(0,positionName,out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = errorInfo }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewTaskForMoveBackRemain(int taskID)
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.CreateNewTaskForMoveBackRemain(taskID, out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = errorInfo }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishTaskUseTaskID(int taskID)
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.FinishTask(taskID, out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = errorInfo }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishTask(int taskID, string orderType, string orderID, int allotID, string originCellCode, string targetCellCode, string originStorageCode, string targetStorageCode)
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.FinishTask(taskID, orderType, orderID, allotID,originCellCode,targetCellCode, originStorageCode, targetStorageCode, out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = errorInfo }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishStockOutTask(int taskID,int stockOutQuantity)
        {
            string errorInfo = string.Empty;
            int iResult = TaskService.FinishStockOutTask(taskID, stockOutQuantity, out errorInfo);
            return Json(new RestReturn() { IsSuccess = true, Message = errorInfo, Data = iResult }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishInventoryTask(int taskID,int realQuantity)
        {
            string errorInfo = string.Empty;
            int iResult = TaskService.FinishInventoryTask(taskID, realQuantity, out errorInfo);
            return Json(new RestReturn() { IsSuccess = true, Message = errorInfo, Data = iResult }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoCreateMoveBill()
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.CreateAutoMoveBill(out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = errorInfo }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOutTask(string positionType,string orderType)
        {
            RestReturn restReturn = new RestReturn();
            TaskService.GetOutTask(positionType, orderType, restReturn);
            return Json(restReturn, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishOutTask(string taskID)
        {
            RestReturn restReturn = new RestReturn();
            TaskService.FinishTask(taskID, restReturn);
            return Json(restReturn, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAutoMoveCell()
        {
            string errorInfo = string.Empty;
            bool bResult = TaskService.CreateAutoMoveCell(out errorInfo);
            return Json(new RestReturn() { IsSuccess = bResult, Message = errorInfo }, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
