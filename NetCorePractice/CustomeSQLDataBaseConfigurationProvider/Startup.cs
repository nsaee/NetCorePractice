using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NS.CustomeConfigurationProvider.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.CustomeConfigurationProvider
{
    public class Startup
    {
        private readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var baseConfiguration = new CustomeBaseConfigData();
            _configuration.GetSection(nameof(CustomeBaseConfigData)).Bind(baseConfiguration);
           
            services.AddSingleton(baseConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CustomeBaseConfigData baseConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    if (baseConfiguration != null)
                    {
                        await context.Response.WriteAsync($"{nameof(baseConfiguration.UserName)}:{baseConfiguration.UserName}\r\n{nameof(baseConfiguration.Password)}:{baseConfiguration.Password}\r\n{nameof(baseConfiguration.IsOnTest)}:{baseConfiguration.IsOnTest}\r\n");
                    }

                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
