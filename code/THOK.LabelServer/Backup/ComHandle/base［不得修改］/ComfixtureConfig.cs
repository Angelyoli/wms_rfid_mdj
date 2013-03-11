using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Xml;

namespace THOK.Zeng.ComfixtureHandle
{
    public class ComfixtureConfig
    {
        private string _XMLFilePath = "D:\\［项目_现场］\\代码\\电子标签系统\\新电子标签控件C#版\\ComHandle\\ComfixtureConfig.xml";

        public string XMLFilePath 
        {
            get { return _XMLFilePath; }
            set { _XMLFilePath = value; }
        }
        public string GetCom_strReadSTX()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return doc.GetElementsByTagName("strReadSTX").Item(0).InnerText;
        }

        public string  GetCom_strReadEND()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return doc.GetElementsByTagName("strReadEND").Item(0).InnerText;
        }

        internal IEncoder GetCom_Encoder()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return (IEncoder)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(doc.GetElementsByTagName("Encoder").Item(0).Attributes.Item(0).Value);
        }

        public int GetCom_SerialPortSet_BaudRate()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return Convert.ToInt32(doc.GetElementsByTagName("BaudRate").Item(0).InnerText);
        }

        public Parity GetCom_SerialPortSet_Parity()
        {
            Parity parity ;
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            switch (Convert.ToInt32(doc.GetElementsByTagName("Parity").Item(0).InnerText))
            {
                case 0:
                    parity = Parity.None;
                    break;
                case 1:
                    parity = Parity.Odd;
                    break;
                case 2:
                    parity = Parity.Even;
                    break;
                case 3:
                    parity = Parity.Mark;
                    break;
                case 4:
                    parity = Parity.Space;
                    break;
                default:
                    parity = Parity.None;
                    break;
            }
            return parity;
        }

        public int GetCom_SerialPortSet_DataBits()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return Convert.ToInt32(doc.GetElementsByTagName("DataBits").Item(0).InnerText);
        }

        public StopBits GetCom_SerialPortSet_StopBits()
        {
            StopBits stopbits;
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            switch (Convert.ToInt32(doc.GetElementsByTagName("StopBits").Item(0).InnerText))
            {
                case 0:
                    stopbits = StopBits.None;
                    break;
                case 1:
                    stopbits = StopBits.One;
                    break;
                case 2:
                    stopbits = StopBits.Two;
                    break;
                case 3:
                    stopbits = StopBits.OnePointFive;
                    break;
                default:
                    stopbits = StopBits.None;
                    break;
            }
            return stopbits;
        }

        public ComfixtureHandle GetComfixtureHandle()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return (ComfixtureHandle)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(doc.GetElementsByTagName("ComfixtureHandle").Item(0).Attributes.Item(0).Value);
        }

        public int GetComfixtureHandle_TimeOut()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return Convert.ToInt32(doc.GetElementsByTagName("TimeOut").Item(0).InnerText);
        }

        public string GetComfixtureHandle_ShowModeName(int portNum)
        {
            string showmode = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            
            XmlNodeList nodes = doc.GetElementsByTagName("ShowMode").Item(0).ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.InnerText.Split(","[0]).Contains(portNum.ToString()))
                {
                    showmode = node.Name;
                    return showmode;
                }
            }
            return showmode;
        }
    }
}
