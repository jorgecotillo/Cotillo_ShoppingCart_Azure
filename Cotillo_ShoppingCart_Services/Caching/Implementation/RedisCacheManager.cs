using Cotillo_ShoppingCart_Services.Caching.Interface;
using CSRedis;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Cotillo_ShoppingCart_Services.Caching.Implementation
{
    /// <summary>
    /// Use this cache manager if you are on a distributed cache, use Amazon ElasticCache with Redis engine
    /// </summary>
    public class RedisCacheManager : ICacheManager
    {
        readonly string _redisServerURI;
        readonly int _port;
        readonly bool _ssl;
        readonly string _password;

        /// <summary>
        /// Uses default port (6379) and no ssl
        /// </summary>
        /// <param name="redisServerURI"></param>
        public RedisCacheManager(string redisServerURI)
            :this(redisServerURI, port: 6379, ssl: false, password: string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisServerURI"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        public RedisCacheManager(string redisServerURI, int port)
            : this(redisServerURI, port, ssl: false, password: string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisServerURI"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        public RedisCacheManager(string redisServerURI, int port, bool ssl)
            : this(redisServerURI, port, ssl, string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisServerURI"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        public RedisCacheManager(string redisServerURI, int port, bool ssl, string password)
        {
            _redisServerURI = redisServerURI;
            _port = port;
            _ssl = ssl;
            _password = password;
        }

        private RedisClient GetRedisClient()
        {
            RedisClient client = new RedisClient(_redisServerURI, _port, _ssl);

            if (!String.IsNullOrEmpty(_password))
                client.Auth(_password);

            return client;
        }

        public T Get<T>(string key, bool getAsReferenceType)
        {
            if (getAsReferenceType)
                return GetReferenceType<T>(key);
            else
                return GetValueType<T>(key);
        }

        private T GetReferenceType<T>(string key)
        {
            using (RedisClient redisClient = GetRedisClient())
            {
                var serializedEntity = redisClient.Get(key);
                if (serializedEntity != null)
                {
                    return JsonConvert.DeserializeObject<T>(serializedEntity);
                }
                else
                    return default(T);
            }
        }

        private T GetValueType<T>(string key)
        {
            using (RedisClient redisClient = GetRedisClient())
            {
                var serializedEntity = redisClient.Get(key);
                if (serializedEntity != null)
                {
                    if (IsNullableType(typeof(T)))
                    {
                        TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                        return (T)conv.ConvertFrom(serializedEntity);
                    }
                    else
                        return (T)Convert.ChangeType(serializedEntity, typeof(T));
                }
                else
                    return default(T);
            }
        }

        private bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType &&
            theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public void Set(string key, object data, int cacheTime, bool jsonSerialize = true)
        {
            //Do not set any value if the object to be cached is null.
            if (data == null)
                return;

            using (RedisClient redisClient = GetRedisClient())
            {
                if (jsonSerialize)
                    redisClient.Set(key, JsonConvert.SerializeObject(data), expiration: DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(cacheTime));
                else
                    redisClient.Set(key, data, expiration: DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(cacheTime));
            }
        }

        public bool IsSet(string key)
        {
            using (RedisClient redisClient = GetRedisClient())
            {
                return redisClient.Exists(key);
            }
        }

        public void Remove(string key)
        {
            using (RedisClient redisClient = GetRedisClient())
            {
                if (IsSet(key))
                    redisClient.Del(key);
            }
        }

        public void Clear()
        {
            using (RedisClient redisClient = GetRedisClient())
            {
                redisClient.FlushDb();
            }
        }

        /// <summary>
        /// In Redis, use Scan method instead of Keys, Scan uses an incremental iteration making the retrieval of the keys more efficient because Keys command
        /// gets all the matching keys (based on a pattern specified) from the list of all the keys, so it might fetch 100 keys from a list that contains a million keys
        /// </summary>
        /// <param name="pattern">Pattern to match the keys in Redis, by default this method adds a "*" sign at the end of the pattern search criteria in order to do a complete search, otherwise it will do a hard match on the pattern specified</param>
        public void RemoveByPattern(string pattern)
        {
            using (RedisClient redisClient = GetRedisClient())
            {
                long cursor = 0;
                //Redis Scan command is an incremental search, that is why a loop is used to retrieve all the matches, Scan command starts the loop at cursor zero and finishes the loop when Redis sends back cursor zero again
                //More reference: http://redis.io/commands/scan
                do
                {
                    //add * sign at the end of the pattern so it can retrieve all the values that matches the pattern, otherwise it will do a hard match
                    //on the pattern sent
                    if (!pattern.EndsWith("*"))
                        pattern = pattern + "*";

                    var keys = redisClient.Scan(cursor: cursor, pattern: pattern);

                    //assign the cursor value sent back from Redis
                    cursor = keys.Cursor;

                    foreach (var key in keys.Items)
                    {
                        if (IsSet(key))
                            redisClient.Del(key);
                    }
                    //identifies if Redis sent back Cursor = 0, it means is the end of loop//identifies if Redis sent back Cursor = 0, it means is the end of loop
                } while (cursor != 0);
            }
        }
    }
}
