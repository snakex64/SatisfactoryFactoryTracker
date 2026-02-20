var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var database = postgres.AddDatabase("sftdb");

builder.AddProject("sft-blazor-server", "../SFT.Blazor.Server/SFT.Blazor.Server.csproj")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
