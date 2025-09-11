using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using RepoForge.Domain.Interfaces;
using RepoForge.Infrastructure.DynamoDb.Persistence;

namespace RepoForge.Infrastructure.DynamoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddDynamoRepository(this IServiceCollection services)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IDynamoDBContext, DynamoDBContext>();

        services.AddScoped(typeof(IRepository<>), typeof(DynamoRepository<>));

        return services;
    }
}
