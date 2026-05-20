using StockReplenishment.Core.Enums;
using StockReplenishment.Core.Models;

namespace StockReplenishment.Api.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        // Avoid re-seeding if data already exists
        if (context.StockLocations.Any()) return;

        // ── Priorities ─────────────────────────────────────────
        var priorities = new List<Priority>
        {
            new() { Id = 1, Name = "Low", Description = "Low priority - routine restocking", DisplayOrder = 1, IsActive = true },
            new() { Id = 2, Name = "Normal", Description = "Normal priority - standard request", DisplayOrder = 2, IsActive = true },
            new() { Id = 3, Name = "Urgent", Description = "Urgent priority - immediate attention needed", DisplayOrder = 3, IsActive = true }
        };
        context.Priorities.AddRange(priorities);
        context.SaveChanges();

        // ── Locations ──────────────────────────────────────────
        var locations = new List<StockLocation>
        {
            new() { Id = 1, Code = "LINE-A1", Description = "Assembly Line A, Station 1" },
            new() { Id = 2, Code = "LINE-A2", Description = "Assembly Line A, Station 2" },
            new() { Id = 3, Code = "LINE-B1", Description = "Assembly Line B, Station 1" },
            new() { Id = 4, Code = "WELD-01", Description = "Welding Bay 01" },
            new() { Id = 5, Code = "PAINT-01", Description = "Paint Shop, Station 1" },
            new() { Id = 6, Code = "LINE-C1", Description = "Assembly Line C, Station 1" },
            new() { Id = 7, Code = "LINE-C2", Description = "Assembly Line C, Station 2" },
            new() { Id = 8, Code = "WELD-02", Description = "Welding Bay 02" },
            new() { Id = 9, Code = "QC-01", Description = "Quality Control Station 1" },
            new() { Id = 10, Code = "PACK-01", Description = "Packaging Area 1" }
        };
        context.StockLocations.AddRange(locations);
        context.SaveChanges();

        // ── Requests ───────────────────────────────────────────
        var requests = new List<ReplenishmentRequest>
        {
            // 1. Draft request
            new()
            {
                Id = 1,
                RequestNumber = "REQ-0001",
                Status = RequestStatus.Draft,
                Priority = RequestPriority.Normal,
                StockLocationId = 1,
                CreatedBy = "worker.anna",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                Notes = "Running low on bolts at station 1",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-10045", Description = "M8 Hex Bolt",         RequestedQuantity = 500 },
                    new() { ArticleNumber = "ART-10046", Description = "M8 Hex Nut",           RequestedQuantity = 500 }
                }
            },

            // 2. Submitted — stock check pending
            new()
            {
                Id = 2,
                RequestNumber = "REQ-0002",
                Status = RequestStatus.Submitted,
                Priority = RequestPriority.Urgent,
                StockLocationId = 2,
                CreatedBy = "worker.ben",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                SubmittedAt = DateTime.UtcNow.AddHours(-3),
                StockCheckPassed = null,   // still processing
                StockCheckMessage = "Stock check in progress...",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-20011", Description = "Bearing Assembly 6204", RequestedQuantity = 20 }
                }
            },

            // 3. Submitted — stock check passed
            new()
            {
                Id = 3,
                RequestNumber = "REQ-0003",
                Status = RequestStatus.Submitted,
                Priority = RequestPriority.Normal,
                StockLocationId = 3,
                CreatedBy = "worker.chen",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                SubmittedAt = DateTime.UtcNow.AddHours(-2),
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-30055", Description = "Welding Wire 1.2mm",   RequestedQuantity = 10 },
                    new() { ArticleNumber = "ART-30056", Description = "Welding Shield Gas",   RequestedQuantity = 5  }
                }
            },

            // 4. Approved
            new()
            {
                Id = 4,
                RequestNumber = "REQ-0004",
                Status = RequestStatus.Approved,
                Priority = RequestPriority.Low,
                StockLocationId = 4,
                CreatedBy = "worker.diana",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                SubmittedAt = DateTime.UtcNow.AddDays(-2),
                ReviewedAt = DateTime.UtcNow.AddDays(-1),
                ReviewedBy = "supervisor.mike",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-40021", Description = "Safety Gloves (L)",    RequestedQuantity = 50 },
                    new() { ArticleNumber = "ART-40022", Description = "Safety Goggles",       RequestedQuantity = 20 }
                }
            },

            // 5. Rejected
            new()
            {
                Id = 5,
                RequestNumber = "REQ-0005",
                Status = RequestStatus.Rejected,
                Priority = RequestPriority.Normal,
                StockLocationId = 1,
                CreatedBy = "worker.anna",
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                SubmittedAt = DateTime.UtcNow.AddDays(-3),
                ReviewedAt = DateTime.UtcNow.AddDays(-2),
                ReviewedBy = "supervisor.mike",
                RejectionReason = "Duplicate request — REQ-0001 already covers these materials.",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-10045", Description = "M8 Hex Bolt",         RequestedQuantity = 200 }
                }
            },

            // 6. Fulfilled
            new()
            {
                Id = 6,
                RequestNumber = "REQ-0006",
                Status = RequestStatus.Fulfilled,
                Priority = RequestPriority.Urgent,
                StockLocationId = 5,
                CreatedBy = "worker.erik",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                SubmittedAt = DateTime.UtcNow.AddDays(-4),
                ReviewedAt = DateTime.UtcNow.AddDays(-3),
                FulfilledAt = DateTime.UtcNow.AddDays(-1),
                ReviewedBy = "supervisor.sarah",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-50033", Description = "Primer Coat Paint 5L", RequestedQuantity = 8,  FulfilledQuantity = 8  },
                    new() { ArticleNumber = "ART-50034", Description = "Top Coat Paint 5L",    RequestedQuantity = 6,  FulfilledQuantity = 5  }
                }
            },

            // 7. Draft - Large order with multiple items
            new()
            {
                Id = 7,
                RequestNumber = "REQ-0007",
                Status = RequestStatus.Draft,
                Priority = RequestPriority.Urgent,
                StockLocationId = 6,
                CreatedBy = "worker.frank",
                CreatedAt = DateTime.UtcNow.AddHours(-6),
                Notes = "Urgent restocking needed for new production batch",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-60001", Description = "Aluminum Sheet 2mm",     RequestedQuantity = 100 },
                    new() { ArticleNumber = "ART-60002", Description = "Steel Rod 10mm",         RequestedQuantity = 200 },
                    new() { ArticleNumber = "ART-60003", Description = "Rubber Gasket O-Ring",   RequestedQuantity = 1000 },
                    new() { ArticleNumber = "ART-60004", Description = "Copper Wire 2.5mm",      RequestedQuantity = 50 }
                }
            },

            // 8. Submitted - Stock check failed
            new()
            {
                Id = 8,
                RequestNumber = "REQ-0008",
                Status = RequestStatus.Submitted,
                Priority = RequestPriority.Urgent,
                StockLocationId = 7,
                CreatedBy = "worker.grace",
                CreatedAt = DateTime.UtcNow.AddHours(-12),
                SubmittedAt = DateTime.UtcNow.AddHours(-10),
                StockCheckPassed = false,
                StockCheckMessage = "Insufficient stock for article ART-70005 (Hydraulic Pump HP-300). Requested: 15.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-70005", Description = "Hydraulic Pump HP-300",  RequestedQuantity = 15 },
                    new() { ArticleNumber = "ART-70006", Description = "Hydraulic Hose 10m",     RequestedQuantity = 20 }
                }
            },

            // 9. Approved - Ready to fulfill
            new()
            {
                Id = 9,
                RequestNumber = "REQ-0009",
                Status = RequestStatus.Approved,
                Priority = RequestPriority.Normal,
                StockLocationId = 8,
                CreatedBy = "worker.henry",
                CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(-5),
                SubmittedAt = DateTime.UtcNow.AddDays(-2),
                ReviewedAt = DateTime.UtcNow.AddDays(-1).AddHours(-6),
                ReviewedBy = "supervisor.lisa",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-80011", Description = "Welding Electrode 3.2mm", RequestedQuantity = 100 },
                    new() { ArticleNumber = "ART-80012", Description = "Grinding Disc 125mm",     RequestedQuantity = 50 },
                    new() { ArticleNumber = "ART-80013", Description = "Cutting Disc 125mm",      RequestedQuantity = 40 }
                }
            },

            // 10. Draft - Small urgent request
            new()
            {
                Id = 10,
                RequestNumber = "REQ-0010",
                Status = RequestStatus.Draft,
                Priority = RequestPriority.Low,
                StockLocationId = 9,
                CreatedBy = "worker.iris",
                CreatedAt = DateTime.UtcNow.AddHours(-3),
                Notes = "Regular monthly refill",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-90021", Description = "Calibration Gauge Set",  RequestedQuantity = 2 }
                }
            },

            // 11. Submitted - Just submitted, stock check passed
            new()
            {
                Id = 11,
                RequestNumber = "REQ-0011",
                Status = RequestStatus.Submitted,
                Priority = RequestPriority.Normal,
                StockLocationId = 10,
                CreatedBy = "worker.jack",
                CreatedAt = DateTime.UtcNow.AddHours(-5),
                SubmittedAt = DateTime.UtcNow.AddHours(-1),
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-10001", Description = "Cardboard Box Large",    RequestedQuantity = 500 },
                    new() { ArticleNumber = "ART-10002", Description = "Bubble Wrap Roll",       RequestedQuantity = 20 },
                    new() { ArticleNumber = "ART-10003", Description = "Packing Tape 50mm",      RequestedQuantity = 100 }
                }
            },

            // 12. Fulfilled - Recently completed
            new()
            {
                Id = 12,
                RequestNumber = "REQ-0012",
                Status = RequestStatus.Fulfilled,
                Priority = RequestPriority.Normal,
                StockLocationId = 1,
                CreatedBy = "worker.karen",
                CreatedAt = DateTime.UtcNow.AddDays(-6),
                SubmittedAt = DateTime.UtcNow.AddDays(-5),
                ReviewedAt = DateTime.UtcNow.AddDays(-4),
                FulfilledAt = DateTime.UtcNow.AddHours(-12),
                ReviewedBy = "supervisor.mike",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-10047", Description = "M10 Hex Bolt",          RequestedQuantity = 300, FulfilledQuantity = 300 },
                    new() { ArticleNumber = "ART-10048", Description = "M10 Washer",            RequestedQuantity = 300, FulfilledQuantity = 300 },
                    new() { ArticleNumber = "ART-10049", Description = "M10 Lock Nut",          RequestedQuantity = 300, FulfilledQuantity = 280 }
                }
            },

            // 13. Rejected - Budget constraints
            new()
            {
                Id = 13,
                RequestNumber = "REQ-0013",
                Status = RequestStatus.Rejected,
                Priority = RequestPriority.Low,
                StockLocationId = 3,
                CreatedBy = "worker.leo",
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                SubmittedAt = DateTime.UtcNow.AddDays(-6),
                ReviewedAt = DateTime.UtcNow.AddDays(-5),
                ReviewedBy = "supervisor.sarah",
                RejectionReason = "Request exceeds monthly budget allocation. Please resubmit next month.",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-30099", Description = "TIG Welding Torch",      RequestedQuantity = 5 }
                }
            },

            // 14. Approved - High priority waiting fulfillment
            new()
            {
                Id = 14,
                RequestNumber = "REQ-0014",
                Status = RequestStatus.Approved,
                Priority = RequestPriority.Urgent,
                StockLocationId = 2,
                CreatedBy = "worker.maria",
                CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-8),
                SubmittedAt = DateTime.UtcNow.AddDays(-1).AddHours(-6),
                ReviewedAt = DateTime.UtcNow.AddHours(-2),
                ReviewedBy = "supervisor.lisa",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-20055", Description = "Precision Bearing 6205",  RequestedQuantity = 30 },
                    new() { ArticleNumber = "ART-20056", Description = "Bearing Grease Tube",     RequestedQuantity = 10 }
                }
            },

            // 15. Fulfilled - Complete with all quantities
            new()
            {
                Id = 15,
                RequestNumber = "REQ-0015",
                Status = RequestStatus.Fulfilled,
                Priority = RequestPriority.Normal,
                StockLocationId = 4,
                CreatedBy = "worker.nancy",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                SubmittedAt = DateTime.UtcNow.AddDays(-9),
                ReviewedAt = DateTime.UtcNow.AddDays(-8),
                FulfilledAt = DateTime.UtcNow.AddDays(-7),
                ReviewedBy = "supervisor.mike",
                StockCheckPassed = true,
                StockCheckMessage = "All items available in warehouse stock.",
                LineItems = new List<RequestLineItem>
                {
                    new() { ArticleNumber = "ART-40055", Description = "Work Gloves XL",         RequestedQuantity = 100, FulfilledQuantity = 100 },
                    new() { ArticleNumber = "ART-40056", Description = "Hard Hat Yellow",        RequestedQuantity = 50,  FulfilledQuantity = 50 },
                    new() { ArticleNumber = "ART-40057", Description = "Safety Vest Orange",     RequestedQuantity = 50,  FulfilledQuantity = 50 },
                    new() { ArticleNumber = "ART-40058", Description = "Ear Plugs Box/100",      RequestedQuantity = 20,  FulfilledQuantity = 20 }
                }
            }
        };

        context.Requests.AddRange(requests);
        context.SaveChanges();
    }
}
