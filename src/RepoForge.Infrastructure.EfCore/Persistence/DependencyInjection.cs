using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoForge.Domain.Interfaces;
using RepoForge.Infrastructure.EfCore.Persistence;

namespace RepoForge.Infrastructure.EfCore;

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