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

## Interactive Render Mode
- Any page or component that handles user events (button clicks, form submissions, etc.) **must** declare `@rendermode InteractiveServer`. Without this the page is rendered as static SSR and all click handlers are silently ignored.
- The `@rendermode` directive is placed in the **page** file (e.g. `FactoryPlanner.razor`), immediately after the `@page` directive.

## Playwright Test Project (SFT.Tests.Playwright)
- Uses `Microsoft.Playwright.NUnit` with a custom `PlaywrightWebApplicationFactory` (in `Infrastructure/`) that starts Kestrel on a random loopback port and replaces PostgreSQL with EF Core InMemory.
- Before running tests, Playwright browser binaries must be installed. The `InstallPlaywrightBrowsers` MSBuild target in the project file automates this via `playwright.ps1 install --with-deps chromium`.
- Run tests with: `dotnet test SFT.Tests.Playwright/SFT.Tests.Playwright.csproj`

## data-test-id Attribute Inventory
The following `data-test-id` attributes are used by Playwright to locate elements.  When modifying the relevant Razor files, keep these attributes in place.

### `SFT.Blazor.Core/FactoryPlannerPage.razor`
| Attribute value      | Element                          | Purpose                          |
|----------------------|----------------------------------|----------------------------------|
| `add-mine-btn`       | Add Mine `MudIconButton`         | Open the "Add Mine" dialog       |
| `add-factory-btn`    | Add Factory `MudIconButton`      | Open the "Add Factory" dialog    |

### `SFT.Blazor.Core/Dialogs/FactoryDialog.razor`
| Attribute value          | Element                  | Purpose                              |
|--------------------------|--------------------------|--------------------------------------|
| `factory-dialog-name`    | Factory Name `MudTextField` | Factory name input field          |
| `factory-dialog-submit`  | Submit `MudButton`       | Confirm and save the factory         |
| `factory-dialog-cancel`  | Cancel `MudButton`       | Dismiss the dialog without saving    |

### `SFT.Blazor.Core/Dialogs/MineDialog.razor`
| Attribute value       | Element               | Purpose                           |
|-----------------------|-----------------------|-----------------------------------|
| `mine-dialog-name`    | Mine Name `MudTextField` | Mine name input field          |
| `mine-dialog-submit`  | Submit `MudButton`    | Confirm and save the mine         |
| `mine-dialog-cancel`  | Cancel `MudButton`    | Dismiss the dialog without saving |

### `SFT.Blazor.Core/Dialogs/FactoryLevelDialog.razor`
| Attribute value          | Element                      | Purpose                              |
|--------------------------|------------------------------|--------------------------------------|
| `level-dialog-identifier`| Level Name `MudTextField`    | Level identifier input field         |
| `level-dialog-submit`    | Submit `MudButton`           | Confirm and save the level           |
| `level-dialog-cancel`    | Cancel `MudButton`           | Dismiss the dialog without saving    |

### `SFT.Blazor.Core/Dialogs/ConfirmDialog.razor`
| Attribute value          | Element            | Purpose                          |
|--------------------------|--------------------|----------------------------------|
| `confirm-dialog-confirm` | Confirm `MudButton`| Confirm the destructive action   |
| `confirm-dialog-cancel`  | Cancel `MudButton` | Dismiss without taking action    |

