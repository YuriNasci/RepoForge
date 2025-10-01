using Microsoft.Extensions.DependencyInjection;
using RepoForge.Abstractions;
using RepoForge.DataAdapters.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.DataAdapters.Csv
{
    internal static class DependencyInjection
    {
        /// <summary>
        /// Registers <see cref="ICsvDataAdapter"/> backed by <see cref="CsvDataAdapter"/>.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection for chaining.</returns>
        public static IServiceCollection AddCsvDataAdapter(this IServiceCollection services)
        {
            services.AddScoped<ICsvDataAdapter, CsvDataAdapter>();
            return services;
        }
    }
}
