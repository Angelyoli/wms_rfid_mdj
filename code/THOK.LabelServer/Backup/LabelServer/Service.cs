using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRabbit.DBAccessing;
using DataRabbit.DBAccessing.Application;
using DataRabbit.HashOrm;
using System.Threading;
namespace THOK.Application.LabelServer
{
    /// <summary>
    /// 静态功能方法。
    /// </summary>
    class Service
    {
        private static System.Windows.Forms.Form MainThreadForm = null;
        private static System.Windows.Forms.ToolStripStatusLabel MainThreadFormStatusLabel = null;
        private static System.Windows.Forms.Timer timer = null;
        private static Adapter[] adapters;

        private static Dictionary<string, Dictionary<bool, string>> addressDictionary = new Dictionary<string, Dictionary<bool, string>>();

        /// <summary>
        /// 回调到主线程的委托。
        /// </summary>
        /// <param name="msgtype">信息类型</param>
        /// <param name="msg">信息内容</param>
        private delegate void CallBack(int msgtype, string msg);
        private static event CallBack ServiceStatusChanged = null;

        public static void InitializeService(System.Windows.Forms.Form _MainThreadForm, System.Windows.Forms.ToolStripStatusLabel _MainThreadFormStatusLabel, System.Windows.Forms.Timer _timer, Adapter[] _adapters)
        {
            MainThreadForm = _MainThreadForm;
            MainThreadFormStatusLabel = _MainThreadFormStatusLabel;
            timer = _timer;
            adapters = _adapters;
            ServiceStatusChanged += new CallBack(Service_ServiceStatusChanged);
        }

