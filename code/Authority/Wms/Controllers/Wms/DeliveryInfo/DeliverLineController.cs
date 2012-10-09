using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
using THOK.Wms.DbModel;

namespace Wms.Controllers.Wms.InterfaceInfo
{
    public class DeliverLineController : Controller
    {
        [Dependency]
        public IDeliverLineService DeliverLineService { get; set; }

        //
        // GET: /DeliverLine/
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


        //下载线路信息
        // GET: /DeliverLine/DownloadDeliverLineSave/
        public ActionResult DownloadDeliverLineSave()
        {
            string errorInfo = string.Empty;
            bool bResult = DeliverLineService.DownDeliverLine(out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }
        // GET: /DeliverLine/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string DeliverLineCode = collection["DeliverLineCode"] ?? "";
            string CustomCode = collection["CustomCode"] ?? "";
            string DeliverLineName = collection["DeliverLineName"] ?? "";
            string DistCode = collection["DistCode"] ?? "";
            string DeliverOrder = collection["DeliverOrder"] ?? "";
            string IsActive = collection["IsActive"] ?? ""; ;
            var users = DeliverLineService.GetDetails(page, rows, DeliverLineCode, CustomCode, DeliverLineName, DistCode, DeliverOrder, IsActive);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /DeliverLine/Create
        [HttpPost]
        public ActionResult Create(DeliverLine deliverLine)
        {
            string strResult = string.Empty;
            bool bResult = DeliverLineService.Add(deliverLine, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /DeliverLine/D_Details/

        public ActionResult D_Details(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "CustomCode";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = DeliverLineService.D_Details(page, rows, QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /DeliverLine/Edit/
        [HttpPost]
        public ActionResult Edit(DeliverLine deliverLine)
        {
            string strResult = string.Empty;
            bool bResult = DeliverLineService.Save(deliverLine, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /DeliverLine/Delete/

        [HttpPost]
        public ActionResult Delete(string DeliverLineCode)
        {
            bool bResult = DeliverLineService.Delete(DeliverLineCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
