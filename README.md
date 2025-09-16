# RepoForge

[![Build](https://img.shields.io/github/actions/workflow/status/yourusername/repoforge/build.yml?branch=main)](https://github.com/yourusername/repoforge/actions)
[![NuGet](https://img.shields.io/nuget/v/RepoForge.Core.svg)](https://www.nuget.org/packages/RepoForge.Core/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET Version](https://img.shields.io/badge/dotnet-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)

RepoForge is a robust repository pattern implementation for .NET, designed with Clean Architecture principles. It provides a consistent abstraction layer over various data sources including relational databases, NoSQL, and blob storage.

## Key Features

- 🏗️ Clean Architecture implementation
- 🔄 Multiple database providers (EF Core, DynamoDB, S3)
- 📊 Data adapters for JSON and CSV
- 🧪 Testable design with dependency injection
- ⚡ High performance and scalable
- 🔒 Thread-safe implementations

---
## 📑 Índice

- [✨ Filosofia de Design](#-filosofia-de-design)
- [📂 Estrutura do Projeto](#-estrutura-do-projeto)
- [🏗️ Arquitetura (Clean Architecture)](#️-arquitetura-clean-architecture)
- [⚙️ Instalação](#️-instalação)
- [🚀 Como Usar](#-como-usar)
  - [Configuração no Program.cs](#configuração-no-programcs)
  - [Exemplo: Exportar Usuários para JSON e CSV](#exemplo-exportar-usuários-para-json-e-csv)
- [📌 Roadmap](#-roadmap)
- [🤝 Contribuição](#-contribuição)
- [📜 Licença](#-licença)

---

## ✨ Filosofia de Design

- **Domain define contratos**:  
  - `IRepository<T>`, `IUnitOfWork`, `IBlobRepository`, `IJsonDataAdapter`, `ICsvDataAdapter`.  
- **Infrastructure implementa contratos**:  
  - `S3Repository` (para blobs no Amazon S3).  
  - `JsonDataAdapter` / `CsvDataAdapter` (adapta streams para dados semi-estruturados).  
- **Separação clara de responsabilidades**:  
  - `IBlobRepository` lida apenas com **bytes/streams**.  
  - Adapters (`Json`, `Csv`, etc.) lidam com **formatação de dados**.  
- **Extensível**: fácil adicionar novos adaptadores (XML, Parquet, Avro...) sem quebrar contratos existentes.

---

##  Estrutura do Projeto

```
RepoForge/
├── src/
│   ├── Abstractions/              # Core interfaces and DTOs
│   │   └── RepoForge.Abstractions/
│   │       └── Interfaces/
│   │           ├── IRepository<T>
│   │           ├── IUnitOfWork
│   │           ├── IBlobRepository
│   │           ├── IJsonDataAdapter
│   │           └── ICsvDataAdapter
│   │
│   ├── Core/                      # Core implementations
│   │   └── RepoForge.Core/
│   │       ├── Repositories/
│   │       └── Adapters/
│   │
│   ├── Data Adapters/             # Data adapters
│   │   ├── RepoForge.Infrastructure.DataAdapters.Csv/
│   │   └── RepoForge.Infrastructure.DataAdapters.Json/
│   │
│   └── Providers/                 # Persistence providers
│       ├── RepoForge.Infrastructure.EfCore/     # EF Core (PostgreSQL, SQL Server)
│       ├── RepoForge.Infrastructure.DynamoDb/   # AWS DynamoDB
│       └── RepoForge.Infrastructure.S3/         # AWS S3 Storage
│
└── tests/
    └── Tests/
        ├── RepoForge.Tests.Unit/         # Unit tests
        └── RepoForge.Tests.Integration/  # Integration tests
```

---

##  Arquitetura (Clean Architecture)

```

+------------------------------------------------------------+
\|                       Presentation                         |
\| (MyApp.Api, MyApp.Worker, MyApp.Console)                   |
+------------------------------------------------------------+
|
v
+------------------------------------------------------------+
\|                       Application                          |
\| Usa apenas contratos de RepoForge.Domain                   |
\| Ex.: UserService depende de IRepository<User>              |
\| Ex.: ImportExportService depende de IJsonDataAdapter       |
\|      e ICsvDataAdapter                                     |
+------------------------------------------------------------+
|
v
+------------------------------------------------------------+
\|                        Domain                              |
\| RepoForge.Domain                                           |
\| - Entidades                                                |
\| - IRepository<T>                                           |
\| - IUnitOfWork                                              |
\| - IBlobRepository                                          |
\| - IJsonDataAdapter                                         |
\| - ICsvDataAdapter                                          |
+------------------------------------------------------------+
^
|
+------------------------------------------------------------+
\|                    Infrastructure                          |
\| RepoForge.Infrastructure.EfCore → Relacional (Postgres)    |
\| RepoForge.Infrastructure.DynamoDb → NoSQL (AWS DynamoDB)   |
\| RepoForge.Infrastructure.S3 → Storage (AWS S3)             |
\| RepoForge.Infrastructure.DataAdapters.Json → JsonAdapter   |
\| RepoForge.Infrastructure.DataAdapters.Csv → CsvAdapter     |
+------------------------------------------------------------+

````

---

## ⚙️ Installation

RepoForge is distributed as a set of NuGet packages. You can install only the packages you need.

### 1. Install Core Package (Required)
```bash
dotnet add package RepoForge.Core
```

### 2. Add Required Providers (Choose as needed)
```bash
# For Entity Framework Core
dotnet add package RepoForge.Providers.EfCore

# For AWS DynamoDB
dotnet add package RepoForge.Providers.DynamoDb

# For AWS S3 Storage
dotnet add package RepoForge.Providers.S3
```

### 3. Add Data Adapters (Optional)
```bash
# For JSON data handling
dotnet add package RepoForge.DataAdapters.Json

# For CSV data handling
dotnet add package RepoForge.DataAdapters.Csv
```

### Building from Source

```bash
# Clone the repository
git clone https://github.com/yourusername/repoforge.git
cd repoforge

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Create NuGet packages
dotnet pack -c Release -o ./nupkgs
```

---

## 🚀 Getting Started

### Basic Configuration in `Program.cs`

```csharp
using RepoForge.Providers.EfCore;
using RepoForge.Providers.S3;
using RepoForge.Providers.DynamoDb;
using RepoForge.DataAdapters.Json;
using RepoForge.DataAdapters.Csv;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register RepoForge services
builder.Services.AddRepoForge()
    .AddEntityFrameworkRepository<AppDbContext>()
    .AddDynamoDbRepository<MyDynamoDbEntity>(options =>
    {
        options.TableName = "MyTable";
        options.CreateIfNotExists = true;
    })
    .AddS3Repository<MyS3Entity>(options => 
    {
        options.BucketName = "my-app-bucket";
        options.Region = "us-east-1";
    });

// Register data adapters
builder.Services.AddJsonDataAdapter();
builder.Services.AddCsvDataAdapter();

var app = builder.Build();
app.Run();
```

---

### Example: Exporting Data to JSON and CSV

```csharp
public class UserExport
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

public class UserExportService
{
    private readonly IRepository<User> _userRepository;
    private readonly IJsonDataAdapter _jsonAdapter;
    private readonly ICsvDataAdapter _csvAdapter;

    public UserExportService(
        IRepository<User> userRepository,
        IJsonDataAdapter jsonAdapter,
        ICsvDataAdapter csvAdapter)
    {
        _userRepository = userRepository;
        _jsonAdapter = jsonAdapter;
        _csvAdapter = csvAdapter;
    }

    public UserService(IJsonDataAdapter json, ICsvDataAdapter csv)
    {
        // Query users from the repository
        var users = await _userRepository.FindAsync(u => 
            u.CreatedAt >= startDate && 
            u.CreatedAt <= endDate);

        // Map to DTO
        var userExports = users.Select(u => new UserExport
        {
            Id = u.Id,
            Name = u.FullName,
            Email = u.EmailAddress,
            CreatedAt = u.CreatedAt
        }).ToList();

        // Ensure directory exists
        Directory.CreateDirectory(exportPath);

        // Export to JSON
        string jsonPath = Path.Combine(exportPath, $"users_export_{DateTime.UtcNow:yyyyMMddHHmmss}.json");
        await _jsonAdapter.WriteToFileAsync(userExports, jsonPath);
        
        // Export to CSV
        string csvPath = Path.Combine(exportPath, $"users_export_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
        await _csvAdapter.WriteToFileAsync(userExports, csvPath);

        return (jsonPath, csvPath);
    }
}
```

---

## 📌 Roadmap

### Next Up
- [ ] Add support for MongoDB provider
- [ ] Add XML data adapter
- [ ] Implement distributed transaction support
- [ ] Add comprehensive documentation with examples
- [ ] Performance benchmarking and optimization

### Future Enhancements
- [ ] Support for CosmosDB provider
- [ ] Add Parquet data adapter
- [ ] Implement caching layer
- [ ] Add GraphQL integration
- [ ] Support for multi-tenant scenarios

---

## 🤝 Contributing

We welcome contributions from the community! Here's how you can help:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines
- Follow the existing code style and patterns
- Write unit tests for new features
- Update documentation as needed
- Keep pull requests focused and small
- Use meaningful commit messages

## 📜 License

Distributed under the MIT License. See `LICENSE` for more information.

## 🙏 Acknowledgments

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/)
- [CsvHelper](https://joshclose.github.io/CsvHelper/)
- [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview)

---

Built with ❤️ by the RepoForge Team.
