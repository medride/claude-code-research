# Technical Debt

Track known technical debt items for future resolution.

---

## Critical
*Issues that could cause production problems or security vulnerabilities*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| EF Core Model Configuration | Domain Model + Infrastructure | 58+ entities/value objects need proper EF Core configuration | 2026-01-21 |

### EF Core Model Configuration Issues

**Full Analysis:** See [ef-core-configuration-issues.md](./ef-core-configuration-issues.md)

**Summary:**
The domain model has 58+ value objects and entities that require explicit EF Core configuration due to:
- Complex nested value objects (e.g., `TripConstraints` has 3-level deep nesting)
- `Dictionary<K,V>` properties that EF Core cannot auto-map
- `List<T>` collections of value objects needing `OwnsMany()`
- Same value object type owned by multiple entities (ownership ambiguity)

**Impact:**
- Infrastructure tests fail at DbContext initialization
- API integration tests cannot run
- Cannot generate database migrations

**Root Cause:**
C# records with positional constructors containing complex types. EF Core cannot bind constructor parameters to navigation properties (owned types).

**Required Fixes:**
1. Add `modelBuilder.Ignore<T>()` for 13 JSON-only types
2. Create 15 new `IEntityTypeConfiguration<T>` files
3. Update 6 existing configurations to use `.ToJson()`
4. Add value converters for `Dictionary` and `List<Enum>` properties

---

## High Priority
*Issues that significantly impact development velocity or code quality*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| - | - | No high priority debt | - |

---

## Medium Priority
*Issues that should be addressed when working in the area*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| List<object> placeholders | Route.cs, Shift.cs | `List<object>` should be typed (fixed to `List<BaseStop>` and `List<DriverServiceStop>`) | 2026-01-21 |

---

## Low Priority
*Nice-to-have improvements for future consideration*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| - | - | No low priority debt | - |

---

## Resolved
*Debt items that have been addressed*

| Item | Location | Resolution | Date |
|------|----------|------------|------|
| EF Core InMemory Provider | Infrastructure.Tests | Switched to SQLite in-memory provider | 2026-01-21 |
| Value Object Constructors | Domain ValueObjects | Added parameterless constructors for EF Core compatibility | 2026-01-21 |

### EF Core InMemory Provider (Resolved)

**Original Issue:**
The `TenantSettings` owned type contains nested owned types which the EF Core InMemory provider cannot properly configure.

**Resolution:**
- Switched from `Microsoft.EntityFrameworkCore.InMemory` to `Microsoft.EntityFrameworkCore.Sqlite`
- Updated test base classes to use SQLite in-memory with proper connection management
- Created `SqliteTestBase` helper class

**Note:** While the provider switch is complete, the underlying model configuration issues (documented above) still need to be addressed.

### Value Object Constructors (Resolved)

**Original Issue:**
C# records with positional constructors caused EF Core constructor binding errors.

**Resolution:**
- Converted 35+ value objects from positional record syntax to property syntax
- Added parameterless constructors for EF Core and JSON serialization
- Added parameterized constructors for domain code convenience

---

## How to Use This File

### Adding Debt
When you notice technical debt:
1. Assess severity (Critical/High/Medium/Low)
2. Add to appropriate section
3. Include location and brief description
4. Note when it was added

### Debt Categories

**Critical:**
- Security vulnerabilities
- Data integrity risks
- Production stability issues
- Blocking development/testing

**High:**
- Performance bottlenecks
- Missing tests for critical paths
- Outdated dependencies with known issues

**Medium:**
- Code duplication
- Missing error handling
- Inconsistent patterns

**Low:**
- Minor refactoring opportunities
- Documentation gaps
- Style inconsistencies

### Resolving Debt
When addressing debt:
1. Move item to "Resolved" section
2. Note how it was resolved
3. Add resolution date
