using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;

namespace THOK.PDA.Util
{
    public class ConfigUtil
    {


        private XmlDocument doc = new XmlDocument();

        private string fileName;


        public ConfigUtil()
        {
            try
            {
                fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)+"\\config.xml";
               // fileName = "\\config.xml";
                doc.Load(fileName);
                
            }
            catch
            {
              //  MessageBox.Show("找不到配置文件!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Dictionary<string, string> GetConfig(string nodeName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            XmlNode node = doc.GetElementsByTagName(nodeName)[0];

            foreach (XmlNode var in node.ChildNodes)
            {
                parameters.Add(var.Attributes["name"].InnerText, var.Attributes["value"].InnerText);
            }
            return parameters;
        }


        public void SaveConfig(string nodeName, Dictionary<string, string> prameters)
        {
            doc.Load(fileName);

            XmlNode node = doc.GetElementsByTagName(nodeName)[0];

            foreach (XmlNode var in node.ChildNodes)
            {
                var.Attributes["value"].InnerText = prameters[var.Attributes["name"].InnerText];
            }

            doc.Save(fileName);


        }
    }
}
