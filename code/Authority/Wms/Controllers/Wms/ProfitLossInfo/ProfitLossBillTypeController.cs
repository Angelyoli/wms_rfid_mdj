using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.Security;

namespace Authority.Controllers.Wms.ProfitLossInfo
{
    [TokenAclAuthorize]
    public class ProfitLossBillTypeController : Controller
    {
        //
        // GET: /StockInBillType/
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
            string BillClass = "0005";
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

        #region /ProfitLossBillType/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billTypeCode = Request.QueryString["billTypeCode"];
            string billTypeName = Request.QueryString["billTypeName"];
            string billClass = "0005";
            string isActive = Request.QueryString["isActive"];

            System.Data.DataTable dt = BillTypeService.GetBillType(page, rows, billTypeCode, billTypeName, billClass, isActive);
            string headText = "损益类型设置";
            string headFont = "微软雅黑"; short headSize = 20;
            string colHeadFont = "Arial"; short colHeadSize = 10;
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
