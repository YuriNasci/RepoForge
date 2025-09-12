# RepoForge

Um framework de repositórios genérico para .NET 8.0 que simplifica a implementação do padrão Repository e Unit of Work.

## Estrutura do Projeto

```
/RepoForge
│── RepoForge.sln
│
├── src/
│   ├── RepoForge.Domain/                    # Contratos
│   │   ├── Entities/                        # Entidades (opcional)
│   │   ├── Interfaces/
│   │   │   ├── IRepository.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   ├── IBlobRepository.cs
│   │   │   ├── IJsonDataAdapter.cs
│   │   │   └── ICsvDataAdapter.cs
│   │   └── RepoForge.Domain.csproj
│   │
│   ├── RepoForge.Infrastructure.EfCore/     # Relacional
│   │   ├── Persistence/
│   │   │   ├── Repository.cs
│   │   │   ├── UnitOfWork.cs
│   │   │   └── DependencyInjection.cs
│   │   └── RepoForge.Infrastructure.EfCore.csproj
│   │
│   ├── RepoForge.Infrastructure.DynamoDb/   # NoSQL (AWS DynamoDB)
│   │   ├── Persistence/
│   │   │   ├── DynamoRepository.cs
│   │   │   └── DependencyInjection.cs
│   │   └── RepoForge.Infrastructure.DynamoDb.csproj
│   │
│   ├── RepoForge.Infrastructure.S3/         # Storage (AWS S3)
│   │   ├── Persistence/
│   │   │   ├── S3Repository.cs
│   │   │   └── DependencyInjection.cs
│   │   └── RepoForge.Infrastructure.S3.csproj
│   │
│   ├── RepoForge.Infrastructure.DataAdapters.Json/  # Adaptadores JSON
│   │   ├── JsonDataAdapter.cs
│   │   ├── DependencyInjection.cs
│   │   └── RepoForge.Infrastructure.DataAdapters.Json.csproj
│   │
│   ├── RepoForge.Infrastructure.DataAdapters.Csv/   # Adaptadores CSV
│   │   ├── CsvDataAdapter.cs
│   │   ├── DependencyInjection.cs
│   │   └── RepoForge.Infrastructure.DataAdapters.Csv.csproj
│   │
│   └── RepoForge.Shared/                    # Utilitários compartilhados
│       └── RepoForge.Shared.csproj
│
└── tests/
    ├── RepoForge.UnitTests/
    └── RepoForge.IntegrationTests/
```

## Versão 1.0 - Base Sólida

Esta versão fornece o núcleo mínimo utilizável do RepoForge:

- **RepoForge.Domain**: Contratos e interfaces (`IRepository<T>`, `IUnitOfWork`)
- **RepoForge.Infrastructure.EfCore**: Implementação genérica usando EF Core com PostgreSQL

## Versão 2.0 - Expansão para NoSQL & Storage

Esta versão expande o RepoForge para cenários além do relacional:

- **RepoForge.Infrastructure.DynamoDb**: Implementação para AWS DynamoDB
- **RepoForge.Infrastructure.S3**: Implementação para AWS S3 (armazenamento de blobs)
- **RepoForge.IntegrationTests**: Testes de integração para todos os providers

## Versão 2.1 - DataAdapters para Formatos Estruturados

Esta versão adiciona suporte a formatos de dados estruturados:

- **RepoForge.Infrastructure.DataAdapters.Json**: Adaptador para serialização/deserialização JSON
- **RepoForge.Infrastructure.DataAdapters.Csv**: Adaptador para processamento de arquivos CSV

## Tecnologias

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL (Npgsql)
- AWS DynamoDB (AWSSDK.DynamoDBv2)
- AWS S3 (AWSSDK.S3)
- System.Text.Json (serialização JSON)
- CsvHelper (processamento CSV)
- xUnit (para testes)

## Como Usar (ex.: API)
`appsettings.json`
```json
{
  "AWS": {
    "Region": "us-east-1",
    "Profile": "default"
  }
}
```

`AppDbContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
```

`Program.cs`
```csharp
using Amazon.Extensions.NETCore.Setup;
using RepoForge.Infrastructure.DynamoDb;
using RepoForge.Infrastructure.S3;
using RepoForge.Infrastructure.EfCore;
using RepoForge.Infrastructure.DataAdapters.Json;
using RepoForge.Infrastructure.DataAdapters.Csv;
using MyApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configuração global da AWS
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Repositório relacional (Postgres)
builder.Services.AddPostgresRepository<AppDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

// Repositório NoSQL (DynamoDB)
builder.Services.AddDynamoRepository();

// Repositório de blobs (S3)
builder.Services.AddS3Repository("my-app-bucket");

// DataAdapters para formatos estruturados
builder.Services.AddJsonDataAdapter();
builder.Services.AddCsvDataAdapter();

var app = builder.Build();
app.MapControllers();
app.Run();
```

## Exemplo de Uso dos DataAdapters

### Usando JsonDataAdapter
```csharp
public class UserService
{
    private readonly IJsonDataAdapter _jsonAdapter;
    
    public UserService(IJsonDataAdapter jsonAdapter)
    {
        _jsonAdapter = jsonAdapter;
    }
    
    public async Task SaveUserAsync(User user)
    {
        await _jsonAdapter.UploadJsonAsync($"users/{user.Id}", user);
    }
    
    public async Task<User?> GetUserAsync(string userId)
    {
        return await _jsonAdapter.DownloadJsonAsync<User>($"users/{userId}");
    }
}
```

### Usando CsvDataAdapter
```csharp
public class ReportService
{
    private readonly ICsvDataAdapter _csvAdapter;
    
    public ReportService(ICsvDataAdapter csvAdapter)
    {
        _csvAdapter = csvAdapter;
    }
    
    public async Task ExportUsersToCsvAsync(IEnumerable<User> users)
    {
        await _csvAdapter.UploadCsvAsync("reports/users.csv", users);
    }
    
    public async Task<IEnumerable<User>?> ImportUsersFromCsvAsync()
    {
        return await _csvAdapter.DownloadCsvAsync<User>("reports/users.csv");
    }
}
```

## Roadmap

Consulte o arquivo `docs/roadmap.md` para ver o plano de evolução do projeto através das versões 1.0 → 4.0.
