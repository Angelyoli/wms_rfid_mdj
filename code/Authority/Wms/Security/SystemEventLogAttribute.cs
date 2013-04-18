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
        SystemEventLogFactory EventLogFactory = new SystemEventLogFactory();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string eventName = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string eventDescription = httpContext.Request.RawUrl;
            if (httpContext.Request.Cookies["username"] != null && httpContext.Request.Cookies["systemid"] != null && httpContext.Request.Cookies["ipAdress"]!=null)
            {
                string operateUser = httpContext.Request.Cookies["username"].Value;
                Guid targetSystem = Guid.Parse(httpContext.Request.Cookies["systemid"].Value);
                string idAdress = httpContext.Request.Cookies["ipAdress"].Value;
                if (operateUser != "" && operateUser != null)
                {
                    EventLogFactory.SystemEventLogService.CreateEventLog(eventName, eventDescription, operateUser, targetSystem,idAdress);
                }
            }
            return true;
        }
    }
}