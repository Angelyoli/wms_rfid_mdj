using System;
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
            Region reg = new Region();
            reg.RegionName = collection["RegionName"] ?? "";
            reg.State = collection["State"] ?? "";
            var regDetail = RegionService.GetDetails(page, rows, reg);
            return Json(regDetail, "text", JsonRequestBehavior.AllowGet);
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
            bool bResult = RegionService.Add(region);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Region/Edit/5
        public ActionResult Edit(Region region)
        {
            string strResult = string.Empty;
            bool bResult = RegionService.Save(region);
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
            bResult = RegionService.Delete(regionId);
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
             int RegionID = Convert.ToInt32(Request.QueryString["ID"]);
             string RegionName = Request.QueryString["RegionName"];
             string Description = Request.QueryString["Description"];
             string State = Request.QueryString["State"];
             Region region = new Region();
             region.ID = RegionID;
             region.RegionName = RegionName;
             region.Description = Description;
             region.State = State;

             THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
             ep.DT1 = RegionService.GetRegion(page, rows, region);
             ep.HeadTitle1 = "区域信息";
             System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
             return new FileStreamResult(ms, "application/ms-excel");
          
        }  
    }
}
