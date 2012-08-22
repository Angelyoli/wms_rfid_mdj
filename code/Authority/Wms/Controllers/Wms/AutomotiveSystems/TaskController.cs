using System;
using System.Web.Mvc;
using THOK.Wms.AutomotiveSystems.Models;
using System.Web.Script.Serialization;
using Microsoft.Practices.Unity;
using THOK.Wms.AutomotiveSystems.Interfaces;

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
                        TaskService.GetBillDetail(taskParameter.BillMasters, taskParameter.productCode, taskParameter.OperateType, taskParameter.OperateAreas, taskParameter.Operator, result);
                        break;
                    case "apply":
                        TaskService.Apply(taskParameter.BillDetails, result);
                        break;
                    case "cancel":
                        TaskService.Cancel(taskParameter.BillDetails, result);
                        break;
                    case "execute":
                        TaskService.Execute(taskParameter.BillDetails, result);
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
    }
}
