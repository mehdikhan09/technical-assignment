# Stock Replenishment Request System

## Running the Application

### Option 1: Visual Studio (Recommended)

1. **Open the solution** in Visual Studio 2026
2. **Right-click the Solution** in Solution Explorer → **Properties**
3. **Select "Configure Startup Projects"** → **Multiple Startup Projects**
4. **Set both projects to "Start":**
   - `StockReplenishment.Api` → **Start**
   - `StockReplenishment.Web` → **Start**
5. **Press F5** to run both projects

### Option 2: Command Line

Open two terminal windows:

**Terminal 1 - API:**
```powershell
cd StockReplenishment.Api
dotnet run
```

**Terminal 2 - Web:**
```powershell
cd StockReplenishment.Web
dotnet run
```

## Application URLs

- **API (Swagger):** https://localhost:7101/swagger
- **Web UI:** https://localhost:5001 (or check terminal output)

## Features

### Pages

| Page | URL | Description |
|------|-----|-------------|
| Home | `/` | Landing page with quick links |
| Request List | `/requests` | Browse all requests with filters and pagination |
| Create Request | `/requests/create` | Create new draft request with line items |
| Request Detail | `/requests/{id}` | View details, perform actions, watch stock check live |

### Workflow

1. **Worker creates draft** → `/requests/create`
2. **Worker submits** → Triggers background stock check (3-8 sec)
3. **UI polls** every 2 seconds until stock check completes
4. **Supervisor reviews** → Approve or Reject
5. **Warehouse fulfills** → Mark quantities fulfilled

### Stock Check Polling

The most interesting feature! When a request is submitted:
- ✅ API responds immediately (non-blocking)
- ⏳ Background service runs stock check (simulated 3-8 seconds)
- 🔄 UI automatically polls every 2 seconds
- ✅/❌ Result appears live without page refresh

## Seed Data

The API includes 6 pre-seeded requests demonstrating all statuses:
- **REQ-0001:** Draft (ready to submit)
- **REQ-0002:** Submitted (stock check pending)
- **REQ-0003:** Submitted (stock check passed)
- **REQ-0004:** Approved (ready to fulfill)
- **REQ-0005:** Rejected (with reason)
- **REQ-0006:** Fulfilled (partial fulfillment example)

## Technology Stack

- **.NET 10** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful backend
- **Blazor Server** - Interactive UI with MudBlazor
- **Entity Framework Core** - In-memory database
- **MudBlazor** - Material Design component library
- **IServiceScopeFactory** - Background task pattern

## Architecture Highlights

### Background Stock Check Service
```
Worker submits → API returns 200 immediately ← User NOT blocked
						↓
			Background task starts (3-8 sec)
						↓
		Result saved → UI polls → Updates live
```

### Key Patterns
- ✅ Fire-and-forget background processing
- ✅ IServiceScopeFactory for scoped DbContext in background tasks
- ✅ Client-side polling with cancellation token
- ✅ Three-state logic (null/true/false)
- ✅ Structured logging
- ✅ Graceful error handling

## API Endpoints

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/requests` | List with filters + pagination |
| GET | `/api/requests/{id}` | Get single request |
| POST | `/api/requests` | Create draft |
| POST | `/api/requests/{id}/submit` | Submit for approval |
| POST | `/api/requests/{id}/approve` | Approve request |
| POST | `/api/requests/{id}/reject` | Reject with reason |
| POST | `/api/requests/{id}/fulfill` | Mark as fulfilled |
| GET | `/api/stocklocations` | List locations |

## Development Notes

### API Configuration
- Port: **7101** (HTTPS), **5035** (HTTP)
- Swagger UI available at: `/swagger`
- In-memory database (resets on restart)

### Web Configuration
- Configured to call API at `https://localhost:7101/`
- Update `appsettings.json` → `ApiBaseUrl` if API port changes
- MudBlazor theme and components pre-configured

## Troubleshooting

**API not starting?**
- Check port 7101 is not in use
- Verify `launchSettings.json` in API project

**Web can't connect to API?**
- Ensure API is running first
- Verify `ApiBaseUrl` in `appsettings.json` matches API port
- Check browser console for CORS errors (shouldn't happen with same-origin)

**Stock check not updating?**
- Check browser console for errors
- Verify API logs show "Stock check started/completed"
- Polling automatically stops after result is received
