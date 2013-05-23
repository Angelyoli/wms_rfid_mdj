using System;
using System.Web.Mvc;
using THOK.Wms.AutomotiveSystems.Models;
using System.Web.Script.Serialization;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Common.WebUtil;
using THOK.Wms.DownloadWms.Bll;

namespace Wms.Controllers.Wms.AutomotiveSystems
{
    public class TaskController : Controller
    {
        [Dependency]
        public THOK.Wms.AutomotiveSystems.Interfaces.ITaskService TaskService { get; set; }

        [Dependency]
        public IInBillMasterHistoryService InBillMasterHistory { get; set; }

        [Dependency]
        public IOutBillMasterHistoryService OutBillMasterHistory { get; set; }

        [Dependency]
        public IMoveBillMasterHistoryService MoveBillMasterHistory { get; set; }

        [Dependency]
        public ICheckBillMasterHistoryService CheckBillMasterHistory { get; set; }

        [Dependency]
        public IProfitLossBillMasterHistoryService ProfitLossBillMasterHistory { get; set; }

        [Dependency]
        public IDailyBalanceHistoryService DailyBalanceHistoryService { get; set; }

        public ActionResult Index(string parameter)
        {
            Result result = new Result();
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                TaskParameter taskParameter = serializer.Deserialize<TaskParameter>(parameter);
                switch (taskParameter.Method)
                {
                    case "getMaster":
                        TaskService.GetBillMaster(taskParameter.BillTypes, result);
                        break;
                    case "getDetail":
                        TaskService.GetBillDetail(taskParameter.BillMasters, taskParameter.productCode, taskParameter.OperateType, taskParameter.OperateArea, taskParameter.Operator, result);
                        break;
                    case "apply":
                        TaskService.Apply(taskParameter.BillDetails, taskParameter.UseTag, result);
                        break;
                    case "cancel":
                        TaskService.Cancel(taskParameter.BillDetails, taskParameter.UseTag, result);
                        break;
                    case "execute":
                        TaskService.Execute(taskParameter.BillDetails, taskParameter.UseTag, result);
                        break;
                    case "getRfidInfo":
                        TaskService.SearchRfidInfo(taskParameter.RfidId, result);
                        break;
                    default:
                        result.IsSuccess = false;
                        result.Message = "调用了未定义的方法！";
                        break;
                }

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "调用服务器服务失败，详情：" + e.Message;
            }
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendSortInfo(string orderdate, string batchId, string sortingLineCode, string orderId)
        {
            AddXmlValueBll bll = new AddXmlValueBll();
            string text = orderId + " - " + sortingLineCode + " - " + orderdate + " - " + batchId;
            bll.insert("sortInfo", text);
            string msg = @"<?xml version='1.0' encoding='GB2312' ?>
                            <message>
			                    <!--** 操作是否成功 true/false **-->
		                        <issuccess text='{0}'></issuccess>
			                    <!--** 返回信息 1:操作成功；2:其他错误。**-->
			                    <msg text='{1}' desc='{2}'></msg>
                            </message>";
            string error = string.Empty;
            if (TaskService.ProcessSortInfo(orderdate, batchId, sortingLineCode, orderId, ref error))
            {
                msg = string.Format(msg, "true", "1", "操作成功");
            }
            else
            {
                bll.insert("sortErr", error);
                msg = string.Format(msg, "true", "1", error);
            }
            return new ContentResult() { Content = msg, ContentEncoding = Encoding.GetEncoding("GB2312"), ContentType = "text" };
        }

        public ActionResult MoveDataIsHistory(string dateTime, string billType)
        {
            string msg = string.Empty;
            bool bResult;
            string strResult = string.Empty;
            DateTime time = Convert.ToDateTime(dateTime);
            switch (billType)
            {
                case "1"://入库单据
                    bResult = InBillMasterHistory.Add(time, out strResult);
                    if (!bResult) msg = "入库操作：" + strResult;
                    break;
                case "2"://出库单据
                    bResult = OutBillMasterHistory.Add(time, out strResult);
                    if (!bResult) msg = "出库操作：" + strResult;
                    break;
                case "3"://移库单据
                    bResult = MoveBillMasterHistory.Add(time, out strResult);
                    if (!bResult) msg = "移库操作：" + strResult;
                    break;
                case "4"://盘点单据
                    bResult = CheckBillMasterHistory.Add(time, out strResult);
                    if (!bResult) msg = "盘点操作：" + strResult;
                    break;
                case "5"://损益单据
                    bResult = ProfitLossBillMasterHistory.Add(time, out strResult);
                    if (!bResult) msg = "损益操作：" + strResult;
                    break;
                case "6"://日结
                    bResult = DailyBalanceHistoryService.Add(time, out strResult);
                    if (!bResult) msg = "出库操作：" + strResult;
                    break;
                default:
                    msg = "调用了未定义的方法！";
                    break;
            }
            return new ContentResult() { Content = msg, ContentEncoding = Encoding.GetEncoding("GB2312"), ContentType = "text" };
        }
    }
}
