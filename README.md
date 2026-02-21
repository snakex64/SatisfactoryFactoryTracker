# SatisfactoryFactoryTracker

SatisfactoryFactoryTracker is a .NET 10 Blazor Server application for tracking Satisfactory production chains:
- which mines produce which resources and at what rate
- which factory levels consume and produce resources
- flow rates between resources, mines, and factory levels

## Architecture
- **SFT.Core**: domain models, EF Core DbContext, and query services
- **SFT.Blazor.Core**: shared MudBlazor UI components
- **SFT.Blazor.Server**: ASP.NET Core Blazor Server host
- **SFT.AppHost**: Aspire host for local orchestration (including PostgreSQL in Docker)

## Tech Stack
- .NET 10
- MudBlazor 9
- EF Core + Npgsql
- PostgreSQL
- .NET Aspire

## Getting Started
1. Ensure Docker is installed and running.
   - Linux: https://docs.docker.com/engine/install/ubuntu/
   - Windows/macOS: https://docs.docker.com/desktop/
   - Verify setup:
     ```bash
     docker --version
     docker info
     ```
2. Run the Aspire host:
   ```bash
   dotnet run --project SFT.AppHost/SFT.AppHost.csproj
   ```
3. Open the URL shown in console output.

## Development
Build:
```bash
dotnet build SatisfactoryFactoryTracker.slnx
```

Test:
```bash
dotnet test SatisfactoryFactoryTracker.slnx
```
