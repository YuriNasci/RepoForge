# RepoForge.EntityFrameworkCore

EF Core-based repository and unit-of-work implementations for `RepoForge.Abstractions`.

Provides:
- `Repository<T>` implementing `IRepository<T>`
- `UnitOfWork` implementing `IUnitOfWork`
- DI extension: `AddPostgresRepository<TDbContext>(string connectionString)`

## Installation
Reference this package alongside `RepoForge.Abstractions` and your EF Core provider (PostgreSQL included via `Npgsql.EntityFrameworkCore.PostgreSQL`).

## Dependency Injection
Register EF Core and RepoForge persistence with PostgreSQL:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RepoForge.EntityFrameworkCore;
using RepoForge.Abstractions;

// Your DbContext
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    // public DbSet<MyEntity> MyEntities => Set<MyEntity>();
}

var services = new ServiceCollection();
services.AddPostgresRepository<AppDbContext>(
    connectionString: "Host=localhost;Port=5432;Database=app;Username=app;Password=secret");

var provider = services.BuildServiceProvider();
```

The extension does the following:
- Registers `AppDbContext` with `UseNpgsql(connectionString)`
- Registers `IRepository<>` to `Repository<>`
- Registers `IUnitOfWork` to `UnitOfWork`

## Usage

```csharp
var repo = provider.GetRequiredService<IRepository<MyEntity>>();
var uow  = provider.GetRequiredService<IUnitOfWork>();

await repo.AddAsync(new MyEntity { /* ... */ });
await uow.SaveChangesAsync();

var all = await repo.GetAllAsync();
var found = await repo.FindAsync(e => e.IsActive);
var one = await repo.GetByIdAsync(id);
await repo.DeleteAsync(id);
await uow.SaveChangesAsync();
```

## Notes
- Generic repository uses the injected `DbContext`'s `DbSet<T>`.
- Ensure your models are configured in your `DbContext` as usual with EF Core.
- PostgreSQL is configured via `UseNpgsql`; you can extend this pattern for other providers if needed.
