using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;

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

        #region /Cargospace/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string type = Request.QueryString["type"];
            string id = Request.QueryString["id"];

            System.Data.DataTable dt = CargospaceService.GetCargospace(page, rows, type, id);
            string headText = "货位库存查询";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10; Int16 colHeadWidth = 300;
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize,
                colHeadFont, colHeadSize, colHeadWidth, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion

        #region this.GetResponse
        private void GetResponse(string headText)
        {
            string filename = headText + DateTime.Now.ToString("yyMMdd-HHmm-ss");
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";
        }
        #endregion
    }
}
