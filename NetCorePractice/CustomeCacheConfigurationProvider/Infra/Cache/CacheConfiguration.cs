using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomeCacheConfigurationProvider.Infra.Cache
{
    public static class CacheConfiguration
    {
        public const string CacheConfigurationSection = "CacheConfiguration";
        public const string CacheConfigurationTypeKey = "CacheConfiguration:Type";

        public static void CacheConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            string cacheConfigType = string.Empty;

            var cacheConfig = configuration.GetSection(CacheConfigurationTypeKey);

            if (!cacheConfig.Exists())
                return;

            cacheConfigType = cacheConfig.Value;

            switch (cacheConfigType.ToLower())
            {
                case "distributedsqlservercache":
                    var distributedSqlServerCacheOptions = new SqlServerCacheOptions();
                    configuration.GetSection($"{CacheConfigurationSection}:DistributedSqlServerCache").Bind(distributedSqlServerCacheOptions);

                    services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString = distributedSqlServerCacheOptions.ConnectionString;
                        options.TableName = distributedSqlServerCacheOptions.TableName;
                        options.SchemaName = distributedSqlServerCacheOptions.SchemaName;
                    });
                    services.AddSingleton<ICacheAdapter, DistributedCacheAdapter>();
                    break;

                case "memorycache":
                    services.AddMemoryCache();
                    services.AddSingleton<ICacheAdapter, MemoryCacheAdapter>();
                    break;

                case "distributedmemorycache":
                    services.AddDistributedMemoryCache();
                    services.AddSingleton<ICacheAdapter, DistributedCacheAdapter>();
                    break;

                default:
                    break;
            }
        }
    }
}
