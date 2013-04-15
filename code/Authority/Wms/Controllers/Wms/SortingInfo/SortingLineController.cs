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

namespace Authority.Controllers.Wms.SortingInfo
{
    [TokenAclAuthorize]
    public class SortingLineController : Controller
    {
        [Dependency]
        public ISortingLineService SortingLineService { get; set; }
        //
        // GET: /SortingLine/
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
        // GET: /SortingLine/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string sortingLineCode = collection["sortingLineCode"] ?? "";
            string sortingLineName = collection["sortingLineName"] ?? "";
            string SortingLineType = collection["SortingLineType"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var sortOrder = SortingLineService.GetDetails(page, rows, sortingLineCode, sortingLineName, SortingLineType, IsActive);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /SortingLine/GetSortLine/
        public ActionResult GetSortLine()
        {
            var sortOrder = SortingLineService.GetSortLine();
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLine/Create/
        public ActionResult Create(SortingLine sortLine)
        {
            bool bResult = SortingLineService.Add(sortLine);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLine/Edit/
        public ActionResult Edit(SortingLine sortLine)
        {
            bool bResult = SortingLineService.Save(sortLine);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLine/Delete/
        public ActionResult Delete(string sortLineCode)
        {
            bool bResult = SortingLineService.Delete(sortLineCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        #region /SortingLine/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string sortingLineCode = Request.QueryString["sortingLineCode"];
            string sortingLineName = Request.QueryString["sortingLineName"];
            string sortingLineType = Request.QueryString["SortingLineType"];
            string isActive = Request.QueryString["IsActive"];
            System.Data.DataTable dt = SortingLineService.GetSortingLine(page, rows, sortingLineCode, sortingLineName, sortingLineType, isActive);
            string headText = "分拣线信息设置";
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
