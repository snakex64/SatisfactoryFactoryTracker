using System.Reflection;
using System.Text.Json;
using GameDataSchema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DomainResource = SFT.Core.Domain.Resource;
using SFT.Core.Domain;

namespace SFT.Core.Data;

public class GameDataSeeder(SatisfactoryDbContext dbContext, ILogger<GameDataSeeder> logger)
{
    public async Task SeedFromEmbeddedJsonAsync(CancellationToken cancellationToken = default)
    {
        if (await dbContext.ProductionRecipes.AnyAsync(cancellationToken))
        {
            return;
        }

        var gameData = LoadGameData();

        // Build a lookup of raw-resource categories so item/fluid names don't override them.
        var rawResourceCategories = gameData.Resources
            .ToDictionary(r => r.KeyName, r => r.Category);

        var resourceMeta = gameData.Resources
            .ToDictionary(r => r.KeyName, r => (Name: r.KeyName, r.Category));

        foreach (var item in gameData.Items)
        {
            // Preserve the raw-resource category if this key is in the resources list.
            var category = rawResourceCategories.TryGetValue(item.KeyName, out var rawCat) ? rawCat : "item";
            resourceMeta[item.KeyName] = (item.Name, category);
        }

        foreach (var fluid in gameData.Fluids)
        {
            var category = rawResourceCategories.TryGetValue(fluid.KeyName, out var rawCat) ? rawCat : "fluid";
            resourceMeta[fluid.KeyName] = (fluid.Name, category);
        }

        var resourcesByKey = await dbContext.Resources
            .ToDictionaryAsync(r => r.KeyName, cancellationToken);

        DomainResource EnsureResource(string resourceKeyName)
        {
            if (!resourcesByKey.TryGetValue(resourceKeyName, out var resource))
            {
                var (name, category) = resourceMeta.TryGetValue(resourceKeyName, out var metadata)
                    ? metadata
                    : (resourceKeyName, "unknown");

                if (category == "unknown")
                {
                    logger.LogWarning("Resource '{ResourceKeyName}' was not found in metadata. Using fallback values.", resourceKeyName);
                }

                resource = new DomainResource
                {
                    KeyName = resourceKeyName,
                    Name = name,
                    Category = category
                };

                dbContext.Resources.Add(resource);
                resourcesByKey[resourceKeyName] = resource;
            }

            return resource;
        }

        foreach (var recipe in gameData.Recipes)
        {
            var productionRecipe = new ProductionRecipe
            {
                KeyName = recipe.KeyName,
                Name = recipe.Name,
                Category = recipe.Category,
                CraftTimeSeconds = (decimal)recipe.Time
            };

            foreach (var ingredient in recipe.Ingredients)
            {
                if (TryParseRecipeAmount(ingredient, out var keyName, out var amount))
                {
                    productionRecipe.Resources.Add(new ProductionRecipeResource
                    {
                        Resource = EnsureResource(keyName),
                        Amount = amount,
                        IsInput = true
                    });
                }
            }

            foreach (var product in recipe.Products)
            {
                if (TryParseRecipeAmount(product, out var keyName, out var amount))
                {
                    productionRecipe.Resources.Add(new ProductionRecipeResource
                    {
                        Resource = EnsureResource(keyName),
                        Amount = amount,
                        IsInput = false
                    });
                }
            }

            dbContext.ProductionRecipes.Add(productionRecipe);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static GameData LoadGameData()
    {
        var assembly = typeof(SatisfactoryDbContext).Assembly;
        var resourceName = assembly.GetManifestResourceNames()
            .First(name => name.EndsWith(".data.json", StringComparison.OrdinalIgnoreCase));

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' was not found.");
        var data = JsonSerializer.Deserialize<GameData>(stream);
        return data ?? throw new InvalidOperationException("Embedded game data could not be deserialized.");
    }

    private static bool TryParseRecipeAmount(IReadOnlyList<JsonElement> parts, out string keyName, out decimal amount)
    {
        keyName = string.Empty;
        amount = 0;

        if (parts.Count < 2)
        {
            return false;
        }

        if (parts[0].ValueKind != JsonValueKind.String)
        {
            return false;
        }

        keyName = parts[0].GetString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(keyName))
        {
            return false;
        }

        amount = parts[1].ValueKind switch
        {
            JsonValueKind.Number when parts[1].TryGetDecimal(out var value) => value,
            JsonValueKind.Number => (decimal)parts[1].GetDouble(),
            _ => 0
        };

        return amount > 0;
    }
}
