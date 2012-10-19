﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.ProfitLossInfo
{
    public class ProfitLossBillController : Controller
    {
        [Dependency]
        public IProfitLossBillMasterService ProfitLossBillMasterService { get; set; }
        [Dependency]
        public IProfitLossBillDetailService ProfitLossBillDetailService { get; set; }
        //
        // GET: /ProfitLossBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasAudit = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /ProfitLossBillMaster/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string WareHouseCode = collection["WareHouseCode"] ?? "";
            string beginDate = collection["beginDate"] ?? "";
            string endDate = collection["endDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string CheckPersonCode = collection["CheckPersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var profitLossBillMaster = ProfitLossBillMasterService.GetDetails(page, rows, BillNo,WareHouseCode,beginDate,endDate,OperatePersonCode,CheckPersonCode, Status, IsActive);
            return Json(profitLossBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /ProfitLossBillMaster/GenMoveBillNo/

        public ActionResult GenProfitLossBillNo()
        {
            var profitLossBillNo = ProfitLossBillMasterService.GenProfitLossBillNo(this.User.Identity.Name.ToString());
            return Json(profitLossBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Create/

        [HttpPost]
        public ActionResult Create(ProfitLossBillMaster profitLossBillMaster)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillMasterService.Add(profitLossBillMaster, this.User.Identity.Name.ToString(), out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Edit/

        [HttpPost]
        public ActionResult Edit(ProfitLossBillMaster profitLossBillMaster)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillMasterService.Save(profitLossBillMaster, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Delete/
        public ActionResult Delete(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillMasterService.Delete(BillNo, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Audit/
        public ActionResult Audit(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString(), out strResult);
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /ProfitLossBillDetail/ProfitLossBillDetails/

        public ActionResult ProfitLossBillDetails(int page, int rows, string BillNo)
        {
            var moveBillDetail = ProfitLossBillDetailService.GetDetails(page, rows, BillNo);
            return Json(moveBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillDetail/ProfitLossBillDetailDelete/

        public ActionResult ProfitLossBillDetailDelete(string ID)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillDetailService.Delete(ID, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillDetail/ProfitLossBillDetailCreate/

        [HttpPost]
        public ActionResult ProfitLossBillDetailCreate(ProfitLossBillDetail profitLossBillDetail)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillDetailService.Add(profitLossBillDetail, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillDetail/ProfitLossBillDetailEdit/

        [HttpPost]
        public ActionResult ProfitLossBillDetailEdit(ProfitLossBillDetail profitLossBillDetail)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillDetailService.Save(profitLossBillDetail, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        #region /ProfitLossBillDetail/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billNo = Request.QueryString["billNo"];
            System.Data.DataTable dt = ProfitLossBillDetailService.GetProfitLoassBillDetail(page, rows, billNo);
            string headText = "损益单明细";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize
                , 0, true, colHeadFont, colHeadSize, 0, true, 0, HeaderFooder, null, 0);
            return new FileStreamResult(ms, "application/ms-excel");
        } 
        #endregion
    }
}
