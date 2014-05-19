using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using THOK.Security;
using Microsoft.Practices.Unity;
using THOK.SMS.Bll.Interfaces;
using THOK.SMS.DbModel;

using THOK.Common.WebUtil;

namespace Wms.Controllers.SMS
{
      [TokenAclAuthorize]
    public class LedController : Controller
    {
        //
        // GET: /Led/

          [Dependency]

          public ILedService LedService { get; set; }



        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            //ViewBag.hasAudit = true;
            //ViewBag.hasAntiTrial = true;
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
        // GET: /Led/Details/5

        public ActionResult Details(int page, int rows, string Status, string LedName, string LedGroupCode, string LedType, string LedCode)

        //public ActionResult Details(int page, int rows, string Status, string LedCode, string LedName, string LedType, string LedGroupCode)
        {

            var srmDetail = LedService.GetDetails(page, rows, Status, LedName,LedGroupCode,LedType,LedCode);
            return Json(srmDetail, "text", JsonRequestBehavior.AllowGet);
        }


     
        public ActionResult Create(Led ledInfo)
        {
            string strResult = string.Empty;
            bool bResult = LedService.Add(ledInfo, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Led ledInfo)
        {
            string strResult = string.Empty;
            bool bResult = LedService.Save(ledInfo, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string ledCode)
        {
            string strResult = string.Empty;
            bool bResult = LedService.Delete(ledCode, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

          //获取
        public ActionResult GetLedGroupCode(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "LedName";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = LedService.GetLedGroupCode(page, rows, QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
