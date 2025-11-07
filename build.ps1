# Build script for Test Case Generator
Write-Host "Building Test Case Generator..." -ForegroundColor Cyan

# Restore dependencies
Write-Host "`nRestoring dependencies..." -ForegroundColor Yellow
dotnet restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to restore dependencies" -ForegroundColor Red
    exit 1
}

# Build the solution
Write-Host "`nBuilding solution..." -ForegroundColor Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed" -ForegroundColor Red
    exit 1
}

Write-Host "`nBuild completed successfully!" -ForegroundColor Green
Write-Host "`nTo run the application:" -ForegroundColor Cyan
Write-Host "  dotnet run --project TestCaseGenerator" -ForegroundColor White
Write-Host "`nOr publish for distribution:" -ForegroundColor Cyan
Write-Host "  dotnet publish TestCaseGenerator -c Release -o ./publish" -ForegroundColor White
