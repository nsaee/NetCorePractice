using CustomeCacheConfigurationProvider.Infra;
using CustomeCacheConfigurationProvider.Infra.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomeCacheConfigurationProvider.Middlewares
{
    public class NewsCountMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NewsRepository _newsRepository;
        private readonly ICacheAdapter _cacheAdapter;

        public NewsCountMiddleware(RequestDelegate next, NewsRepository newsRepository, ICacheAdapter cacheAdapter)
        {
            _next = next;
            _newsRepository = newsRepository;
            _cacheAdapter = cacheAdapter;
        }

        public async Task Invoke(HttpContext context)
        {
            var newsCount = _cacheAdapter.Get<string>("NewsCount");
            if (newsCount == null)
            {
                newsCount = _newsRepository.GetNewsCount().ToString();

                _cacheAdapter.Set<string>("NewsCount", newsCount);
            }

            await context.Response.WriteAsync($"News count is {newsCount}\n");
            await _next(context);
        }
    }
}
