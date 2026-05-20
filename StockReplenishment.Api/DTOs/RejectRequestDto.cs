using System.ComponentModel.DataAnnotations;

namespace StockReplenishment.Api.DTOs;

public class RejectRequestDto
{
    [Required(ErrorMessage = "A rejection reason is required.")]
    public string Reason { get; set; } = string.Empty;
}
