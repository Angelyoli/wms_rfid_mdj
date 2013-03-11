using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Xml;
using DataRabbit.DBAccessing;

namespace DataRabbit.HashOrm
{
    class DataConfig
    {
        private string _XMLFilePath = @".\DataConfig.xml";

        public string XMLFilePath 
        {
            get { return _XMLFilePath; }
            set { _XMLFilePath = value; }
        }
        public DataConfig()
        {
        }
        public DataConfig(string path)
        {
            _XMLFilePath = path;
        }
        public DataConfiguration CreateConfig()
        {
            DataConfiguration config = new DataConfiguration();
            config.DataBaseType = DataBaseType.SqlServer;
            config.IP = getIP();
            config.User = getUser();
            config.Password =  getPwd();
            config.DataBaseName = getDbName();
            return config;
        }
        private string getIP()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return doc.GetElementsByTagName("IP").Item(0).InnerText;
        }
        private string getUser()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return doc.GetElementsByTagName("User").Item(0).InnerText;
        }
        private string getPwd()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return doc.GetElementsByTagName("Pwd").Item(0).InnerText;
        }
        private string getDbName()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);
            return doc.GetElementsByTagName("DbName").Item(0).InnerText;
        }
    }
}
