using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace THOK.Zeng.ComfixtureHandle
{
    /// <summary>
    /// 处理数据封装类
    /// </summary>
    public  class SendDataPacket
    {
        public object data = null;
        public byte[] btdata = null;
        public int sendTime = 0;
    }
    public abstract class ComfixtureHandle:IDisposable
    {
        private string _ShowModeName = "";
        public delegate void TimeOutEventHandler();
        public event TimeOutEventHandler TimeOutEvent;

        public string ShowModeName
        {
            get { return _ShowModeName; }
            set { _ShowModeName = value; }
        }

        /// <summary>
        /// 存储等待处理的数据 的消息队列
        /// </summary>
        public LinkedList<SendDataPacket> DataQueue = new LinkedList<SendDataPacket>();
        /// <summary>
        /// 当前正在处理的数据包
        /// </summary>
        public SendDataPacket CurrentData = null;
        private Thread myThread = null;
        /// <summary>
        /// 重发超时次数
        /// </summary>
        public int Timeout = 3;
        private Com com = null;
        internal Com Com
        {
            get { return com; }
            set { com = value;}
        }

        /// <summary>
        /// 开始工作，使对象处于待命的数据处理状态；
        /// </summary>
        public void Start()
        {
            if (this.myThread == null)
            {
                this.com.Data_Return += new Com.Data_ReturnHandler(com_Data_Return);
                this.myThread = new Thread(new ThreadStart(this.Run));
                this.myThread.Name = "PortThread:" + this.com.SerialPort.PortName;
                this.myThread.Start();
            }
        }
        /// <summary>
        /// 采用独立线程对数据进行，存取处理。
        /// </summary>
        private void Run()
        {
            try
            {
                if (this.DataQueue.Count != 0 && this.CurrentData == null)
                {
                    this.CurrentData = this.DataQueue.First.Value;
                    this.DataQueue.RemoveFirst();
                    this.com.Write(this.CurrentData);
                    this.CurrentData.sendTime = this.Timeout;
                    Thread.Sleep(4000);
                }
            }
            catch { }

            while (this.com.SerialPort != null && this.com.SerialPort.IsOpen)
            {
                try
                {
                    while (this.DataQueue.Count == 0 && this.CurrentData == null)
                    {
                        this.com.DataReceived();
                        Thread.Sleep(100);
                        Com.strTest = "1";
                    }
                    int i = 0;
                    while (this.CurrentData != null && i <10)
                    {
                        Thread.Sleep(50);
                        this.com.DataReceived();
                        i++;
                    }

                    if (this.CurrentData != null)
                    {
                        this.DataQueue.AddFirst(this.CurrentData);
                        this.CurrentData = null;
                    }

                    while (this.DataQueue.First == null && this.DataQueue.Count > 0)
                    {
                        this.DataQueue.RemoveFirst();
                    }

                    if (this.DataQueue.Count > 0)
                    {
                        this.CurrentData = this.DataQueue.First.Value;
                        this.DataQueue.RemoveFirst();

                        if (this.CurrentData.sendTime <= this.Timeout)
                        {
                            this.CurrentData.sendTime++;
                            this.com.Write(this.CurrentData);
                        }
                        else
                        {
                            if (TimeOutEvent != null)
                            {
                                TimeOutEvent();
                            } 
                            // 发送超时次数超过设定值，应记录故障，以便查找原因。
                            this.com.WriteLog("当前数据包发送多次超时：" + this.com.ByteToString(this.CurrentData.btdata));
                            this.CurrentData = null;
                        }
                    }
                }
                catch { }
            }            
        }
        /// <summary>
        /// 将等待处理加入消息队列
        /// </summary>
        /// <param name="Data"></param>
        public void Send(object Data)
        {
             
            this.DataQueue.AddLast(new SendDataPacket() {data = Data ,btdata = this.Com.Encoder.Encode(Data)});
            if (this.DataQueue.Last == null)
            {
                throw new Exception("LastNull");
            }
        }
        /// <summary>
        /// 串口数据返回，译码后进行业务处理，及通知相关订阅事件，必须要重写该方法；
        /// </summary>
        /// <param name="Data"></param>
        abstract public void com_Data_Return(object Data);

        #region IDisposable 成员

        public void Dispose()
        {
            if (this.myThread != null && this.myThread.IsAlive == true)
            {
                this.myThread.Abort();
                while (this.myThread.IsAlive)
                {
                    Thread.Sleep(50);
                }
            }
            if (this.com != null)
            {
                this.Com.CloseSerialPort();
                this.Com = null;
            }
        }

        #endregion
    }
}