        private static void Service_ServiceStatusChanged(int msgtype, string msg)
        {
            switch (msgtype)
            {
                case 0:// 显示状态
                    MainThreadFormStatusLabel.Text = msg + "   当前任务总用时：" + timer.Tag.ToString() + " 秒"; 
                    break;
                case 1:// 计时开始
                    timer.Tag = 0;
                    timer.Start();
                    MainThreadFormStatusLabel.Tag = msg;
                    MainThreadFormStatusLabel.Text = msg + "   用时：" + timer.Tag.ToString() + " 秒"; 
                    break;
                case 2:// 计时完成
                    timer.Stop();
                    MainThreadFormStatusLabel.Tag = "";
                    MainThreadFormStatusLabel.Text = msg + "   用时：" + timer.Tag.ToString() + " 秒"; 
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 服务启动入口。
        /// </summary>
        /// <param name="form">启动线程的主线程窗体</param>
        /// <param name="timer">主线程窗体的计时器</param>
        /// <param name="adapters">串口适配器数组</param>
        public static void Run()
        {
            int getKeyTimer = 0;
            try
            {
                while (true)
                {
                    // 检查是否有新的数据到达。
                    while (!IsExistNewData())
                    {
                        SendData();
                        Thread.Sleep(1000);
                        MainThreadForm.Invoke(ServiceStatusChanged, 0, "没有新数据到达，等待任务！");
                        SendData();
                        Thread.Sleep(1000);
                        
                        getKeyTimer++;
                        if (getKeyTimer > 10)
                        {
                            getKeyTimer = 0;
                            GetKey();
                        }
                        if (false)
                        {
                            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
                            storagesAccesser.ExecuteNonQuery("update storages set sign = 1");
                            SendData();                            
                        }
                    }

                    // 新数据到达延时1000毫秒处理，以便数据接收完整以进行一次处理，以免多次处理影响性能。
                    Thread.Sleep(1000);

                    // 从接口中间表，同步数据到任务队列（填入编码映射表）。
                    MainThreadForm.Invoke(ServiceStatusChanged, 1, "开始同步数据到任务队列！");
                    PushData();
                    MainThreadForm.Invoke(ServiceStatusChanged, 2, "完成同步数据到任务队列！");

                    // 从编码映射表取得任务队列进行发送。
                    MainThreadForm.Invoke(ServiceStatusChanged, 1, "开始任务进行发送！");
                    SendData();
                    while (!IsComplete())
                    {
                        Thread.Sleep(2000);
                    }
                    // 当前任务队列完成发送。
                    MainThreadForm.Invoke(ServiceStatusChanged, 2, "完成当前任务发送！");
                }
            }
            catch (Exception e)
            {
                MainThreadForm.Invoke(ServiceStatusChanged, 0, "服务停止！"  + e.Message );
            }

        }

        /// <summary>
        /// 检查中间表是否有新数据需要处理。
        /// </summary>
        /// <returns>返回是否有新数据需要处理。</returns>
        public static bool IsExistNewData()
        {
            bool bIsExist = false;

            using (TransactionScope Scope = DBFactory.NewTransactionScope(false))
            {
                IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
                IFilter filter = new Filter(Sy_ShowInfo._ReadState, 0, ComparisonOperators.Equal);
                IList<Sy_ShowInfo> sy_showinfos = sy_showinfosAccesser.Select<Sy_ShowInfo>(filter);

                if (sy_showinfos.Count > 0)
                {
                    bIsExist = true;
                }
            }

            return bIsExist;
        }
        
        /// <summary>
        /// 检查所传入的串口适配器是否完成所有指令的执行。
        /// </summary>
        /// <param name="adapters">传入的串口适配器数组</param>
        /// <returns>返回是否完成任务</returns>
        public static bool IsComplete()
        {
            bool bIsComplete = true;
            foreach (Adapter adapter in adapters)
            {
                if (adapter != null && !adapter.elOpertor.IsComplete ())
                {
                    bIsComplete = false;
                    return bIsComplete;
                }
            }
            return bIsComplete;
        }

        /// <summary>
        /// 同步数据到任务队列。
        /// </summary>
        public static void PushData()
        {
            //开始任务前同步标签系统数据。
            using (TransactionScope Scope = DBFactory.NewTransactionScope(false))
            {
                IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
                IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
                IFilter filter1 = new Filter(Sy_ShowInfo._ReadState, 0, ComparisonOperators.Equal);
                IList<Sy_ShowInfo> sy_showinfos = sy_showinfosAccesser.Select<Sy_ShowInfo>(filter1);
                IFilter filter2 = new Filter(Storages._Sign, 0, ComparisonOperators.Equal);
                storagesAccesser.SelectToHash<Storages>(filter2);

                foreach (Sy_ShowInfo sy_showinfo in sy_showinfos)
                {
                    IList<Storages> storages = storagesAccesser.Select<Storages,string>(Storages._StorageID, sy_showinfo.StorageID);
                    if (storages.Count == 1)
                    {
                        Storages storage = storages[0];
                        switch (sy_showinfo.OperateType)
                        {
                            case 1:
                                storage.Act = "入库";
                                break;
                            case 2:
                                storage.Act = "出库";
                                break;
                            case 4:
                                storage.Act = "盘点";
                                break;
                            case 5:
                                storage.Act = "移入";
                                break;
                            case 6:
                                storage.Act = "移出";
                                break;
                            case 3:
                                storage.Act = "移库";
                                break;
                            default:
                                break;
                        }

                        storage.ProductName = sy_showinfo.TobaccoName;
                        if (sy_showinfo.Contents == "")
                        {
                            storage.Contents = (sy_showinfo.OperatePiece > 0 ? sy_showinfo.OperatePiece.ToString() + "件" : "") + (sy_showinfo.OperateItem > 0 ? sy_showinfo.OperateItem + "条" : "");
                        }
                        else
                        {
                            storage.Contents = sy_showinfo.Contents;
                        }
                        
                        storage.Sign = 1;
                        storage.NumberShow = sy_showinfo.OperateType.ToString().PadLeft(1, "0"[0]) + storage.Row +"-" + sy_showinfo.OperatePiece.ToString().PadLeft(2, "0"[0]);
                        storage.NumberShow = storage.NumberShow[0] + " " + storage.NumberShow[1] + " " + storage.NumberShow[2] + " " + storage.NumberShow[3] + " " + storage.NumberShow[4] + " ";
                        
                        
                        storagesAccesser.Update(storage);
                        sy_showinfo.ReadState = 1;
                        sy_showinfosAccesser.Update(sy_showinfo);
                    }
                }
            }
        }
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object lock1 = new object();
        /// <summary>
        /// 组织数据并传入到串口适配器。
        /// </summary>
        /// <param name="adapters">串口适配器数组</param>
        public static void SendData()
        {
            lock (lock1)
            {
                using (TransactionScope Scope = DBFactory.NewTransactionScope(false))
                {
                    IList<Storages> ModifiyStorages = new List<Storages>();
                    IList<Sy_ShowInfo> ModifiySy_ShowInfo = new List<Sy_ShowInfo>();

                    IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
                    IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();

                    IFilter filter1 = new Filter(Sy_ShowInfo._ReadState, 1, ComparisonOperators.Equal);
                    IFilter filter2 = new Filter(Sy_ShowInfo._HardwareReadState, 0, ComparisonOperators.Equal);
                    sy_showinfosAccesser.SelectToHash<Sy_ShowInfo>(new FilterTree("{0} and {1}", filter1, filter2));

                    IFilter filter3 = new Filter(Storages._Sign, 0, ComparisonOperators.NotEqual);
                    storagesAccesser.SelectToHash<Storages>(filter3);

                    IFilter filter4 = new Filter(Storages._Sign, 1, ComparisonOperators.Equal);
                    IList<Storages> storages = storagesAccesser.Select<Storages>(filter4);

                    IEnumerable<string> addresses = from storage in storages
                                                    orderby storage.Address.Substring(storage.Address.Length - 3 - 1, 3), storage.Port.PadLeft(4, "0"[0])
                                                    select storage.Address;
                    //++
                    foreach (string address in addressDictionary.Keys)
                    {
                        IList<Storages> stotagelist = storagesAccesser.Select<Storages, string>(Storages._Address, address);
                        if (addressDictionary[address].ContainsKey(true) && (stotagelist.Count == 0 || addresses.Distinct().Contains(address)))
                        {
                            adapters[Convert.ToInt32(addressDictionary[address][true])].elOpertor.ResetElectronicLabel(int.Parse(address.Substring(address.Length - 3, 3)));
                            //adapters[Convert.ToInt32(addressDictionary[address][true])].elOpertor.SendData(int.Parse(address.Substring(address.Length - 3, 3)),new string[] {"1","2","3","",""});
                            addressDictionary[address].Remove(true);
                        }
                    }
                    //++

                    foreach (string address in addresses.Distinct())
                    {
                        IList<Storages> stotagelist = storagesAccesser.Select<Storages, string>(Storages._Address,address);         
                        string _port = "";

                        string[] data = new string[5];
                        for (int i = 0; i < 5; i++)
                            data[i] = "";

                            foreach (Storages storage in stotagelist)
                            {
                                string s1, s2, s3, s4;
                                _port = storage.Port;
                                switch (adapters[Convert.ToInt32(storage.Port)].elOpertor.GetShowModeName())
                                {
                                    case "mode1":  //货架汉显示标签 显示模式 <大标签>
                                        s1 = storage.StorageName.Trim() + " " + storage.Act.Trim() + " " + storage.Contents.Trim() + storage.ProductName.Trim() + "".PadRight(56, " "[0]);
                                        s1 = StrHandle.GetStringWith(s1, 56);

                                        if (storage.Row == "4")
                                        {
                                            continue;
                                        }

                                        if (storage.Row == "3")
                                        {
                                            if (data[0] != "")
                                            {
                                                continue;
                                            }
                                            data[0] = s1;
                                        }
                                        if (storage.Row == "2")
                                        {
                                            if (data[1] != "")
                                            {
                                                continue;
                                            }
                                            data[1] = s1;
                                        }
                                        if (storage.Row == "1")
                                        {
                                            if (data[2] != "")
                                            {
                                                continue;
                                            }
                                            data[2] = s1;
                                        }
                                        break;
                                    case "mode2":  //条烟柜汉显标签 显示模式 《大标签》
                                        s1 = "储位：" + storage.StorageName.Trim();
                                        s1 = StrHandle.GetStringWith(s1, 30);

                                        s2 = "操作：" + storage.Act.Trim();
                                        s2 = StrHandle.GetStringWith(s2, 30);

                                        s3 = "品牌：" + storage.ProductName.Trim();
                                        s3 = StrHandle.GetStringWith(s3, 30);

                                        s4 = "数量：" + storage.Contents.Trim();
                                        s4 = StrHandle.GetStringWith(s4, 26);

                                        data[0] = s1;
                                        data[0] = data[0] + s2;
                                        data[0] = data[0] + s3;
                                        data[0] = data[0] + s4;
                                        break;
                                    case "mode3": // 数显标签 显示模式 《数显标签》
                                        data[0] = storage.NumberShow;
                                        break;
                                    case "mode4": // 联控标签 未使用 《联控标签》
                                        break;
                                    default:
                                        break;
                                }
                                storage.Sign = 2;

                                try
                                {
                                    IList<Sy_ShowInfo> sy_showinfolist = sy_showinfosAccesser.Select<Sy_ShowInfo, string>(Sy_ShowInfo._StorageID, storage.StorageID);

                                    if (sy_showinfolist.Count == 1)
                                    {
                                        sy_showinfolist[0].HardwareReadState = 1;
                                        ModifiyStorages.Add(new Storages() { StorageID = storage.StorageID, Sign = storage.Sign });
                                        ModifiySy_ShowInfo.Add(sy_showinfolist[0]);
                                    }

                                    if (adapters[Convert.ToInt32(storage.Port)].elOpertor.GetShowModeName() == "mode1")
                                    {
                                        if (data[0] != "" && data[1] != "" && data[2] != "")
                                        {
                                            break;
                                        }
                                    }
                                    if (adapters[Convert.ToInt32(storage.Port)].elOpertor.GetShowModeName() == "mode2")
                                    {
                                        break;
                                    }
                                    if (adapters[Convert.ToInt32(storage.Port)].elOpertor.GetShowModeName() == "mode3")
                                    {
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    throw new Exception("e" + e.StackTrace);
                                }
                            }

                            if (_port != "")
                            {
                                //++
                                addressDictionary[address] = new Dictionary<bool, string>();
                                addressDictionary[address][true] = _port;
                                //++
                                adapters[Convert.ToInt32(_port)].elOpertor.SendData(int.Parse(address.Substring(address.Length - 3, 3)), data);
                            }
             
                        storagesAccesser.ExecuteNonQuery("Update [Storages] Set [Sign] = 3 where [Address] = '" + address + "' and [Sign] = 1 ");
                    }
                    storagesAccesser.Update(ModifiyStorages);
                    sy_showinfosAccesser.Update(ModifiySy_ShowInfo);
                }
            }
        }

        /// <summary>
        /// 查询，标签按钮状态。
        /// </summary>
        public static void GetKey()  // 联控标签 未使用 《联控标签》
        {
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
            IFilter filter = new Filter(Storages._Sign, 2, ComparisonOperators.Equal);
            IList<Storages> storages = storagesAccesser.Select<Storages>(filter);
            foreach (Storages  storage in storages)
            {
                if (adapters[Convert.ToInt32(storage.Port)].elOpertor.GetShowModeName() == "mode4")
                {
                    adapters[Convert.ToInt32(storage.Port)].elOpertor.GetKey(int.Parse(storage.Address.Substring(storage.Address.Length - 3, 3)));
                }                
            }
        }
    }
}
