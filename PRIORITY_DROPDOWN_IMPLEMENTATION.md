# Priority Dropdown Implementation

## Overview
Implemented a complete database-driven priority dropdown for the "New Replenishment Request" page, replacing the hardcoded values with data from the database.

---

## What Was Created

### 1. **Database Model** - `StockReplenishment.Core/Models/Priority.cs`
```csharp
public class Priority
{
	public int Id { get; set; }
	public string Name { get; set; }              // "Low", "Normal", "Urgent"
	public string? Description { get; set; }      // Detailed description
	public int DisplayOrder { get; set; }         // Controls sort order
	public bool IsActive { get; set; }            // Enable/disable priorities
}
```

### 2. **Database Context** - Updated `AppDbContext.cs`
- Added `DbSet<Priority> Priorities`
- Added entity configuration with constraints:
  - Name: Required, max 50 characters
  - Description: Optional, max 200 characters

### 3. **Seed Data** - Updated `SeedData.cs`
Added 3 default priorities:
- **Low** - "Low priority - routine restocking" (DisplayOrder: 1)
- **Normal** - "Normal priority - standard request" (DisplayOrder: 2)
- **Urgent** - "Urgent priority - immediate attention needed" (DisplayOrder: 3)

### 4. **API Controller** - `StockReplenishment.Api/Controllers/PrioritiesController.cs`

#### Endpoints:
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/priorities` | Get all active priorities (ordered by DisplayOrder) |
| GET | `/api/priorities/{id}` | Get specific priority by ID |

**Example Response:**
```json
[
  {
	"id": 1,
	"name": "Low",
	"description": "Low priority - routine restocking",
	"displayOrder": 1,
	"isActive": true
  },
  {
	"id": 2,
	"name": "Normal",
	"description": "Normal priority - standard request",
	"displayOrder": 2,
	"isActive": true
  },
  {
	"id": 3,
	"name": "Urgent",
	"description": "Urgent priority - immediate attention needed",
	"displayOrder": 3,
	"isActive": true
  }
]
```

### 5. **DTO** - `StockReplenishment.Api/DTOs/PriorityDto.cs`
```csharp
public record PriorityDto(
	int Id,
	string Name,
	string? Description,
	int DisplayOrder,
	bool IsActive
);
```

### 6. **Web Service** - Updated `ApiService.cs`
Added:
- `PriorityDto` record
- `GetPrioritiesAsync()` method

### 7. **UI Component** - Updated `CreateRequest.razor`

**Before (Hardcoded):**
```razor
<MudSelect T="string" Label="Priority" @bind-Value="_model.Priority">
	<MudSelectItem Value="@("Low")">Low</MudSelectItem>
	<MudSelectItem Value="@("Normal")">Normal</MudSelectItem>
	<MudSelectItem Value="@("Urgent")">Urgent</MudSelectItem>
</MudSelect>
```

**After (Database-Driven):**
```razor
<MudSelect T="string" Label="Priority" @bind-Value="_model.Priority" Required="true">
	@foreach (var p in _priorities)
	{
		<MudSelectItem Value="@p.Name">@p.Name</MudSelectItem>
	}
</MudSelect>
```

**Code-Behind Changes:**
```csharp
private List<PriorityDto> _priorities = new();

protected override async Task OnInitializedAsync()
{
	_locations = await Api.GetLocationsAsync();
	_priorities = await Api.GetPrioritiesAsync();  // NEW

	if (_locations.Any())
		_model.StockLocationId = _locations.First().Id;

	if (_priorities.Any())
		_model.Priority = _priorities.First(p => p.Name == "Normal").Name;  // NEW
}
```

---

## Benefits of This Implementation

✅ **Dynamic** - Priorities can be added/modified in the database without code changes  
✅ **Maintainable** - Centralized priority management  
✅ **Extensible** - Easy to add new fields (color, icon, etc.)  
✅ **Filterable** - `IsActive` flag allows hiding priorities without deletion  
✅ **Ordered** - `DisplayOrder` controls presentation sequence  
✅ **Consistent** - Same API pattern as locations  

---

## Testing

### 1. Test API Endpoint
```powershell
Invoke-RestMethod -Uri "http://localhost:5035/api/priorities" -Method Get
```

**Expected Result:**
```json
[
  { "id": 1, "name": "Low", "description": "...", "displayOrder": 1, "isActive": true },
  { "id": 2, "name": "Normal", "description": "...", "displayOrder": 2, "isActive": true },
  { "id": 3, "name": "Urgent", "description": "...", "displayOrder": 3, "isActive": true }
]
```

### 2. Test UI
1. Navigate to http://localhost:5165/requests/create
2. Click the Priority dropdown
3. Verify all 3 priorities appear: Low, Normal, Urgent
4. Select each priority
5. Verify "Normal" is pre-selected by default

---

## Database Schema

```
┌─────────────────┐
│    Priority     │
├─────────────────┤
│ Id (PK)         │
│ Name            │
│ Description     │
│ DisplayOrder    │
│ IsActive        │
└─────────────────┘
```

---

## Future Enhancements

- Add color coding for priorities (e.g., Red for Urgent)
- Add icons for visual distinction
- Admin UI to manage priorities
- Soft-delete functionality
- Audit trail for priority changes
- Multi-language support for descriptions

---

## Files Modified

1. ✅ `StockReplenishment.Core/Models/Priority.cs` (new)
2. ✅ `StockReplenishment.Api/Data/AppDbContext.cs` (updated)
3. ✅ `StockReplenishment.Api/Data/SeedData.cs` (updated)
4. ✅ `StockReplenishment.Api/Controllers/PrioritiesController.cs` (new)
5. ✅ `StockReplenishment.Api/DTOs/PriorityDto.cs` (new)
6. ✅ `StockReplenishment.Web/Services/ApiService.cs` (updated)
7. ✅ `StockReplenishment.Web/Components/Pages/CreateRequest.razor` (updated)

---

## Status

✅ **Implementation Complete**  
✅ **API Running** - http://localhost:5035  
✅ **Web Running** - http://localhost:5165  
✅ **Database Seeded** - 3 priorities loaded  
✅ **Dropdown Working** - Dynamic priority selection functional
