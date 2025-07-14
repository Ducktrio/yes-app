# PowerShell script to build and run the .NET server
param(
    [Parameter(Position=0)]
    [ValidateSet("dev", "prod", "development", "production")]
    [string]$Mode = "dev",
    
    [switch]$Help
)

# Show help
if ($Help) {
    Write-Host "Usage: .\run-server.ps1 [dev|prod] [-Help]"
    Write-Host "  dev, development   Use dotnet run (default)"
    Write-Host "  prod, production   Build and run executable"
    Write-Host "  -Help              Show this help message"
    exit 0
}

# Normalize mode
if ($Mode -eq "production") { $Mode = "prod" }
if ($Mode -eq "development") { $Mode = "dev" }

# Colors for output (PowerShell style)
$Red = "Red"
$Green = "Green"
$Yellow = "Yellow"
$Blue = "Blue"
$Cyan = "Cyan"

# Script directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ServerDir = Join-Path $ScriptDir "server"

Write-Host "Yes App - Build & Run Script ($Mode mode)" -ForegroundColor $Blue
Write-Host "==================================================" -ForegroundColor $Blue

# Check if .NET is installed
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: .NET is not installed or not in PATH" -ForegroundColor $Red
    exit 1
}

Write-Host "Checking .NET Version:" -ForegroundColor $Yellow
dotnet --version

# Navigate to server directory
if (-not (Test-Path $ServerDir)) {
    Write-Host "ERROR: Server directory not found: $ServerDir" -ForegroundColor $Red
    exit 1
}

Set-Location $ServerDir

# Clean previous builds
Write-Host ""
Write-Host "Cleaning previous builds..." -ForegroundColor $Yellow
dotnet clean

# Restore dependencies
Write-Host ""
Write-Host "Restoring dependencies..." -ForegroundColor $Yellow
$restoreResult = dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to restore dependencies" -ForegroundColor $Red
    exit 1
}

# Build the project
Write-Host ""
Write-Host "Building the project..." -ForegroundColor $Yellow
if ($Mode -eq "dev") {
    # For development, just do a quick build check
    $buildResult = dotnet build
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Build failed" -ForegroundColor $Red
        exit 1
    }
} else {
    # For production, do a full release build
    $buildResult = dotnet build --configuration Release
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Build failed" -ForegroundColor $Red
        exit 1
    }
}

Write-Host ""
Write-Host "Build successful!" -ForegroundColor $Green

# Run the server
Write-Host ""
Write-Host "Starting the server..." -ForegroundColor $Yellow
Write-Host "Server will be available at:" -ForegroundColor $Blue
Write-Host "  HTTP:  http://localhost:5257" -ForegroundColor $Cyan
Write-Host "  HTTPS: https://localhost:7231" -ForegroundColor $Cyan
Write-Host "  Swagger: http://localhost:5257/swagger" -ForegroundColor $Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the server" -ForegroundColor $Yellow
Write-Host "==================================" -ForegroundColor $Blue

if ($Mode -eq "dev") {
    Write-Host "Running in development mode with dotnet run" -ForegroundColor $Green
    # Use dotnet run for development (supports hot reload)
    dotnet run --urls "https://localhost:7231;http://localhost:5257"
} else {
    Write-Host "Running in production mode from executable" -ForegroundColor $Green
    # Run the built executable directly
    $ExePath = ".\bin\Release\net8.0\Yes.exe"
    if (-not (Test-Path $ExePath)) {
        Write-Host "ERROR: Executable not found: $ExePath" -ForegroundColor $Red
        Write-Host "Make sure the build completed successfully" -ForegroundColor $Yellow
        exit 1
    }
    
    # Set environment variables for production
    $env:ASPNETCORE_ENVIRONMENT = "Production"
    $env:ASPNETCORE_URLS = "https://localhost:7231;http://localhost:5257"
    
    # Run the executable
    & $ExePath
}
