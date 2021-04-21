using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NS.CustomeConfigurationProvider.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomeCacheConfigurationProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                {
                    configurationBuilder.Sources.Clear();

                    configurationBuilder.AddJsonFile("appsettings.json", true, true);
                    configurationBuilder.AddEnvironmentVariables();

                    var config = configurationBuilder.Build();
                    var configType = config.GetSection("Configuration:Type");

                    if (configType.Exists() && configType.Value == "EF")
                    {
                        configurationBuilder.AddEF();
                    }
                    else
                    {
                     configurationBuilder.AddJsonFile("appsettingsCache.json", true, true);
                    }

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                        configurationBuilder.AddUserSecrets<Startup>();

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
