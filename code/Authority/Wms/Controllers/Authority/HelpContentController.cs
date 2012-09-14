using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Web.Script.Serialization;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.Bll.Interfaces;

namespace Authority.Controllers.Authority
{
    public class HelpContentController : Controller
    {
        [Dependency]
        public IHelpContentService HelpContentService { get; set; }

        //
        // GET: /HelpContent/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

    }
}
