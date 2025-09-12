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
│   │   │   └── IBlobRepository.cs
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
│   └── RepoForge.Infrastructure.S3/         # Storage (AWS S3)
│       ├── Persistence/
│       │   ├── S3Repository.cs
│       │   └── DependencyInjection.cs
│       └── RepoForge.Infrastructure.S3.csproj
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

## Tecnologias

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL (Npgsql)
- AWS DynamoDB (AWSSDK.DynamoDBv2)
- AWS S3 (AWSSDK.S3)
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

var app = builder.Build();
app.MapControllers();
app.Run();
```

## Roadmap

Consulte o arquivo `docs/roadmap.md` para ver o plano de evolução do projeto através das versões 1.0 → 4.0.
