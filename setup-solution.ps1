# Remove existing solution file if it exists
if (Test-Path "src/RepoForge.sln") {
    Remove-Item "src/RepoForge.sln"
}

# Create new solution
dotnet new sln -n RepoForge -o src

# Add projects to solution in appropriate folders

# Abstractions
dotnet sln src/RepoForge.sln add src/RepoForge.Abstractions/RepoForge.Abstractions.csproj --solution-folder "Abstractions"

# Core
dotnet sln src/RepoForge.sln add src/RepoForge.Core/RepoForge.Core.csproj --solution-folder "Core"

# Data Adapters
dotnet sln src/RepoForge.sln add src/RepoForge.Infrastructure.DataAdapters.Csv/RepoForge.Infrastructure.DataAdapters.Csv.csproj --solution-folder "Data Adapters"
dotnet sln src/RepoForge.sln add src/RepoForge.Infrastructure.DataAdapters.Json/RepoForge.Infrastructure.DataAdapters.Json.csproj --solution-folder "Data Adapters"

# Providers
dotnet sln src/RepoForge.sln add src/RepoForge.Infrastructure.DynamoDb/RepoForge.Infrastructure.DynamoDb.csproj --solution-folder "Providers"
dotnet sln src/RepoForge.sln add src/RepoForge.Infrastructure.EfCore/RepoForge.Infrastructure.EfCore.csproj --solution-folder "Providers"
dotnet sln src/RepoForge.sln add src/RepoForge.Infrastructure.S3/RepoForge.Infrastructure.S3.csproj --solution-folder "Providers"

# Tests
dotnet sln src/RepoForge.sln add tests/RepoForge.Tests.Unit/RepoForge.Tests.Unit.csproj --solution-folder "Tests"
dotnet sln src/RepoForge.sln add tests/RepoForge.Tests.Integration/RepoForge.Tests.Integration.csproj --solution-folder "Tests"

Write-Host "Solution structure created successfully!"
