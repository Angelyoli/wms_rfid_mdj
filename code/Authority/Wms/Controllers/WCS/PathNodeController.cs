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

namespace Wms.Controllers.WCS
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
            //string PathName = collection["PathName"] ?? "";
            //string PositionName = collection["PositionName"] ?? "";
            //string PathNodeOrder = collection["PathNodeOrder"] ?? "";
            string PathID = collection["PathID"] ?? "";
            if (PathID != null && PathID != "")
            {
                pathNode.PathID = Convert.ToInt32(PathID);
            }
            if (PathID != null && PathID != "")
            {
                pathNode.PathID = Convert.ToInt32(PathID);
            }
            var srm = PathNodeService.GetDetails(page, rows, pathNode);
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
            bool bResult = PathNodeService.Add(pathNode);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /PathNode/Edit
        public ActionResult Edit(PathNode pathNode)
        {
            string strResult = string.Empty;
            bool bResult = PathNodeService.Save(pathNode);
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
            bResult = PathNodeService.Delete(pathNode);
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
        // GET: /PathNode/PathDetails/
        [HttpPost]
        public ActionResult PathDetails(string PathId)
        {
            var functions = PathNodeService.GetDetails(PathId);
            return Json(functions, "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /PathNode/PathCreate/
        [HttpPost]
        public ActionResult PathCreate(string pathID, string PositionID, string PathNodeOrder)
        {
            PathNode pathNode = new PathNode();
            pathNode.PathID =Convert.ToInt32(pathID);
            pathNode.PositionID = Convert.ToInt32(PositionID);
            pathNode.PathNodeOrder = Convert.ToInt32(PathNodeOrder);
            bool bResult = PathNodeService.Add(pathNode);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /PathNode/PathEdit/
        [HttpPost]
        public ActionResult PathEdit(string pathID, string PositionID, string PathNodeOrder)
        {
            PathNode pathNode = new PathNode();
            pathNode.ID = Convert.ToInt32(pathID);
            pathNode.PositionID = Convert.ToInt32(PositionID);
            pathNode.PathNodeOrder = Convert.ToInt32(PathNodeOrder);
            bool bResult = PathNodeService.Save(pathNode);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /PathNode/PathDelete/
        [HttpPost]
        public ActionResult PathDelete(string pathID, string PositionID, string PathNodeOrder)
        {
            PathNode pathNode = new PathNode();
            pathNode.ID = Convert.ToInt32(pathID);
            bool bResult = PathNodeService.Delete(pathNode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
