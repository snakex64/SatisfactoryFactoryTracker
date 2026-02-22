namespace SFT.Core.Domain;

public class MineOutput
{
    public int Id { get; set; }
    public int MineId { get; set; }
    public Mine? Mine { get; set; }
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    public decimal AmountPerMinute { get; set; }
    public string? RecipeKeyName { get; set; }
    public decimal InputAmountPerMinute { get; set; }
}
