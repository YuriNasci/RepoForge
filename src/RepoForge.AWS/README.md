# RepoForge.AWS

This package provides implementations for interacting with Amazon Web Services (AWS). It includes support for Amazon S3 and Amazon DynamoDB, integrating with the abstractions defined in the `RepoForge.Abstractions` package.

## Features

- **Amazon S3**: Provides functionalities for object storage operations.
- **Amazon DynamoDB**: Offers a NoSQL database service for applications that need consistent, single-digit millisecond latency at any scale.

## Dependencies

- [AWSSDK.S3](https://www.nuget.org/packages/AWSSDK.S3)
- [AWSSDK.DynamoDBv2](https://www.nuget.org/packages/AWSSDK.DynamoDBv2)
- [AWSSDK.Extensions.NETCore.Setup](https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup)
- `RepoForge.Abstractions`

## Getting Started

To use this package, you need to configure the AWS services in your application's service collection.

### S3 Blob Repository

```csharp
using Microsoft.Extensions.DependencyInjection;
using RepoForge.AWS.S3; // AddS3Repository
using RepoForge.Abstractions; // IBlobRepository

var services = new ServiceCollection();

// Requires AWSSDK.Extensions.NETCore.Setup and proper AWS credentials configuration
services.AddS3Repository(bucketName: "your-bucket-name");

var provider = services.BuildServiceProvider();
var blobs = provider.GetRequiredService<IBlobRepository>();
```
### DynamoDB Generic Repository

```csharp
using Microsoft.Extensions.DependencyInjection;
using RepoForge.AWS.DynamoDB; // AddDynamoRepository
using RepoForge.Abstractions; // IRepository<>

var services = new ServiceCollection();

// Registers IAmazonDynamoDB, IDynamoDBContext and IRepository<>
services.AddDynamoRepository();

var provider = services.BuildServiceProvider();
var repo = provider.GetRequiredService<IRepository<MyEntity>>();
