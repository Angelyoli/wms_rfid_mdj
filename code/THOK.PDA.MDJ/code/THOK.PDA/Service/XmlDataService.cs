using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using THOK.PDA.Util;

namespace THOK.PDA.Service
{
    public class XmlDataService
    {
        XmlDocument doc = new XmlDocument();
        string uploadFile = "uploadFile.xml";
        string importFile = "exportBill.xml";

        public XmlDataService()
        {

        }
        public void SaveBill(string billId, string detailId)
        {
            XmlElement root = null;
            if (File.Exists(uploadFile))
            {
                doc.Load(uploadFile);
                root = doc.DocumentElement;
            }
            else
            {
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "GB2312", ""));
                root = doc.CreateElement("Bill");
                doc.AppendChild(root);
            }
            XmlElement node = doc.CreateElement("billInfo");
            node.SetAttribute("masteId", billId);
            node.SetAttribute("detailId", detailId);
            root.AppendChild(node);
            doc.Save(uploadFile);
        }
        public void ReadBill()
        {
            if (!File.Exists(importFile))
            {
                SystemCache.DetailTable = null;
                SystemCache.MasterTable = null;
                return;
            }
            DataTable detailTable = null;
            DataTable masterTable = null;

            doc.Load(importFile);
            XmlElement root = doc.DocumentElement;

            if (root.ChildNodes.Count == 0)
            {
                SystemCache.DetailTable = null;
                SystemCache.MasterTable = null;
                return;
            }
            foreach (XmlElement node in root.ChildNodes)
            {
                //为DataTable创建表结构
                if (masterTable == null)
                {
                    masterTable = new DataTable();
                    foreach (XmlAttribute masterAttribute in node.Attributes)
                    {
                        masterTable.Columns.Add(masterAttribute.Name);
                    }
                }
                if (detailTable == null)
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        detailTable = new DataTable();
                        foreach (XmlAttribute detailAttribute in node.ChildNodes[0].Attributes)
                        {
                            detailTable.Columns.Add(detailAttribute.Name);
                        }
                    }
                }
                //为DataTable添加数据 添加主单据数据
                DataRow masterRow = masterTable.NewRow();
                foreach (XmlAttribute masterAttribute in node.Attributes)
                {
                    masterRow[masterAttribute.Name] = masterAttribute.Value;
                }
                masterTable.Rows.Add(masterRow);
                //添加明细单据数据
                foreach (XmlElement detailNode in node.ChildNodes)
                {
                    DataRow detailRow = detailTable.NewRow();
                    foreach (XmlAttribute detailAttribute in detailNode.Attributes)
                    {
                        detailRow[detailAttribute.Name] = detailAttribute.Value;
                    }
                    detailTable.Rows.Add(detailRow);
                }
            }
            SystemCache.MasterTable = masterTable;
            SystemCache.DetailTable = detailTable;
        }
        public void UpdateBill(string billId, string detailId, string piece, string item)
        {
            doc.Load(importFile);
            XmlNode node = doc.SelectNodes(@"/Bill/billInfo/detailInfo[@Master='" + billId + "'][@DetailID='" + detailId + "']").Item(0);
            node.Attributes["ConfirmState"].Value = "3";

            node.Attributes["StateName"].Value = "已执行";
            node.Attributes["OperatePiece"].Value = piece;
            node.Attributes["OperateItem"].Value = item;
            doc.Save(importFile);
            SystemCache.DetailTable.Select("Master='" + billId + "' AND DetailID='" + detailId + "'")[0]["ConfirmState"] = "3";
        }
    }
}
