using Microsoft.EntityFrameworkCore;
using SFT.Core.Data;
using SFT.Core.Domain;

namespace SFT.Core.Commands;

public class FactoryTrackerCommands(IDbContextFactory<SatisfactoryDbContext> dbContextFactory) : IFactoryTrackerCommands
{
    public async Task<Mine> AddMineAsync(Mine mine, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.Mines.Add(mine);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mine;
    }

    public async Task UpdateMineAsync(Mine mine, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await dbContext.Mines.FindAsync([mine.Id], cancellationToken)
            ?? throw new InvalidOperationException($"Mine {mine.Id} not found.");
        existing.Name = mine.Name;
        existing.ResourceId = mine.ResourceId;
        existing.NodePurity = mine.NodePurity;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMineAsync(int mineId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var mine = await dbContext.Mines.FindAsync([mineId], cancellationToken)
            ?? throw new InvalidOperationException($"Mine {mineId} not found.");
        dbContext.Mines.Remove(mine);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<MineOutput> AddMineOutputAsync(MineOutput output, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var duplicate = await dbContext.MineOutputs
            .AnyAsync(o => o.MineId == output.MineId && o.ResourceId == output.ResourceId, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("This resource is already added as an output for this mine.");
        dbContext.MineOutputs.Add(output);
        await dbContext.SaveChangesAsync(cancellationToken);
        return output;
    }

    public async Task UpdateMineOutputAsync(MineOutput output, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await dbContext.MineOutputs.FindAsync([output.Id], cancellationToken)
            ?? throw new InvalidOperationException($"MineOutput {output.Id} not found.");
        var duplicate = await dbContext.MineOutputs
            .AnyAsync(o => o.MineId == existing.MineId && o.ResourceId == output.ResourceId && o.Id != output.Id, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("This resource is already added as an output for this mine.");
        existing.ResourceId = output.ResourceId;
        existing.AmountPerMinute = output.AmountPerMinute;
        existing.RecipeKeyName = output.RecipeKeyName;
        existing.InputAmountPerMinute = output.InputAmountPerMinute;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMineOutputAsync(int outputId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var output = await dbContext.MineOutputs.FindAsync([outputId], cancellationToken)
            ?? throw new InvalidOperationException($"MineOutput {outputId} not found.");
        dbContext.MineOutputs.Remove(output);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<MiningStation> AddMiningStationAsync(MiningStation station, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var duplicate = await dbContext.MiningStations
            .AnyAsync(s => s.MineId == station.MineId && s.MinerMk == station.MinerMk && s.OverclockLevel == station.OverclockLevel, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("A mining station with this miner type and overclock level already exists for this mine.");
        dbContext.MiningStations.Add(station);
        await dbContext.SaveChangesAsync(cancellationToken);
        return station;
    }

    public async Task UpdateMiningStationAsync(MiningStation station, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await dbContext.MiningStations.FindAsync([station.Id], cancellationToken)
            ?? throw new InvalidOperationException($"MiningStation {station.Id} not found.");
        var duplicate = await dbContext.MiningStations
            .AnyAsync(s => s.MineId == existing.MineId && s.MinerMk == station.MinerMk && s.OverclockLevel == station.OverclockLevel && s.Id != station.Id, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("A mining station with this miner type and overclock level already exists for this mine.");
        existing.MinerMk = station.MinerMk;
        existing.OverclockLevel = station.OverclockLevel;
        existing.Quantity = station.Quantity;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMiningStationAsync(int stationId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var station = await dbContext.MiningStations.FindAsync([stationId], cancellationToken)
            ?? throw new InvalidOperationException($"MiningStation {stationId} not found.");
        dbContext.MiningStations.Remove(station);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Factory> AddFactoryAsync(Factory factory, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.Factories.Add(factory);
        await dbContext.SaveChangesAsync(cancellationToken);
        return factory;
    }

    public async Task UpdateFactoryAsync(Factory factory, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await dbContext.Factories.FindAsync([factory.Id], cancellationToken)
            ?? throw new InvalidOperationException($"Factory {factory.Id} not found.");
        existing.Name = factory.Name;
        existing.Description = factory.Description;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFactoryAsync(int factoryId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var factory = await dbContext.Factories.FindAsync([factoryId], cancellationToken)
            ?? throw new InvalidOperationException($"Factory {factoryId} not found.");
        dbContext.Factories.Remove(factory);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<FactoryLevel> AddFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var maxSortIndex = await dbContext.FactoryLevels
            .Where(l => l.FactoryId == level.FactoryId)
            .Select(l => (int?)l.SortIndex)
            .MaxAsync(cancellationToken);
        level.SortIndex = (maxSortIndex ?? -1) + 1;
        dbContext.FactoryLevels.Add(level);
        await dbContext.SaveChangesAsync(cancellationToken);
        return level;
    }

    public async Task UpdateFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
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
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var level = await dbContext.FactoryLevels.FindAsync([levelId], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryLevel {levelId} not found.");
        dbContext.FactoryLevels.Remove(level);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MoveLevelUpAsync(int levelId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var level = await dbContext.FactoryLevels.FindAsync([levelId], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryLevel {levelId} not found.");
        var predecessor = await dbContext.FactoryLevels
            .Where(l => l.FactoryId == level.FactoryId && l.SortIndex < level.SortIndex)
            .OrderByDescending(l => l.SortIndex)
            .FirstOrDefaultAsync(cancellationToken);
        if (predecessor is null)
            return;
        (predecessor.SortIndex, level.SortIndex) = (level.SortIndex, predecessor.SortIndex);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MoveLevelDownAsync(int levelId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var level = await dbContext.FactoryLevels.FindAsync([levelId], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryLevel {levelId} not found.");
        var successor = await dbContext.FactoryLevels
            .Where(l => l.FactoryId == level.FactoryId && l.SortIndex > level.SortIndex)
            .OrderBy(l => l.SortIndex)
            .FirstOrDefaultAsync(cancellationToken);
        if (successor is null)
            return;
        (successor.SortIndex, level.SortIndex) = (level.SortIndex, successor.SortIndex);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<FactoryOutput> AddFactoryOutputAsync(FactoryOutput output, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var duplicate = await dbContext.FactoryOutputs
            .AnyAsync(o => o.FactoryLevelId == output.FactoryLevelId && o.ResourceId == output.ResourceId, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("This resource is already added as an output for this level.");
        dbContext.FactoryOutputs.Add(output);
        await dbContext.SaveChangesAsync(cancellationToken);
        return output;
    }

    public async Task UpdateFactoryOutputAsync(FactoryOutput output, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await dbContext.FactoryOutputs.FindAsync([output.Id], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryOutput {output.Id} not found.");
        var duplicate = await dbContext.FactoryOutputs
            .AnyAsync(o => o.FactoryLevelId == existing.FactoryLevelId && o.ResourceId == output.ResourceId && o.Id != output.Id, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("This resource is already added as an output for this level.");
        existing.ResourceId = output.ResourceId;
        existing.AmountPerMinute = output.AmountPerMinute;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFactoryOutputAsync(int outputId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var output = await dbContext.FactoryOutputs.FindAsync([outputId], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryOutput {outputId} not found.");
        dbContext.FactoryOutputs.Remove(output);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<FactoryInput> AddFactoryInputAsync(FactoryInput input, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var duplicate = await dbContext.FactoryInputs
            .AnyAsync(i => i.FactoryLevelId == input.FactoryLevelId && i.ResourceId == input.ResourceId, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("This resource is already added as an input for this level.");
        dbContext.FactoryInputs.Add(input);
        await dbContext.SaveChangesAsync(cancellationToken);
        return input;
    }

    public async Task UpdateFactoryInputAsync(FactoryInput input, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await dbContext.FactoryInputs.FindAsync([input.Id], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryInput {input.Id} not found.");
        var duplicate = await dbContext.FactoryInputs
            .AnyAsync(i => i.FactoryLevelId == existing.FactoryLevelId && i.ResourceId == input.ResourceId && i.Id != input.Id, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("This resource is already added as an input for this level.");
        existing.ResourceId = input.ResourceId;
        existing.AmountPerMinute = input.AmountPerMinute;
        existing.SourceMineId = input.SourceMineId;
        existing.SourceFactoryLevelId = input.SourceFactoryLevelId;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFactoryInputAsync(int inputId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var input = await dbContext.FactoryInputs.FindAsync([inputId], cancellationToken)
            ?? throw new InvalidOperationException($"FactoryInput {inputId} not found.");
        dbContext.FactoryInputs.Remove(input);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
