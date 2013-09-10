using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.WebUtil;
using THOK.WMS.DownloadWms.Bll;
using THOK.Wms.DownloadWms.Bll;
using THOK.Authority.Bll.Interfaces;
using THOK.Security;
using THOK.WCS.Bll.Interfaces;

namespace Authority.Controllers.Wms.StockIn
{
    [TokenAclAuthorize]
    public class StockInBillController : Controller
    {
        [Dependency]
        public IInBillMasterService InBillMasterService { get; set; }
        [Dependency]
        public IInBillDetailService InBillDetailService { get; set; }
        [Dependency]
        public ISystemParameterService SystemParameterService { get; set; }
        [Dependency]
        public ITaskService TaskService { get; set; }
		[Dependency]
        public IInBillMasterHistoryService InBillMasterHistoryService { get; set; }

        //
        // GET: /StockInBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasDownload = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasAllot = true;
            ViewBag.hasTask = true;
            ViewBag.hasSettle = true;
            ViewBag.hasMigration = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        public ActionResult SearchPage()
        {
            return View();
        }

        //
        // GET: /StockInBill/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string WareHouseCode = collection["WareHouseCode"] ?? "";
            string BeginDate = collection["BeginDate"] ?? "";
            string EndDate = collection["EndDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? string.Empty;
            string CheckPersonCode = collection["CheckPersonCode"] ?? string.Empty;
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var inBillMaster = InBillMasterService.GetDetails(page, rows, BillNo, WareHouseCode, BeginDate, EndDate, OperatePersonCode, CheckPersonCode, Status, IsActive);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockInBill/InBillDetails/

        public ActionResult InBillDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = InBillDetailService.GetDetails(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockInBill/GenInBillNo/

        public ActionResult GenInBillNo()
        {
            var inBillNo = InBillMasterService.GenInBillNo(this.User.Identity.Name.ToString());
            return Json(inBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/Create/

        [HttpPost]
        public ActionResult Create(InBillMaster inBillMaster)
        {
            string strResult = string.Empty;
            bool bResult = InBillMasterService.Add(inBillMaster, this.User.Identity.Name.ToString(), out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/Edit/

        [HttpPost]
        public ActionResult Edit(InBillMaster inBillMaster)
        {
            string strResult = string.Empty;
            bool bResult = InBillMasterService.Save(inBillMaster, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/Delete/
        public ActionResult Delete(string BillNo)
        {
            DownDecidePlanBll planBll = new DownDecidePlanBll();
            string strResult = string.Empty;
            bool bResult = InBillMasterService.Delete(BillNo, out strResult);
            if (bResult)
                planBll.DeleteMiddleBill(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/InBillDetailCreate/

        [HttpPost]
        public ActionResult InBillDetailCreate(InBillDetail inBillDetail)
        {
            string strResult = string.Empty;
            bool bResult = InBillDetailService.Add(inBillDetail, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/InBillDetailDelete/

        [HttpPost]
        public ActionResult InBillDetailDelete(string ID)
        {
            string strResult = string.Empty;
            bool bResult = InBillDetailService.Delete(ID, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/Audit/
        public ActionResult Audit(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = InBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString(), out strResult);
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/AntiTria/
        public ActionResult AntiTrial(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = InBillMasterService.AntiTrial(BillNo, out strResult);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/InBillDetailEdit/

        [HttpPost]
        public ActionResult InBillDetailEdit(InBillDetail inBillDetail)
        {
            string strResult = string.Empty;
            bool bResult = InBillDetailService.Save(inBillDetail, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/GetBillTypeDetail/

        [HttpPost]
        public ActionResult GetBillTypeDetail(string BillClass, string IsActive)
        {
            var billType = InBillMasterService.GetBillTypeDetail(BillClass, IsActive);
            return Json(billType, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/GetWareHouseDetail/

        [HttpPost]
        public ActionResult GetWareHouseDetail(string IsActive)
        {
            var wareHouse = InBillMasterService.GetWareHouseDetail(IsActive);
            return Json(wareHouse, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/GetProductDetails/

        [HttpPost]
        public ActionResult GetProductDetails(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "ProductCode";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = InBillDetailService.GetProductDetails(page, rows, QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        //入库结单
        // POST: /StockInBill/inBillMasterSettle/
        public ActionResult InBillMasterSettle(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = InBillMasterService.Settle(BillNo, out strResult);
            string msg = bResult ? "结单成功" : "结单失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //入库订单作业
        // POST: /StockInBill/InBillTask/
        
        public ActionResult InBillTask(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = TaskService.InBillTask(BillNo, out strResult);
            string msg = bResult ? "作业成功" : "作业失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /StockInBill/DownInBillMaster/
        public ActionResult DownInBillMaster(string beginDate, string endDate, string wareCode, string billType, bool isInDown)
        {
            bool bResult = false;
            DownUnitBll ubll = new DownUnitBll();
            DownProductBll pbll = new DownProductBll();
            DownInBillBll ibll = new DownInBillBll();
            DownDecidePlanBll planBll = new DownDecidePlanBll();
            string errorInfo = string.Empty;
            try
            {
                beginDate = Convert.ToDateTime(beginDate).ToString("yyyyMMdd");
                endDate = Convert.ToDateTime(endDate).ToString("yyyyMMdd");
                if (!SystemParameterService.SetSystemParameter())
                {
                    ubll.DownUnitCodeInfo();
                    pbll.DownProductInfo();
                    if (isInDown)
                        bResult = ibll.GetInBill(beginDate, endDate, this.User.Identity.Name.ToString(), wareCode, billType, out errorInfo);
                    else
                        bResult = planBll.GetInBillMiddle(beginDate, endDate, this.User.Identity.Name.ToString(), wareCode, billType, out errorInfo);
                }
                else
                {
                    ubll.DownUnitInfo();
                    pbll.DownProductInfos();
                    if (isInDown)
                        bResult = ibll.GetInBills(beginDate, endDate, this.User.Identity.Name.ToString(), wareCode, billType, out errorInfo);
                    else
                        bResult = planBll.GetInBillMiddle(beginDate, endDate, this.User.Identity.Name.ToString(), wareCode, billType, out errorInfo);
                }
            }
            catch (Exception e)
            {
                errorInfo += e.Message;
            }

            string msg = bResult ? "下载成功" : "下载失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockInBill/InBillMasterHistory/
        public ActionResult InBillMasterHistory(string datetime)
        {
            string result = string.Empty;
            string strResult = string.Empty;
            bool bResult = InBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out strResult);
            string msg = bResult ? "迁移成功" : "迁移失败";
            if (msg != "迁移成功") result = "原因：" + strResult;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
