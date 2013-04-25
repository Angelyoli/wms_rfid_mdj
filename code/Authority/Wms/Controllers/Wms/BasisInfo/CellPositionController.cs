using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.Security;

namespace Wms.Controllers.Wms.BasisInfo
{
    [TokenAclAuthorize]
    public class CellPositionController : Controller
    {
        [Dependency]
        public ICellPositionService CellPositionService { get; set; }
        //
        // GET: /CellPosition/

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

        //
        // GET: /CellPosition/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
           
            string CellCode = collection["CellCode"] ?? "";
            string StockInPosition = collection["StockInPosition"] ?? "";
            string StockOutPosition = collection["StockOutPosition"] ?? "";
            var productSize = CellPositionService.GetDetails(page, rows,CellCode,StockInPosition,StockOutPosition);
            return Json(productSize, "text", JsonRequestBehavior.AllowGet);
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
        // POST: /CellPosition/Create/
        [HttpPost]
        public ActionResult Create(CellPosition cellPosition)
        {
            string strResult = string.Empty;
            bool bResult = CellPositionService.Add(cellPosition);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /CellPosition/Edit/5
        public ActionResult Edit(CellPosition cellPosition)
        {
            string strResult = string.Empty;
            bool bResult = CellPositionService.Save(cellPosition, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);

        }


        //
        // POST: /CellPosition/Delete/
        [HttpPost]
        public ActionResult Delete(int cellPositionId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = CellPositionService.Delete(cellPositionId);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        #region /CellPosition/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            int CellPositionID = Convert.ToInt32(Request.QueryString["ID"]);
            string CellCode = Request.QueryString["CellCode"];
            CellPosition cp= new CellPosition();
            cp.ID = CellPositionID;
            cp.CellCode = CellCode;
            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = CellPositionService.GetCellPosition(page, rows, cp);
            ep.HeadTitle1 = "货位信息";
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
           
        }
        #endregion
    }
}
