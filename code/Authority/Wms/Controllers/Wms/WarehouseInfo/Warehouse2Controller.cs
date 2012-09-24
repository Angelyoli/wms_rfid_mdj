using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.Wms.Bll.Service;
using THOK.Wms.SignalR.Connection;
namespace Wms.Controllers.Wms.WarehouseInfo
{
    public class Warehouse2Controller : Controller
    {
        [Dependency]
        public IWarehouseService WarehouseService { get; set; }
        [Dependency]
        public ICargospaceService CargospaceService { get; set; }
        [Dependency]
        public ICellService CellService { get; set; }
        //
        // GET: /Warehouse2/

        public ActionResult Index(string moduleID)
        {
            AutomotiveSystemsNotify.Notify();
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //查询仓库信息表
        // POST: /Warehouse/Details
        [HttpPost]
        public ActionResult Details(int page, int rows, string type, string id)
        {
            var warehouse = CellService.GetDetail(page, rows, type, id);
            return Json(warehouse, "text", JsonRequestBehavior.AllowGet);
        }
        //查询仓库信息表
        // POST: /Warehouse/FindWarehouse
        [HttpPost]
        public ActionResult FindWarehouse(string parameter)
        {
            var warehouse = WarehouseService.FindWarehouse(parameter);
            return Json(warehouse, "text", JsonRequestBehavior.AllowGet);
        }

        //添加仓库信息表
        // POST: /Warehouse/WareCreate
        [HttpPost]
        public ActionResult WareCreate(Warehouse warehouse)
        {
            bool bResult = WarehouseService.Add(warehouse);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //编辑仓库表
        // GET: /Warehouse/Edit/
        public ActionResult Edit(Warehouse warehouse)
        {
            bool bResult = WarehouseService.Save(warehouse);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //删除仓库表
        // POST: /Warehouse/Delete/
        [HttpPost]
        public ActionResult Delete(string warehouseCode)
        {
            bool bResult = WarehouseService.Delete(warehouseCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //获取生成的仓库编码
        // GET: /Warehouse/GetWareCode/
        public ActionResult GetWareCode()
        {
            var warehouseCode = WarehouseService.GetWareCode();
            return Json(warehouseCode, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Warehouse2/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string type = Request.QueryString["type"];
            string id = Request.QueryString["id"];

            System.Data.DataTable dt = CellService.GetCell(page, rows, type, id);
            string headText = "仓库信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10; Int16 colHeadWidth = 300;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize
                , 0, true, colHeadFont, colHeadSize, 0, true, 0, HeaderFooder);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
