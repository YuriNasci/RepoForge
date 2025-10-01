using Moq;
using RepoForge.Abstractions;
using RepoForge.DataAdapters.Json;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepoForge.UnitTests.DataAdapters.Json
{
    public class JsonDataAdapterTests
    {
        private readonly Mock<IBlobRepository> _blobRepositoryMock;
        private readonly JsonDataAdapter _jsonDataAdapter;

        public JsonDataAdapterTests()
        {
            _blobRepositoryMock = new Mock<IBlobRepository>();
            _jsonDataAdapter = new JsonDataAdapter(_blobRepositoryMock.Object);
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [Fact]
        public async Task UploadJsonAsync_ShouldCallBlobRepositoryUpload()
        {
            // Arrange
            var key = "test.json";
            var data = new TestModel { Id = 1, Name = "Test" };

            // Act
            await _jsonDataAdapter.UploadJsonAsync(key, data);

            // Assert
            _blobRepositoryMock.Verify(repo => repo.UploadAsync(key, It.IsAny<MemoryStream>()), Times.Once);
        }

        [Fact]
        public async Task DownloadJsonAsync_ShouldReturnData_WhenBlobExists()
        {
            // Arrange
            var key = "test.json";
            var data = new TestModel { Id = 1, Name = "Test" };
            var jsonContent = JsonSerializer.Serialize(data);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
            _blobRepositoryMock.Setup(repo => repo.DownloadAsync(key)).ReturnsAsync(stream);

            // Act
            var result = await _jsonDataAdapter.DownloadJsonAsync<TestModel>(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task DownloadJsonAsync_ShouldReturnNull_WhenBlobDoesNotExist()
        {
            // Arrange
            var key = "nonexistent.json";
            _blobRepositoryMock.Setup(repo => repo.DownloadAsync(key)).ReturnsAsync((Stream?)null);

            // Act
            var result = await _jsonDataAdapter.DownloadJsonAsync<TestModel>(key);

            // Assert
            Assert.Null(result);
        }
    }
}
