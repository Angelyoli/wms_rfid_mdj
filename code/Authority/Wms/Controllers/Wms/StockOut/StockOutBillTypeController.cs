using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
namespace Authority.Controllers.Wms.StockOut
{
    public class StockOutBillTypeController : Controller
    {
        //
        // GET: /StockOutBillType/
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
            string BillClass = "0002";
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

        #region /StockOutBillType/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billTypeCode = Request.QueryString["billTypeCode"];
            string billTypeName = Request.QueryString["billTypeName"];
            string billClass = "0002";
            string isActive = Request.QueryString["isActive"];

            System.Data.DataTable dt = BillTypeService.GetBillType(page, rows, billTypeCode, billTypeName, billClass, isActive);
            string headText = "出库类型设置";
            string headFont = "微软雅黑"; short headSize = 20;
            string colHeadFont = "Arial"; short colHeadSize = 10; short colHeadWidth = 300;
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            this.GetResponse(headText);

            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize,
                colHeadFont, colHeadSize, colHeadWidth, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion

        #region this.GetResponse
        private void GetResponse(string headText)
        {
            string filename = headText + System.DateTime.Now.ToString("yyMMdd-HHmm-ss");
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";
        }
        #endregion
    }
}
