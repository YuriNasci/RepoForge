using Amazon.S3;
using Amazon.S3.Model;
using RepoForge.Domain.Interfaces;

namespace RepoForge.Infrastructure.S3.Persistence;

public class S3Repository : IBlobRepository
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Repository(IAmazonS3 s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }

    public async Task UploadAsync(string key, Stream data)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = data
        };
        await _s3Client.PutObjectAsync(request);
    }

    public async Task<Stream?> DownloadAsync(string key)
    {
        var response = await _s3Client.GetObjectAsync(_bucketName, key);
        return response.ResponseStream;
    }

    public async Task DeleteAsync(string key)
    {
        await _s3Client.DeleteObjectAsync(_bucketName, key);
    }
}
