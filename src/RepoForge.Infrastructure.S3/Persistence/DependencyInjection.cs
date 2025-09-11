using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using RepoForge.Domain.Interfaces;
using RepoForge.Infrastructure.S3.Persistence;

namespace RepoForge.Infrastructure.S3;

public static class DependencyInjection
{
    public static IServiceCollection AddS3Repository(
        this IServiceCollection services,
        string bucketName)
    {
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IBlobRepository>(sp =>
        {
            var s3Client = sp.GetRequiredService<IAmazonS3>();
            return new S3Repository(s3Client, bucketName);
        });

        return services;
    }
}
