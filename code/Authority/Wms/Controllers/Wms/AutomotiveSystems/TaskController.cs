using System;
using System.Web.Mvc;
using THOK.Wms.AutomotiveSystems.Models;
using System.Web.Script.Serialization;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;
using System.Text;

namespace Wms.Controllers.Wms.AutomotiveSystems
{
    public class TaskController : Controller
    {
        [Dependency]
        public ITaskService TaskService { get; set; }

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
                        TaskService.Apply(taskParameter.BillDetails, taskParameter.UseTag,result);
                        break;
                    case "cancel":
                        TaskService.Cancel(taskParameter.BillDetails, taskParameter.UseTag,result);
                        break;
                    case "execute":
                        TaskService.Execute(taskParameter.BillDetails,taskParameter.UseTag, result);
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

        public ActionResult SendSortInfo(string orderdate,string batchId,string sortingLineCode, string orderId)
        {
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
                msg = string.Format(msg, "false", "2", error);
            }
            return new ContentResult() { Content = msg, ContentEncoding = Encoding.GetEncoding("GB2312"), ContentType= "text" };
        }
    }
}
