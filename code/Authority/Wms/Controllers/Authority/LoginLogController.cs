using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;

namespace Authority.Controllers.Authority
{
    public class LoginLogController : Controller
    {
        //
        // GET: /LoginLog/
        [Dependency]
        public ISystemEventLogService SystemEventLogService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /LoginLog/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LoginLog/Create

        public ActionResult Create(string login_time, string logout_time, string user_name, string system_ID)
        {
            login_time = DateTime.Now.ToString();
            logout_time = "";
            bool bResult =SystemEventLogService.CreateLoginLog(login_time,logout_time,user_name,Guid.Parse(system_ID));
            return Json(bResult, JsonRequestBehavior.AllowGet);
        } 

        //
        // POST: /LoginLog/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /LoginLog/Edit/5

        public ActionResult Edit(string user_name, string logout_time)
        {
            logout_time = DateTime.Now.ToString();
            bool bResult = SystemEventLogService.UpdateLoginLog(user_name, logout_time);
            return Json(bResult,JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /LoginLog/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /LoginLog/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /LoginLog/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
