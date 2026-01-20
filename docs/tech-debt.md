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
| - | - | No high priority debt | - |

---

## Medium Priority
*Issues that should be addressed when working in the area*

| Item | Location | Description | Added |
|------|----------|-------------|-------|
| EF Core InMemory Test Failures | Infrastructure.Tests, Api.Tests | See details below | 2026-01-20 |

### EF Core InMemory Provider Limitation

**Affected Tests:**
- `NemtPlatform.Infrastructure.Tests` - 15/16 failing
- `NemtPlatform.Api.Tests` - 2/15 failing, 13 skipped

**Root Cause:**
The `TenantSettings` owned type contains nested owned types (`RegionalSettings`, `BrandingSettings`, `InspectionSettings`) which the EF Core InMemory provider cannot properly configure. This is a known limitation of the InMemory provider with complex owned type hierarchies.

**Impact:**
- Domain tests (47) and Application tests (11) pass - core logic is validated
- Infrastructure and API integration tests are scaffolded but fail at DbContext initialization

**Recommended Fix:**
Switch from EF Core InMemory to SQLite in-memory for integration tests:

```csharp
// In test setup, replace:
options.UseInMemoryDatabase("TestDb");

// With:
options.UseSqlite("DataSource=:memory:");
```

Required package: `Microsoft.EntityFrameworkCore.Sqlite`

**Alternative:**
Accept scaffolded tests as templates and configure with a real test database (SQL Server LocalDB or containerized PostgreSQL).

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
| - | - | - | - |

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
