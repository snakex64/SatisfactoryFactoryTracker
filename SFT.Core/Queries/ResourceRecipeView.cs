namespace SFT.Core.Queries;

public sealed record MineRecipeOption(string KeyName, string Name, decimal InputOutputRatio);

public sealed record ResourceRecipeView(
    string ResourceName,
    string ResourceKeyName,
    IReadOnlyList<RecipeView> ProducedBy,
    IReadOnlyList<RecipeView> ConsumedBy);

public sealed record RecipeView(
    string RecipeName,
    string RecipeKeyName,
    string Category,
    decimal CraftTimeSeconds,
    decimal Amount,
    decimal AmountPerMinute,
    IReadOnlyList<RecipeResourceAmountView> Inputs,
    IReadOnlyList<RecipeResourceAmountView> Outputs);

public sealed record RecipeResourceAmountView(string ResourceName, decimal Amount);
