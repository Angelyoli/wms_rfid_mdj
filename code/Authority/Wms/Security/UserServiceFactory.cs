using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Authority.Bll.Interfaces;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.ServiceLocation;
using System.Web.Routing;
using THOK.Authority.Bll.Service;

namespace THOK.Security
{
    public class UserServiceFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;
        public IUserService userService;
        public ILoginLogService LoginLogService;
        public ISystemEventLogService SystemEventLogService;
        public IExceptionalLogService ExceptionalLogService;
        public UserServiceFactory()
        {
            _container = new UnityContainer();          
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_container, "defaultContainer");
            userService = _container.Resolve<IUserService>() as UserService;
            LoginLogService = _container.Resolve<ILoginLogService>() as LoginLogService;
            SystemEventLogService = _container.Resolve<ISystemEventLogService>() as SystemEventLogService;
            ExceptionalLogService = _container.Resolve<IExceptionalLogService>() as ExceptionalLogService;
            ServiceLocatorProvider sp = new ServiceLocatorProvider(GetServiceLocator);
            ServiceLocator.SetLocatorProvider(sp);
        }
        public IServiceLocator GetServiceLocator()
        {
            return new UnityServiceLocator(_container);
        }
    }
}