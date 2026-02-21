namespace SFT.Core.Domain;

public class Factory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<FactoryLevel> Levels { get; set; } = [];
}
