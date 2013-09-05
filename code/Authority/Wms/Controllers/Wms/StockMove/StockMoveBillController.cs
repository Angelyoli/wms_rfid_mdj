using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.WebUtil;
using THOK.Wms.AutomotiveSystems.Models;
using THOK.Security;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;
using THOK.WCS.Bll.Interfaces;

namespace Authority.Controllers.Wms.StockMove
{
    [TokenAclAuthorize]
    public class StockMoveBillController : Controller
    {
        [Dependency]
        public IMoveBillMasterService MoveBillMasterService { get; set; }
        [Dependency]
        public IMoveBillDetailService MoveBillDetailService { get; set; }
        [Dependency]
        public ITaskService TaskService { get; set; }
        [Dependency]
        public IMoveBillMasterHistoryService MoveBillMasterHistoryService { get; set; }
        //
        // GET: /StockMoveBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasTask = true;
            ViewBag.hasSettle = true;
            ViewBag.hasMigration = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /StockMoveBill/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string WareHouseCode = collection["WareHouseCode"] ?? "";
            string beginDate = collection["beginDate"] ?? "";
            string endDate = collection["endDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string CheckPersonCode = collection["CheckPersonCode"] ?? string.Empty;
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var moveBillMaster = MoveBillMasterService.GetDetails(page, rows, BillNo, WareHouseCode, beginDate, endDate, OperatePersonCode, CheckPersonCode, Status, IsActive);
            return Json(moveBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockMoveBill/MoveBillDetails/

        public ActionResult MoveBillDetails(int page, int rows, string BillNo)
        {
            var moveBillDetail = MoveBillDetailService.GetDetails(page, rows, BillNo);
            return Json(moveBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockMoveBill/GenMoveBillNo/

        public ActionResult GenMoveBillNo()
        {
            var moveBillNo = MoveBillMasterService.GenMoveBillNo(this.User.Identity.Name.ToString());
            return Json(moveBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/Create/

        [HttpPost]
        public ActionResult Create(MoveBillMaster moveBillMaster)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillMasterService.Add(moveBillMaster, this.User.Identity.Name.ToString(), out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/Edit/

        [HttpPost]
        public ActionResult Edit(MoveBillMaster moveBillMaster)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillMasterService.Save(moveBillMaster, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/Delete/
        public ActionResult Delete(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillMasterService.Delete(BillNo, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/Audit/
        public ActionResult Audit(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString(), out strResult);
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/AntiTrial/
        public ActionResult AntiTrial(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillMasterService.AntiTrial(BillNo, out strResult);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/MoveBillDetailDelete/

        public ActionResult MoveBillDetailDelete(string ID)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillDetailService.Delete(ID, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/MoveBillDetailCreate/

        [HttpPost]
        public ActionResult MoveBillDetailCreate(MoveBillDetail moveBillDetail)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillDetailService.Add(moveBillDetail, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/MoveBillDetailEdit/

        [HttpPost]
        public ActionResult MoveBillDetailEdit(MoveBillDetail moveBillDetail)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillDetailService.Save(moveBillDetail, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneratePalletTag(string billNo)
        {
            string strResult = string.Empty;
            Result result = new Result();
            result.IsSuccess = MoveBillMasterService.GeneratePalletTag(billNo, ref strResult);
            result.Message = result.IsSuccess ? "BC类烟自动组盘成功" : "BC类烟自动组盘失败";
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockMoveBill/moveBillMasterSettle/

        public ActionResult MoveBillMasterSettle(string billNo)
        {
            string strResult = string.Empty;
            bool bResult = MoveBillMasterService.Settle(billNo, out strResult);
            string msg = bResult ? "结单成功" : "结单失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //移库作业
        // POST: /StockMoveBill/MoveBillTask/       
        public ActionResult MoveBillTask(string moveBillNo)
        {
            string strResult = string.Empty;
            bool bResult = TaskService.MoveBillTask(moveBillNo, out strResult);
            string msg = bResult ? "作业成功" : "作业失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        #region /StockMoveBill/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billNo = Request.QueryString["billNo"];
            bool isAbnormity = Convert.ToBoolean(Request.QueryString["isAbnormity"]);
            bool isGroup = Convert.ToBoolean(Request.QueryString["isGroup"]);
            string sortingName = string.Empty;
            ExportParam ep = new ExportParam();
            ep.DT1 = MoveBillDetailService.GetMoveBillDetail(page, rows, billNo, isAbnormity, isGroup, out sortingName);
            ep.HeadTitle1 = sortingName + "移库单明细";
            return PrintService.Print(ep);
        }
        #endregion

        public ActionResult MoveBillMasterHistory(DateTime datetime)
        {
            string result = string.Empty;
            string strResult = string.Empty;
            bool bResult = MoveBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out strResult);
            string msg = bResult ? "迁移成功" : "迁移失败";
            if (msg != "迁移成功") result = "原因：" + strResult;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
