using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Wms.Controllers.Wms.BasisInfo
{
    public class SRMController : Controller
    {
        [Dependency]
        public ISRMService SRMService { get; set; }
        //
        // GET: /SRM/

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
        // GET: /SRM/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string SRMName = collection["SRMName"] ?? "";
            string State = collection["State"] ?? "";
            var srm = SRMService.GetDetails(page, rows,SRMName,State);
            return Json(srm, "text", JsonRequestBehavior.AllowGet);
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
        // POST: /SRM/Create/
        [HttpPost]
        public ActionResult Create(SRM srm)
        {
            string strResult = string.Empty;
            bool bResult = SRMService.Add(srm, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SRM/Edit/5
        public ActionResult Edit(SRM srm)
        {
            string strResult = string.Empty;
            bool bResult = SRMService.Save(srm, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SRM/Delete/
        [HttpPost]
        public ActionResult Delete(int  srmId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = SRMService.Delete(srmId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /SRM/GetSRM/
        public ActionResult GetSRM(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "ID";
            }
            if (value == null)
            {
                value = "";
            }
            var srm = SRMService.GetSRM(page, rows, queryString, value);
            return Json(srm, "text", JsonRequestBehavior.AllowGet);
        }

        //  /SRM/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            //int id=0;// =Convert .ToInt32( Request.QueryString["id"]);
            //if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            //{
            //    id = Convert.ToInt32(Request.QueryString["id"]);
            //}
            string srmName = Request.QueryString["srmName"];
            string state = Request.QueryString["state"];

            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = SRMService.GetSRM(page, rows, srmName, state,null);
            ep.HeadTitle1 = "堆垛机信息";
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }  
    }
}
