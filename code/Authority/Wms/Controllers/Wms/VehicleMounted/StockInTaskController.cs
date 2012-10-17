using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalR.Client;
using THOK.Wms.VehicleMounted;
using THOK.Wms.VehicleMounted.Common;
using THOK.Wms.VehicleMounted.Model;

namespace Wms.Controllers.Wms.VehicleMounted
{
    public class StockInTaskController : Controller
    {
        //
        // GET: /StockInTask/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasApply = true;
            ViewBag.hasCancel = true;
            ViewBag.hasFinish = true;
            ViewBag.hasBatch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public void Load()
        {
           
        }
        public void Search()
        {
            
        }
    }
}
