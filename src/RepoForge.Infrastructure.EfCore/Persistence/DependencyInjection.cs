using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoForge.Abstractions;
using RepoForge.EntityFrameworkCore.Persistence;

namespace RepoForge.EntityFrameworkCore;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresRepository<TDbContext>(
        this IServiceCollection services,
        string connectionString)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}