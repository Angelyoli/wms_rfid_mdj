using System;
using System.Web;
using System.Web.Mvc;

namespace THOK.Security
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false, Inherited = true)]
    public class TokenAclAuthorizeAttribute :AuthorizeAttribute
    {
        UserServiceFactory UserFactory = new UserServiceFactory();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = true;
            if (httpContext.Request.Cookies.Keys.Count>1)
            {
                string user = httpContext.Request.Cookies["username"].Value;
                string ipAdress = UserFactory.userService.GetUserIp(user);
                string localip = UserFactory.userService.GetLocalIp(user);
                if (ipAdress != localip)
                {
                    result = false;
                }
            }
            if (!result)
            {
                httpContext.Response.StatusCode = 403;
            }
            return result;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                throw new  UnauthorizedAccessException("该账户在别的地方已登录，您可以尝试重新登陆或退出！");
            }
        }
    }
}
