using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class HelpContentService : ServiceBase<HelpContent>, IHelpContentService
    {
        [Dependency]
        public IHelpContentRepository HelpContentRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public bool Add(HelpContent helpContent, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var help = new HelpContent();
                if (helpContent != null)
                {
                    try
                    {
                        help.ID = Guid.NewGuid();
                        help.ContentCode = helpContent.ContentCode;
                        help.ContentName = helpContent.ContentName;
                        help.ContentPath = helpContent.ContentPath;
                        help.NodeType = helpContent.NodeType;
                        help.FatherNodeID = helpContent.FatherNodeID != null ? helpContent.FatherNodeID : help.ID;
                        help.ModuleID = helpContent.ModuleID;
                        help.NodeOrder = helpContent.NodeOrder;
                        help.IsActive = helpContent.IsActive;
                        help.UpdateTime = DateTime.Now;

                        HelpContentRepository.Add(help);
                        HelpContentRepository.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        strResult = "原因：" + ex.Message;
                    }
                }
            return result;
        }

        public string WhatType(string nodeType)
        {
            string typeStr = "";
            switch (nodeType)
            {
                case "1":
                    typeStr = "第一节点";
                    break;
                case "2":
                    typeStr = "中间节点";
                    break;
                case "3":
                    typeStr = "末端节点";
                    break;
            }
            return typeStr;
        }
        public object GetDetails(int page, int rows, string QueryString, string Value)
        {
            string ContentName = "";
            string ContentCode = "";
            if (QueryString == "ContentName")
            {
                ContentName = Value;
            }
            else
            {
                ContentCode = Value;
            }
            IQueryable<HelpContent> HelpContentQuery = HelpContentRepository.GetQueryable();
            var HelpContent = HelpContentQuery.Where(c => c.ContentName.Contains(ContentName) && c.ContentCode.Contains(ContentCode))
                .OrderBy(c => c.ContentCode)
                .Select(c => c);
            if (!ContentName.Equals(string.Empty))
            {
                HelpContent = HelpContent.Where(p => p.ContentName == ContentName);
            }
            int total = HelpContent.Count();
            HelpContent = HelpContent.Skip((page - 1) * rows).Take(rows);

            var temp = HelpContent.ToArray().Select(c => new
            {
                ID = c.ID,
                ContentCode = c.ContentCode,
                ContentName = c.ContentName,
                FatherNode = c.ID ==c.FatherNodeID? "":  c.FatherNode.ContentCode + " " + c.FatherNode.ContentName,
                NodeType = WhatType(c.NodeType),
                NodeOrder = c.NodeOrder,
                IsActive = c.IsActive == "1" ? "可用" : "不可用"
            });
            return new { total, rows = temp.ToArray() };
        }

        public bool Save(string ID, string ContentCode, string ContentName, string ContentPath, string FatherNodeID, string ModuleID, int NodeOrder, string IsActive, out string strResult)
        {
            strResult = string.Empty;
            try
            {
                Guid new_ID = new Guid(ID);
                var help = HelpContentRepository.GetQueryable()
                    .FirstOrDefault(i => i.ID == new_ID);
                help.ContentCode = ContentCode;
                help.ContentName = ContentName;
                help.ContentPath = ContentPath;
                help.FatherNodeID = new Guid(FatherNodeID);
                help.ModuleID = new Guid(ModuleID);
                help.NodeOrder = NodeOrder;
                help.IsActive = IsActive;
                HelpContentRepository.SaveChanges();
               
            }
            catch (Exception ex)
            {
                strResult = "原因：" + ex.Message;
            }
            return true;
        }

        public object GetDetails2(int page, int rows, string ContentCode, string ContentName, string NodeType, string FatherNodeID, string ModuleID, string IsActive)
        {
            IQueryable<HelpContent> HelpContentQuery = HelpContentRepository.GetQueryable();
            var HelpContent = HelpContentQuery.Where(c => c.ContentCode.Contains(ContentCode) &&
                                                          c.ContentName.Contains(ContentName) &&
                                                          c.IsActive.Contains(IsActive) &&
                                                          c.NodeType.Contains(NodeType));
            if (!FatherNodeID.Equals(string.Empty) && FatherNodeID != null)
            {
                Guid Father_NodeID = new Guid(FatherNodeID);
                HelpContent = HelpContent.Where(h => h.FatherNodeID == Father_NodeID);
            }
            if (!ModuleID.Equals(string.Empty) && ModuleID != null)
            {
                Guid Module_ID = new Guid(ModuleID);
                HelpContent = HelpContent.Where(h => h.ModuleID == Module_ID);
            }
            HelpContent = HelpContent.OrderBy(h => h.ContentCode);
            int total = HelpContent.Count();
            HelpContent = HelpContent.Skip((page - 1) * rows).Take(rows);

            var temp = HelpContent.ToArray().Select(c => new
            {
               ID=c.ID,
               ContentCode= c.ContentCode,
               ContentName= c.ContentName,
               ContentPath= c.ContentPath,
               FatherNodeName =c.ID ==c.FatherNodeID? "": c.FatherNode.ContentName,
               ModuleID= c.ModuleID,
               ModuleName = c.Module.ModuleName,
               FatherNodeID= c.FatherNodeID,
               NodeType= WhatType(c.NodeType),
               NodeOrder= c.NodeOrder,
               IsActive = c.IsActive == "1" ? "可用" : "不可用",
               UpdateTime=c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
            return new { total, rows = temp.ToArray() };
        }

        public bool Delete(string ID)
        {
            Guid new_ID = new Guid(ID);
            var help = HelpContentRepository.GetQueryable()
                .FirstOrDefault(i => i.ID == new_ID);
            if (ID != null)
            {
                HelpContentRepository.Delete(help);
                HelpContentRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }
    }
}
