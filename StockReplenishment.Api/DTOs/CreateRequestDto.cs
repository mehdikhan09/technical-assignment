using System.ComponentModel.DataAnnotations;

namespace StockReplenishment.Api.DTOs;

public class CreateRequestDto
{
    [Required]
    public string Priority { get; set; } = "Normal";

    [Required]
    public int StockLocationId { get; set; }

    [Required]
    public string CreatedBy { get; set; } = string.Empty;

    public string? Notes { get; set; }

    [Required, MinLength(1, ErrorMessage = "At least one line item is required.")]
    public List<CreateLineItemDto> LineItems { get; set; } = new();
}

public class CreateLineItemDto
{
    [Required]
    public string ArticleNumber { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int RequestedQuantity { get; set; }
}
