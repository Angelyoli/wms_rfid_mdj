using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.WCS.DbModel;
using THOK.Common.WebUtil;
using THOK.Security;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Wms.Controllers.Wms.BasisInfo
{
    [TokenAclAuthorize]
    public class ProductSizeController : Controller
    {
        [Dependency]
        public IProductSizeService ProductSizeService { get; set; }
        //
        // GET: /ProductSize/

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
        // GET: /ProductSize/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            ProductSize productSize = new ProductSize();
            productSize.ProductCode = collection["ProductCode"] ?? "";
            //productSize.ProductName = collection["ProductName"] ?? "";
            string ProductNo = collection["ProductNo"] ?? "";
            string SizeNo = collection["SizeNo"] ?? "";
            string AreaNo = collection["AreaNo"] ?? "";
            if (SizeNo != "" && SizeNo != null)
            {
                productSize.SizeNo = Convert.ToInt32(SizeNo);
            }
            if (AreaNo != "" && AreaNo != null)
            {
                productSize.AreaNo = Convert.ToInt32(AreaNo);
            }
            if (ProductNo != "" && ProductNo != null)
            {
                productSize.ProductNo = Convert.ToInt32(ProductNo);
            }
            var productSizeDetail = ProductSizeService.GetDetails(page, rows, productSize);
            return Json(productSizeDetail, "text", JsonRequestBehavior.AllowGet);
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
        // POST: /ProductSize/Create/
        [HttpPost]
        public ActionResult Create(ProductSize productSize)
        {
            string strResult = string.Empty;
            bool bResult = ProductSizeService.Add(productSize);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProductSize/Edit/5
        public ActionResult Edit(ProductSize productSize)
        {
            string strResult = string.Empty;
            bool bResult = ProductSizeService.Save(productSize);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /ProductSize/Delete/
        [HttpPost]
        public ActionResult Delete(int productSizeId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = ProductSizeService.Delete(productSizeId);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /ProductSize/GetProductSize/
        public ActionResult GetProductSize(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "ID";
            }
            if (value == null)
            {
                value = "";
            }
            var productSize = ProductSizeService.GetProductSize(page, rows, queryString, value);
            return Json(productSize, "text", JsonRequestBehavior.AllowGet);
        }

        //  /ProductSize/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string ProductCode = Request.QueryString["ProductCode"];
            int ProductNo =Convert .ToInt32( Request.QueryString["ProductNo"]);
            //int SizeNo =Convert .ToInt32( Request.QueryString["SizeNo"]);
            int Length =Convert .ToInt32( Request.QueryString["Length"]);
            int Width =Convert .ToInt32( Request.QueryString["Width"]);
            int Height =Convert .ToInt32( Request.QueryString["Height"]);
            int AreaNo =Convert .ToInt32( Request.QueryString["AreaNo"]);
            ProductSize productSize = new ProductSize();
            productSize.ProductCode = ProductCode;
            productSize.ProductNo = ProductNo;
            //productSize.SizeNo = SizeNo;
            productSize.Length = Length;
            productSize.Width = Width;
            productSize.Height = Height;
            productSize.AreaNo = AreaNo;

            ExportParam ep = new ExportParam();
            ep.DT1 = ProductSizeService.GetProductSize(page, rows, productSize);
            ep.HeadTitle1 = "卷烟件烟尺寸信息";
            return PrintService.Print(ep);
        }  
    }
}
