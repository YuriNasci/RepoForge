using Moq;
using RepoForge.Abstractions;
using RepoForge.DataAdapters.Csv;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.UnitTests.DataAdapters.Csv
{
    public class CsvDataAdapterTests
    {
        private readonly Mock<IBlobRepository> _blobRepositoryMock;
        private readonly CsvDataAdapter _csvDataAdapter;

        public CsvDataAdapterTests()
        {
            _blobRepositoryMock = new Mock<IBlobRepository>();
            _csvDataAdapter = new CsvDataAdapter(_blobRepositoryMock.Object);
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [Fact]
        public async Task UploadCsvAsync_ShouldCallBlobRepositoryUpload()
        {
            // Arrange
            var key = "test.csv";
            var data = new List<TestModel> { new TestModel { Id = 1, Name = "Test" } };

            // Act
            await _csvDataAdapter.UploadCsvAsync(key, data);

            // Assert
            _blobRepositoryMock.Verify(repo => repo.UploadAsync(key, It.IsAny<MemoryStream>()), Times.Once);
        }

        [Fact]
        public async Task DownloadCsvAsync_ShouldReturnData_WhenBlobExists()
        {
            // Arrange
            var key = "test.csv";
            var csvContent = "Id,Name\n1,Test\n";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            _blobRepositoryMock.Setup(repo => repo.DownloadAsync(key)).ReturnsAsync(stream);

            // Act
            var result = await _csvDataAdapter.DownloadCsvAsync<TestModel>(key);

            // Assert
            Assert.NotNull(result);
            var record = result.Single();
            Assert.Equal(1, record.Id);
            Assert.Equal("Test", record.Name);
        }

        [Fact]
        public async Task DownloadCsvAsync_ShouldReturnNull_WhenBlobDoesNotExist()
        {
            // Arrange
            var key = "nonexistent.csv";
            _blobRepositoryMock.Setup(repo => repo.DownloadAsync(key)).ReturnsAsync((Stream?)null);

            // Act
            var result = await _csvDataAdapter.DownloadCsvAsync<TestModel>(key);

            // Assert
            Assert.Null(result);
        }
    }
}
