using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.WebUtil;

namespace Wms.Controllers.WMS.WarehouseInfo
{
    public class SplitPalletCellController : Controller
    {
        [Dependency]
        public ICellService CellService { get; set; }

        //
        // GET: /SplitPalletCell/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasEdit = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /SplitPalletCell/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string shelfType = collection["ShelfType"] ?? "";
            var cell = CellService.GetSplitPalletCell(page, rows, shelfType);
            return Json(cell, "text", JsonRequestBehavior.AllowGet);
        }

        // GET: /SplitPalletCell/Edit/
        public ActionResult Edit(Cell cell)
        {
            string strResult = string.Empty;
            bool bResult = CellService.SaveSplitPalletCell(cell, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
