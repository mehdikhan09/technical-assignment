namespace StockReplenishment.Api.DTOs;

public class RequestSummaryDto
{
    public int Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int LineItemCount { get; set; }
    public bool? StockCheckPassed { get; set; }
}


