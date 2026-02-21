using Microsoft.EntityFrameworkCore;
using SFT.Core.Data;
using SFT.Core.Domain;

namespace SFT.Core.Queries;

public class FactoryTrackerQueries(SatisfactoryDbContext dbContext) : IFactoryTrackerQueries
{
    public async Task<IReadOnlyList<Mine>> GetMinesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Mines
            .Include(m => m.Resource)
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Factory>> GetFactoriesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Factories
            .Include(f => f.Levels)
                .ThenInclude(l => l.Inputs)
                    .ThenInclude(i => i.Resource)
            .Include(f => f.Levels)
                .ThenInclude(l => l.Inputs)
                    .ThenInclude(i => i.SourceMine)
            .Include(f => f.Levels)
                .ThenInclude(l => l.Inputs)
                    .ThenInclude(i => i.SourceFactoryLevel)
            .Include(f => f.Levels)
                .ThenInclude(l => l.Outputs)
                    .ThenInclude(o => o.Resource)
            .AsSplitQuery()
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);
    }
}
