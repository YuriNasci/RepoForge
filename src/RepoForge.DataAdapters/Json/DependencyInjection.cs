using Microsoft.Extensions.DependencyInjection;
using RepoForge.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.DataAdapters.Json
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddJsonDataAdapter(this IServiceCollection services)
        {
            services.AddScoped<IJsonDataAdapter, JsonDataAdapter>();
            return services;
        }
    }
}
