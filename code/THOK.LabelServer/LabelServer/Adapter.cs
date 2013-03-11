using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Log;
using System.IO;
using THOK.Zeng.ComfixtureHandle.el102;
using THOK.Zeng.ComfixtureHandle;
using System.Collections;
using DataRabbit.HashOrm;
using THOK.Zeng.ComfixtureHandle.el103;
using THOK.WES.Dal;
using THOK.WES.Interface.Model;
using THOK.WES.Interface;

namespace THOK.Application.LabelServer
{
    public partial class Adapter 
    {
        private ConfigUtil configUtil = new ConfigUtil();
        private string url = "";
        public IELabelOperator elOpertor;
        private int _Port;
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        public Adapter()
        {
           
        }

        void elOpertor_Ack(int address, string msg, string type)
        {
            try
            {
                switch (type)
                {
                    case "key":
                        this.elOpertor_KeyRaised(address, Convert.ToInt32(msg), "");
                        break;
                    case "success":
                        break;
                    case "err":
                        WriteLog(type + "[" + address + " ]: " + msg);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                WriteLog(e.Message);
            }
        }

        /// <summary>
        /// KeyRaised (int address,int keyNo ,string recv);
        /// 按键触发事件
        /// </summary>
        /// <param name="address">address：电子标签地址</param>
        /// <param name="keyNo">keyNo：按键号，共3个按键</param>
        /// <param name="recv">recv：控制板返回的数据，供调试用</param>
        private void elOpertor_KeyRaised(int address, int keyNo, string recv)
        {
            try
            {
                IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
                IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
                IFilter filter1 = new Filter(Storages._Sign, 2, ComparisonOperators.Equal);
                IFilter filter2 = new Filter(Storages._Address, _Port.ToString() + address.ToString().PadLeft(3, "0"[0]), ComparisonOperators.Equal);
                IFilter filter3 = new Filter(Storages._Row,(keyNo - 4).ToString().Substring(1, 1),ComparisonOperators.Equal);
                
                if (elOpertor.GetShowModeName() == "mode3")
                {
                    filter3 = new Filter("1","1", ComparisonOperators.Equal);
                }
                IList<Storages> storages = storagesAccesser.Select<Storages>(new FilterTree ("{0} and {1} and {2}",new IFilter [] {filter1,filter2,filter3}));
                if (storages.Count == 1)
                {
                    Storages storage = storages.First();
                    storage.Act = "";
                    storage.ProductName = "";
                    storage.Contents = "";
                    storage.Sign = 0;
                    storage.Err = 0;
                    storage.NumberShow = "";                    
                    storagesAccesser.Update(storage);
                    storagesAccesser.ExecuteNonQuery("Update [Storages] Set [Sign] = 1 where [Address] = '" + _Port.ToString() + address.ToString().PadLeft(3, "0"[0]) + "' and [Sign] = 3 ");

                    //++
                    IFilter filter4 = new Filter(Sy_ShowInfo._HardwareReadState, 1, ComparisonOperators.Equal);
                    IFilter filter5 = new Filter(Sy_ShowInfo._StorageID,storage.StorageID, ComparisonOperators.Equal);
                    IFilter filter6 = new Filter(Sy_ShowInfo._ConfirmState, 0, ComparisonOperators.Equal);
                    IList<Sy_ShowInfo> sy_showinfos = sy_showinfosAccesser.Select<Sy_ShowInfo>(new FilterTree("{0} and {1} and {2}", filter4, filter5, filter6));                     
                    if (sy_showinfos.Count == 1)
                    {
                        Sy_ShowInfo sy_showinfo = sy_showinfos.First();
                        sy_showinfo.ConfirmState = 1;
                        sy_showinfosAccesser.Update(sy_showinfo);

                        url = configUtil.GetConfig("URL")["URL"];
                        IList<BillDetail> billDetails = new List<BillDetail>();
                        BillDetail billDetail = new BillDetail();
                        billDetail.BillNo = sy_showinfo.OrderMasterID.ToString();
                        billDetail.BillType = sy_showinfo.OperateType.ToString();
                        billDetail.DetailID = Convert.ToInt32(sy_showinfo.OrderDetailID.ToString());
                        billDetail.Operator = Environment.MachineName;
                        billDetail.OperatePieceQuantity = Convert.ToInt32(sy_showinfo.OperatePiece);
                        billDetail.OperateBarQuantity = Convert.ToInt32(sy_showinfo.OperateItem);
                        billDetails.Add(billDetail);
                        BillDetail[] tmp = new BillDetail[billDetails.Count];
                        billDetails.CopyTo(tmp, 0);
                        Task task = new Task(null, url);
                        task.Execute(tmp, "1");
                    }                   
                    //++

                    sy_showinfosAccesser.ExecuteNonQuery("Update [Sy_ShowInfo] set [ConfirmState] = 1 where HardwareReadState =1 and [StorageID] = '" + storage.StorageID + "'");
                    Service.SendData();
                    if (false)
                    {
                        string[] data = new string[5];
                        for (int i = 0; i < 5; i++)
                            data[i] = "";
                        data[keyNo - 1] = "按钮返回成功！！！";
                        elOpertor.SendData(int.Parse(storage.Address.Substring(storage.Address.Length - 2, 2)), data);
                    }
                    if (false)
                    {
                        DataRabbit.HashOrm.IHashOrmAccesser delAccesser = DataRabbit.HashOrm.DBFactory.NewHashOrmAccesser(DataRabbit.HashOrm.DBFactory.NewTransactionScopeFactory("ServerDataConfig").NewTransactionScope());
                        delAccesser.ExecuteNonQuery("");
                    }
                }              
            }
            catch (Exception e)
            {
                WriteLog(e.Message);
            }
        }

        public void Start()
        {
            try
            {
                ComfixtureConfig config = new ComfixtureConfig() { XMLFilePath = ".\\" + (new AdapterConfig()).GetComfixtureHandle_Type(_Port ) + ".xml" };
                ComfixtureHandleFactory cf = new ComfixtureHandleFactory() { ComfixtureConfig = config };
                this.elOpertor = (IELabelOperator)cf.NewComfixtureHandle(_Port);
                this.elOpertor.Ack += new AckHandler(elOpertor_Ack);

                switch (elOpertor.GetShowModeName())
                {
                    case "mode1": //货架汉显示标签 显示模式 <大标签>
                        elOpertor.SetFunctionType(1);
                        elOpertor.SetRowTextSize(1, 1, 1, 1, 1);
                        elOpertor.SetFunction(1, 0, 1, 0, 0, 0);
                        elOpertor.Start();
                        Reset();
                        break;
                    case "mode2": //条烟柜汉显标签 显示模式 《大标签》
                        elOpertor.SetFunctionType((byte)0);
                        elOpertor.SetRowTextSize(2, 2, 2, 2, 2);
                        elOpertor.SetFunction(1, 0, 1, 0, 0, 0);
                        elOpertor.Start();
                        Reset();
                        break;
                    case "mode3": // 数显标签 显示模式 《数显标签》
                        elOpertor.SetFunctionType((ShowModel)(5 + 16));
                        elOpertor.SetShowColor((TextColor)0);
                        elOpertor.SetFlashState((FlashState)1, (FlashModel)(3), Convert.ToInt32("00001", 2));
                        ////设置按键灯
                        elOpertor.SetKeysState((FuntionState)1, Convert.ToInt32(0),
                            (FuntionState)0, Convert.ToInt32(0),
                            (FuntionState)0, Convert.ToInt32(0));
                        ////设置功能字段
                        elOpertor.SetFunction(FuntionState.Unchanging, 0, (FuntionState)0, Convert.ToByte(0),
                            (FuntionState)0, Convert.ToByte(0));
                        elOpertor.Start();
                        Reset();
                        break;
                    case "mode4": // 联控标签 未使用 《联控标签》                        
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                WriteLog(e.Message);
                throw new Exception(e.Message);
            }
        }
        public void Stop()
        {
            try
            {
                if (elOpertor != null )
                {
                    elOpertor.Dispose();
                }
                
            }
            catch (InvalidOperationException e)
            {
                WriteLog(e.Message);
            }
        }
        public void Reset()
        {
            if (elOpertor.GetShowModeName() == "mode3")  // 数显标签 显示模式 《数显标签》 复位方式不同
            {
                ////设置按键灯
                elOpertor.SetKeysState(FuntionState.Close, Convert.ToInt32(0),
                    (FuntionState)0, Convert.ToInt32(0),
                    (FuntionState)0, Convert.ToInt32(0));
                elOpertor.SendData(0,new string [] {"          "});
                ////设置按键灯
                elOpertor.SetKeysState(FuntionState.Start, Convert.ToInt32(0),
                    (FuntionState)0, Convert.ToInt32(0),
                    (FuntionState)0, Convert.ToInt32(0));
                return;
            }
            else
            {
                elOpertor.ClearDataQueue();

                IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
                IFilter filter1 = new Filter(Storages._Port, _Port.ToString(), ComparisonOperators.Equal);

                IList<Storages> storages = storagesAccesser.Select<Storages>(filter1, Storages._Address);
                IEnumerable<Storages> storageslist = storages.Distinct();
                foreach (Storages storage in storageslist)
                {
                    elOpertor.ResetElectronicLabel(int.Parse(storage.Address.Substring(storage.Address.Length - 3, 3)));
                }
            }
        }
        private void WriteLog(String logMessage)
        {
            try
            {
                if (!Directory.Exists(".\\LOG\\" + DateTime.Now.ToLongDateString()))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(".\\LOG\\" + DateTime.Now.ToLongDateString());
                }
                using (StreamWriter w = File.AppendText(".\\LOG\\" + DateTime.Now.ToLongDateString() + "\\Com" + _Port + "log.txt"))
                {
                    w.Write("\r\nLog Entry : ");
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                    w.WriteLine("  :");
                    w.WriteLine("  :{0}", logMessage);
                    w.WriteLine("-------------------------------");
                    // Update the underlying file.
                    w.Flush();
                    // Close the writer and underlying file.
                    w.Close();
                }
            }
            catch (Exception e)
            {
                WriteLog(e.Message);
            }
        }
    }
}
