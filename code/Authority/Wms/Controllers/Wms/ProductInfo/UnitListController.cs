﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.Security;

namespace Authority.Controllers.Wms.ProductInfo
{
    [TokenAclAuthorize]
    public class UnitListController : Controller
    {
        //
        // GET: /UnitList/

        [Dependency]
        public IUnitListService UnitListService { get; set; }

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
        // GET: /UnitList/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            UnitList ul = new UnitList();
            ul.UnitListCode = collection["UnitListCode"] ?? "";
            ul.UnitListName = collection["UnitListName"] ?? "";
            ul.UniformCode = collection["UniformCode"] ?? "";
            ul.UnitCode01 = collection["UnitCode01"] ?? "";
            ul.UnitCode02 = collection["UnitCode02"] ?? "";
            ul.UnitCode03 = collection["UnitCode03"] ?? "";
            ul.UnitCode04 = collection["UnitCode04"] ?? "";
            ul.IsActive = collection["IsActive"] ?? "";
            var users = UnitListService.GetDetails(page, rows, ul);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /UnitList/Create/

        [HttpPost]
        public ActionResult Create(UnitList unitList)
        {
            bool bResult = UnitListService.Add(unitList);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /UnitList/Delete/

        [HttpPost]
        public ActionResult Delete(string unitListCode)
        {
            bool bResult = UnitListService.Delete(unitListCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /UnitList/Edit/

        public ActionResult Edit(UnitList unitList)
        {
            bool bResult = UnitListService.Save(unitList);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        #region /UnitList/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string unitListCode = Request.QueryString["unitListCode"];
            string unitListName = Request.QueryString["unitListName"];
            string uniformCode = Request.QueryString["uniformCode"];
            string unitCode1 = Request.QueryString["unitCode1"];
            string unitCode2 = Request.QueryString["unitCode2"];
            string unitCode3 = Request.QueryString["unitCode3"];
            string unitCode4 = Request.QueryString["unitCode4"];
            string isActive = Request.QueryString["isActive"];
            UnitList unitlist = new UnitList();
            unitlist.UnitListCode = unitListCode;
            unitlist.UnitListName = unitListName;
            unitlist.UniformCode = uniformCode;
            unitlist.UnitCode01 = unitCode1;
            unitlist.UnitCode02 = unitCode2;
            unitlist.UnitCode03 = unitCode3;
            unitlist.UnitCode04 = unitCode4;
            unitlist.IsActive = isActive;

            System.Data.DataTable dt = UnitListService.GetUnitList(page, rows, unitlist);
            string headText = "单位系列";
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
