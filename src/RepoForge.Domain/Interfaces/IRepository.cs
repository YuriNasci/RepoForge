using System.Linq.Expressions;

namespace RepoForge.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(params object[] keys);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(params object[] keys);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}