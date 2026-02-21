using Microsoft.EntityFrameworkCore;
using SFT.Core.Data;
using SFT.Core.Domain;

namespace SFT.Core.Commands;

public class FactoryTrackerCommands(SatisfactoryDbContext dbContext) : IFactoryTrackerCommands
{
    public async Task<Mine> AddMineAsync(Mine mine, CancellationToken cancellationToken = default)
    {
        dbContext.Mines.Add(mine);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mine;
    }

    public async Task UpdateMineAsync(Mine mine, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Mines.FindAsync([mine.Id], cancellationToken)
            ?? throw new InvalidOperationException($"Mine {mine.Id} not found.");
        existing.Name = mine.Name;
        existing.ResourceId = mine.ResourceId;
        existing.OutputPerMinute = mine.OutputPerMinute;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMineAsync(int mineId, CancellationToken cancellationToken = default)
    {
        var mine = await dbContext.Mines.FindAsync([mineId], cancellationToken)
            ?? throw new InvalidOperationException($"Mine {mineId} not found.");
        dbContext.Mines.Remove(mine);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Factory> AddFactoryAsync(Factory factory, CancellationToken cancellationToken = default)
    {
        dbContext.Factories.Add(factory);
        await dbContext.SaveChangesAsync(cancellationToken);
        return factory;
    }

    public async Task UpdateFactoryAsync(Factory factory, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Factories.FindAsync([factory.Id], cancellationToken)
            ?? throw new InvalidOperationException($"Factory {factory.Id} not found.");
        existing.Name = factory.Name;
        existing.Description = factory.Description;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFactoryAsync(int factoryId, CancellationToken cancellationToken = default)
    {
        var factory = await dbContext.Factories.FindAsync([factoryId], cancellationToken)
            ?? throw new InvalidOperationException($"Factory {factoryId} not found.");
        dbContext.Factories.Remove(factory);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<FactoryLevel> AddFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default)
    {
        dbContext.FactoryLevels.Add(level);
        await dbContext.SaveChangesAsync(cancellationToken);
        return level;
    }

    public async Task UpdateFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.FactoryLevels.FindAsync([level.Id], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryLevel {level.Id} not found.");
        if (existing.FactoryId != level.FactoryId)
            throw new InvalidOperationException("Cannot move a level to a different factory.");
        existing.Identifier = level.Identifier;
        existing.Description = level.Description;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFactoryLevelAsync(int levelId, CancellationToken cancellationToken = default)
    {
        var level = await dbContext.FactoryLevels.FindAsync([levelId], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryLevel {levelId} not found.");
        dbContext.FactoryLevels.Remove(level);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
