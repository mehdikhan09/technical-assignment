namespace StockReplenishment.Core.Models;

public class StockLocation
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;      // e.g. "LINE-A1"
    public string Description { get; set; } = string.Empty; // e.g. "Assembly Line A, Station 1"

    // Navigation
    public ICollection<ReplenishmentRequest> Requests { get; set; } = new List<ReplenishmentRequest>();
}
