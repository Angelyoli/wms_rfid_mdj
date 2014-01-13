using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.WMS.WarehouseInfo
{
    public class StorageCellPreviewController : Controller
    {
        [Dependency]
        public IStorageService StorageService { get; set; }

        //
        // GET: /StorageCellPreview/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        // GET: /StorageCellPreview/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string areaType = collection["AreaType"] ?? "";
            string[] areaCode1 = new string[] { "001-01", "001-02" };
            string[] areaCode2 = new string[] { "", "001-03", "001-04", "001-05", "001-06" };
            object detail = null;
            if (areaCode1.Any(a => a == areaType))
            {
                detail = StorageService.GetStorageCellHasProduct(page, rows, areaType);
            }
            else if (areaCode2.Any(a => a == areaType))
            {
                detail = StorageService.GetStorageCellIsEmpty(page, rows, areaType);
            }
            return Json(detail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}