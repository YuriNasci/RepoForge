# RepoForge.Abstractions

Core contracts that define the boundaries between RepoForge packages. Implementations in other packages (e.g., `RepoForge.AWS`, `RepoForge.EntityFrameworkCore`, `RepoForge.DataAdapters`) depend on these interfaces.

## Interfaces

- **`IRepository<T>`**: Generic repository abstraction for CRUD and query operations.
  - `Task<T?> GetByIdAsync(params object[] keys)`
  - `Task<IEnumerable<T>> GetAllAsync()`
  - `Task AddAsync(T entity)`
  - `Task UpdateAsync(T entity)`
  - `Task DeleteAsync(params object[] keys)`
  - `Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)`

- **`IUnitOfWork`**: Transactional boundary for persistence operations.
  - `Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)`

- **`IBlobRepository`**: Blob storage abstraction used by data adapters.
  - `Task UploadAsync(string key, Stream data)`
  - `Task<Stream?> DownloadAsync(string key)`
  - `Task DeleteAsync(string key)`

- **`IJsonDataAdapter`**: High-level JSON upload/download over a blob store.
  - `Task UploadJsonAsync<T>(string key, T data)`
  - `Task<T?> DownloadJsonAsync<T>(string key)`

- **`ICsvDataAdapter`**: High-level CSV upload/download over a blob store.
  - `Task UploadCsvAsync<T>(string key, IEnumerable<T> data)`
  - `Task<IEnumerable<T>?> DownloadCsvAsync<T>(string key) where T : class, new()`

## Usage
Reference this package from your implementation packages and apps to depend on stable contracts and enable DI-driven architecture.
