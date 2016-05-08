using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cotillo_ShoppingCart_Services.IoCContainer
{
    /// <summary>
    /// Class that serves as a wrapper so that the projects referencing the ServiceContainer
    /// do not need to add a using to, in this case, to SimpleInjector
    /// </summary>
    public class IoCServiceContainer
    {
        private static Container _container = new Container();
        
        public void RegisterMvcControllers(Assembly ExecutingAssembly)
        {
            _container.RegisterMvcControllers(ExecutingAssembly);
        }

        public SimpleInjectorDependencyResolver GetInjectorDependencyResolver()
        {
            return new SimpleInjectorDependencyResolver(_container);
        }

        public void RegisterWebApiControllers(HttpConfiguration configuration)
        {
            _container.RegisterWebApiControllers(configuration);
        }

        public SimpleInjectorWebApiDependencyResolver GetInjectorWebApiDependencyResolver()
        {
            return new SimpleInjectorWebApiDependencyResolver(_container);
        }

        public ContainerOptions Options
        {
            get { return _container.Options; }
        }

        public void UseWebApiRequestLifestyle()
        {
            _container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
        }

        public void UseWebRequestLifestyle()
        {
            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
        }

        public Scope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }

        public Scope BeginExecutionContextScope()
        {
            return _container.BeginExecutionContextScope();
        }

        public TService Resolve<TService>()
            where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public object GetInstance(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        public TService GetInstance<TService>()
            where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
            where TService : class
        {
            return _container.GetAllInstances<TService>();
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return _container.GetAllInstances(type);
        }

        public void RegisterPerWebRequest<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.RegisterPerWebRequest<TService, TImplementation>();
        }

        public void RegisterPerWebRequest<TService>(Func<TService> instanceCreator)
            where TService : class
        {
            _container.RegisterPerWebRequest<TService>(instanceCreator);
        }

        public void RegisterPerWebRequest<TService>(Func<TService> instanceCreator, bool disposeInstanceWhenWebRequestEnds)
            where TService : class
        {
            _container.RegisterPerWebRequest<TService>(instanceCreator, disposeInstanceWhenWebRequestEnds);
        }

        public void RegisterWebApiRequest<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.RegisterWebApiRequest<TService, TImplementation>();
        }

        public void RegisterWebApiRequest<TService>(Func<TService> instanceCreator)
            where TService : class
        {
            _container.RegisterPerWebRequest<TService>(instanceCreator);
        }

        public void RegisterWebApiRequest<TService>(Func<TService> instanceCreator, bool disposeInstanceWhenWebRequestEnds)
            where TService : class
        {
            _container.RegisterWebApiRequest<TService>(instanceCreator, disposeInstanceWhenWebRequestEnds);
        }

        public void RegisterSingle<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.RegisterSingleton<TService, TImplementation>();
        }

        public void RegisterSingle<TService>(Func<TService> instanceCreator)
            where TService : class
        {
            _container.RegisterSingleton<TService>(instanceCreator);
        }

        public void RegisterOpenGeneric(Type openGenericServiceType, Type openGenericImplementation)
        {
            _container.Register(openGenericServiceType, openGenericImplementation);
        }

        public void RegisterOpenGeneric(Type openGenericServiceType, Type openGenericImplementation, Lifestyle lifestyle)
        {
            _container.Register(openGenericServiceType, openGenericImplementation, lifestyle);
        }

        public void RegisterCollection(Type openGenericServiceType, IEnumerable<Type> allOpenGenericImplementation)
        {
            _container.RegisterCollection(openGenericServiceType, allOpenGenericImplementation);
        }

        public void RegisterLifetimeScope<TService>(Func<TService> instanceCreator, bool disposeWhenLifetimeScopeEnds)
            where TService : class
        {
            _container.RegisterLifetimeScope<TService>(instanceCreator, disposeWhenLifetimeScopeEnds);
        }

        public void RegisterLifetimeScope<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.RegisterLifetimeScope<TService, TImplementation>();
        }

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>();
        }

        public void Register<TService, TImplementation>(Lifestyle lifestyle)
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>(lifestyle);
        }

        /*
         * ADAPT - If AOP wants to be included then uncomment this method, notice that IInterceptor
                     is a custom class, it can be found under Toolkits (Simple Injector AOP)
        public void InterceptWith<TInterceptor>(Func<Type, bool> predicate)
            where TInterceptor : class, IInterceptor
        {
            container.InterceptWith<TInterceptor>(predicate);
        }
        */

        /* 
         * ADAPT - Use this method if you would like to use Log4Net with Simple Injector, an example
                     is found under Toolkits (Simple Injector and Log4Net)
        public void RegisterWithContext<TService>(Func<DependencyContext, TService> contextBasedFactory)
            where TService : class
        {
            container.RegisterWithContext<TService>(contextBasedFactory);
        }
        */

        public void Register<TService>(Func<TService> instanceCreator)
            where TService : class
        {
            _container.Register<TService>(instanceCreator);
        }

        public void Register<TService>(Func<TService> instanceCreator, Lifestyle lifestyle)
            where TService : class
        {
            _container.Register<TService>(instanceCreator, lifestyle);
        }

        public void Verify()
        {
            _container.Verify();
        }

        public bool IsVerifying()
        {
            return _container.IsVerifying;
        }
    }
}
