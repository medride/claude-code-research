---
name: api-design
description: Design resource-oriented APIs for this codebase following "API Design Patterns" (JJ Geewax), mapped to existing architecture patterns
---

# API Design for TimefoldInt

Guide based on **"API Design Patterns" (JJ Geewax)** and **Google AIPs**, mapped to this repo's Clean Architecture.

- Full chapter summaries: `TimefoldInt.API/api-design-patterns-summary.md`
- **Principle**: Design the API surface first (resources, methods, responses), then implement through existing architecture
- Canonical example: the Vehicle implementation

---

## 1. Resource-Oriented Design (Ch. 2-4, AIP-121)

- APIs expose **resources (nouns)**, not actions (verbs)
- Collections use **plural nouns**: `/api/v1/vehicles`, `/api/v1/tenants`
- Resource hierarchy forms a **DAG** — each resource has one canonical parent
- Hierarchy reflects real relationships: `/api/v1/tenants/{id}/vehicles` (if sub-resource)
- Maximum depth: 3-4 levels
- **API should NOT mirror database schema** — expose business domain resources, not tables
- Route template: `api/v{version:apiVersion}/[controller]` via `BaseApiController`

## 2. Naming (Ch. 3, AIP-122, AIP-190)

| Element | Convention | Example |
|---------|-----------|---------|
| Collections | Plural nouns, camelCase | `vehicles`, `tenants` |
| JSON fields | camelCase, include units | `sizeBytes`, `durationMs` (not `size`, `duration`) |
| Enums | PascalCase strings | `Active`, `OnLeave`, `InMaintenance` (via `JsonStringEnumConverter`) |
| Controllers | PascalCase plural | `VehiclesController`, `TenantsController` |
| Query params | camelCase | `pageNumber`, `pageSize`, `sortField`, `sortOrder` |

**Rules**:
- Use **American English** spelling
- Avoid vague words: `instance`, `info`, `service`, `object`, `resource`
- Prepositions are noise — prefer `vehicleCount` over `countOfVehicles`
- Boolean fields: use `is`/`has` prefix — `isActive`, `hasCapacity`

## 3. Resource Identification (Ch. 6, AIP-122)

**Never expose internal database IDs.**

| Field | Purpose | Visibility |
|-------|---------|------------|
| `long Id` | Internal database PK | Never in API responses |
| `Guid ExternalId` | Public-facing identifier | Exposed as `id` in DTOs via AutoMapper |

- System-generated IDs should have documented format (this repo: UUID v4)
- Geewax recommends a `name` field (full resource path, e.g., `tenants/abc/vehicles/xyz`) — consider for future enhancement
- Reference: `BaseEntity.cs`, `VehicleProfile.cs`

## 4. Standard Methods (Ch. 7, AIP-131 through AIP-135)

| Method | HTTP | Key Rules | MediatR Flow |
|--------|------|-----------|--------------|
| List | `GET /resources` | Must support pagination, filtering, sorting. Response = collection + page metadata | Query params → Query → Handler → Specification → Repository → DTOs |
| Get | `GET /resources/{id}` | Must exist for all resources. Return 404 if missing | ExternalId → Query → Handler → `GetByExternalIdAsync` → DTO |
| Create | `POST /resources` | Return created resource. Support optional client-provided IDs for idempotency | Body → Command → Handler validates → maps → persists → returns Guid |
| Update | `PUT /resources/{id}` | Strongly-typed command defines updatable fields. See section 7 | Body + ExternalId → Command → Handler finds → maps → saves |
| Delete | `DELETE /resources/{id}` | Support optional `force` for cascade. Consider soft-delete | ExternalId → Command → Handler finds → removes → saves |

**PUT vs PATCH**: Geewax recommends PATCH with field masks to prevent accidental field wiping. This codebase uses PUT with strongly-typed commands — the commands explicitly define which fields are updatable (read-only fields like `Id`, `ExternalId`, audit fields are excluded via `opt.Ignore()` in AutoMapper). This eliminates the field-wiping risk Geewax warns about. Consider migrating to PATCH with field masks in a future version for stricter REST semantics.

**Permission before existence**: Always check permissions BEFORE checking if a resource exists. Returning 404 for a resource the user can't access prevents information leakage (don't reveal that a resource exists to unauthorized users).

