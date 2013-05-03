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
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;
namespace Authority.Controllers.Wms.StockCheckInfo
{
    [TokenAclAuthorize]
    public class CheckBillTypeController : Controller
    {
        //
        // GET: /CheckType/
        //
        // GET: /StockInBillType/
        [Dependency]
        public IBillTypeService BillTypeService { get; set; }
        //
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
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillTypeCode = collection["BillTypeCode"] ?? "";
            string BillTypeName = collection["BillTypeName"] ?? "";
            string BillClass = "0004";
            string IsActive = collection["IsActive"] ?? "";
            var brand = BillTypeService.GetDetails(page, rows, BillTypeCode, BillTypeName, BillClass, IsActive);
            return Json(brand, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(BillType billtype)
        {
            bool bResult = BillTypeService.Add(billtype);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(BillType billtype)
        {
            bool bResult = BillTypeService.Save(billtype);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string billtypeCode)
        {
            bool bResult = BillTypeService.Delete(billtypeCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        
        #region /CheckBillType/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billTypeCode = Request.QueryString["billTypeCode"];
            string billTypeName = Request.QueryString["billTypeName"];
            string billClass = "0004";
            string isActive = Request.QueryString["isActive"];

            ExportParam ep = new ExportParam();
            ep.DT1 = BillTypeService.GetBillType(page, rows, billTypeCode, billTypeName, billClass, isActive);
            ep.HeadTitle1 = "盘点类型设置";
            return PrintService.Print(ep);
        }
        #endregion
    }
}
