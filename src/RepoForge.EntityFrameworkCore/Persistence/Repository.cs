using Microsoft.EntityFrameworkCore;
using RepoForge.Abstractions;
using System.Linq.Expressions;

namespace RepoForge.EntityFrameworkCore.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IRepository{T}"/> backed by <see cref="DbContext"/>.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    /// <summary>
    /// Creates a repository bound to the provided <see cref="DbContext"/>.
    /// </summary>
    /// <param name="context">The EF Core <see cref="DbContext"/>.</param>
    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <inheritdoc />
    public async Task<T?> GetByIdAsync(params object[] keys) =>
        await _dbSet.FindAsync(keys);

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    /// <inheritdoc />
    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    /// <inheritdoc />
    public async Task UpdateAsync(T entity) =>
        Task.Run(() => _dbSet.Update(entity));

    /// <inheritdoc />
    public async Task DeleteAsync(params object[] keys)
    {
        var entity = await _dbSet.FindAsync(keys);
        if (entity != null)
            _dbSet.Remove(entity);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();
}