namespace SFT.Core.Domain;

public class ProductionRecipe
{
    public int Id { get; set; }
    public string KeyName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CraftTimeSeconds { get; set; }
    public ICollection<ProductionRecipeResource> Resources { get; set; } = [];
}
