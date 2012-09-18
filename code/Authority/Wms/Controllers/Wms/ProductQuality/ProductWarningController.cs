using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using THOK.WebUtil;

namespace Wms.Controllers.Wms.ProductQuality
{
    public class ProductWarningController : Controller
    {
        //
        // GET: /ProductWarning/
        [Dependency]
        public IProductWarningService ProductWarningService { get; set; }

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
            string productCode = collection["ProductCode"] ?? "";
            decimal minLimited=100000;
            decimal maxLimited=100000;
            decimal assemblyTime=3600;
            if (collection["MinLimited"] != null && collection["MinLimited"] != "")
            {
                minLimited = decimal.Parse(collection["MinLimited"]);
            }
            if (collection["MaxLimited"] != null && collection["MaxLimited"] != "")
            {
                maxLimited = decimal.Parse(collection["MaxLimited"]);
            }
            if (collection["AssemblyTime"] != null && collection["AssemblyTime"] != "")
            {
                assemblyTime = decimal.Parse(collection["AssemblyTime"]);
            }
            var productWarn = ProductWarningService.GetDetail(page, rows, productCode, minLimited, maxLimited, assemblyTime);
            return Json(productWarn, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(ProductWarning productWarning)
        {
            bool bResult = ProductWarningService.Add(productWarning);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string ProductCode)
        {
            bool bResult = ProductWarningService.Delete(ProductCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(ProductWarning productWarning)
        {
            bool bResult = ProductWarningService.Save(productWarning);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWarningPrompt()
        {
            var productWarn = ProductWarningService.GetWarningPrompt();
            return Json(productWarn, "text", JsonRequestBehavior.AllowGet);
        }

        #region /ProductWarning/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string productCode = Request.QueryString["productCode"];
            decimal minLimited = Convert.ToDecimal( Request.QueryString["minLimited"]);
            decimal maxLimited = Convert.ToDecimal(Request.QueryString["maxLimited"]);
            decimal assemblyTime = Convert.ToDecimal(Request.QueryString["assemblyTime"]);

            System.Data.DataTable dt = ProductWarningService.GetProductWarning(page, rows, productCode,minLimited,maxLimited,assemblyTime);
            string headText = "产品预警信息设置";
            string headFontName = "微软雅黑"; Int16 headFontSize = 20;
            string colHeadFontName = "Arial"; Int16 colHeadFontSize = 10;
           
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFontName, headFontSize, colHeadFontName, colHeadFontSize);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
