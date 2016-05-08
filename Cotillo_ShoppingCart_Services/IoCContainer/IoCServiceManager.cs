using SimpleInjector;
using SimpleInjector.Integration.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.IoCContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class IoCServiceManager
    {
        #region Fields
        private static object _currentLock = new object();
        private static IoCServiceContainer _container = null;
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static IoCServiceManager _manager = new IoCServiceManager();
        #endregion

        private IoCServiceManager()
        {
            lock(_currentLock)
            {
                if (_container == null)
                {
                    _container = new IoCServiceContainer();

                    //We can get the enum by looking at a value in the config file, for now is being hard coded to use Web Api
                    new IoCServiceRegistration()
                        .Register(_container, IoCLifestyleScope.WebApi);
                }
            }
        }

        public static IoCServiceContainer Container
        {
            get
            {
                return _container;
            }
        }
    }
}
