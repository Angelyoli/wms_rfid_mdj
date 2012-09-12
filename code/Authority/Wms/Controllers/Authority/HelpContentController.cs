using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Web.Script.Serialization;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.DbModel;
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
        //
        // POST: /HelpContent/Create
        [HttpPost]
        public ActionResult Create(HelpContent helpContent)
        {
            string strResult = string.Empty;
            bool bResult = HelpContentService.Add(helpContent, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
