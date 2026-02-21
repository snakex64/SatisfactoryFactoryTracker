var builder = DistributedApplication.CreateBuilder(args);

var conn = builder.AddParameter("postgres-conn", "Password456!");

var postgres = builder.AddPostgres("postgres")
    .WithPassword(conn)
    .WithDataVolume("postgres-data");
var database = postgres.AddDatabase("sftdb");

builder.AddProject("sft-blazor-server", "../SFT.Blazor.Server/SFT.Blazor.Server.csproj")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
