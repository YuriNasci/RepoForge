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
        public static IServiceCollection AddCsvDataAdapter(this IServiceCollection services)
        {
            services.AddScoped<ICsvDataAdapter, CsvDataAdapter>();
            return services;
        }
    }
}
