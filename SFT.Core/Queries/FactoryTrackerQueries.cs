using Microsoft.EntityFrameworkCore;
using SFT.Core.Data;
using SFT.Core.Domain;

namespace SFT.Core.Queries;

public class FactoryTrackerQueries(IDbContextFactory<SatisfactoryDbContext> dbContextFactory) : IFactoryTrackerQueries
{
    public async Task<IReadOnlyList<Mine>> GetMinesAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Mines
            .Include(m => m.Resource)
            .Include(m => m.Outputs)
                .ThenInclude(o => o.Resource)
            .Include(m => m.MiningStations)
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Factory>> GetFactoriesAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
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

    public async Task<IReadOnlyList<Resource>> GetResourcesAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Resources
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    private static readonly string[] RawResourceCategories = ["mineral", "oil", "water"];

    public async Task<IReadOnlyList<Resource>> GetRawResourcesAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Resources
            .Where(r => RawResourceCategories.Contains(r.Category))
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Resource>> GetResourcesProducibleFromAsync(int rawResourceId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        // Returns only resources that are the direct output of a recipe whose inputs include
        // the given raw resource. The raw resource itself is intentionally excluded because
        // mines are expected to output processed goods (e.g. Iron Ingot), not the ore.
        return await dbContext.Resources
            .Where(r => dbContext.ProductionRecipeResources.Any(output =>
                    !output.IsInput
                    && output.ResourceId == r.Id
                    && dbContext.ProductionRecipeResources.Any(input =>
                        input.ProductionRecipeId == output.ProductionRecipeId
                        && input.IsInput
                        && input.ResourceId == rawResourceId)))
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MineRecipeOption>> GetMineOutputRecipesAsync(int outputResourceId, int inputResourceId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var recipes = await dbContext.ProductionRecipes
            .Include(r => r.Resources)
            .Where(r =>
                r.Resources.Any(rr => !rr.IsInput && rr.ResourceId == outputResourceId) &&
                r.Resources.Any(rr => rr.IsInput && rr.ResourceId == inputResourceId))
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        return recipes.Select(r =>
        {
            var outputEntry = r.Resources.First(rr => !rr.IsInput && rr.ResourceId == outputResourceId);
            var inputEntry = r.Resources.First(rr => rr.IsInput && rr.ResourceId == inputResourceId);
            var ratio = outputEntry.Amount > 0 ? inputEntry.Amount / outputEntry.Amount : 0m;
            return new MineRecipeOption(r.KeyName, r.Name, ratio);
        }).ToList();
    }

    public async Task<IReadOnlyList<ResourceRecipeView>> GetResourceRecipesAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var recipes = await dbContext.ProductionRecipes
            .Include(recipe => recipe.Resources)
                .ThenInclude(rr => rr.Resource)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var resources = await dbContext.Resources
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        return resources.Select(resource =>
        {
            IReadOnlyList<RecipeView> BuildRecipes(bool isInput)
                => recipes
                    .Where(recipe => recipe.Resources.Any(rr => rr.ResourceId == resource.Id && rr.IsInput == isInput))
                    .OrderBy(recipe => recipe.Name)
                    .Select(recipe =>
                    {
                        var usage = recipe.Resources.First(rr => rr.ResourceId == resource.Id && rr.IsInput == isInput);
                        var amountPerMinute = recipe.CraftTimeSeconds > 0
                            ? Math.Round(usage.Amount * 60m / recipe.CraftTimeSeconds, 2)
                            : 0;

                        return new RecipeView(
                            recipe.Name,
                            recipe.KeyName,
                            recipe.Category,
                            recipe.CraftTimeSeconds,
                            usage.Amount,
                            amountPerMinute,
                            recipe.Resources
                                .Where(rr => rr.IsInput)
                                .OrderBy(rr => rr.Resource!.Name)
                                .Select(rr => new RecipeResourceAmountView(rr.Resource!.Name, rr.Amount))
                                .ToList(),
                            recipe.Resources
                                .Where(rr => !rr.IsInput)
                                .OrderBy(rr => rr.Resource!.Name)
                                .Select(rr => new RecipeResourceAmountView(rr.Resource!.Name, rr.Amount))
                                .ToList());
                    })
                    .ToList();

            return new ResourceRecipeView(
                resource.Name,
                resource.KeyName,
                BuildRecipes(isInput: false),
                BuildRecipes(isInput: true));
        }).ToList();
    }
}
