# RepoForge.DataAdapters

High-level data adapters built on top of the `IBlobRepository` abstraction from `RepoForge.Abstractions`.
This package provides:

- JSON data adapter: `RepoForge.DataAdapters.Json.JsonDataAdapter` implementing `IJsonDataAdapter`
- CSV data adapter: `RepoForge.DataAdapters.Csv.CsvDataAdapter` implementing `ICsvDataAdapter`

Both adapters depend on an `IBlobRepository` implementation (e.g., S3 from `RepoForge.AWS`).

## Installation

Reference this package and an `IBlobRepository` provider:
- `RepoForge.AWS` for S3 (see `AddS3Repository(...)`)

## Dependency Injection

Register adapters via extension methods:

```csharp
using Microsoft.Extensions.DependencyInjection;
using RepoForge.DataAdapters.Json;
using RepoForge.DataAdapters.Csv;

var services = new ServiceCollection();

// Register a blob repository first (e.g., S3)
// services.AddS3Repository("your-bucket");

// Then register adapters
services.AddJsonDataAdapter();
services.AddCsvDataAdapter();
```

## JSON Adapter

- Interface: `IJsonDataAdapter`
  - `Task UploadJsonAsync<T>(string key, T data)`
  - `Task<T?> DownloadJsonAsync<T>(string key)`

- Implementation: `JsonDataAdapter`
  - Serializes with `System.Text.Json` and stores via `IBlobRepository`.

### Example
```csharp
var json = provider.GetRequiredService<IJsonDataAdapter>();
await json.UploadJsonAsync("data/users/42.json", new { Id = 42, Name = "Ada" });
var dto = await json.DownloadJsonAsync<UserDto>("data/users/42.json");
```

## CSV Adapter

- Interface: `ICsvDataAdapter`
  - `Task UploadCsvAsync<T>(string key, IEnumerable<T> data)`
  - `Task<IEnumerable<T>?> DownloadCsvAsync<T>(string key) where T : class, new()`

- Implementation: `CsvDataAdapter`
  - Uses `CsvHelper` with `CultureInfo.InvariantCulture`.

### Example
```csharp
var csv = provider.GetRequiredService<ICsvDataAdapter>();
await csv.UploadCsvAsync("exports/report.csv", rows);
var rowsBack = await csv.DownloadCsvAsync<MyRow>("exports/report.csv");
```

## Notes
- Ensure your `IBlobRepository` implementation is registered before adding adapters.
- Keys are logical paths used by the underlying blob store (e.g., S3 object keys).
