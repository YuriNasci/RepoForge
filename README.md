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

## Como Usar

```csharp
// No Program.cs - Versão 1.0 (Relacional)
builder.Services.AddPostgresRepository(
    builder.Configuration.GetConnectionString("DefaultConnection"));

// No Program.cs - Versão 2.0 (NoSQL + Storage)
builder.Services.AddDynamoRepository("us-east-1");
builder.Services.AddS3Repository("meu-bucket", "us-east-1");
```

## Roadmap

Consulte o arquivo `docs/roadmap.md` para ver o plano de evolução do projeto através das versões 1.0 → 4.0.
