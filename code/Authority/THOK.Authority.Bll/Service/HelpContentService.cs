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

        #region IHelpContentService 成员

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

        #endregion

        #region IHelpContentService 成员

        #endregion
        #region IHelpContentService 成员


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
                ContentCode = c.ContentCode,
                ContentName = c.ContentName,
                FatherNode = c.FatherNode.ContentName,
                FatherNodeID = c.FatherNodeID,
                NodeType = c.NodeType,
                NodeOrder = c.NodeOrder,
                IsActive = c.IsActive == "1" ? "可用" : "不可用"
            });
            return new { total, rows = temp.ToArray() };
        }

        #endregion

        #region IHelpContentService 成员


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
                IsActive = help.IsActive == "1" ? "可用" : "不可用";
                HelpContentRepository.SaveChanges();
               
            }
            catch (Exception ex)
            {
                strResult = "原因：" + ex.Message;
            }
            return true;
        }

        #endregion

        #region IHelpContentService 成员


        public object GetDetails2(int page, int rows, string ContentCode, string ContentName, string ModuleName, string NodeType, string FatherNodeID, string NodeOrder, string ModuleID, string IsActive, string UpdateTime)
        {
            IQueryable<HelpContent> HelpContentQuery = HelpContentRepository.GetQueryable();
            var HelpContent = HelpContentQuery.Where(c =>
                c.ContentName.Contains(ContentName)
                && c.Module.ModuleName.Contains(ModuleName))
                .OrderBy(c => c.ContentCode)
                .Select(c => c);
            if (!ContentCode.Equals(string.Empty))
            {
                HelpContent = HelpContent.Where(p => p.ContentCode == ContentCode);
            }
            if (!ContentName.Equals(string.Empty))
            {
                HelpContent = HelpContent.Where(p => p.ContentName == ContentName);
            }
            int total = HelpContent.Count();
            HelpContent = HelpContent.Skip((page - 1) * rows).Take(rows);

            var temp = HelpContent.ToArray().Select(c => new
            {
               ID=c.ID,
               ContentCode= c.ContentCode,
               ContentName= c.ContentName,
               ContentPath= c.ContentPath,
               FatherNode= c.FatherNode.ContentName,
               ModuleID= c.ModuleID,
               ModuleName = c.Module.ModuleName,
               FatherNodeID= c.FatherNodeID,
               NodeType= c.NodeType,
               NodeOrder= c.NodeOrder,
               IsActive = c.IsActive == "1" ? "可用" : "不可用",
               UpdateTime=c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
            return new { total, rows = temp.ToArray() };
        }

        #endregion



        #region IHelpContentService 成员


        public bool Delete(string ContentCode)
        {
            var help = HelpContentRepository.GetQueryable()
                .FirstOrDefault(b => b.ContentCode == ContentCode);
            if (ContentCode != null)
            {
                HelpContentRepository.Delete(help);
                HelpContentRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        #endregion
    }
}
