using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace THOK.Security
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false, Inherited = true)]
    public class TokenAclAuthorizeAttribute :AuthorizeAttribute
    {
        UserServiceFactory UserFactory = new UserServiceFactory();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = true;
            if (httpContext.Request.Cookies["username"] != null && httpContext.Request.Cookies["ipAdress"]!=null)
            {
                string user = httpContext.Request.Cookies["username"].Value;
                string ipAdress = UserFactory.userService.GetUserIp(user);
                string localip = httpContext.Request.Cookies["ipAdress"].Value;
                if (ipAdress != localip)
                {
                    result = false;
                }
            }
            return result;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                FormsAuthentication.SignOut();
                throw new UnauthorizedAccessException("该账户在别的地方已登录，您可以尝试重新登陆或退出！");
            }
        }
    }
}
