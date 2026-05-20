using System.ComponentModel.DataAnnotations;

namespace StockReplenishment.Api.DTOs;

public class FulfillRequestDto
{
    [Required]
    public List<FulfillLineItemDto> LineItems { get; set; } = new();
}

public class FulfillLineItemDto
{
    public int LineItemId { get; set; }

    [Range(0, int.MaxValue)]
    public int FulfilledQuantity { get; set; }
}
