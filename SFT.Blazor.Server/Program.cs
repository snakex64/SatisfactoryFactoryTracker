using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SFT.Blazor.Server.Components;
using SFT.Core.Data;
using SFT.Core.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

builder.Services.AddDbContext<SatisfactoryDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("sftdb")
        ?? "Host=localhost;Port=5432;Database=sftdb;Username=postgres;Password=postgres";

    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IFactoryTrackerQueries, FactoryTrackerQueries>();

var app = builder.Build();

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
    .AddInteractiveServerRenderMode();

app.Run();
