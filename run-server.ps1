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

# Generate SSL certificates if they don't exist
Write-Host ""
Write-Host "Checking SSL certificates..." -ForegroundColor $Yellow
$HttpsDir = Join-Path $ServerDir "https"
$CertPath = Join-Path $HttpsDir "aspnetcore-dev-cert.pfx"

if (-not (Test-Path $CertPath)) {
    Write-Host "Generating SSL certificates..." -ForegroundColor $Yellow
    
    # Create https directory if it doesn't exist
    if (-not (Test-Path $HttpsDir)) {
        New-Item -ItemType Directory -Path $HttpsDir -Force | Out-Null
    }
    
    # Check if OpenSSL is available
    $opensslCommand = Get-Command openssl -ErrorAction SilentlyContinue
    if ($opensslCommand) {
        # Use OpenSSL if available
        Write-Host "Using OpenSSL to generate certificates..." -ForegroundColor $Cyan
        
        # Generate SSL certificate
        $keyPath = Join-Path $HttpsDir "aspnetcore-dev-key.pem"
        $certPemPath = Join-Path $HttpsDir "aspnetcore-dev-cert.pem"
        
        # Try OpenSSL with config workaround for Windows
        try {
            # Set OPENSSL_CONF environment variable to avoid config file issues
            $env:OPENSSL_CONF = ""
            
            $opensslResult1 = & openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout $keyPath -out $certPemPath -subj "/CN=localhost" -config NUL 2>&1
            if ($LASTEXITCODE -ne 0) {
                # Fallback without -config if that fails
                $opensslResult1 = & openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout $keyPath -out $certPemPath -subj "/CN=localhost" 2>&1
                if ($LASTEXITCODE -ne 0) {
                    throw "OpenSSL certificate generation failed"
                }
            }
            
            # Convert to PKCS12 format for ASP.NET Core
            $opensslResult2 = & openssl pkcs12 -export -out $CertPath -inkey $keyPath -in $certPemPath -password pass:password 2>&1
            if ($LASTEXITCODE -ne 0) {
                throw "OpenSSL PKCS12 conversion failed"
            }
            
            Write-Host "SSL certificates generated successfully with OpenSSL!" -ForegroundColor $Green
        }
        catch {
            Write-Host "OpenSSL failed, falling back to dotnet dev-certs..." -ForegroundColor $Yellow
            Write-Host "OpenSSL Error: $($opensslResult1)" -ForegroundColor $Red
            
            # Clean up any partial files
            if (Test-Path $keyPath) { Remove-Item $keyPath -Force }
            if (Test-Path $certPemPath) { Remove-Item $certPemPath -Force }
            if (Test-Path $CertPath) { Remove-Item $CertPath -Force }
            
            # Fall back to dotnet dev-certs
            Write-Host "Using dotnet dev-certs as fallback..." -ForegroundColor $Cyan
            
            # Clean existing dev certificates
            dotnet dev-certs https --clean | Out-Null
            
            # Generate new dev certificate
            $devCertResult = dotnet dev-certs https --export-path $CertPath --format Pfx --password password --trust
            if ($LASTEXITCODE -ne 0) {
                Write-Host "ERROR: Failed to generate SSL certificate with dotnet dev-certs" -ForegroundColor $Red
                exit 1
            }
            
            Write-Host "SSL certificates generated successfully with dotnet dev-certs!" -ForegroundColor $Green
        }
    } else {
        # Use .NET dev-certs as fallback
        Write-Host "OpenSSL not found, using dotnet dev-certs..." -ForegroundColor $Cyan
        
        # Clean existing dev certificates
        dotnet dev-certs https --clean | Out-Null
        
        # Generate new dev certificate
        $devCertResult = dotnet dev-certs https --export-path $CertPath --format Pfx --password password --trust
        if ($LASTEXITCODE -ne 0) {
            Write-Host "ERROR: Failed to generate SSL certificate with dotnet dev-certs" -ForegroundColor $Red
            exit 1
        }
        
        Write-Host "SSL certificates generated successfully with dotnet dev-certs!" -ForegroundColor $Green
    }
} else {
    Write-Host "SSL certificates already exist" -ForegroundColor $Green
}

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
