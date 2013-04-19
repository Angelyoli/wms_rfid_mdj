using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Allot.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Security;

namespace Authority.Controllers.Wms.StockIn
{
    [TokenAclAuthorize]
    public class StockInBillAllotController : Controller
    {
        [Dependency]
        public IInBillAllotService InBillAllotService { get; set; }
        [Dependency]
        public IInBillDetailService InBillDetailService { get; set; }

        public ActionResult Search(string billNo, int page, int rows)
        {
            var result = InBillAllotService.Search(billNo, page, rows);
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotDelete(string billNo, long id)
        {
            string strResult = string.Empty;
            bool bResult = InBillAllotService.AllotDelete(billNo, id, out strResult);
            string msg = bResult ? "删除分配明细成功" : "删除分配明细失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotEdit(string billNo, long id, string cellCode, decimal allotQuantity)
        {
            string strResult = string.Empty;
            bool bResult = InBillAllotService.AllotEdit(billNo, id, cellCode, allotQuantity, out strResult);
            string msg = bResult ? "修改分配成功" : "修改分配失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotConfirm(string billNo)
        {
            string strResult = string.Empty;
            bool bResult = InBillAllotService.AllotConfirm(billNo, out strResult);
            string msg = bResult ? "确认分配成功" : "确认分配失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotCancelConfirm(string billNo)
        {
            string strResult = string.Empty;
            bool bResult = InBillAllotService.AllotCancelConfirm(billNo, out strResult);
            string msg = bResult ? "取消分配确认成功" : "取消分配确认失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotCancel(string billNo)
        {
            string strResult = string.Empty;
            bool bResult = InBillAllotService.AllotCancel(billNo, out strResult);
            string msg = bResult ? "取消分配成功" : "取消分配失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotAdd(string billNo, long id, string cellCode, decimal allotQuantity)
        {
            string strResult = string.Empty;
            bool bResult = InBillAllotService.AllotAdd(billNo, id, cellCode, allotQuantity, out strResult);
            string msg = bResult ? "添加分配成功" : "添加分配失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotAdds(string billNo, long id, string cellCode, decimal allotQuantity, string productname)
        {
            string strResult = string.Empty;
            bool bResult = InBillAllotService.AllotAdd(billNo, id, cellCode, allotQuantity, productname, out strResult);
            string msg = bResult ? "" : "添加分配失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        #region /StockInBillAllot/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billNo = Request.QueryString["billNo"];
            
            THOK.NPOI.Models.ExportParam ep = new THOK.NPOI.Models.ExportParam();
            ep.DT1 = InBillDetailService.GetInBillDetail(page, rows, billNo);
            ep.DT2 = InBillAllotService.AllotSearch(page, rows, billNo);;
            ep.HeadTitle1 = "入库单据分配";
            ep.HeadTitle2 = "入库单据分配明细";
            System.IO.MemoryStream ms = THOK.NPOI.Service.ExportExcel.ExportDT(ep);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
