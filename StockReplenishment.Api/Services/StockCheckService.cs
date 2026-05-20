using Microsoft.EntityFrameworkCore;
using StockReplenishment.Api.Data;

namespace StockReplenishment.Api.Services;

public class StockCheckService : IStockCheckService
{
    // We use IServiceScopeFactory because DbContext is scoped
    // but this service runs outside the request lifecycle
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockCheckService> _logger;

    public StockCheckService(IServiceScopeFactory scopeFactory,
                             ILogger<StockCheckService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    public async Task RunCheckAsync(int requestId)
    {
        _logger.LogInformation(
            "Stock check started for Request {RequestId}", requestId);

        try
        {
            // ── Simulate slow external service (3–8 seconds) ──
            var delay = Random.Shared.Next(3000, 8000);
            await Task.Delay(delay);

            // ── Simulate check result (80% pass rate) ──
            var passed = Random.Shared.NextDouble() > 0.2;

            // ── Persist result using a fresh scope ──
            using var scope   = _scopeFactory.CreateScope();
            var db            = scope.ServiceProvider
                                     .GetRequiredService<AppDbContext>();

            var request = await db.Requests
                .Include(r => r.LineItems)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request is null)
            {
                _logger.LogWarning(
                    "Stock check: Request {RequestId} not found.", requestId);
                return;
            }

            if (passed)
            {
                request.StockCheckPassed  = true;
                request.StockCheckMessage =
                    "All items available in warehouse stock.";
            }
            else
            {
                // Pick a random line item to flag as unavailable
                var flagged = request.LineItems
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefault();

                request.StockCheckPassed  = false;
                request.StockCheckMessage = flagged is not null
                    ? $"Insufficient stock for article {flagged.ArticleNumber} " +
                      $"({flagged.Description}). Requested: {flagged.RequestedQuantity}."
                    : "One or more items are unavailable in warehouse stock.";
            }

            await db.SaveChangesAsync();

            _logger.LogInformation(
                "Stock check completed for Request {RequestId}. Passed: {Passed}",
                requestId, passed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Stock check failed unexpectedly for Request {RequestId}", requestId);

            // Save a failure state so the client isn't stuck polling forever
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db          = scope.ServiceProvider
                                       .GetRequiredService<AppDbContext>();

                var request = await db.Requests.FindAsync(requestId);
                if (request is not null)
                {
                    request.StockCheckPassed  = false;
                    request.StockCheckMessage =
                        "Stock check failed due to an internal error. Please retry.";
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception innerEx)
            {
                _logger.LogError(innerEx,
                    "Failed to save error state for Request {RequestId}", requestId);
            }
        }
    }
}
