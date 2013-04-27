using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using THOK.Common.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Security;
namespace Authority.Controllers.ProductInfo
{
    [TokenAclAuthorize]
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        [Dependency]
        public IProductService ProductService { get; set; }

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
            string productName = collection["ProductName"] ?? "";
            string productCode = collection["ProductCode"] ?? "";
            string customCode = collection["CustomCode"] ?? "";
            string brandCode = collection["BrandCode"] ?? "";
            string uniformCode = collection["UniformCode"] ?? "";
            string abcTypeCode = collection["AbcTypeCode"] ?? "";
            string shortCode = collection["ShortCode"] ?? "";
            string priceLevelCode = collection["PriceLevelCode"] ?? "";
            string supplierCode = collection["SupplierCode"] ?? "";
            var users = ProductService.GetDetails(page, rows, productName, productCode, customCode, brandCode, uniformCode, abcTypeCode, shortCode, priceLevelCode, supplierCode);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(Product product)
        {
            bool bResult = ProductService.Add(product);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string ProductCode)
        {
            bool bResult = ProductService.Delete(ProductCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(Product product)
        {
            bool bResult = ProductService.Save(product);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //下限查询卷烟信息 2012年7月24日 16:29:25
        // GET: /Product/FindProduct/
        public ActionResult FindProduct(int page, int rows, string QueryString, string value)
        {
            var product = ProductService.FindProduct(page, rows, QueryString, value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        //盘点查询卷烟信息 
        // GET: /Product/CheckFindProduct/
        public ActionResult CheckFindProduct(string QueryString, string value)
        {
            if (QueryString == null)
            {
                QueryString = "ProductCode";
            }
            if (value == null)
            {
                value = "";
            }
            var product = ProductService.checkFindProduct(QueryString,value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        // GET: /Product/GetProductBy/
        public ActionResult GetProductBy(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "ProductCode";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = ProductService.GetProductBy(page,rows,QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet); 
         }


        #region /Product/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string productName = Request.QueryString["productName"];
            string productCode = Request.QueryString["productCode"];
            string customCode = Request.QueryString["customCode"];
            string brandCode = Request.QueryString["brandCode"];
            string uniformCode = Request.QueryString["uniformCode"];
            string abcTypeCode = Request.QueryString["abcTypeCode"];
            string shortCode = Request.QueryString["shortCode"];
            string priceLevelCode = Request.QueryString["priceLevelCode"];
            string supplierCode = Request.QueryString["supplierCode"];

            THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
            ep.DT1 = ProductService.GetProduct(page, rows, productName, productCode, customCode, brandCode, uniformCode, abcTypeCode, shortCode, priceLevelCode, supplierCode);
            ep.HeadTitle1 = "卷烟信息";
            System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
