# NEMT Platform - Implementation Constraints

Reference document for constraints, patterns, and deferred items in the NEMT Platform implementation.

---

## Architecture Constraints

### Pattern: Modular Monolith with Clean Architecture

| Layer | Projects | Responsibility |
|-------|----------|----------------|
| Core | Domain, Application, Application.Contracts | Business logic, no external dependencies |
| Infrastructure | Infrastructure, Infrastructure.Identity, Infrastructure.MultiTenancy | Database, external services |
| Presentation | Api | HTTP endpoints, authentication |
| Tests | Domain.Tests, Application.Tests, Infrastructure.Tests, Api.Tests | Quality assurance |

**Key Rule:** Domain and Application layers must not reference Infrastructure or Presentation layers.

---

## Entity Constraints

### Base Class Hierarchy

```
IEntity (Id: string)
    └── Entity
        └── AuditableEntity (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)
            └── TenantEntity (TenantId: string)
```

### Requirements

| Entity Type | Base Class | Has TenantId | Auto-Audit |
|-------------|------------|--------------|------------|
| Tenant | `AuditableEntity` | No (is the tenant) | Yes |
| All other entities | `TenantEntity` | Yes (required) | Yes |

### Automatic Behaviors (in DbContext.SaveChangesAsync)

1. **CreatedAt** - Set automatically on insert (UTC)
2. **UpdatedAt** - Set automatically on update (UTC)
3. **TenantId** - Set automatically from `ITenantContext` if empty

---

## Multi-Tenancy Constraints

### Data Isolation Strategy

- **Method:** EF Core global query filters on `TenantId`
- **Applied via:** Entity configurations inheriting from `TenantEntityConfiguration<T>`
- **Bypass:** Requires explicit `.IgnoreQueryFilters()` call

### Tenant Resolution

| Strategy | Source | Use Case |
|----------|--------|----------|
| Header | `X-Tenant-Id` header | API calls |
| Subdomain | `{tenant}.example.com` | Web app |
| Claim | JWT `tenant_id` claim | Authenticated users |
| Query | `?tenantId=xxx` | Development/testing |

**Warning:** Never allow cross-tenant data access without explicit authorization.

---

## Value Object Constraints

### Pattern: Immutable C# Records

```csharp
public record GpsLocation(double Latitude, double Longitude);
```

### Requirements

- All value objects must be `record` types (immutable)
- Use positional parameters for simple types
- Use `with` expressions for modifications
- Store as owned types in EF Core

### Known Issue: Nested Owned Types

EF Core InMemory provider cannot handle deeply nested owned types. Example:

```csharp
// This causes issues with InMemory provider:
TenantSettings
    ├── RegionalSettings (owned)
    ├── BrandingSettings (owned)
    └── InspectionSettings (owned)
```

**Workaround:** Use SQLite in-memory for integration tests.

---

## Database Constraints

### EF Core Version

- **Version:** 9.x (required for .NET 9.0)
- **Provider:** SQL Server (production), InMemory/SQLite (testing)

### Entity Configuration Pattern

```csharp
public class TripConfiguration : TenantEntityConfiguration<Trip>
{
    public override void Configure(EntityTypeBuilder<Trip> builder)
    {
        base.Configure(builder); // Applies tenant filter

        // Additional configuration...
    }
}
```

### String ID Pattern

- All entities use `string Id` (not int/Guid)
- Allows for ULID, custom prefixes, or external IDs
- Format: `{prefix}-{uniquepart}` (e.g., `trip-abc123`)

---

## Security Constraints (Deferred)

### Not Yet Implemented

| Feature | Status | Planned Location |
|---------|--------|------------------|
| JWT Authentication | Deferred | Infrastructure.Identity |
| Policy-based Authorization | Deferred | Api layer |
| Permission checks | Scaffolded only | Application layer |
| Password hashing | Not implemented | Infrastructure.Identity |

### RBAC Model (Scaffolded)

```
User ──┬── Role ──── Permission
       └── Employee ── DriverProfile
```

**When implementing:**
- Use ASP.NET Core Identity or custom JWT provider
- Implement `IPermissionService` for permission checks
- Add `[Authorize(Policy = "...")]` to API endpoints

---

## Validation Constraints (Deferred)

### Not Yet Implemented

- **FluentValidation** mentioned in ADR but not implemented
- No input validation on entities currently
- API request validation not configured

### Recommended Implementation

```csharp
// In Application layer:
public class CreateTripCommandValidator : AbstractValidator<CreateTripCommand>
{
    public CreateTripCommandValidator()
    {
        RuleFor(x => x.PassengerId).NotEmpty();
        RuleFor(x => x.FundingSourceId).NotEmpty();
    }
}
```

---

## Testing Constraints

### Test Pyramid

| Layer | Framework | Approach |
|-------|-----------|----------|
| Domain | xUnit | Unit tests, no mocks needed |
| Application | xUnit + Moq | Mock repositories |
| Infrastructure | xUnit + SQLite | Integration tests (not InMemory) |
| Api | xUnit + WebApplicationFactory | HTTP integration tests |

### Known Limitation

**EF Core InMemory provider fails** with nested owned types (TenantSettings).
See `docs/tech-debt.md` for details and workaround.

---

## Deferred Items Summary

| Item | Priority | Notes |
|------|----------|-------|
| JWT Authentication | High | Required before production |
| Policy-based Authorization | High | Required before production |
| FluentValidation | Medium | Add to Application layer |
| SQLite for integration tests | Medium | Fix Infrastructure/Api tests |
| API endpoints | Low | Only health check exists |
| Swagger/OpenAPI | Low | Document API when built |

---

## Version Requirements

| Component | Version | Notes |
|-----------|---------|-------|
| .NET | 9.0 | Target framework |
| EF Core | 9.x | Database ORM |
| xUnit | 2.x | Test framework |
| Moq | 4.x | Mocking framework |
| ASP.NET Core | 9.0 | Web framework |

---

## Quick Reference

### Adding a New Entity

1. Create class inheriting from `TenantEntity`
2. Add `DbSet<T>` to `NemtPlatformDbContext`
3. Create `EntityConfiguration` inheriting from `TenantEntityConfiguration<T>`
4. Add domain tests for entity behavior

### Adding a New Value Object

1. Create as `record` type in `Domain/Common/ValueObjects/`
2. Configure as owned type in parent entity configuration
3. Add equality tests

### Adding a New Enum

1. Create in `Domain/Common/Enums/`
2. Use int backing values for database storage
3. Add enum parsing tests

---

*Last updated: 2026-01-20*
