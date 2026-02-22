using System.ComponentModel.DataAnnotations.Schema;

namespace SFT.Core.Domain;

public class Mine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    /// <summary>Node purity: 0 = Impure (0.5×), 1 = Normal (1×), 2 = Pure (2×).</summary>
    public int NodePurity { get; set; } = 1;
    public ICollection<MineOutput> Outputs { get; set; } = [];
    public ICollection<MiningStation> MiningStations { get; set; } = [];

    /// <summary>
    /// Total raw ore extracted per minute, computed from configured mining stations.
    /// Returns 0 when no stations are configured.
    /// </summary>
    [NotMapped]
    public decimal OutputPerMinute => MiningStations.Sum(s => s.ComputeOutputPerMinute(NodePurity));
}
