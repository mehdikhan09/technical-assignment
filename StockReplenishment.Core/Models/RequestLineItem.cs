namespace StockReplenishment.Core.Models;

public class RequestLineItem
{
    public int Id { get; set; }
    public int ReplenishmentRequestId { get; set; }
    public ReplenishmentRequest Request { get; set; } = null!;

    public string ArticleNumber { get; set; } = string.Empty;   // e.g. "ART-10045"
    public string Description { get; set; } = string.Empty;     // e.g. "M8 Hex Bolt"
    public int RequestedQuantity { get; set; }
    public int? FulfilledQuantity { get; set; }                 // filled when Fulfilled
}
