using System;
using THOK.WES.Interface.Model;
using System.Net;
using LitJson;
using System.Data;

namespace THOK.WES.Interface
{
    public class Task
    {
        private Uri url = null;

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

        public delegate void GetRfidInfoCompletedEventHandler(bool isSuccess, string msg, BillDetail[] billDetails);

        public event GetRfidInfoCompletedEventHandler GetRfidInfoCompleted;

        public delegate void BcComposeEventHandler(bool isSuccess, string msg);

        public event BcComposeEventHandler BcComposeCompleted;

        public delegate void GetShelfEventHandler(bool isSuccess, string msg, ShelfInfo[] shelfInfo);

        public event GetShelfEventHandler GetShelf;

        public Task(string url)
        {
            this.url = new Uri(url);
        }

        #region 浪潮接口
        //仓储入库进度反馈
        public void GetWarehouseInBillProgressFeedback(BillDetail[] billDetails)
        {
            taskType = "WarehouseInBillProgressFeedback";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billDetails);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getWarehouseInBillProgressFeedback','BillDetails':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }
        //仓储入库完成
        public void GetWarehouseInBillFinish(BillDetail[] billDetails)
        {
            taskType = "WarehouseInBillFinish";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billDetails);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getWarehouseInBillFinish','BillDetails':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }
        //仓储出库进度反馈
        public void GetWarehouseOutBillProgressFeedback(BillDetail[] billDetails)
        {
            taskType = "WarehouseOutBillProgressFeedback";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billDetails);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getWarehouseOutBillProgressFeedback','BillDetails':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }
        //仓储出库完成
        public void GetWarehouseOutBillFinish(BillDetail[] billDetails)
        {
            taskType = "WarehouseOutBillFinish";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            string parameter = JsonMapper.ToJson(billDetails);
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getWarehouseOutBillFinish','BillDetails':" + parameter + "}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        } 
        #endregion

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

        //根据rfid查询卷烟和数量；
        public void SearchRfidInfo(string rfid)
        {
            taskType = "getRfidInfo";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            //string parameter = JsonMapper.ToJson(rfid.Split(','));
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getRfidInfo','RfidId':'" + rfid + "'}");
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
        //查询shelf的信息
        public void Getshelf()
        {
            taskType = "getShelf";
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = @"application/x-www-form-urlencoded; charset=UTF-8";
            client.UploadStringAsync(url, "post", @"Parameter={'Method':'getShelf'}");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        }

        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs ex)
        {           
            switch (taskType)
            {
                #region 主单
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
                #endregion
                #region 细单
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
                #endregion
                #region 申请
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
                #endregion
                #region 取消
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
                #endregion
                #region 确认
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
                #endregion
                #region RFID
                case "getRfidInfo":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (GetRfidInfoCompleted != null)
                            {
                                GetRfidInfoCompleted(true, r.Message, r.BillDetails);
                            }
                        }
                        else
                        {
                            if (GetRfidInfoCompleted != null)
                            {
                                GetRfidInfoCompleted(false, r.Message, r.BillDetails);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (GetRfidInfoCompleted != null)
                        {
                            GetRfidInfoCompleted(false, e.Message, null);
                        }
                    }
                    break;
                #endregion
                #region 货架
                case "getShelf":
                    try
                    {
                        string result = ex.Result;
                        Result r = JsonMapper.ToObject<Result>(result);
                        if (r.IsSuccess)
                        {
                            if (GetShelf != null)
                            {
                                GetShelf(true, r.Message,r.ShelfInfo);
                            }
                        }
                        else
                        {
                            if (GetShelf != null)
                            {
                                GetShelf(false, r.Message,null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (GetShelf != null)
                        {
                            GetShelf(false, e.Message,null);
                        }
                    }
                    break;
                #endregion
                #region 其他
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
                #endregion
                default:
                    break;
            }
        }
    }
}