### Response Envelope

All responses use a consistent wrapper:

```
Success (2xx):  { "status": "success", "data": {...} }              → ApiSuccessResponse<T>
Client error:   { "status": "fail", "data": {...} }                 → ApiFailResponse (4xx)
Server error:   { "status": "error", "message": "...", "code": N }  → ApiErrorResponse (5xx)
```

Reference: `ApiResponse.cs`, `ExceptionMiddleware.cs`, `VehiclesController.cs`

## 5. Pagination (Ch. 21, AIP-158)

### This repo: offset-based

- Query params: `pageNumber` (1-based, default 1), `pageSize` (default 25, max 100)
- Response via `PagedListResponse<T>`:

```json
{
  "status": "success",
  "data": {
    "data": [...],
    "pageNumber": 1,
    "pageSize": 25,
    "totalCount": 142,
    "totalPages": 6
  }
}
```

- Pagination logic in `GenericRepository.GetAsync()` via Specification pattern

### Geewax recommends: token/cursor-based (AIP-158)

- Better for data that changes between pages (offset can skip/duplicate items)
- Scales better for large datasets (no `COUNT(*)` needed)
- Rules if migrating:
  - Tokens must be opaque, URL-safe strings
  - Empty `next_page_token` = end of collection
  - Tokens should expire (~3 days recommended)
  - Response shape: `{ "results": [...], "nextPageToken": "..." }`

**Trade-off**: Offset is simpler and supports "jump to page N". Token-based is more reliable under concurrent writes. Current offset approach is acceptable for this repo's scale.

Reference: `PagedListResponse.cs`, `GetVehicleListQuery.cs`, `GenericRepository.cs`

## 6. Filtering & Sorting (Ch. 22, AIP-160)

### This repo: individual query params + Specification pattern

- Filters as individual query parameters (not a filter expression string)
- Sorting via `sortField` + `sortOrder` (`"asc"` or `"desc"`)
- Specifications compose criteria with `Expression.AndAlso`
- Each resource gets its own `{Resource}FilterSpecification` class

Example: `GET /api/v1/vehicles?status=ACTIVE&tenantId=abc&sortField=name&sortOrder=asc&pageNumber=1&pageSize=25`

### Geewax/Google AIP-160: expression-based filter string

- Single `filter` parameter with expression syntax
- Operators: `=`, `!=`, `<`, `>`, `<=`, `>=`, `AND`, `OR`, `NOT`
- Nested traversal: `author.country="France"`
- Has operator for repeated fields: `tags:fantasy`
- Example: `GET /vehicles?filter=status="ACTIVE" AND capacity>10`

**Note**: The current Specification pattern is a valid filtering implementation — filter params arrive as individual query params instead of a single expression string. The individual param approach is simpler to validate and document.

Reference: `VehicleFilterSpecification.cs`, `BaseSpecification.cs`, `SpecificationEvaluator.cs`

## 7. Partial Updates — Field Masks (Ch. 8, AIP-161)

When implementing Update (PATCH), use field masks to specify which fields to update:

- `updateMask` parameter: comma-separated field paths (e.g., `name,status`)
- Paths are relative to resource (use `name`, not `vehicle.name`)
- Nested fields use `.` notation (e.g., `capacityProfile.seating`)
- When `updateMask` omitted: update all populated fields (non-null)
- Special value `*`: full replacement (behaves like PUT)

**Rules**:
- Read-only fields in `updateMask` → return `INVALID_ARGUMENT` (400)
- Immutable fields should be documented and rejected on update
- Output-only fields (e.g., `createdAt`) silently ignored in mask

**This repo**: Not yet implemented. Current approach uses strongly-typed UpdateCommands that define updatable fields explicitly. Field masks would add runtime flexibility but increase complexity. Consider as a future enhancement when clients need partial update control.

## 8. Custom Methods (Ch. 9, AIP-136)

For operations that don't fit standard CRUD:

- URI pattern: `POST /resources/{id}:verb` (colon separator)
- Naming: **VerbNoun** pattern, no prepositions (`archiveVehicle`, not `moveToArchive`)
- Must NOT reuse standard method names (Get, List, Create, Update, Delete)
- Use `GET` for read-only custom methods, `POST` for side effects
- Use sparingly — prefer standard methods
- This repo example: `POST /api/v1/vehicles/process-batch`

