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

namespace Wms.Controllers.Wms.DeliveryInfo
{
    [TokenAclAuthorize]
    public class DeliverDistController : Controller
    {
        [Dependency]
        public IDeliverDistService DeliverDistService { get; set; }

        //
        // GET: /DeliverDist/

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
        // GET: /DeliverDist/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string DistCode = collection["DistCode"] ?? "";
            string CustomCode = collection["CustomCode"] ?? "";
            string DistName = collection["DistName"] ?? "";
            string CompanyCode = collection["CompanyCode"] ?? "";
            string UniformCode = collection["UniformCode"] ?? "";
            string IsActive = collection["IsActive"] ?? ""; ;
            var users = DeliverDistService.GetDetails(page, rows, DistCode, CustomCode, DistName, CompanyCode, UniformCode, IsActive);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /DeliverDist/S_Details/

        public ActionResult S_Details(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "DistName";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = DeliverDistService.S_Details(page, rows, QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /DeliverDist/Create
        [HttpPost]
        public ActionResult Create(DeliverDist deliverDist)
        {
            string strResult = string.Empty;
            bool bResult = DeliverDistService.Add(deliverDist, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /DeliverDis/Edit/
        [HttpPost]
        public ActionResult Edit(string DistCode, string CustomCode, string DistName, string DistCenterCode, string CompanyCode, string UniformCode, string Description,string IsActive)
        {
            string strResult = string.Empty;
            bool bResult = DeliverDistService.Save(DistCode, CustomCode, DistName, DistCenterCode, CompanyCode, UniformCode, Description, IsActive, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /DeliverDis/Delete/

        [HttpPost]
        public ActionResult Delete(string DistCode)
        {
            bool bResult = DeliverDistService.Delete(DistCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
