# 1. NEMT Platform Clean Architecture .NET Solution

## Status

Accepted

## Date

2026-01-20

## Context

We have a comprehensive TypeScript domain model (`entities.ts`, 3200+ lines) for a multi-tenant NEMT (Non-Emergency Medical Transportation) platform. The model contains 15 sections covering:

- Core enums and value objects
- User/Identity management with RBAC
- Passenger management (patients, students)
- Operations (trips, routes, shifts, journeys)
- Fleet management (vehicles, equipment, maintenance)
- Billing and financial (claims, authorizations, remittances)
- Compliance and credentials
- Dynamic form configuration

We need to create a .NET solution that:
1. Implements Clean Architecture principles
2. Supports multi-tenancy with proper data isolation
3. Is secure by design (RBAC, audit trails)
4. Is testable at all layers
5. Can scale to microservices later if needed

## Decision

We will implement a **Modular Monolith** using Clean Architecture with the following structure:

```
src/
├── Core/
│   ├── NemtPlatform.Domain/
│   ├── NemtPlatform.Application/
│   └── NemtPlatform.Application.Contracts/
├── Infrastructure/
│   ├── NemtPlatform.Infrastructure/
│   ├── NemtPlatform.Infrastructure.Identity/
│   └── NemtPlatform.Infrastructure.MultiTenancy/
├── Presentation/
│   └── NemtPlatform.Api/
└── Tests/
    ├── NemtPlatform.Domain.Tests/
    ├── NemtPlatform.Application.Tests/
    ├── NemtPlatform.Infrastructure.Tests/
    └── NemtPlatform.Api.Tests/
```

### Domain Organization (Bounded Contexts)

| Domain | Key Entities |
|--------|-------------|
| Core/SharedKernel | Enums, Value Objects, TenantId, AuditableEntity |
| Identity | User, Employee, DriverProfile, Role, Permission |
| Passengers | Passenger, PatientProfile, StudentProfile, Guardian |
| Locations | Place, AccessPoint, Region |
| Operations | Trip, Stop, Route, Shift, Journey, StandingOrder |
| Fleet | Vehicle, Equipment, MaintenanceRecord, FuelLog |
| Billing | FundingSource, Authorization, Claim, Remittance |
| Execution | TripExecution, StopReconciliation, Incident |
| Compliance | Credential, EmployeeCredential, AttributeDefinition |
| Configuration | FormConfiguration, ViewConfiguration, ProcedureSet |

## Alternatives Considered

### Option A: Simple Monolith
**Pros:**
- Fastest to build
- Simple deployment

**Cons:**
- Tightly coupled
- Hard to scale
- Difficult to maintain with 50+ entities

### Option B: Modular Monolith (CHOSEN)
**Pros:**
- Clean domain boundaries
- Single deployment unit
- Can extract to microservices later
- Testable in isolation
- Matches multi-tenant SaaS patterns

**Cons:**
- More initial structure
- Requires discipline to maintain boundaries

### Option C: Microservices
**Pros:**
- Independent scaling
- Technology flexibility per service

**Cons:**
- Overkill for initial scaffold
- Complex DevOps
- Network latency between services
- Distributed transactions complexity

## Consequences

### Positive
- Clear separation of concerns
- Each domain can be developed/tested independently
- Security built-in from the start (multi-tenancy, RBAC)
- Easy to onboard new developers (clear structure)
- Path to microservices if needed

### Negative
- More projects to manage initially
- Need to be disciplined about not crossing boundaries

### Neutral
- EF Core will handle most data access patterns
- Standard ASP.NET Core patterns apply

## Implementation Notes

### Execution Phases
1. Solution structure + Core/SharedKernel
2. Identity domain + Security infrastructure
3. Multi-tenancy infrastructure
4. Passengers domain
5. Locations domain
6. Operations domain
7. Fleet domain
8. Billing domain
9. Execution & Compliance domains
10. Configuration domain
11. Infrastructure (DbContext, EF configs)
12. Test projects scaffold

### Key Patterns
- **Multi-tenancy**: Global query filters on `TenantId`
- **Audit**: `IAuditableEntity` with CreatedAt/UpdatedAt/CreatedBy
- **RBAC**: Policy-based authorization with Permission checks
- **Validation**: FluentValidation in Application layer

## References

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture)
- Source: `entities.ts` (3200+ lines TypeScript domain model)

---

*Decision made by: Development Team*
*Last updated: 2026-01-20*
