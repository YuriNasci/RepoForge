# RepoForge

Um framework de repositórios genérico para .NET 8.0 que simplifica a implementação do padrão Repository e Unit of Work.

## Estrutura do Projeto

```
/RepoForge
│── RepoForge.sln
│
├── src/
│   ├── RepoForge.Domain/
│   │   ├── Entities/         # Entidades (opcional)
│   │   ├── Interfaces/
│   │   │   ├── IRepository.cs
│   │   │   └── IUnitOfWork.cs
│   │   └── RepoForge.Domain.csproj
│   │
│   └── RepoForge.Infrastructure.EfCore/
│       ├── Persistence/
│       │   ├── Repository.cs
│       │   ├── UnitOfWork.cs
│       │   └── DependencyInjection.cs
│       └── RepoForge.Infrastructure.EfCore.csproj
│
└── tests/
    └── RepoForge.UnitTests/
```

## Versão 1.0 - Base Sólida

Esta versão fornece o núcleo mínimo utilizável do RepoForge:

- **RepoForge.Domain**: Contratos e interfaces (`IRepository<T>`, `IUnitOfWork`)
- **RepoForge.Infrastructure.EfCore**: Implementação genérica usando EF Core com PostgreSQL

## Tecnologias

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL (Npgsql)
- xUnit (para testes)

## Como Usar

```csharp
// No Program.cs
builder.Services.AddPostgresRepository(
    builder.Configuration.GetConnectionString("DefaultConnection"));
```

## Roadmap

Consulte o arquivo `docs/roadmap.md` para ver o plano de evolução do projeto através das versões 1.0 → 4.0.
