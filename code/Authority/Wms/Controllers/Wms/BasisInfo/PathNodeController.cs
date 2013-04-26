using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.WebUtil;
using THOK.Security;

namespace Wms.Controllers.Wms.BasisInfo
{
    [TokenAclAuthorize]
    public class PathNodeController : Controller
    {
        [Dependency]
        public IPathNodeService PathNodeService { get; set; }
        //
        // GET: /PathNode/
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
        // GET: /PathNode/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            PathNode pathNode = new PathNode();
            //string ID = collection["ID"] ?? "";
            string PathName = collection["PathName"] ?? "";
            string PositionName = collection["PositionName"] ?? "";
            string PathNodeOrder = collection["PathNodeOrder"] ?? "";
            var srm = PathNodeService.GetDetails(page, rows,PathName, PositionName, PathNodeOrder);
            return Json(srm, "text", JsonRequestBehavior.AllowGet);
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
        // POST: /PathNode/Create/
        [HttpPost]
        public ActionResult Create(PathNode pathNode)
        {
            string strResult = string.Empty;
            bool bResult = PathNodeService.Add(pathNode, strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /PathNode/Edit
        public ActionResult Edit(PathNode pathNode)
        {
            string strResult = string.Empty;
            bool bResult = PathNodeService.Save(pathNode, strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /PathNode/Delete/
        [HttpPost]
        public ActionResult Delete(PathNode pathNode)
        {
            string strResult = string.Empty;
            bool bResult = false;
            bResult = PathNodeService.Delete(pathNode, strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        //        
        // POST: /PathNode/GetPathNode/
        public ActionResult GetPathNode(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "ID";
            }
            if (value == null)
            {
                value = "";
            }
            var srm = PathNodeService.GetPathNode(page, rows, queryString, value);
            return Json(srm, "text", JsonRequestBehavior.AllowGet);
        }
        //
        //PathNode/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string id = Request.QueryString["id"];

            THOK.Common.NPOI.Models.ExportParam ep = new THOK.Common.NPOI.Models.ExportParam();
            ep.DT1 =  PathNodeService.GetPathNode(page, rows, id);
            ep.HeadTitle1 = "路径节点信息";
            System.IO.MemoryStream ms = THOK.Common.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
    }
}
