#!/bin/bash

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Script directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SERVER_DIR="$SCRIPT_DIR/server"

# Default mode
MODE="dev"

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --prod|--production)
            MODE="prod"
            shift
            ;;
        --dev|--development)
            MODE="dev"
            shift
            ;;
        -h|--help)
            echo "Usage: $0 [--dev|--prod]"
            echo "  --dev, --development   Use dotnet run (default)"
            echo "  --prod, --production   Build and run executable"
            exit 0
            ;;
        *)
            echo "Unknown option $1"
            exit 1
            ;;
    esac
done

echo -e "${BLUE}🚀 Yes App - Build & Run Script (${MODE} mode)${NC}"
echo "=================================================="

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}❌ .NET is not installed or not in PATH${NC}"
    exit 1
fi

echo -e "${YELLOW}📋 .NET Version:${NC}"
dotnet --version

# Navigate to server directory
if [ ! -d "$SERVER_DIR" ]; then
    echo -e "${RED}❌ Server directory not found: $SERVER_DIR${NC}"
    exit 1
fi

cd "$SERVER_DIR"

# Clean previous builds
echo -e "\n${YELLOW}🧹 Cleaning previous builds...${NC}"
dotnet clean

# Restore dependencies
echo -e "\n${YELLOW}📦 Restoring dependencies...${NC}"
if ! dotnet restore; then
    echo -e "${RED}❌ Failed to restore dependencies${NC}"
    exit 1
fi

# Build the project
echo -e "\n${YELLOW}🔨 Building the project...${NC}"
if [ "$MODE" = "dev" ]; then
    # For development, just do a quick build check
    if ! dotnet build; then
        echo -e "${RED}❌ Build failed${NC}"
        exit 1
    fi
else
    # For production, do a full release build
    if ! dotnet build --configuration Release; then
        echo -e "${RED}❌ Build failed${NC}"
        exit 1
    fi
fi

echo -e "\n${GREEN}✅ Build successful!${NC}"

# Run the server
echo -e "\n${YELLOW}🏃 Starting the server...${NC}"
echo -e "${BLUE}Server will be available at:${NC}"
echo -e "  • HTTP:  http://localhost:5257"
echo -e "  • HTTPS: https://localhost:7231"
echo -e "  • Swagger: http://localhost:5257/swagger"
echo -e "\n${YELLOW}Press Ctrl+C to stop the server${NC}"
echo "=================================="

if [ "$MODE" = "dev" ]; then
    echo -e "${GREEN}Running in development mode with dotnet run${NC}"
    # Use dotnet run for development (supports hot reload)
    dotnet run --urls "https://localhost:7231;http://localhost:5257"
else
    echo -e "${GREEN}Running in production mode from executable${NC}"
    # Run the built executable directly
    EXE_PATH="./bin/Release/net8.0/Yes"
    if [ ! -f "$EXE_PATH" ]; then
        echo -e "${RED}❌ Executable not found: $EXE_PATH${NC}"
        echo -e "${YELLOW}Make sure the build completed successfully${NC}"
        exit 1
    fi
    
    # Set environment variables for production
    export ASPNETCORE_ENVIRONMENT=Production
    export ASPNETCORE_URLS="https://localhost:7231;http://localhost:5257"
    
    # Run the executable
    $EXE_PATH
fi
