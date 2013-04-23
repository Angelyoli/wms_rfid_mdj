﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.WebUtil;
using THOK.Security;

namespace Wms.Controllers.Wms.BasisInfo
{
    [TokenAclAuthorize]
    public class SizeController : Controller
    {
        [Dependency]
        public ISizeService SizeService { get; set; }

        //
        // GET: /Size/

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
        // GET: /Size/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string SizeName = collection["SizeName"] ?? "";
            string SizeNo = collection["SizeNo"] ?? "";
            var srm = SizeService.GetDetails(page, rows, SizeName, SizeNo);
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
        // POST: /Size/Create/
        [HttpPost]
        public ActionResult Create(Size size)
        {
            string strResult = string.Empty;
            bool bResult = SizeService.Add(size);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Size/Edit/5
        public ActionResult Edit(Size size)
        {
            string strResult = string.Empty;
            bool bResult = SizeService.Save(size);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Size/Delete/
        [HttpPost]
        public ActionResult Delete(int sizeId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = SizeService.Delete(sizeId);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Size/GetSize/
        public ActionResult GetSize(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "ID";
            }
            if (value == null)
            {
                value = "";
            }
            var srm = SizeService.GetSize(page, rows, queryString, value);
            return Json(srm, "text", JsonRequestBehavior.AllowGet);
        }

       //  /Size/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string sizeName = Request.QueryString["sizeName"];
            //string state = Request.QueryString["state"];

            THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
            ep.DT1 = SizeService.GetSize(page, rows, sizeName);
            ep.HeadTitle1 = "件烟尺寸信息";
            System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }  

    }
}
