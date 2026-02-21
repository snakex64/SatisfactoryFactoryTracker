namespace SFT.Core.Domain;

public class ProductionRecipeResource
{
    public int Id { get; set; }
    public int ProductionRecipeId { get; set; }
    public ProductionRecipe? ProductionRecipe { get; set; }
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    public decimal Amount { get; set; }
    public bool IsInput { get; set; }
}
