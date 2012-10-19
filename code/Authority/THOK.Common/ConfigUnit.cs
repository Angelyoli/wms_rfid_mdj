using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#region
using System.Xml;
#endregion

namespace THOK.Common
{
    public class ConfigUnit
    {
        static XmlDocument doc = new XmlDocument();        
        static string fileName;

        public ConfigUnit()
        {
            try
            {
                doc.Load("AFConfig.xml");
                fileName = "AFConfig.xml";
            }
            catch(Exception)
            {
                //MessageBox.Show("找不到配置文件!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static Dictionary<string, string> GetConfig(string nodeName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            XmlNode node = doc.GetElementsByTagName(nodeName)[0];

            foreach (XmlNode var in node.ChildNodes)
            {
                parameters.Add(var.Attributes["name"].InnerText, var.Attributes["value"].InnerText);
            }
            return parameters;
        }
        public static void SaveConfig(string nodeName, Dictionary<string, string> prameters)
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
