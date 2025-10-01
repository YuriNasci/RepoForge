using Microsoft.EntityFrameworkCore;
using RepoForge.EntityFrameworkCore.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepoForge.UnitTests.EntityFrameworkCore.Persistence
{
    public class RepositoryTests
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

        private readonly TestDbContext _context;
        private readonly Repository<TestModel> _repository;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new TestDbContext(options);
            _repository = new Repository<TestModel>(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var entity = new TestModel { Id = 1, Name = "Test" };

            // Act
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.TestModels.FindAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            _context.TestModels.AddRange(new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test1" },
                new TestModel { Id = 2, Name = "Test2" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}
