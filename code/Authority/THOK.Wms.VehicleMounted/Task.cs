using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using THOK.Wms.VehicleMounted.Model;
using LitJson;

namespace THOK.Wms.VehicleMounted
{
    public class Task
    {
        private Uri url = null;

        private string taskType = string.Empty;

        public delegate void GetBillMasterCompletedEventHandler(bool isSuccess, string msg, BillMaster[] billMasters);

        public event GetBillMasterCompletedEventHandler GetBillMasterCompleted;

        public delegate void GetBillDetailCompletedEventHandler(bool isSuccess, string msg, BillDetail[] billDetails);

        public event GetBillDetailCompletedEventHandler GetBillDetailCompleted;

        public delegate void ApplyCompletedEventHandler(bool isSuccess, string msg);

        public event ApplyCompletedEventHandler ApplyCompleted;

        public delegate void CancelCompletedEventHandler(bool isSuccess, string msg);

        public event CancelCompletedEventHandler CancelCompleted;

        public delegate void ExecuteCompletedEventHandler(bool isSuccess, string msg);

        public event ExecuteCompletedEventHandler ExecuteCompleted;

        public delegate void BcComposeEventHandler(bool isSuccess, string msg);

        public event BcComposeEventHandler BcComposeCompleted;

        public Task(string url)
        {
            this.url = new Uri(url);
        }

        //查询所有可以执行的主单；
        public void SearchBillMaster(string parameter)
        {
            taskType = "getBillMaster";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            parameter = JsonMapper.ToJson(parameter.Split(','));
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getMaster','BillTypes':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }

        //查询选择的所有主单里所有未执行细单；
        public void SearchBillDetail(BillMaster[] billMasters, string productCode, string operateType, string OperateArea, string @operator)
        {
            taskType = "getBillDetail";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billMasters);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getDetail','ProductCode': '" + productCode + "','OperateType':'" + operateType + "','OperateArea':'" + OperateArea + "','Operator':'" + @operator + "','BillMasters':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);

        }

        //请求所有选择的细单；
        public void Apply(BillDetail[] billDetails, string useTag)
        {
            taskType = "apply";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billDetails);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'apply','UseTag':'" + useTag + "','BillDetails':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }

        //取消所有选择的细单的请求；
        public void Cancel(BillDetail[] billDetails, string useTag)
        {
            taskType = "cancel";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billDetails);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'cancel','UseTag':'" + useTag + "','BillDetails':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }

        //执行所有选择的细单；
        public void Execute(BillDetail[] billDetails, string useTag)
        {
            taskType = "execute";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billDetails);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'execute','UseTag':'" + useTag + "','BillDetails':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }

        public void BcCompose(string billNo)
        {
            taskType = "compose";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            client.UploadStringAsync(url, "post", @"billNo=" + billNo);
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }

        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs ex)
        {
            switch (taskType)
            {
                case "getBillMaster":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (GetBillMasterCompleted != null)
                            {
                                GetBillMasterCompleted(true, r.Message, r.BillMasters);
                            }
                        }
                        else
                        {
                            if (GetBillMasterCompleted != null)
                            {
                                GetBillMasterCompleted(false, r.Message, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (GetBillMasterCompleted != null)
                        {
                            GetBillMasterCompleted(false, e.Message, null);
                        }
                    }

                    break;
                case "getBillDetail":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (GetBillDetailCompleted != null)
                            {
                                GetBillDetailCompleted(true, r.Message, r.BillDetails);
                            }
                        }
                        else
                        {
                            if (GetBillDetailCompleted != null)
                            {
                                GetBillDetailCompleted(false, r.Message, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (GetBillDetailCompleted != null)
                        {
                            GetBillDetailCompleted(false, e.Message, null);
                        }
                    }
                    break;
                case "apply":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (ApplyCompleted != null)
                            {
                                ApplyCompleted(true, r.Message);
                            }
                        }
                        else
                        {
                            if (ApplyCompleted != null)
                            {
                                ApplyCompleted(false, r.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (ApplyCompleted != null)
                        {
                            ApplyCompleted(false, e.Message);
                        }
                    }
                    break;
                case "cancel":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (CancelCompleted != null)
                            {
                                CancelCompleted(true, r.Message);
                            }
                        }
                        else
                        {
                            if (CancelCompleted != null)
                            {
                                CancelCompleted(false, r.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (CancelCompleted != null)
                        {
                            CancelCompleted(false, e.Message);
                        }
                    }
                    break;
                case "execute":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (ExecuteCompleted != null)
                            {
                                ExecuteCompleted(true, r.Message);
                            }
                        }
                        else
                        {
                            if (ExecuteCompleted != null)
                            {
                                ExecuteCompleted(false, r.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (ExecuteCompleted != null)
                        {
                            ExecuteCompleted(false, e.Message);
                        }
                    }
                    break;
                case "compose":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (BcComposeCompleted != null)
                            {
                                BcComposeCompleted(true, r.Message);
                            }
                        }
                        else
                        {
                            if (BcComposeCompleted != null)
                            {
                                BcComposeCompleted(false, r.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (BcComposeCompleted != null)
                        {
                            BcComposeCompleted(false, e.Message);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
