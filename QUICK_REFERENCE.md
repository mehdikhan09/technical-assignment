# Quick Reference - Stock Replenishment APIs & Pages

## 🔌 API Endpoints Summary

### **Base URL**: `http://localhost:5035/api`

| Method | Endpoint | Purpose | Key Features |
|--------|----------|---------|--------------|
| `GET` | `/requests` | Browse & filter requests | Pagination, status/priority/location filters |
| `GET` | `/requests/{id}` | Get request details | Full request with line items |
| `POST` | `/requests` | Create draft request | Validates line items, generates REQ number |
| `POST` | `/requests/{id}/submit` | Submit for approval | **Async stock check (3-8s, non-blocking)** |
| `POST` | `/requests/{id}/approve` | Approve request | Only Submitted requests |
| `POST` | `/requests/{id}/reject` | Reject with reason | **Reason required** |
| `POST` | `/requests/{id}/fulfill` | Fulfill request | Fulfilled quantities per item |
| `GET` | `/locations` | Get stock locations | For dropdowns |

---

## 🎨 Blazor Pages Summary

### **Base URL**: `http://localhost:5165`

| Route | Page | Purpose | Key Features |
|-------|------|---------|--------------|
| `/` | Home | Landing page | Quick actions |
| `/dashboard` | Dashboard | Analytics overview | Stats, charts, recent requests |
| `/requests` | Request List | Browse & filter | Pagination, status/priority/location filters |
| `/requests/create` | Create Request | New draft request | Dynamic line items, validation |
| `/requests/{id}` | Request Detail | View & action | **Real-time stock check polling (3s)**, status-dependent actions |

---

## 🔄 Workflow Quick Reference

```
┌─────────┐
│  DRAFT  │  ← Create: POST /api/requests
└────┬────┘    Page: /requests/create
	 │
	 │ Submit: POST /api/requests/{id}/submit
	 │ (Triggers async stock check 3-8 seconds)
	 ▼
┌───────────┐
│ SUBMITTED │  ← Stock check runs in background
└─────┬─────┘    Page auto-polls every 3 seconds
	  │
	  ├──→ Approve: POST /api/requests/{id}/approve
	  │    (If stock check passed)
	  │    ┌──────────┐
	  └───→│ APPROVED │
		   └────┬─────┘
				│
				│ Fulfill: POST /api/requests/{id}/fulfill
				▼ (Enter quantities per item)
		   ┌───────────┐
		   │ FULFILLED │
		   └───────────┘

	  Or Reject: POST /api/requests/{id}/reject
				 (Reason required)
		   ┌──────────┐
		   │ REJECTED │
		   └──────────┘
```

---

## 📊 Status Codes

| Status | Color | Meaning | Next Action |
|--------|-------|---------|-------------|
| 🟦 Draft | Gray | Incomplete | Submit |
| 🟦 Submitted | Blue | Awaiting approval | Approve/Reject |
| 🟩 Approved | Green | Ready to fulfill | Fulfill |
| 🟥 Rejected | Red | Denied (terminal) | None |
| 🟪 Fulfilled | Purple | Complete (terminal) | None |

---

## 🎯 Priority Levels

| Priority | Color | Use Case |
|----------|-------|----------|
| 🟢 Low | Green | Regular restocking |
| 🟠 Normal | Orange | Standard requests |
| 🔴 Urgent | Red | Production line stopped |

---

## 📝 Sample API Calls

### Create Draft Request
```bash
curl -X POST http://localhost:5035/api/requests \
  -H "Content-Type: application/json" \
  -d '{
	"priority": "Normal",
	"stockLocationId": 1,
	"createdBy": "worker.john",
	"notes": "Running low on bolts",
	"lineItems": [
	  {
		"articleNumber": "ART-10045",
		"description": "M8 Hex Bolt",
		"requestedQuantity": 500
	  }
	]
  }'
```

### Submit Request (Triggers Stock Check)
```bash
curl -X POST http://localhost:5035/api/requests/1/submit
# Response: { "message": "Stock check running in background" }
# Stock check completes in 3-8 seconds asynchronously
```

### Browse with Filters
```bash
curl "http://localhost:5035/api/requests?status=Submitted&priority=Urgent&page=1&pageSize=10"
```

### Approve Request
```bash
curl -X POST "http://localhost:5035/api/requests/1/approve?reviewedBy=supervisor.mike"
```

### Reject Request (Reason Required)
```bash
curl -X POST "http://localhost:5035/api/requests/1/reject?reviewedBy=supervisor.mike" \
  -H "Content-Type: application/json" \
  -d '{"reason": "Insufficient budget this quarter"}'
```

### Fulfill Request
```bash
curl -X POST http://localhost:5035/api/requests/1/fulfill \
  -H "Content-Type: application/json" \
  -d '{
	"lineItems": [
	  {"lineItemId": 1, "fulfilledQuantity": 500},
	  {"lineItemId": 2, "fulfilledQuantity": 450}
	]
  }'
```

---

## 🔑 Key Features

### 1. **Non-Blocking Stock Check** ⚡
- Submit API returns immediately (<100ms)
- Stock check runs 3-8 seconds in background
- No degradation of user experience

### 2. **Real-Time Updates** 🔄
- Request Detail page polls every 3 seconds
- Automatically shows stock check result
- No manual refresh needed

### 3. **Required Rejection Reason** ✅
- `[Required]` validation on DTO
- Client-side and server-side validation
- Cannot reject without reason

### 4. **Comprehensive Filtering** 🔍
- Status: Draft, Submitted, Approved, Rejected, Fulfilled
- Priority: Low, Normal, Urgent
- Location: All 10 stock locations
- Pagination: Configurable page size

### 5. **Validation** 🛡️
- Status transition rules enforced
- At least 1 line item required
- Quantities must be >= 1
- Rejection reason required
- Proper error messages

---

## 🧪 Test Data Available

**15 Pre-Seeded Requests**:
- 3 Draft (REQ-0001, REQ-0007, REQ-0010)
- 4 Submitted (REQ-0002, REQ-0003, REQ-0008, REQ-0011)
- 3 Approved (REQ-0004, REQ-0009, REQ-0014)
- 2 Rejected (REQ-0005, REQ-0013)
- 3 Fulfilled (REQ-0006, REQ-0012, REQ-0015)

**10 Stock Locations**:
- LINE-A1, LINE-A2, LINE-B1, LINE-C1, LINE-C2
- WELD-01, WELD-02, PAINT-01, QC-01, PACK-01

---

## 📱 Navigation Tips

### From Dashboard:
- Click stat card → Filtered request list
- Click recent request → Request detail
- Quick actions → Direct navigation

### From Request List:
- Click row → Request detail
- Filter dropdowns → Instant filtering
- Pagination → Page through results

### From Request Detail:
- Status-dependent action buttons
- Real-time stock check updates
- Back to list navigation

---

## 🚀 Quick Start

1. **Start API**: `cd StockReplenishment.Api && dotnet run`
2. **Start Web**: `cd StockReplenishment.Web && dotnet run`
3. **Open Browser**: http://localhost:5165
4. **Try Workflow**:
   - Open REQ-0001 (Draft)
   - Click "Submit for Approval"
   - Watch stock check update in 3-8 seconds
   - Approve or reject

---

## 📞 Status Endpoints

Check if system is running:
```bash
curl http://localhost:5035/api/locations
curl http://localhost:5165
```

---

**Quick Reference Version**: 1.0  
**Last Updated**: 2025
