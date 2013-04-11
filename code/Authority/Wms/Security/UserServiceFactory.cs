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
        public ISystemEventLogService SystemEventLogService;
        public UserServiceFactory()
        {
            _container = new UnityContainer();          
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_container, "defaultContainer");
            userService = _container.Resolve<IUserService>() as UserService;
            SystemEventLogService = _container.Resolve<ISystemEventLogService>() as SystemEventLogService;
            ServiceLocatorProvider sp = new ServiceLocatorProvider(GetServiceLocator);
            ServiceLocator.SetLocatorProvider(sp);
        }

        public UserServiceFactory(IUnityContainer container)
        {
            _container = container;
            ServiceLocatorProvider sp = new ServiceLocatorProvider(GetServiceLocator);
            ServiceLocator.SetLocatorProvider(sp);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                return _container.Resolve(controllerType) as IController;
            }
            return base.GetControllerInstance(requestContext, controllerType);
        }

        public IServiceLocator GetServiceLocator()
        {
            return new UnityServiceLocator(_container);
        }
    }
}