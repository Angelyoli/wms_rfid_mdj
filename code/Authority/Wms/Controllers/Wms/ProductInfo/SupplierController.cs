using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.ProductInfo
{
    public class SupplierController : Controller
    {
        [Dependency]
        public ISupplierService SupplierService { get; set; }
        //
        // GET: /Supplier/

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
        // GET: /Supplier/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string SupplierCode = collection["SupplierCode"] ?? "";
            string SupplierName = collection["SupplierName"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var supplier=SupplierService.GetDetails(page,rows,SupplierCode,SupplierName,IsActive);
            return Json(supplier, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Supplier/Create/

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            bool bResult = SupplierService.Add(supplier);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Supplier/Edit/

        public ActionResult Edit(Supplier supplier)
        {
            bool bResult = SupplierService.Save(supplier);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Supplier/Delete/

        [HttpPost]
        public ActionResult Delete(string supplierCode)
        {
            bool bResult = SupplierService.Delete(supplierCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        #region /Supplier/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string supplierCode = Request.QueryString["supplierCode"];
            string supplierName = Request.QueryString["supplierName"];
            string isActive = Request.QueryString["isActive"];

            System.Data.DataTable dt = SupplierService.GetSupplier(page, rows, supplierCode, supplierName, isActive);
            string headText = "厂商信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10; 
            
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize,
                colHeadFont, colHeadSize);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
