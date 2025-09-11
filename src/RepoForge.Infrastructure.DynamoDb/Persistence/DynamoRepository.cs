using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using RepoForge.Domain.Interfaces;
using System.Linq.Expressions;

namespace RepoForge.Infrastructure.DynamoDb.Persistence;

public class DynamoRepository<T> : IRepository<T> where T : class
{
    private readonly IDynamoDBContext _context;

    public DynamoRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.LoadAsync<T>(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.ScanAsync<T>(default).GetRemainingAsync();

    public async Task AddAsync(T entity) =>
        await _context.SaveAsync(entity);

    public async Task UpdateAsync(T entity) =>
        await _context.SaveAsync(entity);

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.LoadAsync<T>(id);
        if (entity != null) await _context.DeleteAsync(entity);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        // DynamoDB não suporta LINQ diretamente, mas podemos simular com ScanAsync
        return await _context.ScanAsync<T>(default).GetRemainingAsync();
    }
}
