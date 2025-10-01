using Amazon.DynamoDBv2.DataModel;
using Moq;
using RepoForge.AWS.DynamoDB;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RepoForge.UnitTests.AWS.DynamoDB
{
    public class DynamoRepositoryTests
    {
        private readonly Mock<IDynamoDBContext> _contextMock;
        private readonly DynamoRepository<TestModel> _repository;

        public class TestModel
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }

        public DynamoRepositoryTests()
        {
            _contextMock = new Mock<IDynamoDBContext>();
            _repository = new DynamoRepository<TestModel>(_contextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_WithOneKey_ShouldCallLoadAsyncWithOneKey()
        {
            // Arrange
            var key = "123";
            var expected = new TestModel { Id = key, Name = "Test" };
            _contextMock.Setup(c => c.LoadAsync<TestModel>(key, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

            // Act
            var result = await _repository.GetByIdAsync(key);

            // Assert
            Assert.Equal(expected, result);
            _contextMock.Verify(c => c.LoadAsync<TestModel>(key, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldCallSaveAsync()
        {
            // Arrange
            var entity = new TestModel { Id = "123", Name = "Test" };

            // Act
            await _repository.AddAsync(entity);

            // Assert
            _contextMock.Verify(c => c.SaveAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync()
        {
            // Arrange
            var entity = new TestModel { Id = "123", Name = "Updated Test" };

            // Act
            await _repository.UpdateAsync(entity);

            // Assert
            _contextMock.Verify(c => c.SaveAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenEntityExists_ShouldCallDeleteAsync()
        {
            // Arrange
            var key = "123";
            var entity = new TestModel { Id = key, Name = "Test" };
            _contextMock.Setup(c => c.LoadAsync<TestModel>(key, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

            // Act
            await _repository.DeleteAsync(key);

            // Assert
            _contextMock.Verify(c => c.DeleteAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenEntityDoesNotExist_ShouldNotCallDeleteAsync()
        {
            // Arrange
            var key = "123";
            _contextMock.Setup(c => c.LoadAsync<TestModel>(key, It.IsAny<CancellationToken>())).ReturnsAsync((TestModel?)null);

            // Act
            await _repository.DeleteAsync(key);

            // Assert
            _contextMock.Verify(c => c.DeleteAsync(It.IsAny<TestModel>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
