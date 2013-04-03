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
    public class RegionController : Controller
    {
        [Dependency]
        public IRegionService RegionService { get; set; }

        //
        // GET: /Region/
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
        // GET: /Region/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string RegionName = collection["RegionName"] ?? "";
            string State = collection["State"] ?? "";
            var srm = RegionService.GetDetails(page, rows,RegionName, State);
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
        // POST: /Region/Create/
        [HttpPost]
        public ActionResult Create(Region region)
        {
            string strResult = string.Empty;
            bool bResult = RegionService.Add(region, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Region/Edit/5
        public ActionResult Edit(Region region)
        {
            string strResult = string.Empty;
            bool bResult = RegionService.Save(region, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Region/Delete/
        [HttpPost]
        public ActionResult Delete(int regionId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = RegionService.Delete(regionId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Region/GetRegion/
        public ActionResult GetRegion(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "id";
            }
            if (value == null)
            {
                value = "";
            }
            var region = RegionService.GetRegion(page, rows, queryString, value);
            return Json(region, "text", JsonRequestBehavior.AllowGet);
        }

        //  /SRM/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string regionName = Request.QueryString["regionName"];
            string state = Request.QueryString["state"];

            System.Data.DataTable dt = RegionService.GetRegion(page, rows, regionName, state, null);
            string headText = "区域信息";
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
    }
}
