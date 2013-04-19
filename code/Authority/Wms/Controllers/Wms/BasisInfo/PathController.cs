using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Common.WebUtil;
using THOK.Wms.DbModel;
using THOK.Security;

namespace Wms.Controllers.Wms.BasisInfo
{
    [TokenAclAuthorize]
    public class PathController : Controller
    {
       
            [Dependency]
            public IPathService PathService { get; set; }

            //
            // GET: /Path/Index

            public ActionResult Index(string moduleID)
            {
                ViewBag.hasSearch = true;
                ViewBag.hasAdd = true;
                ViewBag.hasEdit = true;
                ViewBag.hasDelete = true;
                ViewBag.hasNode = true;
                ViewBag.hasPrint = true;
                ViewBag.hasHelp = true;
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
            // GET: /Path/Details/5

            public ActionResult Details(int page, int rows, FormCollection collection)
            {
               
                string PathName = collection["PathName"] ?? "";
                string State = collection["State"] ?? "";
                string OriginRegion = collection["OriginID"] ?? "";
                string TargetRegion = collection["TargetID"] ?? "";
                var systems = PathService.GetDetails(page, rows, OriginRegion, PathName, TargetRegion, State);
                return Json(systems, "text", JsonRequestBehavior.AllowGet);
            }


            //
            // POST: /Path/Create

            [HttpPost]
            public ActionResult Create(Path path)
            {
                string strResult = string.Empty;
                bool bResult = PathService.Add(path, out strResult);
                string msg = bResult ? "新增成功" : "新增失败";
                return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
            }

          
            //
            // POST: /Path/Edit/5

            [HttpPost]
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





            // POST: /Path/GetPath/
            public ActionResult GetPath(int page, int rows, string queryString, string value)
            {
                if (queryString == null)
                {
                    queryString = "PathName";
                }
                if (value == null)
                {
                    value = "";
                }
                var path = PathService.GetPath(page, rows, queryString, value);
                return Json(path, "text", JsonRequestBehavior.AllowGet);
            }

            #region /Path/CreateExcelToClient/
            public FileStreamResult CreateExcelToClient()
            {
                int page = 0, rows = 0;
                string Id = Request.QueryString["Id"];
                string pathName = Request.QueryString["pathName"];
                string originId = Request.QueryString["originId"];
                string targetId = Request.QueryString["targetId"];
                
                string state = Request.QueryString["state"];

                THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
                ep.DT1 = PathService.GetPath(page, rows, Id, pathName, originId, targetId, state);
                ep.HeadTitle1 = "路径信息";
                System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
                return new FileStreamResult(ms, "application/ms-excel");
            }
            #endregion
    }
}



