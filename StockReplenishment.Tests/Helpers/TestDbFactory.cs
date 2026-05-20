using Microsoft.EntityFrameworkCore;
using StockReplenishment.Api.Data;
using StockReplenishment.Core.Enums;
using StockReplenishment.Core.Models;

namespace StockReplenishment.Tests.Helpers;

public static class TestDbFactory
{
    // Each test gets its own isolated in-memory database
    public static AppDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        Seed(context);
        return context;
    }

    private static void Seed(AppDbContext context)
    {
        var location = new StockLocation
        {
            Id          = 1,
            Code        = "LINE-A1",
            Description = "Assembly Line A, Station 1"
        };
        context.StockLocations.Add(location);

        context.Requests.AddRange(
            new ReplenishmentRequest
            {
                Id            = 1,
                RequestNumber = "REQ-0001",
                Status        = RequestStatus.Draft,
                Priority      = RequestPriority.Normal,
                StockLocationId = 1,
                CreatedBy     = "worker.test",
                CreatedAt     = DateTime.UtcNow,
                LineItems     = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-001", Description = "Test Bolt", RequestedQuantity = 100 }
                }
            },
            new ReplenishmentRequest
            {
                Id              = 2,
                RequestNumber   = "REQ-0002",
                Status          = RequestStatus.Submitted,
                Priority        = RequestPriority.Urgent,
                StockLocationId = 1,
                CreatedBy       = "worker.test",
                CreatedAt       = DateTime.UtcNow,
                SubmittedAt     = DateTime.UtcNow,
                StockCheckPassed = true,
                StockCheckMessage = "All items available.",
                LineItems       = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-002", Description = "Test Nut", RequestedQuantity = 50 }
                }
            },
            new ReplenishmentRequest
            {
                Id              = 3,
                RequestNumber   = "REQ-0003",
                Status          = RequestStatus.Approved,
                Priority        = RequestPriority.Low,
                StockLocationId = 1,
                CreatedBy       = "worker.test",
                CreatedAt       = DateTime.UtcNow,
                SubmittedAt     = DateTime.UtcNow,
                ReviewedAt      = DateTime.UtcNow,
                ReviewedBy      = "supervisor.test",
                StockCheckPassed = true,
                LineItems       = new List<RequestLineItem>
                {
                    new() { Id = 10, ArticleNumber = "ART-003", Description = "Test Washer", RequestedQuantity = 200 }
                }
            }
        );

        context.SaveChanges();
    }
}
