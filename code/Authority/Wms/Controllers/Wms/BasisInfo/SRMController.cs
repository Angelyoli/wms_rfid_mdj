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

namespace Wms.Controllers.Wms.BasisInfo
{
    [TokenAclAuthorize]
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
            SRM srm = new SRM();
            srm.SRMName = collection["SRMName"] ?? "";
            srm.OPCServiceName = collection["OPCServiceName"] ?? "";
            srm.GetRequest = collection["GetRequest"] ?? "";
            srm.GetAllow = collection["GetAllow"] ?? "";
            srm.GetComplete = collection["GetComplete"] ?? "";
            srm.PutRequest = collection["PutRequest"] ?? "";
            srm.PutAllow = collection["PutAllow"] ?? "";
            srm.PutComplete = collection["PutComplete"] ?? "";
            srm.State = collection["State"] ?? "";
            srm.State = collection["CancelTask"] ?? "";
            var srmDetail = SRMService.GetDetails(page, rows,srm);
            return Json(srmDetail, "text", JsonRequestBehavior.AllowGet);
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
            bool bResult = SRMService.Add(srm);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SRM/Edit/5
        public ActionResult Edit(SRM srm)
        {
            string strResult = string.Empty;
            bool bResult = SRMService.Save(srm);
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
            bResult = SRMService.Delete(srmId);
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
            int SRMID =Convert.ToInt32(Request.QueryString["ID"]);
            string SRMName = Request.QueryString["SRMName"];
            string OPCServiceName = Request.QueryString["OPCServiceName"];
            string GetRequest = Request.QueryString["GetRequest"];
            string GetAllow = Request.QueryString["GetAllow"];
            string GetComplete = Request.QueryString["GetComplete"];
            string PutRequest = Request.QueryString["PutRequest"];
            string PutAllow = Request.QueryString["PutAllow"];
            string PutComplete = Request.QueryString["PutComplete"];
            string Description = Request.QueryString["Description"];
            string State = Request.QueryString["State"];
            string CancelTask = Request.QueryString["CancelTask"];
            SRM srm = new SRM();
            srm.ID = SRMID;
            srm.SRMName = SRMName;
            srm.OPCServiceName = OPCServiceName;
            srm.GetRequest = GetRequest;
            srm.GetAllow = GetAllow;
            srm.GetComplete = GetComplete;
            srm.PutRequest = PutRequest;
            srm.PutAllow = PutAllow;
            srm.PutComplete = PutComplete;
            srm.Description = Description;
            srm.State = State;
            srm.CancelTask = CancelTask;

            ExportParam ep = new ExportParam();
            ep.DT1 = SRMService.GetSRM(page, rows, srm);
            ep.HeadTitle1 = "堆垛机信息";
            return PrintService.Print(ep);
        }  
    }
}