## 9. Long-Running Operations (Ch. 10-11, AIP-151)

For time-consuming operations, return an Operation resource:

1. Controller calls `IJobService.CreateAndQueueJobAsync("JobType", payload)`
2. Returns `202 Accepted` with `{ "jobId": "...", "statusUrl": "/api/v1/jobs/{jobId}" }`
3. Client polls `GET /api/v1/jobs/{jobId}` for status
4. Job statuses: `Accepted` → `Processing` → `Completed` / `Failed`

**Geewax rules**:
- Operation resource should include: `done` (bool), `metadata` (progress info), `response`/`error`
- Must implement GetOperation and ListOperations for discoverability
- Idempotency: completed jobs skip re-processing on retry

Reference: `VehiclesController.cs` (`process-batch`), `JobProcessorService.cs`

## 10. Batch Operations (Ch. 18, AIP-231 through AIP-235)

- Dedicated endpoint: `POST /resources/process-batch`
- Returns `202 Accepted` with JobId (delegates to long-running operation)

**Geewax rules**:
- Sync batches: **MUST be atomic** (all succeed or all fail)
- Async batches: MAY support partial success
- Response order must match request order
- Individual item errors should be reported per-item, not as a single batch error

**This repo**: Fan-out pattern — parent job reads the batch, queues individual child jobs. Never process large batches synchronously in a single job.

Reference: `ProcessVehicleBatchCommandHandler.cs`

## 11. Error Handling (AIP-193)

| Scenario | Error Code | HTTP | This Repo |
|----------|-----------|------|-----------|
| Invalid request / validation | INVALID_ARGUMENT | 400 | `BadRequestException` with `ValidationErrors` |
| Resource not found | NOT_FOUND | 404 | `NotFoundException` |
| User lacks access | PERMISSION_DENIED | 403 | — (not yet implemented) |
| Resource already exists | ALREADY_EXISTS | 409 | — |
| Etag mismatch | ABORTED | 409 | — |
| Rate limited | RESOURCE_EXHAUSTED | 429 | — |
| Children exist (no force) | FAILED_PRECONDITION | 400 | — |
| Unhandled server error | INTERNAL | 500 | Generic catch in `ExceptionMiddleware` |

**Rules**:
- Check permissions before existence (prevents information leakage)
- Error messages should be **actionable** — help users resolve the issue
- Include specific field names in validation errors
- Unhandled exceptions: generic message in production, stack trace in Development only

Reference: `ExceptionMiddleware.cs`, `Application/Exceptions/`

## 12. Versioning & Backwards Compatibility (AIP-180, AIP-185)

- **Only major versions**: `v1`, `v2` — no `v1.0` or `v1.4.2`
- URL path versioning: `/api/v1/...`, `/api/v2/...`
- Controller attributes: `[ApiVersion(1)]` + `[MapToApiVersion("1")]`

**Safe changes** (no new version needed):
- Add optional fields to request/response
- Add new methods or resources
- Add new enum values

**Breaking changes** (require new version):
- Remove or rename fields/methods/resources
- Change field types or semantics
- Change default values
- Change error behavior

**Rule**: Adding a new version = new handler classes. Never modify v1 handlers to add v2 behavior.

## 13. ETags & Optimistic Concurrency (AIP-154)

For resources where concurrent modification is a risk:

- Include `etag` field on the resource (opaque string, typically hash of resource state)
- On Update: if client sends an `etag` that doesn't match current, return `ABORTED` (409)
- If client omits `etag`: proceed without concurrency check (opt-in model)
- On Delete: same etag check to prevent deleting a resource that changed since last read

**This repo**: Not yet implemented. Consider adding ETags for resources with high concurrent modification risk (e.g., Vehicles with multiple dispatchers updating status).

## 14. Singleton Resources (Ch. 12, AIP-156)

For resources that exist **exactly once per parent** (e.g., settings, config):

- No Create/Delete — resource implicitly exists with its parent
- Only **Get** and **Update** methods
- Use **singular** name: `/tenants/{id}/settings` (not `/tenants/{id}/settings/1`)
- Always returns 200 (never 404 — it always exists)

