using Cotillo_ShoppingCart_Services.IoCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Cotillo_ShoppingCart_Azure
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        public static IoCServiceContainer Container
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            //IoCServiceManager.Container property has the necessary code to initialize a container and register all
            //interfaces
            IoCServiceContainer container = IoCServiceManager.Container;

            GlobalConfiguration
                .Configure(WebApiConfig.Register);
            
            //Register all Web Api controllers
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            //Inject Web Api dependency resolver, this line will replace the default ASP .NET instantiation and will
            //use Simple Injector to resolve the instances
            GlobalConfiguration.Configuration.DependencyResolver = container.GetInjectorWebApiDependencyResolver();

            //Let's verify that the registrations are valid
            container.Verify();

            //Setting the container property so it can be available in every controller
            //if resolving an instance on demand is required
            Container = container;
        }
    }
}
