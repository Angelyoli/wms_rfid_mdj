using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.WCS.DbModel;
using THOK.Common.WebUtil;
using THOK.Security;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Wms.Controllers.WCS
{
    [TokenAclAuthorize]
    public class PositionController : Controller
    {
        [Dependency]
        public IPositionService PositionService { get; set; }
        //
        // GET: /Position/

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
        public ActionResult SearchPage()
        {
            return View();
        }

        public ActionResult AddPage()
        {
            return View();
        }

        //
        // GET: /Position/Details/5

        public ActionResult Details (int page, int rows, FormCollection collection)
        {
            Position position = new Position();
            position.PositionName = collection["PositionName"] ?? "";
            position.PositionType = collection["PositionType"] ?? "";
            position.SRMName = collection["SRMName"] ?? "";
            position.State = collection["State"] ?? "";
           
            var positionDetail = PositionService.GetDetails(page, rows, position);
            return Json(positionDetail, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Position/Create

        [HttpPost]
        public ActionResult Create(Position position)
        {
            string strResult = string.Empty;
            bool bResult = PositionService.Add(position);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        
      
        //
        // POST: /Position/Edit/5

        [HttpPost]
        public ActionResult Edit(Position position)
        {
            string strResult = string.Empty;
            bool bResult = PositionService.Save(position);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Position/Delete/5

        [HttpPost]
        public ActionResult Delete(int positionId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = PositionService.Delete(positionId);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
         // POST: /Position/GetPosition/
        public ActionResult GetPosition(int page, int rows, string queryString, string value)
        {
            
            if (queryString == null)
            {
                queryString = "PositionName";
            }
            if (value == null)
            {
                value = "";
            }
            var employee = PositionService.GetPosition(page, rows, queryString, value);
            return Json(employee, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Position/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string positionName = Request.QueryString["positionName"];
            string PositionType = Request.QueryString["PositionType"];
            string SRMName = Request.QueryString["SRMName"];
            string State = Request.QueryString["State"];
            Position position = new Position();
            position.PositionName = positionName;
            position.PositionType = PositionType;
            position.SRMName = SRMName;
            position.State = State;

            ExportParam ep = new ExportParam();
            ep.DT1 = PositionService.GetPosition(page, rows, position);
            ep.HeadTitle1 = "位置信息";
            return PrintService.Print(ep);
        } 
        #endregion

    }
}
    

