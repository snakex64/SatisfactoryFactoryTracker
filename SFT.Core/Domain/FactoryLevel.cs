namespace SFT.Core.Domain;

public class FactoryLevel
{
    public int Id { get; set; }
    public int FactoryId { get; set; }
    public Factory? Factory { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<FactoryInput> Inputs { get; set; } = [];
    public ICollection<FactoryOutput> Outputs { get; set; } = [];
}
