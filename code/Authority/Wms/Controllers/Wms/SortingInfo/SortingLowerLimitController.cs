using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.Wms.SignalR.Dispatch.Interfaces;
using THOK.Security;

namespace Authority.Controllers.Wms.SortingInfo
{
    [TokenAclAuthorize]
    public class SortingLowerlimitController : Controller
    {
        [Dependency]
        public ISortingLowerlimitService SortingLowerlimitService { get; set; }
        [Dependency]
        public ISortOrderWorkDispatchService SortOrderWorkDispatchService { get; set; }
        //
        // GET: /SortingLowerLimit/

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
        // GET: /SortingLowerLimit/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string sortingLineCode = collection["sortingLineCode"] ?? "";
            string productCode = collection["productCode"] ?? "";
            string sortingLineName = collection["sortingLineName"] ?? "";
            string productName = collection["productName"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var sortOrder = SortingLowerlimitService.GetDetails(page, rows, sortingLineCode, sortingLineName, productName, productCode, IsActive);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLowerLimit/Create/
        public ActionResult Create(SortingLowerlimit sortLowerlimin)
        {
            bool bResult = SortingLowerlimitService.Add(sortLowerlimin);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLowerLimit/Edit/
        public ActionResult Edit(SortingLowerlimit sortLowerlimin)
        {
            bool bResult = SortingLowerlimitService.Save(sortLowerlimin);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLowerLimit/Delete/
        public ActionResult Delete(string id)
        {
            bool bResult = SortingLowerlimitService.Delete(id);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //根据下限生成移库单
        // POST: /SortingLowerLimit/LowerLimitMove/
        public ActionResult LowerLimitMove(bool isEnableStocking)
        {
            string errorInfo = string.Empty;
            bool bResult = SortOrderWorkDispatchService.LowerLimitMoveLibrary(this.User.Identity.Name.ToString(), isEnableStocking, out errorInfo);
            string msg = bResult ? "生成成功" : "生成失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        #region /SortingLowerLimit/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string sortingLineCode = Request.QueryString["sortingLineCode"];
            string sortingLineName = Request.QueryString["sortingLineName"];
            string productName = Request.QueryString["productName"];
            string productCode = Request.QueryString["productCode"];
            string isActive = Request.QueryString["IsActive"];

            System.Data.DataTable dt = SortingLowerlimitService.GetSortingLowerlimit(page, rows, sortingLineCode, sortingLineName, productName,productCode, isActive);

            string headText = "备货区下限设置";
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
