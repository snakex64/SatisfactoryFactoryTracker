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
            .Include(m => m.Outputs)
                .ThenInclude(o => o.Resource)
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

    public async Task<IReadOnlyList<Resource>> GetResourcesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Resources
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    private static readonly string[] RawResourceCategories = ["mineral", "oil", "water"];

    public async Task<IReadOnlyList<Resource>> GetRawResourcesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Resources
            .Where(r => RawResourceCategories.Contains(r.Category))
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Resource>> GetResourcesProducibleFromAsync(int rawResourceId, CancellationToken cancellationToken = default)
    {
        // Returns the raw resource itself plus any resource that is the output of a recipe
        // that takes the raw resource as an input ingredient.
        return await dbContext.Resources
            .Where(r => r.Id == rawResourceId
                || dbContext.ProductionRecipeResources.Any(output =>
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

    public async Task<IReadOnlyList<ResourceRecipeView>> GetResourceRecipesAsync(CancellationToken cancellationToken = default)
    {
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
