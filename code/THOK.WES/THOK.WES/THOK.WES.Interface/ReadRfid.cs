using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Threading;

namespace THOK.WES.Interface
{
    public delegate void AnalyDataCallback(MessageTran msgTran);
    public class ReadRfid
    {
        public AnalyDataCallback AnalyCallback;
        private SerialPort iSerialPort;      
        List<string> listRfid;
        int m_nLenth = 0;
        byte[] m_btAryBuffer = new byte[4096];
        public ReadRfid()
        {
            iSerialPort = new SerialPort();          
        }

        //关闭串口
        public void CloseCom()
        {
            string con = iSerialPort.PortName;
            iSerialPort.Dispose();
            if (iSerialPort.IsOpen)
            {
                iSerialPort.Close();
            }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="port"></param>
        /// <param name="com"></param>
        public int OpenCom(string strPort, int nBaudrate, out string strException)
        {
            int m_nType = -1;
            strException = string.Empty;

            if (iSerialPort.IsOpen)
            {
                iSerialPort.Close();
            }

            try
            {
                iSerialPort.PortName = strPort;
                iSerialPort.BaudRate = nBaudrate;
                iSerialPort.ReadTimeout = 200;
                iSerialPort.Open();
                m_nType = 0;
            }
            catch (System.Exception ex)
            {
                strException = ex.Message;
                return -1;
            }

            return m_nType;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        public List<string> ReadTrayRfid(string strPort, int nBaudrate,out string errString)
        {
            errString = string.Empty;
            try
            {
                int nType = this.OpenCom(strPort, nBaudrate, out errString);
                if (nType == 0)
                {
                    DateTime now = DateTime.Now;
                    byte btReadId = 0xFF;
                    byte btCmd = 0xb0;
                    listRfid = new List<string>();
                    MessageTran msgTran = new MessageTran(btReadId, btCmd);
                    iSerialPort.Write(msgTran.AryTranData, 0, msgTran.AryTranData.Length);//给串口发送盘存命令
                    //读取二秒就返回信息
                    do
                    {
                        Thread.Sleep(1000);
                        int nCount = iSerialPort.BytesToRead;
                        if (nCount == 0)
                        {
                            return null;
                        }

                        byte[] btAryBuffer = new byte[nCount];
                        int nRead = iSerialPort.Read(btAryBuffer, 0, nCount);//读取串口缓存区数据
                        RunReceiveDataCallback(btAryBuffer);

                    } while (iSerialPort.BytesToRead != 0);
                }
                this.CloseCom();
                return listRfid;
            }
            catch (Exception e)
            {
                throw new Exception("操作串口错误：" + e.Message + "," + errString);
            }
        }

        private void RunReceiveDataCallback(byte[] btAryReceiveData)
        {
            try
            {
                int nCount = btAryReceiveData.Length;
                byte[] btAryBuffer = new byte[nCount + m_nLenth];
                Array.Copy(m_btAryBuffer, btAryBuffer, m_nLenth);
                Array.Copy(btAryReceiveData, 0, btAryBuffer, m_nLenth, btAryReceiveData.Length);

                //分析接收数据，以0xA0为数据起点，以协议中数据长度为数据终止点
                int nIndex = 0;//当数据中存在A0时，记录数据的终止点
                int nMarkIndex = 0;//当数据中不存在A0时，nMarkIndex等于数据组最大索引
                for (int nLoop = 0; nLoop < btAryBuffer.Length; nLoop++)
                {
                    if (btAryBuffer.Length > nLoop + 1)
                    {
                        if (btAryBuffer[nLoop] == 0xA0)
                        {
                            int nLen = Convert.ToInt32(btAryBuffer[nLoop + 1]);
                            if (nLoop + 1 + nLen < btAryBuffer.Length)
                            {
                                byte[] btAryAnaly = new byte[nLen + 2];
                                Array.Copy(btAryBuffer, nLoop, btAryAnaly, 0, nLen + 2);
                                MessageTran msgTran = new MessageTran(btAryAnaly);
                                if (msgTran.AryData.Length == 9)
                                {
                                    string strUID = CCommondMethod.ByteArrayToString(msgTran.AryData, 1, 8);//rfid数据
                                    bool isUID = listRfid.Contains(strUID);//判断集合里面是否有这个rfid数据
                                    if (isUID == false)
                                    {
                                        listRfid.Add(strUID);
                                    }
                                }
                                nLoop += 1 + nLen;
                                nIndex = nLoop + 1;
                            }
                            else
                            {
                                nLoop += 1 + nLen;
                            }
                        }
                        else
                        {
                            nMarkIndex = nLoop;
                        }
                    }
                }

                if (nIndex < nMarkIndex)
                {
                    nIndex = nMarkIndex + 1;
                }

                if (nIndex < btAryBuffer.Length)
                {
                    m_nLenth = btAryBuffer.Length - nIndex;
                    Array.Copy(btAryBuffer, nIndex, m_btAryBuffer, 0, btAryBuffer.Length - nIndex);
                }
                else
                {
                    m_nLenth = 0;
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("读取数据出错了:" + ex.Message);
            }
        }

    }
}
