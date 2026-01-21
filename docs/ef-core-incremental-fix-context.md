# EF Core Incremental Fix - Context Document

**Date**: 2026-01-21
**Purpose**: Provide context for continuing EF Core configuration work in a fresh session

## Current State

All test files have been deleted. The codebase is ready for an incremental approach to fixing EF Core configuration issues.

### What Has Been Done

1. **SQLite Test Infrastructure**: Created `SqliteTestBase.cs` helper (now deleted, recreate when needed)
2. **Value Object Constructors**: 35+ value objects converted from positional record syntax to property syntax with parameterless constructors
3. **JSON Column Ignores**: Added `modelBuilder.Ignore<T>()` in `NemtPlatformDbContext.OnModelCreating()` for 13 types
4. **Partial Configurations**: Some entity configurations updated with `ToJson()` but not fully tested

### What Needs To Be Done

Fix EF Core configuration issues incrementally, group by group. See `docs/ef-core-configuration-issues.md` for full analysis.

## Entity Groups (In Order)

### Group 1: Core Tenant (START HERE)
- **Entities**: `Tenant`
- **Value Objects**: `TenantSettings`, `NotificationPreferences`, `QuietHours`
- **Existing Config**: `TenantConfiguration.cs` (needs `ToJson()` for Settings)
- **Test Focus**: Create/Read tenant with settings

### Group 2: Identity Base
- **Entities**: `User`, `Role`, `Permission`
- **Value Objects**: None complex
- **Test Focus**: User-Role relationships

### Group 3: Identity Extended
- **Entities**: `Employee`, `DriverProfile`
- **Value Objects**: `Address`, `DriverSkills`, `PerformanceMetrics`
- **Test Focus**: Employee with address, driver with skills

### Group 4: Locations
- **Entities**: `Place`, `AccessPoint`, `Region`
- **Value Objects**: `GpsLocation`, `GeoJsonPolygon`
- **Test Focus**: Places with GPS, regions with boundaries

### Group 5: Passengers
- **Entities**: `Passenger`, `Guardian`, `GuardianPassengerLink`
- **Value Objects**: `PersonName`, `PhoneNumber`, `GuardianPermissions`, `NotificationPreferences`
- **Test Focus**: Passenger with constraints, guardian links

### Group 6: Fleet
- **Entities**: `Vehicle`
- **Value Objects**: `CapacityProfile`, `MedicalCapabilities`
- **Test Focus**: Vehicle with capacity

### Group 7: Operations Basic
- **Entities**: `Trip`, `Route`, `Shift`
- **Value Objects**: `TripConstraints`, `PlannedRoute`, `CapacityRequirements`
- **Test Focus**: Trip with constraints (most complex)

### Group 8: Operations Advanced
- **Entities**: `Journey`, `StandingOrder`, `RouteManifest`
- **Value Objects**: `JourneyTemplate`, `JourneyLeg`, etc.
- **Test Focus**: Standing orders with templates

### Group 9: Billing
- **Entities**: `Authorization`, `Claim`, `Remittance`
- **Value Objects**: `ClaimLineItem`, `RemittanceAdjustment`, `DateRange`

### Group 10: Execution & Compliance
- **Entities**: `TripExecution`, `VehicleInspection`, `InspectionTemplate`
- **Value Objects**: `StopReconciliation`, `ChecklistItem`

## Fix Pattern to Follow

### Step 1: Verify Entity and Value Objects
```csharp
// All value objects need this pattern:
public record ValueObjectName
{
    public string Property1 { get; init; }
    public int Property2 { get; init; }

    public ValueObjectName() { }  // Required for EF Core

    public ValueObjectName(string property1, int property2)
    {
        Property1 = property1;
        Property2 = property2;
    }
}
```

### Step 2: Create/Update Entity Configuration
```csharp
// For simple value objects - use OwnsOne:
builder.OwnsOne(e => e.SimpleValue, value =>
{
    value.Property(v => v.Property1).HasMaxLength(100);
});

// For complex nested types - use ToJson():
builder.OwnsOne(e => e.ComplexValue, value =>
{
    value.ToJson();
    // Configure nested owned types within JSON
    value.OwnsMany(v => v.Items);
});
```

### Step 3: Add modelBuilder.Ignore for JSON-Only Types
```csharp
// In NemtPlatformDbContext.OnModelCreating:
modelBuilder.Ignore<TypeOnlyUsedInJsonColumn>();
```

### Step 4: Create Test
```csharp
public class EntityNameTests : SqliteTestBase
{
    [Fact]
    public async Task Can_Create_And_Retrieve_Entity()
    {
        var entity = new EntityName { ... };
        Context.EntityNames.Add(entity);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.EntityNames.FindAsync(entity.Id);
        Assert.NotNull(loaded);
    }
}
```

## Key Files

### Domain Model
- `src/NemtPlatform/Core/NemtPlatform.Domain/Entities/` - All entity classes
- `src/NemtPlatform/Core/NemtPlatform.Domain/Common/ValueObjects/` - All value objects

### Infrastructure
- `src/NemtPlatform/Infrastructure/NemtPlatform.Infrastructure/Persistence/NemtPlatformDbContext.cs`
- `src/NemtPlatform/Infrastructure/NemtPlatform.Infrastructure/Persistence/Configurations/` - Entity configurations

### Tests (Recreate as needed)
- `src/NemtPlatform/Tests/NemtPlatform.Infrastructure.Tests/Helpers/SqliteTestBase.cs`
- `src/NemtPlatform/Tests/NemtPlatform.Infrastructure.Tests/Persistence/`

## SqliteTestBase Template

When creating tests, recreate this helper:

```csharp
namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NemtPlatform.Infrastructure.Persistence;

public abstract class SqliteTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly NemtPlatformDbContext Context;

    protected SqliteTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseSqlite(_connection)
            .ConfigureWarnings(warnings =>
            {
                warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
                warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning);
            })
            .EnableDetailedErrors()
            .Options;

        Context = new NemtPlatformDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected NemtPlatformDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new NemtPlatformDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
```

## Commands to Run

```bash
# Build to check for compilation errors
dotnet build src/NemtPlatform/NemtPlatform.sln

# Run specific test project
dotnet test src/NemtPlatform/Tests/NemtPlatform.Infrastructure.Tests

# Generate migration (after fixes)
dotnet ef migrations add TestMigration --project src/NemtPlatform/Infrastructure/NemtPlatform.Infrastructure --startup-project src/NemtPlatform/Api/NemtPlatform.Api
```

## Next Session Instructions

1. Read this file and `docs/ef-core-configuration-issues.md`
2. Start with **Group 1: Tenant**
3. Read relevant entity and value object files
4. Update `TenantConfiguration.cs` to use `ToJson()` for Settings
5. Recreate `SqliteTestBase.cs`
6. Create `TenantTests.cs`
7. Run tests until passing
8. Move to next group

## Success Criteria

For each group:
- [ ] Entity configuration compiles
- [ ] DbContext model validation passes (no errors on `EnsureCreated()`)
- [ ] Can create entity with all value objects populated
- [ ] Can retrieve entity and value objects are correctly loaded
- [ ] All nested collections are preserved
