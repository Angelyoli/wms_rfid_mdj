using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Web.Script.Serialization;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.DbModel;
using THOK.Authority.Bll.Interfaces;
using System;

namespace Authority.Controllers.Authority
{
    public class HelpEditController : Controller
    {
        //
        // GET: /HelpEdit/
        [Dependency]
        public IHelpContentService HelpContentService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        //帮助tree结构
        // GET: /HelpEdit/GetHelpContent/
        public ActionResult GetHelpContent(string sysId)
        {
            var wareCell = HelpContentService.GetHelpContentTree(sysId);
            return Json(wareCell, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
