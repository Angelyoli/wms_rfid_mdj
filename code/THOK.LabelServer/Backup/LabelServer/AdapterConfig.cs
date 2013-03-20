using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace THOK.Application.LabelServer
{
    class AdapterConfig
    {
        private string _XMLFilePath = @".\AdapterConfig.xml";

        public string XMLFilePath 
        {
            get { return _XMLFilePath; }
            set { _XMLFilePath = value; }
        }
        public AdapterConfig()
        {
        }
        public AdapterConfig(string path)
        {
            _XMLFilePath = path;
        }
        public string GetComfixtureHandle_Type(int portNum)
        {
            string strtype = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(_XMLFilePath);

            XmlNodeList nodes = doc.GetElementsByTagName("ComfixtureHandle_Type").Item(0).ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.InnerText.Split(","[0]).Contains(portNum.ToString()))
                {
                    strtype = node.Name;
                    return strtype;
                }
            }
            return strtype;
        }
    }
}
