using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace THOK.Wms.VehicleMounted.Common
{
    public class ConfigUtil
    {
        static XmlDocument doc = new XmlDocument();
        static string fileName;

        public ConfigUtil()
        {
            try
            {
                doc.Load("AFConfig.xml");
                fileName = "AFConfig.xml";
            }
            catch (Exception)
            {
                //MessageBox.Show("找不到配置文件!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
