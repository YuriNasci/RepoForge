using Microsoft.EntityFrameworkCore;
using RepoForge.Abstractions;

namespace RepoForge.EntityFrameworkCore.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUnitOfWork"/> that delegates to an underlying <see cref="DbContext"/>.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    /// <summary>
    /// Creates a new unit of work bound to the provided <see cref="DbContext"/>.
    /// </summary>
    /// <param name="context">The EF Core <see cref="DbContext"/>.</param>
    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the underlying <see cref="DbContext"/>.
    /// </summary>
    public void Dispose() => _context.Dispose();
}
