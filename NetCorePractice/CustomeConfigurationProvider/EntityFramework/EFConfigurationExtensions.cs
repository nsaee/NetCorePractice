using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.CustomeConfigurationProvider.EntityFramework
{
    public static class EFConfigurationExtensions
    {
        public static IConfigurationBuilder AddEF(this IConfigurationBuilder builder, string nameOrConnectionString="")
        {
            var connectionString = nameOrConnectionString;
            var config = builder.Build();

            if (string.IsNullOrEmpty(nameOrConnectionString))
            {
                connectionString = config.GetConnectionString("sqlConnection");
            }
            else if (!nameOrConnectionString.ToLowerInvariant().Contains("server="))
            {
                connectionString = config.GetConnectionString(nameOrConnectionString);
            }

            var configSource = new EFConfigurationSource(opts => opts.UseSqlServer(connectionString));
            builder.Add(configSource);

            return builder;
        }

    }
}
