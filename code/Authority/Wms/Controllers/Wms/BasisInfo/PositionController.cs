using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Wms.Controllers.Wms.BasisInfo
{
    public class PositionController : Controller
    {
        [Dependency]
        public IPositionService PositionService { get; set; }
        //
        // GET: /Position/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
           
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult SearchPage()
        {
            return View();
        }

        public ActionResult AddPage()
        {
            return View();
        }

        //
        // GET: /Position/Details/5

        public ActionResult Details (int page, int rows, FormCollection collection)
        {
            string ID = collection["ID"] ?? "";
            string PositionName = collection["PositionName"] ?? "";
            string PositionType = collection["PositionType"] ?? "";
            string RegionID = collection["RegionID"] ?? "";
            string SRMName = collection["SRMName"] ?? "";
            string State = collection["State"] ?? "";

            var path = PositionService.GetDetails(page, rows, ID, PositionName, PositionType,SRMName, RegionID, State);
            return Json(path, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Position/Create

        [HttpPost]
        public ActionResult Create(Position position)
        {
            string strResult = string.Empty;
            bool bResult = PositionService.Add(position, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        
      
        //
        // POST: /Position/Edit/5

        [HttpPost]
        public ActionResult Edit(Position position)
        {
            string strResult = string.Empty;
            bool bResult = PositionService.Save(position, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Position/Delete/5

        [HttpPost]
        public ActionResult Delete(int positionId)
        {
            string strResult = string.Empty;
            bool bResult = PositionService.Delete(positionId, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
         // POST: /Position/GetPosition/
        public ActionResult GetPosition(int page, int rows, string queryString, string value)
        {
            
            if (queryString == null)
            {
                queryString = "EmployeeCode";
            }
            if (value == null)
            {
                value = "";
            }
            var employee = PositionService.GetPosition(page, rows, queryString, value);
            return Json(employee, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Position/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string id = Request.QueryString["id"];
            string positioNname = Request.QueryString["positioNname"];
            string positionType = Request.QueryString["positionType"];
            string sRMName = Request.QueryString["sRMName"];
            string regionID = Request.QueryString["regionID"];
            string state = Request.QueryString["state"];

            System.Data.DataTable dt = PositionService.GetPosition(page, rows, id, positioNname, positionType, sRMName, regionID, state);
            string headText = "位置信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize
                , 0, true, colHeadFont, colHeadSize, 0, true, 0, HeaderFooder, null, 0);
            return new FileStreamResult(ms, "application/ms-excel");
        } 
        #endregion
    }
}
    

