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
            //var parent = HelpContentRepository.GetQueryable().FirstOrDefault(p => p.ID == helpContent.ID);

            //var helpExist = HelpContentRepository.GetQueryable().FirstOrDefault(h => h.ContentCode == helpContent.ContentCode);
            var helpExist = 1;
            if (helpExist == 1)
            {
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
                else
                {
                    strResult = "原因：找不到当前登陆用户！请重新登陆！";
                }
            }
            else
            {
                strResult = "原因：该编号已存在！";
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
            string ModuleName = "";
            if (QueryString == "ContentName")
            {
                ContentName = Value;
            }
            else
            {
                ModuleName = Value;
            }
            IQueryable<HelpContent> HelpContentQuery = HelpContentRepository.GetQueryable();
            var HelpContent = HelpContentQuery.Where(c => c.Module.ModuleName.Contains(ModuleName))
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


        public bool Save(string ID, string ContentCode, string ContentName, string ContentPath, string FatherNodeID, string ModuleID, int NodeOrder, string IsActive)
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
               ContentCode2= c.ContentCode,
               ContentName2= c.ContentName,
               ContentPath2= c.ContentPath,
               FatherNode2= c.FatherNode.ContentName,
               ModuleID2= c.ModuleID,
               ModuleName2 = c.Module.ModuleName,
               FatherNodeID2= c.FatherNodeID,
               NodeType2= c.NodeType,
               NodeOrder2= c.NodeOrder,
               IsActive2 = c.IsActive == "1" ? "可用" : "不可用",
               UpdateTime2=c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
            return new { total, rows = temp.ToArray() };
        }

        #endregion

       
    }
}
