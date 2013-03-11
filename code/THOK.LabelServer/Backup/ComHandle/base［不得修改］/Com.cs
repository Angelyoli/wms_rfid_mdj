using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;

namespace THOK.Zeng.ComfixtureHandle
{
    /// <summary>
    /// 封装串口访问通用方法
    /// </summary>
    class Com
    {
        public static string strTest = "";
        private string strRecv = "";
        public string strReadEND = "7D0B";
        public string strReadSTX = "0A7B";
        public int port = 0;

        public delegate void Data_ReturnHandler(object Data);
        public event Data_ReturnHandler Data_Return;
        private SerialPort serialPort = null;

        public SerialPort SerialPort
        {
            get { return serialPort; }
        }

        private IEncoder encoder = null;
        internal IEncoder Encoder
        {
            get { return encoder; }
            set { encoder = value; }
        }

        public void OpenSerialPort(string name, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            try
            {
                this.serialPort = new SerialPort();
                this.serialPort.ReadTimeout = 100;
                this.serialPort.PortName = name;
                this.serialPort.BaudRate = baudRate;
                this.serialPort.Parity = parity;
                this.serialPort.DataBits = dataBits;
                this.serialPort.StopBits = stopBits;
                this.serialPort.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public  void DataReceived()
        {
            try
            {
                if (this.serialPort.BytesToRead == 0) { return; }
                int i;
                do
                {
                    byte[] btRecv = new byte[this.serialPort.ReadBufferSize];
                    int nRead = this.serialPort.Read(btRecv, 0, this.serialPort.BytesToRead);
                    i = 0;
                    while (i < nRead)
                    {
                        strRecv = strRecv + btRecv[i].ToString("X2");
                        i++;
                    }
                }
                while ((this.serialPort.BytesToRead != 0));

                //数据分析 ［处理进入死循环和数据丢失问题］,修改strRecv为全局变量保存等待分析的数据
                string SplitCmd = "";
                
                if (strRecv.LastIndexOf((this.strReadEND)) != -1)
                {
                    SplitCmd = strRecv.Substring(0, strRecv.LastIndexOf(this.strReadEND) + this.strReadEND.Length);
                    strRecv = strRecv.Substring(strRecv.LastIndexOf(this.strReadEND) + this.strReadEND.Length - 1, strRecv.Length - (strRecv.LastIndexOf(this.strReadEND) + this.strReadEND.Length));
                }
                if (SplitCmd == ""){return;}
                string[] strOperator = new string[] { this.strReadSTX, this.strReadEND};
                string[] strCmd = SplitCmd.Split(strOperator, StringSplitOptions.RemoveEmptyEntries);
                if (strCmd.Length <= 0) { return; }
                for (i = 0; i < strCmd.Length; i++)
                {
                    this.Data_Return(this.Encoder.Decode(strCmd[i]));
                }
            }
            catch{}
        }
        public void Write(SendDataPacket CMD)
        {
            byte[] cmd;
            if (CMD.btdata == null)
            {
                cmd = this.Encoder.Encode(CMD.data);
                CMD.btdata = cmd;
            }
            else
            {
                cmd = CMD.btdata;
            }

            this.WriteLog("Length = " + cmd.Length.ToString() + ":" + ByteToString(cmd));

            this.SerialPort.Write(cmd, 0, cmd.Length);
        }
        public string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2} ", InByte);
            }
            return StringOut;
        }
        public void CloseSerialPort()
        {
            try
            {
                if ((this.serialPort != null) && this.serialPort.IsOpen)
                {
                    this.serialPort.Close();
                    this.serialPort = null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void WriteLog(String logMessage)
        {
            try
            {
                if (!Directory.Exists(".\\LOG02\\" + DateTime.Now.ToLongDateString()))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(".\\LOG02\\" + DateTime.Now.ToLongDateString());
                }
                using (StreamWriter w = File.AppendText(".\\LOG02\\" + DateTime.Now.ToLongDateString() + "\\" + this.serialPort.PortName + "-log.txt"))
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
            catch { }
        }
    }
}
