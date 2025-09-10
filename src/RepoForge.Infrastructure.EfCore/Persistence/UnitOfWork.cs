using Microsoft.EntityFrameworkCore;
using RepoForge.Domain.Interfaces;

namespace RepoForge.Infrastructure.EfCore.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _context.Dispose();
}
