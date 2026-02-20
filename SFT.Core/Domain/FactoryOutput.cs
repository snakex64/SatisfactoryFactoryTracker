namespace SFT.Core.Domain;

public class FactoryOutput
{
    public int Id { get; set; }
    public int FactoryId { get; set; }
    public Factory? Factory { get; set; }
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    public decimal AmountPerMinute { get; set; }
}
