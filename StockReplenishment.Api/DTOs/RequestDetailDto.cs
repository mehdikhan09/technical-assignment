namespace StockReplenishment.Api.DTOs;

public class RequestDetailDto
{
    public int Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int StockLocationId { get; set; }
    public string LocationCode { get; set; } = string.Empty;
    public string LocationDescription { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public string? ReviewedBy { get; set; }
    public string? RejectionReason { get; set; }
    public string? Notes { get; set; }
    public bool? StockCheckPassed { get; set; }
    public string? StockCheckMessage { get; set; }
    public List<LineItemDto> LineItems { get; set; } = new();
}

public class LineItemDto
{
    public int Id { get; set; }
    public string ArticleNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RequestedQuantity { get; set; }
    public int? FulfilledQuantity { get; set; }
}
