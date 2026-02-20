# SatisfactoryFactoryTracker

SatisfactoryFactoryTracker is a .NET 10 Blazor Server application for tracking Satisfactory production chains:
- which mines produce which resources and at what rate
- which factories consume and produce resources
- flow rates between resources, mines, and factories

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
1. Ensure Docker is running.
2. Run the Aspire host:
   ```bash
   dotnet run --project /home/runner/work/SatisfactoryFactoryTracker/SatisfactoryFactoryTracker/SFT.AppHost/SFT.AppHost.csproj
   ```
3. Open the URL shown in console output.

## Development
Build:
```bash
dotnet build /home/runner/work/SatisfactoryFactoryTracker/SatisfactoryFactoryTracker/SatisfactoryFactoryTracker.slnx
```

Test:
```bash
dotnet test /home/runner/work/SatisfactoryFactoryTracker/SatisfactoryFactoryTracker/SatisfactoryFactoryTracker.slnx
```
