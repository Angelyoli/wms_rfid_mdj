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
    public class CurrentStockController : Controller
    {
        //
        // GET: /CurrentStock/

        [Dependency]
        public ICurrentStockService CurrentStockService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /CurrentStock/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string productCode = collection["ProductCode"] ?? "";
            string ware = collection["Ware"] ?? "";
            string area = collection["Area"] ?? "";
            string unitType = collection["UnitType"] ?? "";
            var storage = CurrentStockService.GetCellDetails(page, rows, productCode, ware, area, unitType);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        // /CurrentStock/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string productCode = Request.QueryString["productCode"];
            string ware = Request.QueryString["ware"];
            string area = Request.QueryString["area"];
            string unitType = Request.QueryString["unitType"];

            System.Data.DataTable dt = CurrentStockService.GetCurrentStock(page, rows, productCode, ware, area, unitType);
            string strHeaderText = "当前库存";
            string exportDate = "导出时间：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            string filename = strHeaderText + DateTime.Now.ToString("yyMMdd-HHmm-ss");
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";
            string[] str = {
                               "20",        //[0]大标题字体大小
                               "700",       //[1]大标题字体粗宽
                               "10",        //[2]列标题字体大小
                               "700",       //[3]列标题字体粗宽
                               "300",       //[4]excel中有数据表格的大小
                               "微软雅黑",  //[5]大标题字体
                               "Arial",     //[6]小标题字体
                           };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, strHeaderText, str, exportDate);
            return new FileStreamResult(ms, "application/ms-excel");
        }
    }
}
