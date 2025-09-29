# RepoForge
<p align="center">
  <img src="docs/banner.png" alt="RepoForge Banner" width="800"/>
</p>

[![Build](https://img.shields.io/badge/build-passing-brightgreen)](#)
[![License](https://img.shields.io/badge/license-MIT-blue)](#)
[![NuGet](https://img.shields.io/badge/NuGet-internal-lightgrey)](#)

**RepoForge** é uma coleção de bibliotecas para abstração de persistência em diferentes contextos, seguindo os princípios da **Clean Architecture**.  
O objetivo é oferecer uma infraestrutura consistente para repositórios relacionais, NoSQL e storage de blobs, permitindo reuso em múltiplos projetos .NET.

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

## 📂 Estrutura do Projeto

```

RepoForge/
│
├── Domain/
│   └── Interfaces/
│       ├── IRepository<T>
│       ├── IUnitOfWork
│       ├── IBlobRepository
│       ├── IJsonDataAdapter
│       └── ICsvDataAdapter
│
├── Infrastructure/
│   ├── EfCore/                → Relacional (PostgreSQL, SQL Server)
│   ├── DynamoDb/              → NoSQL (AWS DynamoDB)
│   ├── S3/                    → Storage (AWS S3, blobs)
│   └── DataAdapters/
│       ├── Json/              → JsonDataAdapter : IJsonDataAdapter
│       └── Csv/               → CsvDataAdapter : ICsvDataAdapter
│
└── Tests/                     → Unit e Integration tests

```

---

## 🏗️ Arquitetura (Clean Architecture)

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

## ⚙️ Instalação

No momento, o RepoForge é distribuído como **biblioteca interna**.

### 1. Clonar e adicionar como referência
```bash
git clone https://github.com/myorg/repoforge.git
dotnet add reference ../RepoForge/src/RepoForge.Domain/RepoForge.Domain.csproj
dotnet add reference ../RepoForge/src/RepoForge.Infrastructure.S3/RepoForge.Infrastructure.S3.csproj
````

### 2. (Opcional) Criar pacotes locais

```bash
dotnet pack src/RepoForge.Domain -o ./nupkgs
dotnet pack src/RepoForge.Infrastructure.S3 -o ./nupkgs
```

---

## 🚀 Como Usar

### Configuração no `Program.cs`

```csharp
using RepoForge.Infrastructure.EfCore;
using RepoForge.Infrastructure.S3;
using RepoForge.Infrastructure.DataAdapters.Json;
using RepoForge.Infrastructure.DataAdapters.Csv;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPostgresRepository<AppDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddS3Repository("my-bucket");
builder.Services.AddJsonDataAdapter();
builder.Services.AddCsvDataAdapter();

var app = builder.Build();
app.Run();
```

---

### Exemplo 1.: Exportar Usuários para JSON e CSV

```csharp
public class UserExport
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class UserService
{
    private readonly IJsonDataAdapter _json;
    private readonly ICsvDataAdapter _csv;

    public UserService(IJsonDataAdapter json, ICsvDataAdapter csv)
    {
        _json = json;
        _csv = csv;
    }

    public async Task ExportUsers(List<UserExport> users)
    {
        await _json.UploadJsonAsync("exports/users.json", users);
        await _csv.UploadCsvAsync("exports/users.csv", users);
    }
}
```
### Exemplo 2.: Exemplo: Uso com chave simples (Postgres, EF Core)
```csharp
public class UserService
{
    private readonly IRepository<User> _userRepo;

    public UserService(IRepository<User> userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User?> GetUser(Guid id)
        => await _userRepo.GetByIdAsync(id);

    public async Task DeleteUser(Guid id)
        => await _userRepo.DeleteAsync(id);
}
```
### Exemplo 3.: Exemplo: Uso com chave composta (DynamoDB)
```csharp
public class OrderService
{
    private readonly IRepository<Order> _orderRepo;

    public OrderService(IRepository<Order> orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public async Task<Order?> GetOrderAsync(string customerId, string orderId)
        => await _orderRepo.GetByIdAsync(customerId, orderId);

    public async Task DeleteOrderAsync(string customerId, string orderId)
        => await _orderRepo.DeleteAsync(customerId, orderId);
}
```
### Exemplo 4.: Uso Genérico
```csharp
// Get by single key
var user = await userRepo.GetByIdAsync(Guid.Parse("d8e94b73-12ab-45f9-9a7d-1ab2c5f3c111"));

// Get by composite key
var order = await orderRepo.GetByIdAsync("customer-123", "order-456");
```
---

## 📌 Roadmap

* **v1.0** → EF Core (Postgres/SQL Server).
* **v2.0** → Suporte a DynamoDB e S3.
* **v2.1** → Introdução dos DataAdapters (Json e Csv).
* **v3.0 (planejado)** → Cache (Redis), auditoria, queries avançadas.
* **v4.0 (planejado)** → CLI e suporte a novos formatos (XML, Parquet, Avro).

---

## 🤝 Contribuição

1. Faça um fork do repositório
2. Crie uma branch para sua feature: `git checkout -b feature/nome`
3. Commit suas alterações: `git commit -m 'Adiciona suporte a ...'`
4. Envie para o fork: `git push origin feature/nome`
5. Abra um Pull Request

---

## 📜 Licença

Este projeto está licenciado sob os termos da **MIT License**.

---
