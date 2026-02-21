# GitHub Copilot Instructions

## Project Overview
SatisfactoryFactoryTracker tracks Satisfactory production chains:
- mines and their ore output rates
- factories with named levels and their input/output rates
- resource flow between mines and factory levels

## Solution Structure
- `SFT.Core`: domain entities, EF Core DbContext, and query services
- `SFT.Blazor.Core`: reusable MudBlazor UI components
- `SFT.Blazor.Server`: Blazor Server host and app composition
- `SFT.AppHost`: Aspire app host that starts the app and PostgreSQL container

## Tech Stack
- .NET 10
- MudBlazor v9
- EF Core with Npgsql provider
- PostgreSQL (local container orchestrated by Aspire)

## Coding Notes
- Keep business logic in `SFT.Core`
- Keep UI components in `SFT.Blazor.Core`
- Keep hosting/wiring in `SFT.Blazor.Server`
- Prefer small, focused components and services
