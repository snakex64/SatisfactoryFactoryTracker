namespace SFT.Core.Domain;

public class Factory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<FactoryInput> Inputs { get; set; } = [];
    public ICollection<FactoryOutput> Outputs { get; set; } = [];
}
