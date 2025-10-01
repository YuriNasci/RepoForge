using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoForge.Abstractions;
using RepoForge.EntityFrameworkCore.Persistence;

namespace RepoForge.EntityFrameworkCore;

public static class DependencyInjection
{
    /// <summary>
    /// Registers EF Core with Npgsql and wires <see cref="IRepository{T}"/> and <see cref="IUnitOfWork"/> services.
    /// </summary>
    /// <typeparam name="TDbContext">Your application's <see cref="DbContext"/> type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionString">The PostgreSQL connection string.</param>
    /// <returns>The same service collection for chaining.</returns>
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