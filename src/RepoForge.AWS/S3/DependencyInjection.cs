using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using RepoForge.Abstractions;

namespace RepoForge.AWS.S3;

public static class DependencyInjection
{
    /// <summary>
    /// Registers an S3-backed <see cref="IBlobRepository"/> implementation.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="bucketName">The S3 bucket name used for blob operations.</param>
    /// <returns>The same service collection for chaining.</returns>
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