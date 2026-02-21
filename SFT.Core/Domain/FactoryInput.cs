namespace SFT.Core.Domain;

public class FactoryInput
{
    public int Id { get; set; }
    public int FactoryLevelId { get; set; }
    public FactoryLevel? FactoryLevel { get; set; }
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    public decimal AmountPerMinute { get; set; }
    public int? SourceMineId { get; set; }
    public Mine? SourceMine { get; set; }
    public int? SourceFactoryLevelId { get; set; }
    public FactoryLevel? SourceFactoryLevel { get; set; }
}
