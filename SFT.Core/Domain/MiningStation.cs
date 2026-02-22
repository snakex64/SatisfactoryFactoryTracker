namespace SFT.Core.Domain;

public class MiningStation
{
    public int Id { get; set; }
    public int MineId { get; set; }
    public Mine? Mine { get; set; }
    public int OverclockLevel { get; set; }
    public int Quantity { get; set; } = 1;
}
