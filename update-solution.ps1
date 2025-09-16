# Update-RepoForgeSolution.ps1
# This script updates the RepoForge solution to include all projects organized in solution folders

# Define paths
$solutionPath = "src\RepoForge.sln"
$srcDir = "src"

# Define solution folder GUIDs (these should be stable)
$solutionFolders = @{
    "Abstractions" = "{67BFE618-E232-7DE5-34A1-02EE902F7C19}"
    "Core" = "{FBF56CC3-7AE6-AD2D-3F14-7F97FD322CD6}"
    "Data Adapters" = "{A1B2C3D4-E5F6-4A5B-8C7D-9E0F1A2B3C4D}"
    "Infrastructure" = "{D4E5F6A1-B2C3-4D5E-8F6A-7B8C9D0E1F2A}"
    "Domain" = "{1A2B3C4D-5E6F-4A5B-8C7D-9E0F1A2B3C4D}"
}

# Define project to folder mapping
$projectMappings = @{
    "RepoForge.Abstractions" = "Abstractions"
    "RepoForge.Core" = "Core"
    "RepoForge.Domain" = "Domain"
    "RepoForge.Infrastructure.DataAdapters.Csv" = "Data Adapters"
    "RepoForge.Infrastructure.DataAdapters.Json" = "Data Adapters"
    "RepoForge.Infrastructure.DynamoDb" = "Infrastructure"
    "RepoForge.Infrastructure.EfCore" = "Infrastructure"
    "RepoForge.Infrastructure.S3" = "Infrastructure"
}

# Create solution folder structure if it doesn't exist
Write-Host "Ensuring solution folder structure..." -ForegroundColor Cyan
foreach ($folder in $solutionFolders.Keys) {
    $folderGuid = $solutionFolders[$folder]
    $folderExists = dotnet sln $solutionPath list | Select-String -Pattern $folder -Quiet
    
    if (-not $folderExists) {
        Write-Host "  - Creating solution folder: $folder" -ForegroundColor Green
        dotnet new sln --name "RepoForge" --output $srcDir --force
        dotnet sln $solutionPath add /nologo /t:Folder "$folder"
    }
}

# Add projects to solution in their respective folders
Write-Host "`nAdding/updating projects in solution..." -ForegroundColor Cyan
foreach ($project in Get-ChildItem -Path $srcDir -Recurse -Filter "*.csproj") {
    $projectName = [System.IO.Path]::GetFileNameWithoutExtension($project.Name)
    $relativePath = $project.FullName.Substring((Get-Location).Path.Length + 1)
    
    # Check if project is already in solution
    $inSolution = dotnet sln $solutionPath list | Select-String -Pattern $projectName -Quiet
    
    if (-not $inSolution) {
        # Add project to solution
        Write-Host "  - Adding project: $projectName" -ForegroundColor Green
        dotnet sln $solutionPath add $relativePath
        
        # Move to solution folder if mapping exists
        if ($projectMappings.ContainsKey($projectName)) {
            $folder = $projectMappings[$projectName]
            $folderGuid = $solutionFolders[$folder]
            Write-Host "    - Moving to solution folder: $folder" -ForegroundColor Yellow
            
            # This is a simplified approach - in a real scenario, you'd need to modify the .sln file directly
            # or use a more advanced tool like NuGet.CommandLine to properly move projects between solution folders
            # For now, we'll just output the information about where the project should be moved
            Write-Host "    [INFO] Project $projectName should be moved to solution folder: $folder" -ForegroundColor Blue
        }
    } else {
        Write-Host "  - Project already in solution: $projectName" -ForegroundColor Gray
    }
}

Write-Host "`nSolution update complete!" -ForegroundColor Green
Write-Host "Note: Some manual adjustments to the .sln file may be needed for proper solution folder organization." -ForegroundColor Yellow
