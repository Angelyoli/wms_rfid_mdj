﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.Inventory
{
    public class DistributionController : Controller
    {
        //
        // GET: /Distribution/

        [Dependency]
        public IDistributionService DistributionService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /Distribution/Details/

        //public ActionResult Details(int page, int rows, FormCollection collection)
        //{
        //    string productCode = collection["ProductCode"] ?? "";
        //    string ware = collection["Ware"] ?? "";
        //    string area = collection["Area"] ?? "";
        //    string unitType = collection["UnitType"] ?? "";
        //    var storage = DistributionService.GetAreaDetails(page, rows, productCode, ware, area, unitType);
        //    return Json(storage, "text", JsonRequestBehavior.AllowGet);
        //}

        //
        // GET: /Distribution/GetCellDetails/
        public ActionResult GetCellDetails(int page, int rows, string type, string id, string unitType, string productCode)
        {
            var storage = DistributionService.GetCellDetails(page, rows, type, id, unitType, productCode);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //获取库存商品分布树
        // GET: /Distribution/GetProductTreeDetails/
        public ActionResult GetProductTreeDetails(string productCode, string unitType)
        {
            var wareCell = DistributionService.GetProductTree(productCode,unitType);
            return Json(wareCell, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Distribution/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string type = Request.QueryString["type"];
            string id = Request.QueryString["id"];
            string unitType = Request.QueryString["unitType"];
            string productCode = Request.QueryString["productCode"];

            System.Data.DataTable dt = DistributionService.GetDistribution(page, rows, type, id, unitType, productCode);
            string headText = "库存分布查询";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10;
            string[] HeaderFooder = {   
                                         "……"    //眉左
                                        ,"……"  //眉中
                                        ,"……"    //眉右
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
