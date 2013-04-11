using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wms.Security
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class SystemEventLogAttribute : AuthorizeAttribute
    {
        int i = 0;
        SystemEventLogFactory   EventLogFactory = new SystemEventLogFactory();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string eventName = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string eventDescription = httpContext.Request.RawUrl;
            if (i != 0)
            {
                var s = string.IsNullOrEmpty(httpContext.Session["username"].ToString());
                string operateUser = string.IsNullOrEmpty(httpContext.Session["username"].ToString()) ? "" : httpContext.Session["username"].ToString();
                string targetSystem = string.IsNullOrEmpty(httpContext.Session["targetSystem"].ToString()) ? "" : httpContext.Session["targetSystem"].ToString();
                if (operateUser != "" && operateUser != null)
                {
                    EventLogFactory.SystemEventLogService.CreateEventLog(eventName, eventDescription, operateUser, targetSystem);
                }
            }
            i += 1;
            return true;
        }
    }
}