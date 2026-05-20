using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockReplenishment.Api.Data;

namespace StockReplenishment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockLocationsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<StockLocationsController> _logger;

    public StockLocationsController(AppDbContext db, ILogger<StockLocationsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // GET api/stocklocations
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var locations = await _db.StockLocations
                .OrderBy(l => l.Code)
                .Select(l => new { l.Id, l.Code, l.Description })
                .ToListAsync();

            return Ok(locations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving stock locations");
            throw;
        }
    }

    // GET api/stocklocations/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid location ID." });

            var location = await _db.StockLocations
                .Where(l => l.Id == id)
                .Select(l => new { l.Id, l.Code, l.Description })
                .FirstOrDefaultAsync();

            if (location is null)
            {
                _logger.LogWarning("Stock location {LocationId} not found", id);
                return NotFound(new { message = $"Location {id} not found." });
            }

            return Ok(location);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving stock location {LocationId}", id);
            throw;
        }
    }
}
