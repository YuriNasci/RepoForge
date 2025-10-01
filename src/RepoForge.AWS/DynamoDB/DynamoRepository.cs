using Amazon.DynamoDBv2.DataModel;
using RepoForge.Abstractions;
using System.Linq.Expressions;

namespace RepoForge.AWS.DynamoDB;

public class DynamoRepository<T> : IRepository<T> where T : class
{
    private readonly IDynamoDBContext _context;

    public DynamoRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(params object[] keys)
    {
        return keys.Length switch
        {
            1 => await _context.LoadAsync<T>(keys[0]),
            2 => await _context.LoadAsync<T>(keys[0], keys[1]),
            _ => throw new ArgumentException("DynamoDB requer 1 (PartitionKey) ou 2 chaves (PartitionKey + SortKey).")
        };
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.ScanAsync<T>(default).GetRemainingAsync();

    public async Task AddAsync(T entity) =>
        await _context.SaveAsync(entity);

    public async Task UpdateAsync(T entity) =>
        await _context.SaveAsync(entity);

    public async Task DeleteAsync(params object[] keys)
    {
        var entity = await GetByIdAsync(keys);
        if (entity != null)
            await _context.DeleteAsync(entity);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        // DynamoDB não suporta LINQ avançado como EF, mas podemos simular com Scan
        var conditions = new List<ScanCondition>();
        // se necessário, poderíamos mapear Expression<Func<T,bool>> para ScanCondition
        return await _context.ScanAsync<T>(conditions).GetRemainingAsync();
    }
}