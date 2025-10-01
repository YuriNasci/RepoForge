using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using RepoForge.AWS.S3;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RepoForge.UnitTests.AWS.S3
{
    public class S3RepositoryTests
    {
        private readonly Mock<IAmazonS3> _s3ClientMock;
        private readonly S3Repository _s3Repository;
        private const string BucketName = "test-bucket";

        public S3RepositoryTests()
        {
            _s3ClientMock = new Mock<IAmazonS3>();
            _s3Repository = new S3Repository(_s3ClientMock.Object, BucketName);
        }

        [Fact]
        public async Task UploadAsync_ShouldCallPutObjectAsync()
        {
            // Arrange
            var key = "test-key";
            var stream = new MemoryStream();

            // Act
            await _s3Repository.UploadAsync(key, stream);

            // Assert
            _s3ClientMock.Verify(c => c.PutObjectAsync(It.Is<PutObjectRequest>(req =>
                req.BucketName == BucketName &&
                req.Key == key &&
                req.InputStream == stream), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DownloadAsync_ShouldReturnStream_WhenObjectExists()
        {
            // Arrange
            var key = "test-key";
            var expectedStream = new MemoryStream();
            var response = new GetObjectResponse { ResponseStream = expectedStream };
            _s3ClientMock.Setup(c => c.GetObjectAsync(BucketName, key, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            // Act
            var result = await _s3Repository.DownloadAsync(key);

            // Assert
            Assert.Equal(expectedStream, result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteObjectAsync()
        {
            // Arrange
            var key = "test-key";

            // Act
            await _s3Repository.DeleteAsync(key);

            // Assert
            _s3ClientMock.Verify(c => c.DeleteObjectAsync(BucketName, key, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
