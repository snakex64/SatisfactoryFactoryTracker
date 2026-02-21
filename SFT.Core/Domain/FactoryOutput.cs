namespace SFT.Core.Domain;

public class FactoryOutput
{
    public int Id { get; set; }
    public int FactoryLevelId { get; set; }
    public FactoryLevel? FactoryLevel { get; set; }
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    public decimal AmountPerMinute { get; set; }
}
