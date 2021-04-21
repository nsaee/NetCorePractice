using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomeCacheConfigurationProvider.Infra.Cache
{
    public interface ICacheAdapter
    {
        TOutput Get<TOutput>(string key);
        void Set<TIntput>(string key, TIntput value);
    }

   
    public class DistributedCacheAdapter : ICacheAdapter
    {
        private readonly IDistributedCache _disributedCache;

        public DistributedCacheAdapter(IDistributedCache disributedCache)
        {
            _disributedCache = disributedCache;
        }

        public TOutput Get<TOutput>(string key)
        {
            var serializedObject = _disributedCache.GetString(key);

            return !string.IsNullOrEmpty(serializedObject)
                ? JsonConvert.DeserializeObject<TOutput>(serializedObject)
                : default;
        }

        public void Set<TIntput>(string key, TIntput value)
        {
            _disributedCache.SetString(key, JsonConvert.SerializeObject(value));
        }
    }

    public class MemoryCacheAdapter : ICacheAdapter
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheAdapter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public TOutput Get<TOutput>(string key)
        {
            var cacheValue = _memoryCache.Get<TOutput>(key);

            return cacheValue == null
                ? default
                : JsonConvert.DeserializeObject<TOutput>(cacheValue.ToString());
        }

        public void Set<TIntput>(string key, TIntput value)
        {
            _memoryCache.Set(key, JsonConvert.SerializeObject(value));
        }
    }
}
