namespace SFT.Core.Domain;

public class Mine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    public decimal OutputPerMinute { get; set; }
}
