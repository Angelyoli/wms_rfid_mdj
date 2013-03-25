using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.Wms.BasisInfo
{
    public class RegionController : Controller
    {
        [Dependency]
        public IRegionService RegionService { get; set; }

        //
        // GET: /Region/
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
        // GET: /Region/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string  ID = collection["RegionID"] ?? "";
            string RegionName = collection["RegionName"] ?? "";
            string State = collection["State"] ?? "";
            var srm = RegionService.GetDetails(page, rows, ID, RegionName, State);
            return Json(srm, "text", JsonRequestBehavior.AllowGet);
        }

    }
}
