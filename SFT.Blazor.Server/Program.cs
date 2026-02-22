using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SFT.Blazor.Server.Components;
using SFT.Core.Commands;
using SFT.Core.Data;
using SFT.Core.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

builder.Services.AddDbContextFactory<SatisfactoryDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("sftdb");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        var host = Environment.GetEnvironmentVariable("SFT_DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("SFT_DB_PORT") ?? "5432";
        var database = Environment.GetEnvironmentVariable("SFT_DB_NAME") ?? "sftdb";
        var username = Environment.GetEnvironmentVariable("SFT_DB_USERNAME");
        var password = Environment.GetEnvironmentVariable("SFT_DB_PASSWORD");

        var parts = new List<string> { $"Host={host}", $"Port={port}", $"Database={database}" };
        if (!string.IsNullOrWhiteSpace(username))
        {
            parts.Add($"Username={username}");
        }

        if (!string.IsNullOrWhiteSpace(password))
        {
            parts.Add($"Password={password}");
        }

        connectionString = string.Join(';', parts);
    }

    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IFactoryTrackerQueries, FactoryTrackerQueries>();
builder.Services.AddScoped<IFactoryTrackerCommands, FactoryTrackerCommands>();
builder.Services.AddScoped<GameDataSeeder>();
builder.Services.AddScoped<DatabaseInitializer>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    try
    {
        var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.InitializeAsync();
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "Database initialization failed during startup. Check database connection settings and ensure PostgreSQL is reachable.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(SFT.Blazor.Core.Pages.Home).Assembly);

app.Run();

public partial class Program { }
