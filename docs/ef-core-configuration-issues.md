# EF Core Configuration Issues - NemtPlatform Domain Model

**Date**: 2026-01-21
**Status**: Needs Resolution
**Priority**: High

## Executive Summary

Analysis of the NemtPlatform Core domain model identified **58+ significant EF Core configuration issues** across value objects and entities. These issues prevent proper database schema creation and cause model validation errors during testing.

## Root Cause

The domain model uses C# records with positional constructors and complex nested value objects. EF Core cannot automatically:
1. Bind constructor parameters to navigation properties (owned types)
2. Map `Dictionary<K,V>` properties without custom converters
3. Resolve ownership when the same type is owned by multiple entities

## Issue Categories

### 1. Value Objects with Complex Collections

| File | Class | Problematic Properties | Fix Required |
|------|-------|----------------------|--------------|
| `ViewPreferences.cs` | ViewPreferences | `Dictionary<string, bool> ColumnVisibility`<br>`Dictionary<string, int> ColumnSizes`<br>`List<SortingState> Sorting` | Use `.ToJson()` or custom value converter |
| `NotificationPreferences.cs` | NotificationPreferences | `List<NotificationChannel> Channels`<br>`QuietHours? QuietHours` | Configure as JSON |
| `GeoJsonPolygon.cs` | GeoJsonPolygon | `List<List<double[]>> Coordinates` | Use JSON serialization |
| `ProcedureOverrides.cs` | ProcedureOverrides | `List<ProcedureRule>? Add`<br>`List<StopProcedureType>? Remove` | Use `.OwnsMany()` + JSON |
| `JourneyTemplate.cs` | JourneyTemplate | `List<JourneyLegTemplate> Legs`<br>`CapacityRequirements`<br>`TripConstraints?` | Use `.ToJson()` for entire structure |
| `TripConstraints.cs` | TripConstraints | 3-level deep nesting with Lists | Use `.ToJson()` |
| `ClaimLineItem.cs` | ClaimLineItem | `List<string>? Modifiers` | Configure as JSON |
| `MedicalCapabilities.cs` | MedicalCapabilities | `List<string>? OnboardEquipmentIds` | Configure as JSON |

### 2. Deeply Nested Value Objects

The most complex issue is the `TripConstraints` hierarchy:

```
TripConstraints
├─ Preferences: ConstraintSet
│   ├─ Driver: DriverConstraints
│   │   ├─ Ids: List<string>?
│   │   └─ RequiredAttributes: List<string>?
│   └─ Vehicle: VehicleConstraints
│       └─ Ids: List<string>?
├─ Requirements: ConstraintSet (same structure)
└─ Prohibitions: ConstraintSet (same structure)
```

**Solution**: Store entire `TripConstraints` as JSON column using `.ToJson()`.

### 3. Entities Missing Configurations

#### Operations Entities
| Entity | Properties Needing Config |
|--------|--------------------------|
| Trip | `ExternalIds`, `CapacityRequirements`, `PlannedRoute`, `Constraints`, `PostTripDirective`, `Stops` |
| Journey | `List<JourneyLeg> Legs` |
| PassengerStop | `CapacityDelta`, `ProcedureOverrides` |
| StandingOrder | `EffectiveDateRange`, `JourneyTemplate` |
| Route | `List<BaseStop> Stops` |
| Shift | `List<ShiftPersonnel> Personnel`, `StartLocation`, `EndLocation` |

#### Billing Entities
| Entity | Properties Needing Config |
|--------|--------------------------|
| Authorization | `EffectiveDateRange`, `AuthorizedDestinations`, `Limits` |
| Claim | `LineItems`, `List<string>` collections |
| Remittance | `Adjustments` |

#### Fleet Entities
| Entity | Properties Needing Config |
|--------|--------------------------|
| Vehicle | `CapacityProfile`, `MedicalCapabilities` |

#### Identity Entities
| Entity | Properties Needing Config |
|--------|--------------------------|
| DriverProfile | `Skills`, `PerformanceMetrics` |
| Employee | `Address` |

#### Passenger Entities
| Entity | Properties Needing Config |
|--------|--------------------------|
| Passenger | `Name`, `Constraints` |
| PatientProfile | `MedicaidIdVerification` |
| GuardianPassengerLink | `Permissions`, `NotificationPreferences` |

#### Other Entities
| Entity | Properties Needing Config |
|--------|--------------------------|
| AccessPoint | `Gps` |
| Region | `Boundary` |
| ViewConfiguration | `Preferences` |
| TripExecution | `ApproachRoute`, `LiveRoute`, `Reconciliations` |
| InspectionTemplate | `ChecklistItems` |

### 4. Types That Need `modelBuilder.Ignore<T>()`

These types are only used within JSON columns and should be ignored at the model level to prevent EF Core from discovering them:

