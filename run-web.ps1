# Run script for Test Case Generator Web Application
Write-Host "Starting Test Case Generator Web Application..." -ForegroundColor Cyan
Write-Host ""
Write-Host "The web application will be available at:" -ForegroundColor Yellow
Write-Host "  https://localhost:5001" -ForegroundColor Green
Write-Host "  http://localhost:5000" -ForegroundColor Green
Write-Host ""
Write-Host "Press Ctrl+C to stop the server" -ForegroundColor Yellow
Write-Host ""

dotnet run --project TestCaseGenerator.Web
