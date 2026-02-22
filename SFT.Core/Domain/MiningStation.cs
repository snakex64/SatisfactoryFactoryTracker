namespace SFT.Core.Domain;

public class MiningStation
{
    public int Id { get; set; }
    public int MineId { get; set; }
    public Mine? Mine { get; set; }
    /// <summary>Miner tier: 1 = MK1 (60/min), 2 = MK2 (120/min), 3 = MK3 (240/min).</summary>
    public int MinerMk { get; set; } = 1;
    /// <summary>Number of power shards slotted: 0 = 100%, 1 = 150%, 2 = 200%, 3 = 300%.</summary>
    public int OverclockLevel { get; set; }
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Returns the total ore extracted per minute for this station group, taking
    /// miner tier, node purity, overclock level and quantity into account.
    /// </summary>
    public decimal ComputeOutputPerMinute(int nodePurity)
    {
        var baseRate = MinerMk switch { 2 => 120m, 3 => 240m, _ => 60m };
        var purityMultiplier = nodePurity switch { 0 => 0.5m, 2 => 2.0m, _ => 1.0m };
        var overclockMultiplier = OverclockLevel switch { 1 => 1.5m, 2 => 2.0m, 3 => 3.0m, _ => 1.0m };
        return baseRate * purityMultiplier * overclockMultiplier * Quantity;
    }
}
