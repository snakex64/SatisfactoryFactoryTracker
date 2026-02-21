using Microsoft.EntityFrameworkCore;

namespace SFT.Core.Data;

public class DatabaseInitializer(SatisfactoryDbContext dbContext, GameDataSeeder gameDataSeeder)
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
        await gameDataSeeder.SeedFromEmbeddedJsonAsync(cancellationToken);
    }
}
