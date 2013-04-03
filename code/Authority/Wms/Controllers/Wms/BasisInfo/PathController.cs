using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;


namespace Wms.Controllers.Wms.Organization
{
    public class PathController : Controller
    {
        [Dependency]
        public IPathService PathService { get; set; }
        //
        // GET: /Path/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
           
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult AddPage()
        {
            return View();
        }

        public ActionResult SearchPage()
        {
            return View();
        }

        //
        // GET: /Path/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string ID = collection["ID"] ?? "";
            string PathName = collection["PathName"] ?? "";
            string Description = collection["Description"] ?? "";
            string State = collection["State"] ?? "";
            string RegionID = collection["RegionID"] ?? "";
            //string OriginRegionID = collection["OriginRegionID"] ?? "";
            //string  TargetRegionID = collection["TargetRegionID"] ?? "";

            var path = PathService.GetDetails(page, rows, ID, PathName, RegionID, Description, State);
            return Json(path, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Path/Create/

        [HttpPost]
        public ActionResult Create(Path path)
        {
            string strResult = string.Empty;
            bool bResult = PathService.Add(path, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Path/Edit/5

        public ActionResult Edit(Path path)
        {
            string strResult = string.Empty;
            bool bResult = PathService.Save(path, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Path/Delete/

        [HttpPost]
        public ActionResult Delete(int pathId)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = PathService.Delete(pathId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //  POST: /Path/GetPath/
        public ActionResult GetPath(int page, int rows,string value)
        {
            if (value == null)
            {
                value = "";
            }
            var path = PathService.GetPath(page, rows, value);
            return Json(path, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Path/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string PathName = Request.QueryString["PathName"];
            string Description = Request.QueryString["Description"];
            string State = Request.QueryString["State"];

            System.Data.DataTable dt = PathService.GetPath(page, rows, PathName, Description, State);
            string headText = "路径信息";
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