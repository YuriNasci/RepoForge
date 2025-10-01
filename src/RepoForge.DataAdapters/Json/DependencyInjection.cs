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
        /// <summary>
        /// Registers <see cref="IJsonDataAdapter"/> backed by <see cref="JsonDataAdapter"/>.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection for chaining.</returns>
        public static IServiceCollection AddJsonDataAdapter(this IServiceCollection services)
        {
            services.AddScoped<IJsonDataAdapter, JsonDataAdapter>();
            return services;
        }
    }
}
