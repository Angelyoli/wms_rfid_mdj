using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Bll.Models;

namespace Wms.Controllers.Wms.ComplexSearch
{
    public class MigrationDataController : Controller
    {
        #region 接口
        [Dependency]
        public IInBillMasterHistoryService InBillMasterHistoryService { get; set; }
        [Dependency]
        public IOutBillMasterHistoryService OutBillMasterHistoryService { get; set; }
        [Dependency]
        public IDailyBalanceHistoryService DailyBalanceHistoryService { get; set; }
        [Dependency]
        public IMoveBillMasterHistoryService MoveBillMasterHistoryService { get; set; }
        [Dependency]
        public ICheckBillMasterHistoryService CheckBillMasterHistoryService { get; set; }
        [Dependency]
        public IProfitLossBillMasterHistoryService ProfitLossBillMasterHistoryService { get; set; }
        #endregion

        #region 构造
        //
        // GET: /MigrationData/

        public ActionResult Index(string moduleID)
        {
            ViewBag.moduleID = moduleID;
            return View();
        }
        #endregion

        #region 入库
        public ActionResult InBillMasterHistory(string datetime)
        {
            string result = string.Empty;
            string masterResult = string.Empty;
            string detailResult = string.Empty;
            string allotResult = string.Empty;
            string deleteResult = string.Empty;
            bool bResult = InBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out masterResult, out detailResult, out allotResult, out deleteResult);
            string msg = bResult ? "成功！" : "失败！";
            if (msg == "失败")
            {
                result = "-------------------------------------------------【主表：" + masterResult
                            + "】-------------------------------------------------【细表：" + detailResult
                            + "】-------------------------------------------------【分配表：" + allotResult
                            + "】-------------------------------------------------【删除情况：" + deleteResult
                            + "】";
            }
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 出库
        public ActionResult OutBillMasterHistory(DateTime datetime)
        {
            string result = string.Empty;
            string masterResult = string.Empty;
            string detailResult = string.Empty;
            string allotResult = string.Empty;
            string deleteResult = string.Empty;
            bool bResult = OutBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out masterResult, out detailResult, out allotResult, out deleteResult);
            string msg = bResult ? "成功！" : "失败！";
            if (msg == "失败")
            {
                result = "-------------------------------------------------【主表：" + masterResult
                            + "】-------------------------------------------------【细表：" + detailResult
                            + "】-------------------------------------------------【分配表：" + allotResult
                            + "】-------------------------------------------------【删除情况：" + deleteResult
                            + "】";
            }
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 移库
        public ActionResult MoveBillMasterHistory(DateTime datetime)
        {
            string result = string.Empty;
            string masterResult = string.Empty;
            string detailResult = string.Empty;
            string deleteResult = string.Empty;
            bool bResult = MoveBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out masterResult, out detailResult, out deleteResult);
            string msg = bResult ? "成功！" : "失败！";
            if (msg == "失败")
            {
                result = "-------------------------------------------------【主表：" + masterResult
                            + "】-------------------------------------------------【细表：" + detailResult
                            + "】-------------------------------------------------【删除情况：" + deleteResult
                            + "】";
            }
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 盘点
        public ActionResult CheckBillMasterHistory(DateTime datetime)
        {
            string result = string.Empty;
            string masterResult = string.Empty;
            string detailResult = string.Empty;
            string deleteResult = string.Empty;
            bool bResult = CheckBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out masterResult, out detailResult, out deleteResult);
            string msg = bResult ? "成功！" : "失败！";
            if (msg == "失败")
            {
                result = "-------------------------------------------------【主表：" + masterResult
                            + "】-------------------------------------------------【细表：" + detailResult
                            + "】-------------------------------------------------【删除情况：" + deleteResult
                            + "】";
            }
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 损益
        public ActionResult ProfitLossBillMasterHistory(DateTime datetime)
        {
            string result = string.Empty;
            string masterResult = string.Empty;
            string detailResult = string.Empty;
            string deleteResult = string.Empty;
            bool bResult = ProfitLossBillMasterHistoryService.Add(Convert.ToDateTime(datetime), out masterResult, out detailResult, out deleteResult);
            string msg = bResult ? "成功！" : "失败！";
            if (msg == "失败")
            {
                result = "-------------------------------------------------【主表：" + masterResult
                            + "】-------------------------------------------------【细表：" + detailResult
                            + "】-------------------------------------------------【删除情况：" + deleteResult
                            + "】";
            }
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 日结
        public ActionResult DailyBalanceHistory(DateTime datetime)
        {
            string strResult = string.Empty;
            bool bResult = DailyBalanceHistoryService.Add(datetime, out strResult);
            string msg = bResult ? "成功！" : "失败！";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 多选（未完成）
        public object GetTree()
        {
            string[] type = { "入库", "出库", "移库", "盘点", "损益", "日结" }; //{ 1, 2, 3, 4, 5, 6 };
            HashSet<THOK.Wms.Bll.Models.Tree> hashSet = new HashSet<THOK.Wms.Bll.Models.Tree>();
            for (int i = 0; i < type.Length; i++)
            {
                THOK.Wms.Bll.Models.Tree tree = new THOK.Wms.Bll.Models.Tree();
                tree.id = i.ToString();
                tree.text = type[i];
                tree.state = "open";
                hashSet.Add(tree);
            }
            return hashSet.ToArray();
        } 
        #endregion
    }
}
