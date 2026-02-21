# Agent Notes

## Project Structure

### SFT.Blazor.Core — Shared UI Library
- All **page components** must live in `SFT.Blazor.Core/Pages/` and must have `@page "/route"` as the **first line** of the file.
- The shared `MainLayout` lives in `SFT.Blazor.Core/Layout/MainLayout.razor`. Navigation links in the app bar should be kept in sync with the pages in `SFT.Blazor.Core/Pages/`.
- Reusable non-page components (dashboard panels, explorers, dialogs, etc.) live directly in `SFT.Blazor.Core/` or in subdirectories such as `Dialogs/`.

### SFT.Blazor.Server — Host Project
- Only contains host-specific wiring: `App.razor`, `Routes.razor`, `ReconnectModal`, `Error.razor`, and `Program.cs`.
- `Routes.razor` must declare `AdditionalAssemblies` pointing at `SFT.Blazor.Core` so the router can discover all pages.
- `Program.cs` must call `.AddAdditionalAssemblies(typeof(SFT.Blazor.Core.Pages.Home).Assembly)` on `MapRazorComponents<App>()` for server-side route registration.

### SFT.Core — Domain & Data
- Domain models live in `SFT.Core/Domain/`.
- EF Core DbContext and migrations live in `SFT.Core/Data/`.
- Query services implement `IFactoryTrackerQueries` in `SFT.Core/Queries/`.
- Command (write) services implement `IFactoryTrackerCommands` in `SFT.Core/Commands/`.
