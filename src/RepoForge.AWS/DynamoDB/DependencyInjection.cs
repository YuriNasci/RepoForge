using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using RepoForge.Abstractions;

namespace RepoForge.AWS.DynamoDB;

public static class DependencyInjection
{
    /// <summary>
    /// Registers Amazon DynamoDB services and a generic <see cref="IRepository{T}"/> implementation.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <remarks>
    /// This method registers the following services:
    /// <list type="bullet">
    /// <item><see cref="IAmazonDynamoDB"/></item>
    /// <item><see cref="IDynamoDBContext"/></item>
    /// <item><see cref="IRepository{T}"/></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddDynamoRepository(this IServiceCollection services)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IDynamoDBContext, DynamoDBContext>();

        services.AddScoped(typeof(IRepository<>), typeof(DynamoRepository<>));

        return services;
    }
}