```csharp
modelBuilder.Ignore<JourneyLeg>();
modelBuilder.Ignore<LegTransition>();
modelBuilder.Ignore<JourneyTemplate>();
modelBuilder.Ignore<JourneyLegTemplate>();
modelBuilder.Ignore<StopTemplate>();
modelBuilder.Ignore<ProcedureRule>();
modelBuilder.Ignore<SortingState>();
modelBuilder.Ignore<ViewPreferences>();
modelBuilder.Ignore<ChecklistItem>();
modelBuilder.Ignore<InspectionDefect>();
modelBuilder.Ignore<ClaimLineItem>();
modelBuilder.Ignore<RemittanceAdjustment>();
modelBuilder.Ignore<StopReconciliation>();
```

## Recommended Fixes

### Option A: JSON Columns (Recommended for Complex Types)

For complex nested value objects, use EF Core 7+ JSON column support:

```csharp
// In entity configuration
builder.OwnsOne(e => e.Constraints, constraints =>
{
    constraints.ToJson();
});

builder.OwnsOne(e => e.JourneyTemplate, template =>
{
    template.ToJson();
});
```

**Pros**: Simple configuration, works with any nesting depth
**Cons**: Can't query individual properties with SQL

### Option B: Explicit Owned Type Configuration

For simpler value objects, use traditional owned types:

```csharp
builder.OwnsOne(e => e.Address, address =>
{
    address.Property(a => a.Street).HasMaxLength(200);
    address.Property(a => a.City).HasMaxLength(100);
});
```

**Pros**: SQL-queryable properties
**Cons**: Requires configuration for each property, complex for nested types

### Option C: Value Converters for Collections

For `Dictionary<K,V>` and `List<T>` properties:

```csharp
builder.Property(e => e.ColumnVisibility)
    .HasConversion(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        v => JsonSerializer.Deserialize<Dictionary<string, bool>>(v, (JsonSerializerOptions)null));
```

## Implementation Priority

### Phase 1: Critical (Blocking Tests)
1. Add `modelBuilder.Ignore<T>()` for all JSON-only types
2. Configure `TripConstraints` with `.ToJson()`
3. Configure `JourneyTemplate` with `.ToJson()`
4. Configure `ViewPreferences` with `.ToJson()`

### Phase 2: High (Entity Configurations)
1. Create `IEntityTypeConfiguration<T>` for each entity with value objects
2. Configure all `OwnsOne` relationships for simple value objects
3. Configure all `OwnsMany` relationships for collections

### Phase 3: Medium (Collections)
1. Add JSON converters for `List<string>` FK collections
2. Add JSON converters for `List<Enum>` collections
3. Add JSON converters for `Dictionary<K,V>` properties

## Files Requiring New Configurations

Create `IEntityTypeConfiguration<T>` implementations for:

- [ ] `AuthorizationConfiguration.cs`
- [ ] `ClaimConfiguration.cs`
- [ ] `RemittanceConfiguration.cs`
- [ ] `DriverProfileConfiguration.cs`
- [ ] `EmployeeConfiguration.cs`
- [ ] `JourneyConfiguration.cs`
- [ ] `RouteConfiguration.cs`
- [ ] `ShiftConfiguration.cs`
- [ ] `RouteManifestConfiguration.cs`
- [ ] `AccessPointConfiguration.cs`
- [ ] `RegionConfiguration.cs`
- [ ] `ViewConfigurationConfiguration.cs`
- [ ] `TripExecutionConfiguration.cs`
- [ ] `InspectionTemplateConfiguration.cs`
- [ ] `PatientProfileConfiguration.cs`

## Existing Configurations (Need Updates)

- [x] `TenantConfiguration.cs` - Settings needs `.ToJson()`
- [x] `TripConfiguration.cs` - PlannedRoute, Constraints, Stops need `.ToJson()`
- [x] `PassengerConfiguration.cs` - Constraints needs `.ToJson()`
- [x] `VehicleConfiguration.cs` - MedicalCapabilities may need updates
- [x] `GuardianPassengerLinkConfiguration.cs` - Permissions, NotificationPreferences need `.ToJson()`
- [x] `StandingOrderConfiguration.cs` - JourneyTemplate needs `.ToJson()`

## Testing Strategy

After configurations are complete:

1. Run `dotnet ef dbcontext info` to validate model
2. Run `dotnet ef migrations add TestMigration --dry-run` to check schema generation
3. Run Infrastructure tests with SQLite in-memory
4. Run API integration tests

## Related Files

- `/docs/tech-debt.md` - Track as tech debt item
- `/src/NemtPlatform/Infrastructure/NemtPlatform.Infrastructure/Persistence/Configurations/` - Entity configurations
- `/src/NemtPlatform/Tests/NemtPlatform.Infrastructure.Tests/` - Affected tests
