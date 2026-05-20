namespace StockReplenishment.Api.DTOs;

public record PriorityDto(
    int Id,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive
);
