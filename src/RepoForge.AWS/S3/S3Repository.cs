using Amazon.S3;
using Amazon.S3.Model;
using RepoForge.Abstractions;

namespace RepoForge.AWS.S3;

/// <summary>
/// Amazon S3 implementation of <see cref="IBlobRepository"/>.
/// </summary>
public class S3Repository : IBlobRepository
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    /// <summary>
    /// Creates a new <see cref="S3Repository"/>.
    /// </summary>
    /// <param name="s3Client">The Amazon S3 client.</param>
    /// <param name="bucketName">The target S3 bucket name.</param>
    public S3Repository(IAmazonS3 s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<Stream?> DownloadAsync(string key)
    {
        var response = await _s3Client.GetObjectAsync(_bucketName, key);
        return response.ResponseStream;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string key)
    {
        await _s3Client.DeleteObjectAsync(_bucketName, key);
    }
}
