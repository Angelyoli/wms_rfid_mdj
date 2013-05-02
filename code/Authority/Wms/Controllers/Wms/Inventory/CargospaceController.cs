using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Common.WebUtil;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Authority.Controllers.Wms.Inventory
{
    public class CargospaceController : Controller
    {
        //
        // GET: /Cargospace/

        [Dependency]
        public ICargospaceService CargospaceService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();    
        }

        //
        // GET: /Cargospace/Details/

        public ActionResult Details(int page, int rows, string type, string id)
        {
            var storage = CargospaceService.GetCellDetails(page, rows, type, id);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TreeDetails(string type, string id)
        {
            var storage = CargospaceService.GetCellDetails(type, id);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllotInTreeDetails(string type, string id)
        {
            var storage = CargospaceService.GetInCellDetail(type, id);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllotOutTreeDetails(string type, string id, string productCode)
        {
            var storage = CargospaceService.GetOutCellDetail(type,id,productCode);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Cargospace/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string type = Request.QueryString["type"];
            string id = Request.QueryString["id"];

            ExportParam ep = new ExportParam();
            ep.DT1 = CargospaceService.GetCargospace(page, rows, type, id);
            ep.HeadTitle1 = "货位库存查询";
            return PrintService.Print(ep);
        }
        #endregion
    }
}
