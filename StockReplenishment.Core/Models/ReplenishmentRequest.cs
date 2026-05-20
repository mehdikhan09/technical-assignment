using StockReplenishment.Core.Enums;

namespace StockReplenishment.Core.Models;

public class ReplenishmentRequest
{
    public int Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;  // e.g. "REQ-0001"
    public RequestStatus Status { get; set; } = RequestStatus.Draft;
    public RequestPriority Priority { get; set; } = RequestPriority.Normal;

    public int StockLocationId { get; set; }
    public StockLocation StockLocation { get; set; } = null!;

    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }

    public string? ReviewedBy { get; set; }
    public string? RejectionReason { get; set; }   // Required when rejected
    public string? Notes { get; set; }

    // Background stock check result
    public bool? StockCheckPassed { get; set; }       // null = still pending
    public string? StockCheckMessage { get; set; }

    // Navigation
    public ICollection<RequestLineItem> LineItems { get; set; } = new List<RequestLineItem>();
}
