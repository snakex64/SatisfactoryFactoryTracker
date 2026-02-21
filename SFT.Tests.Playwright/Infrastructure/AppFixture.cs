using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using SFT.Blazor.Server.Components;
using SFT.Core.Commands;
using SFT.Core.Data;
using SFT.Core.Queries;
using System.Net;

namespace SFT.Tests.Playwright.Infrastructure;

/// <summary>
/// Starts a real Kestrel-backed Blazor Server on a random loopback port, with
/// PostgreSQL replaced by an EF Core InMemory database.  This allows Playwright's
/// external browser process to connect via HTTP/WebSocket without needing Docker.
/// </summary>
public sealed class AppFixture : IAsyncDisposable
{
    private WebApplication? _app;

    /// <summary>Gets the base URL of the running test server (e.g. "http://127.0.0.1:54321").</summary>
    public string ServerAddress { get; private set; } = string.Empty;

    public async Task StartAsync()
    {
        // Set the content root to the directory that contains SFT.Blazor.Server's
        // assembly so that MapStaticAssets() can locate the assets manifest.
        var serverAssemblyDir = Path.GetDirectoryName(typeof(App).Assembly.Location)!;

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(App).Assembly.GetName().Name,
            ContentRootPath = serverAssemblyDir,
        });

        // Register the static-web-asset file providers (reads the runtime manifest so that
        // _framework/blazor.web.js and MudBlazor assets can be served in tests).
        builder.WebHost.UseStaticWebAssets();

        // Bind Kestrel to a random loopback port.
        builder.WebHost.ConfigureKestrel(options =>
            options.Listen(IPAddress.Loopback, 0));

        // Register the same services as Program.cs …
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddMudServices();

        // … but swap PostgreSQL for an EF Core InMemory database so no Docker is needed.
        builder.Services.AddDbContext<SatisfactoryDbContext>(options =>
            options.UseInMemoryDatabase("PlaywrightTestDb"));

        builder.Services.AddScoped<IFactoryTrackerQueries, FactoryTrackerQueries>();
        builder.Services.AddScoped<IFactoryTrackerCommands, FactoryTrackerCommands>();

        _app = builder.Build();

        _app.UseAntiforgery();
        _app.MapStaticAssets();
        _app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(SFT.Blazor.Core.Pages.Home).Assembly);

        await _app.StartAsync();

        // Read the port that Kestrel actually bound to.
        var server = _app.Services.GetRequiredService<IServer>();
        var feature = server.Features.Get<IServerAddressesFeature>()
            ?? throw new InvalidOperationException("IServerAddressesFeature not available.");
        ServerAddress = feature.Addresses.First();
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}
