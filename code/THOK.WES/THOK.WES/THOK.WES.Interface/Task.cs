using System;
using System.Collections.Generic;
using System.Text;
using THOK.WES.Interface.Model;
using System.Net;
using LitJson;
using System.Windows.Forms;
using THOK.AF.View;

namespace THOK.WES.Interface
{
    public class Task
    {
        private Uri url = null;
        private Form targetForm = null;

        private string taskType = string.Empty;

        public delegate void GetBillMasterCompletedEventHandler(bool isSuccess,string msg,BillMaster[] billMasters);

        public event GetBillMasterCompletedEventHandler GetBillMasterCompleted;

        public delegate void GetBillDetailCompletedEventHandler(bool isSuccess,string msg,BillDetail[] billDetails);

        public event GetBillDetailCompletedEventHandler GetBillDetailCompleted;

        public delegate void ApplyCompletedEventHandler(bool isSuccess,string msg);

        public event ApplyCompletedEventHandler ApplyCompleted;

        public delegate void CancelCompletedEventHandler(bool isSuccess,string msg);

        public event CancelCompletedEventHandler CancelCompleted;

        public delegate void ExecuteCompletedEventHandler(bool isSuccess,string msg);

        public event ExecuteCompletedEventHandler ExecuteCompleted;

        public Task(Form targetForm, string url)
        {
            this.url = new Uri(url); ;
            this.targetForm = targetForm;
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
        public void SearchBillDetail(BillMaster[] billMasters, string productCode, string operateType,string OperateArea, string @operator)
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
                                targetForm.Invoke(GetBillMasterCompleted,true, r.Message, r.BillMasters);
                            }
                        }
                        else
                        {
                            if (GetBillMasterCompleted != null)
                            {
                                targetForm.Invoke(GetBillMasterCompleted, false, r.Message, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (GetBillMasterCompleted != null)
                        {
                            targetForm.Invoke(GetBillMasterCompleted, false, e.Message, null);
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
                                targetForm.Invoke(GetBillDetailCompleted,true, r.Message, r.BillDetails);
                            }
                        }
                        else
                        {
                            if (GetBillDetailCompleted != null)
                            {
                                targetForm.Invoke(GetBillDetailCompleted,false, r.Message, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (GetBillDetailCompleted != null)
                        {
                            targetForm.Invoke(GetBillDetailCompleted,false, e.Message, null);
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
                                targetForm.Invoke(ApplyCompleted,true, r.Message);
                            }
                        }
                        else
                        {
                            if (ApplyCompleted != null)
                            {
                                targetForm.Invoke(ApplyCompleted,false, r.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (ApplyCompleted != null)
                        {
                            targetForm.Invoke(ApplyCompleted,false, e.Message);
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
                                targetForm.Invoke(CancelCompleted,true, r.Message);
                            }
                        }
                        else
                        {
                            if (CancelCompleted != null)
                            {
                                targetForm.Invoke(CancelCompleted,false, r.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (CancelCompleted != null)
                        {
                            targetForm.Invoke(CancelCompleted,false, e.Message);
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
                                targetForm.Invoke(ExecuteCompleted,true, r.Message);
                            }
                        }
                        else
                        {
                            if (ExecuteCompleted != null)
                            {
                                targetForm.Invoke(ExecuteCompleted,false, r.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (ExecuteCompleted != null)
                        {
                            targetForm.Invoke(ExecuteCompleted,false, e.Message);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
