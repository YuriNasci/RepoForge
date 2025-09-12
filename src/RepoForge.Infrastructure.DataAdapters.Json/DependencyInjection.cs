using Microsoft.Extensions.DependencyInjection;
using RepoForge.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.Infrastructure.DataAdapters.Json
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
