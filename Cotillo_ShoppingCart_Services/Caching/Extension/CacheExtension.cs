using Cotillo_ShoppingCart_Services.Caching.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Caching.Extension
{
    /// <summary>
    /// Cache extensions that will be used in any ICacheManager implementation
    /// </summary>    
    public static class CacheExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type to be returned</typeparam>
        /// <param name="cacheManager">Extension of the interface</param>
        /// <param name="key">Unique key to be available to properly get or set the cached object</param>
        /// <param name="acquire">This is the lambda expression (delegate) that is passed in order to execute it if the cached item does not exist</param>
        /// <param name="getAsReferenceType">Enable this flag to instruct the cache manager that the object to be cached is a reference type so that when retrieving the value from the cache, the manager knows how to convert it</param>
        /// <param name="jsonSerialize">Enable this flag if you want to store your object as a JSON object, enable this when you are working with reference types</param>
        /// <returns>Returns the cached object</returns>
        public static T Get<T>(
            this ICacheManager cacheManager, 
            string key, 
            Func<T> acquire, 
            bool getAsReferenceType = true, 
            bool jsonSerialize = true)
        {
            return Get(cacheManager, key, cacheTime: 60, acquire: acquire, getAsReferenceType: getAsReferenceType, jsonSerialize: jsonSerialize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type to be returned</typeparam>
        /// <param name="cacheManager">Extension of the interface</param>
        /// <param name="key">Unique key to be available to properly get or set the cached object</param>
        ///  /// <param name="cacheTime">Lifetime of the cached item, in minutes</param>
        /// <param name="acquire">This is the lambda expression (delegate) that is passed in order to execute it if the cached item does not exist</param>
        /// <param name="getAsReferenceType">Enable this flag to instruct the cache manager that the object to be cached is a reference type so that when retrieving the value from the cache, the manager knows how to convert it</param>
        /// <param name="jsonSerialize">Enable this flag if you want to store your object as a JSON object, enable this when you are working with reference types</param>
        /// <returns>Returns the cached object</returns>
        public static T Get<T>(
            this ICacheManager cacheManager, 
            string key, 
            int cacheTime, 
            Func<T> acquire, 
            bool getAsReferenceType, 
            bool jsonSerialize)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key, getAsReferenceType);
            }
            else
            {
                var result = acquire();
                if (result != null)
                {
                    cacheManager.Set(key, result, cacheTime, jsonSerialize);
                    return result;
                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
