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
using THOK.Authority.Bll.Interfaces;
using THOK.Security;

namespace Authority.Controllers.Wms.StockOut
{
    [TokenAclAuthorize]
    public class StockOutBillController : Controller
    {

        [Dependency]
        public IOutBillMasterService OutBillMasterService { get; set; }
        [Dependency]
        public IOutBillDetailService OutBillDetailService { get; set; }
        [Dependency]
        public ISystemParameterService SystemParameterService { get; set; }
        [Dependency]
        public THOK.Wms.Bll.Interfaces.ITaskService TaskService { get; set; }
        [Dependency]
        public IOutBillMasterHistoryService OutBillMasterHistoryService { get; set; }
        //
        // GET: /StockOutBill/

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

        //查询主单
        // GET: /StockOutBill/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string beginDate = collection["beginDate"] ?? "";
            string endDate = collection["endDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string CheckPersonCode = collection["CheckPersonCode"] ?? string.Empty;
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var outBillMaster = OutBillMasterService.GetDetails(page, rows, BillNo, beginDate, endDate, OperatePersonCode, CheckPersonCode, Status, IsActive);
            return Json(outBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /StockOutBill/OutBillDetails/
        public ActionResult OutBillDetails(int page, int rows, string BillNo)
        {
            var outBillDetail = OutBillDetailService.GetDetails(page, rows, BillNo);
            return Json(outBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //生成单号
        // GET: /StockOutBill/GenInBillNo/
        public ActionResult GenInBillNo()
        {
            var outBillNo = OutBillMasterService.GenInBillNo(this.User.Identity.Name.ToString());
            return Json(outBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //新增主单
        // POST: /StockOutBill/Create/
        public ActionResult Create(OutBillMaster outBillMaster)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Add(outBillMaster, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //修改主单
        // POST: /StockOutBill/Edit/
        public ActionResult Edit(OutBillMaster outBillMaster)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Save(outBillMaster, out errorInfo);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //删除主单
        // POST: /StockOutBill/Delete/
        public ActionResult Delete(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Delete(BillNo, out errorInfo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //新增细单
        // POST: /StockOutBill/OutBillDetailCreate/
        public ActionResult OutBillDetailCreate(OutBillDetail outBillDetail)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Add(outBillDetail, out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //删除细单
        // POST: /StockOutBill/outBillDelete/
        public ActionResult outBillDelete(string BillNo, string ID)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Delete(ID, out errorInfo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //修改细单
        // POST: /StockOutBill/editOutBillDelete
        public ActionResult editOutBillDelete(OutBillDetail outBillDetail)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Save(outBillDetail, out errorInfo);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单审核
        // POST: /StockOutBill/outBillMasterAudit/
        public ActionResult outBillMasterAudit(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单反审
        // POST: /StockOutBill/outBillMasterAntiTrial/
        public ActionResult outBillMasterAntiTrial(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.AntiTrial(BillNo, out errorInfo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单结单
        // POST: /StockOutBill/outBillMasterSettle/
        public ActionResult outBillMasterSettle(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Settle(BillNo, out errorInfo);
            string msg = bResult ? "结单成功" : "结单失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单结单
        // POST: /StockOutBill/DownOutBillMaster/
        public ActionResult DownOutBillMaster(string beginDate, string endDate, string wareCode, string billType)
        {
            string errorInfo = string.Empty;
            beginDate = Convert.ToDateTime(beginDate).ToString("yyyyMMdd");
            endDate = Convert.ToDateTime(endDate).ToString("yyyyMMdd");
            bool bResult = false;
            DownUnitBll ubll = new DownUnitBll();
            DownProductBll pbll = new DownProductBll();
            DownOutBillBll ibll = new DownOutBillBll();
            DownCustomerBll custBll = new DownCustomerBll();
            try
            {
                if (!SystemParameterService.SetSystemParameter())
                {
                    ubll.DownUnitCodeInfo();
                    pbll.DownProductInfo();
                    custBll.DownCustomerInfo();
                }
                else
                {
                    ubll.DownUnitInfo();//创联
                    pbll.DownProductInfos();//创联
                    custBll.DownCustomerInfos();//创联
                }
                bResult = ibll.GetOutBills(beginDate, endDate, this.User.Identity.Name.ToString(), out errorInfo, wareCode, billType);

            }
            catch (Exception e)
            {
                errorInfo += e.Message;
            }

            string msg = bResult ? "下载成功" : "下载失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockOutBill/OutBillMasterHistory/
        public ActionResult OutBillMasterHistory(string datetime)
        {
            string result = string.Empty;
            string strResult = string.Empty;
            bool bResult = OutBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out strResult);
            string msg = bResult ? "迁移成功" : "迁移失败";
            if (msg != "迁移成功") result = "原因：" + strResult;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }

        // 出库订单作业
        // POST: /StockOutBill/OutBillTask/
        public ActionResult OutBillTask(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = TaskService.OutBillTask(BillNo, out strResult);
            string msg = bResult ? "作业成功" : "作业失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
