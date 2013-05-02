using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace THOK.Zeng.ComfixtureHandle
{
    public class ComfixtureHandleFactory
    {
        private ComfixtureConfig _ComfixtureConfig = null;

        public ComfixtureConfig ComfixtureConfig
        {
            get { return _ComfixtureConfig; }
            set { _ComfixtureConfig = value; }
        }

        public ComfixtureHandle NewComfixtureHandle(int PortNum)
        {
            Com com = new Com();
            com.strReadSTX = _ComfixtureConfig.GetCom_strReadSTX();    //"0A7B";  //可配置
            com.strReadEND = _ComfixtureConfig.GetCom_strReadEND();    //"7D0B";  //可配置
            com.Encoder = _ComfixtureConfig.GetCom_Encoder();          //可配置
            com.port = PortNum;
            com.OpenSerialPort("COM" + PortNum.ToString(), _ComfixtureConfig.GetCom_SerialPortSet_BaudRate(), _ComfixtureConfig.GetCom_SerialPortSet_Parity(), _ComfixtureConfig.GetCom_SerialPortSet_DataBits(), _ComfixtureConfig.GetCom_SerialPortSet_StopBits());    //可配置

            ComfixtureHandle elOperator = _ComfixtureConfig.GetComfixtureHandle();          //可配置
            elOperator.ShowModeName = _ComfixtureConfig.GetComfixtureHandle_ShowModeName(PortNum);     //可配置
            elOperator.Timeout = _ComfixtureConfig.GetComfixtureHandle_TimeOut();           //可配置
            elOperator.Com = com; 

            return elOperator;
        }
    }
}