**This repo**: Potential candidate — `TenantSettings` as a singleton sub-resource of Tenant.

## 15. Design Checklist

Before implementing a new resource:

- [ ] Resource is a noun (plural for collection endpoints)
- [ ] Uses `Guid ExternalId` as public identifier (never expose `long Id`)
- [ ] Standard methods (List/Get/Create) use correct HTTP verbs
- [ ] Update uses `PUT` with strongly-typed command (all updatable fields explicit in command class)
- [ ] List supports pagination (`pageNumber`, `pageSize`)
- [ ] List supports filtering via query params + Specification
- [ ] List supports sorting (`sortField`, `sortOrder`)
- [ ] All responses wrapped in `ApiSuccessResponse<T>`
- [ ] Errors throw domain exceptions (handled by `ExceptionMiddleware`)
- [ ] Error handling checks **permissions before existence**
- [ ] Non-CRUD operations use custom methods or long-running job pattern
- [ ] API version attribute is set on controller
- [ ] Resource hierarchy reflects real-world relationships (max 3-4 levels)
- [ ] Batch operations use fan-out job pattern (not synchronous loops)
- [ ] Consider ETags for resources with concurrent modification risk
- [ ] Singleton sub-resources for config/settings patterns

---

## 16. Codebase vs Geewax Deviations

Intentional divergences from Geewax/AIP recommendations, documented for consistency:

| Topic | Geewax Recommends | This Codebase | Rationale |
|-------|-------------------|---------------|-----------|
| Update verb | PATCH with field masks | PUT with strongly-typed commands | Commands explicitly define updatable fields; read-only fields excluded via AutoMapper `Ignore()`. No field-wiping risk. |
| Enum serialization | UPPER_SNAKE_CASE | PascalCase | Idiomatic C# convention. `JsonStringEnumConverter` serializes PascalCase as-is. Consistent with .NET ecosystem. |
| Pagination | Token/cursor-based | Offset-based (`pageNumber`/`pageSize`) | Simpler, supports "jump to page N". Acceptable at current scale. |
| Filtering | Expression-based filter string | Individual query params + Specification | Simpler to validate and document. Specification pattern composes criteria cleanly. |
| Field masks | `updateMask` query parameter | Not implemented | Strongly-typed commands make this unnecessary currently. Future enhancement. |
| Resource names | Full path (`tenants/abc/vehicles/xyz`) | UUID only (`ExternalId` as `id`) | Simpler. Full resource names can be added later as a non-breaking change. |

These are deliberate choices, not oversights. Review periodically as the API matures.

---

## 17. Canonical Reference Files

| Pattern | File |
|---------|------|
| API design principles | `TimefoldInt.API/api-design-patterns-summary.md` |
| Entity with ExternalId | `TimefoldInt.Domain/Common/BaseEntity.cs` |
| Entity example | `TimefoldInt.Domain/Entities/Vehicle.cs` |
| DTO mapping (ExternalId → Id) | `TimefoldInt.Application/MappingProfiles/VehicleProfile.cs` |
| CQRS Command flow | `TimefoldInt.Application/Features/Vehicles/Commands/CreateVehicle/` |
| CQRS Query flow | `TimefoldInt.Application/Features/Vehicles/Queries/GetVehicleList/` |
| Specification pattern | `TimefoldInt.Application/Specifications/BaseSpecification.cs` |
| Filter specification | `TimefoldInt.Application/Features/Vehicles/Queries/GetVehicleList/VehicleFilterSpecification.cs` |
| Pagination model | `TimefoldInt.Application/Models/Common/PagedListResponse.cs` |
| Repository pattern | `TimefoldInt.Persistence/Repositories/GenericRepository.cs` |
| EF Configuration | `TimefoldInt.Persistence/Configurations/VehicleConfiguration.cs` |
| Controller pattern | `TimefoldInt.API/Controllers/VehiclesController.cs` |
| Response models | `TimefoldInt.API/Models/ApiResponse.cs` |
| Error handling | `TimefoldInt.API/Middleware/ExceptionMiddleware.cs` |
| Job processing | `TimefoldInt.Infrastructure/Services/JobProcessing/JobProcessorService.cs` |
