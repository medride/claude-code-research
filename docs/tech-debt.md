# Technical Debt

Track known technical debt items for future resolution.

---

## Critical
*Issues that could cause production problems or security vulnerabilities*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| - | - | No critical debt | - |

---

## High Priority
*Issues that significantly impact development velocity or code quality*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| JWT Authentication | Infrastructure.Identity | Not yet implemented - required before production | 2026-01-20 |
| Policy-based Authorization | Api layer | Not yet implemented - required before production | 2026-01-20 |

---

## Medium Priority
*Issues that should be addressed when working in the area*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| FluentValidation | Application layer | Input validation not configured | 2026-01-20 |
| API Endpoints | Api layer | Only health check exists | 2026-01-20 |

---

## Low Priority
*Nice-to-have improvements for future consideration*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| Swagger/OpenAPI | Api layer | Document API when built | 2026-01-20 |

---

## Resolved
*Debt items that have been addressed*

| Item | Location | Resolution | Date |
|------|----------|------------|------|
| EF Core Model Configuration | Domain + Infrastructure | Added 35+ entity configurations, ToJson() for complex types | 2026-01-21 |
| EF Core InMemory Provider | Infrastructure.Tests | Switched to SQLite in-memory provider | 2026-01-21 |
| Value Object Constructors | Domain ValueObjects | Added parameterless constructors for EF Core compatibility | 2026-01-21 |

### EF Core Model Configuration (Resolved)

**Original Issue:**
58+ entities/value objects needed explicit EF Core configuration due to complex nested value objects, Dictionary properties, and ownership ambiguity.

**Resolution:**
- Created 35+ `IEntityTypeConfiguration<T>` files
- Configured `.ToJson()` for complex nested types (TripConstraints, JourneyTemplate, etc.)
- Added `modelBuilder.Ignore<T>()` for JSON-only types
- All 135 persistence tests now pass

### EF Core InMemory Provider (Resolved)

**Original Issue:**
The EF Core InMemory provider cannot properly configure nested owned types.

**Resolution:**
- Switched to SQLite in-memory provider for tests
- Created isolated test DbContexts for each entity group

### Value Object Constructors (Resolved)

**Original Issue:**
C# records with positional constructors caused EF Core constructor binding errors.

**Resolution:**
- Converted 35+ value objects from positional record syntax to property syntax
- Added parameterless constructors for EF Core and JSON serialization

---

## How to Use This File

### Adding Debt
When you notice technical debt:
1. Assess severity (Critical/High/Medium/Low)
2. Add to appropriate section
3. Include location and brief description
4. Note when it was added

### Resolving Debt
When addressing debt:
1. Move item to "Resolved" section
2. Note how it was resolved
3. Add resolution date
