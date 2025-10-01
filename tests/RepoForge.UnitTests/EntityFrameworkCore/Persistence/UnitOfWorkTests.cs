using Microsoft.EntityFrameworkCore;
using RepoForge.EntityFrameworkCore.Persistence;
using System.Threading.Tasks;

namespace RepoForge.UnitTests.EntityFrameworkCore.Persistence
{
    public class UnitOfWorkTests
    {
        public class TestModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        public class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
            public DbSet<TestModel> TestModels { get; set; }
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldSaveChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            var context = new TestDbContext(options);
            var unitOfWork = new UnitOfWork(context);
            var entity = new TestModel { Id = 1, Name = "Test" };
            await context.TestModels.AddAsync(entity);

            // Act
            var result = await unitOfWork.SaveChangesAsync();

            // Assert
            Assert.Equal(1, result);
            var savedEntity = await context.TestModels.FindAsync(1);
            Assert.NotNull(savedEntity);
        }
    }
}
