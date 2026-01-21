namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Configuration;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;
using Xunit;

/// <summary>
/// Tests for Configuration entities:
/// FormConfiguration, ProcedureDefinition (system-wide), ProcedureSet, ViewConfiguration.
/// </summary>
public class ConfigurationTests : ConfigurationTestBase
{
    #region FormConfiguration Tests

    [Fact]
    public void FormConfiguration_CanPersist_WithRequiredFields()
    {
        // Arrange
        var config = new FormConfiguration
        {
            Id = "form-config-1",
            TenantId = "tenant-1",
            EntityName = "Vehicle",
            Context = FormContext.Create,
            Config = "{\"fields\":[{\"name\":\"vin\",\"type\":\"string\"}]}",
            Version = 1,
            IsActive = true,
            IsSystemDefault = false,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.FormConfigurations.Add(config);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.FormConfigurations.First(f => f.Id == "form-config-1");
        Assert.Equal("tenant-1", loaded.TenantId);
        Assert.Equal("Vehicle", loaded.EntityName);
        Assert.Equal(FormContext.Create, loaded.Context);
        Assert.Contains("vin", loaded.Config);
        Assert.Equal(1, loaded.Version);
        Assert.True(loaded.IsActive);
        Assert.False(loaded.IsSystemDefault);
        Assert.Null(loaded.DerivedFromConfigId);
        Assert.Null(loaded.Tags);
        Assert.Null(loaded.Notes);
    }

    [Fact]
    public void FormConfiguration_CanPersist_WithAllFields()
    {
        // Arrange
        var config = new FormConfiguration
        {
            Id = "form-config-2",
            TenantId = "tenant-1",
            EntityName = "Passenger",
            Context = FormContext.Edit,
            Config = "{\"fields\":[{\"name\":\"name\",\"required\":true}]}",
            Version = 3,
            IsActive = true,
            IsSystemDefault = true,
            DerivedFromConfigId = "form-config-base",
            Tags = new List<string> { "medicaid", "priority", "accessibility" },
            Notes = "Custom configuration for Medicaid passengers",
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.FormConfigurations.Add(config);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.FormConfigurations.First(f => f.Id == "form-config-2");
        Assert.Equal(FormContext.Edit, loaded.Context);
        Assert.Equal(3, loaded.Version);
        Assert.True(loaded.IsSystemDefault);
        Assert.Equal("form-config-base", loaded.DerivedFromConfigId);
        Assert.NotNull(loaded.Tags);
        Assert.Equal(3, loaded.Tags.Count);
        Assert.Contains("medicaid", loaded.Tags);
        Assert.Equal("Custom configuration for Medicaid passengers", loaded.Notes);
    }

    [Fact]
    public void FormConfiguration_CanPersist_AllFormContexts()
    {
        var contexts = Enum.GetValues<FormContext>();
        var idx = 0;

        foreach (var context in contexts)
        {
            var config = new FormConfiguration
            {
                Id = $"form-config-ctx-{idx++}",
                TenantId = "tenant-1",
                EntityName = "Trip",
                Context = context,
                Config = "{}",
                CreatedAt = DateTimeOffset.UtcNow
            };

            Context.FormConfigurations.Add(config);
        }

        Context.SaveChanges();

        using var newContext = CreateNewContext();
        Assert.Equal(contexts.Length, newContext.FormConfigurations.Count());
    }

    [Fact]
    public void FormConfiguration_CanUpdate_Tags()
    {
        // Arrange
        var config = new FormConfiguration
        {
            Id = "form-config-tags",
            TenantId = "tenant-1",
            EntityName = "Driver",
            Context = FormContext.View,
            Config = "{}",
            Tags = new List<string> { "original" },
            CreatedAt = DateTimeOffset.UtcNow
        };

        Context.FormConfigurations.Add(config);
        Context.SaveChanges();

        // Act
        using var updateContext = CreateNewContext();
        var loaded = updateContext.FormConfigurations.First(f => f.Id == "form-config-tags");
        loaded.Tags = new List<string> { "updated", "new-tag" };
        updateContext.SaveChanges();

        // Assert
        using var verifyContext = CreateNewContext();
        var verified = verifyContext.FormConfigurations.First(f => f.Id == "form-config-tags");
        Assert.Equal(2, verified.Tags!.Count);
        Assert.Contains("updated", verified.Tags);
    }

    #endregion

    #region ProcedureDefinition Tests

    [Fact]
    public void ProcedureDefinition_CanPersist_WithRequiredFields()
    {
        // Arrange
        var procDef = new ProcedureDefinition
        {
            Id = "proc-def-1",
            Name = "PassengerSignature",
            Description = "Requires the passenger's signature at the stop"
        };

        // Act
        Context.ProcedureDefinitions.Add(procDef);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.ProcedureDefinitions.First(p => p.Id == "proc-def-1");
        Assert.Equal("PassengerSignature", loaded.Name);
        Assert.Equal("Requires the passenger's signature at the stop", loaded.Description);
    }

    [Fact]
    public void ProcedureDefinition_EnforcesUniqueNameConstraint()
    {
        // Arrange
        var proc1 = new ProcedureDefinition
        {
            Id = "proc-def-u1",
            Name = "UniqueProcedure",
            Description = "First procedure"
        };

        var proc2 = new ProcedureDefinition
        {
            Id = "proc-def-u2",
            Name = "UniqueProcedure",
            Description = "Duplicate name"
        };

        // Act & Assert
        Context.ProcedureDefinitions.Add(proc1);
        Context.SaveChanges();

        Context.ProcedureDefinitions.Add(proc2);
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => Context.SaveChanges());
    }

    #endregion

    #region ProcedureSet Tests

    [Fact]
    public void ProcedureSet_CanPersist_WithEmptyProcedures()
    {
        // Arrange
        var procSet = new ProcedureSet
        {
            Id = "proc-set-1",
            TenantId = "tenant-1",
            Name = "Empty Procedure Set",
            Procedures = new List<ProcedureRule>(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.ProcedureSets.Add(procSet);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.ProcedureSets.First(p => p.Id == "proc-set-1");
        Assert.Equal("Empty Procedure Set", loaded.Name);
        Assert.Empty(loaded.Procedures);
    }

    [Fact]
    public void ProcedureSet_CanPersist_WithProcedureRules()
    {
        // Arrange
        var procSet = new ProcedureSet
        {
            Id = "proc-set-2",
            TenantId = "tenant-1",
            Name = "Medicaid Standard Procedures",
            Procedures = new List<ProcedureRule>
            {
                new ProcedureRule(StopProcedureType.PassengerSignature, ProcedureAppliesTo.Any),
                new ProcedureRule(StopProcedureType.PhotoOfDropoff, ProcedureAppliesTo.Dropoff),
                new ProcedureRule(StopProcedureType.ScanPatientId, ProcedureAppliesTo.Pickup),
                new ProcedureRule(StopProcedureType.SecureMobilityDevice, ProcedureAppliesTo.Any)
            },
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.ProcedureSets.Add(procSet);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.ProcedureSets.First(p => p.Id == "proc-set-2");
        Assert.Equal(4, loaded.Procedures.Count);

        var signatureRule = loaded.Procedures.First(p => p.ProcedureId == StopProcedureType.PassengerSignature);
        Assert.Equal(ProcedureAppliesTo.Any, signatureRule.AppliesTo);

        var photoRule = loaded.Procedures.First(p => p.ProcedureId == StopProcedureType.PhotoOfDropoff);
        Assert.Equal(ProcedureAppliesTo.Dropoff, photoRule.AppliesTo);
    }

    [Fact]
    public void ProcedureSet_CanPersist_AllProcedureTypes()
    {
        var procedureTypes = Enum.GetValues<StopProcedureType>();
        var rules = procedureTypes.Select(pt => new ProcedureRule(pt, ProcedureAppliesTo.Any)).ToList();

        var procSet = new ProcedureSet
        {
            Id = "proc-set-all",
            TenantId = "tenant-1",
            Name = "All Procedures",
            Procedures = rules,
            CreatedAt = DateTimeOffset.UtcNow
        };

        Context.ProcedureSets.Add(procSet);
        Context.SaveChanges();

        using var newContext = CreateNewContext();
        var loaded = newContext.ProcedureSets.First(p => p.Id == "proc-set-all");
        Assert.Equal(procedureTypes.Length, loaded.Procedures.Count);
    }

    [Fact]
    public void ProcedureSet_CanUpdate_Procedures()
    {
        // Arrange
        var procSet = new ProcedureSet
        {
            Id = "proc-set-update",
            TenantId = "tenant-1",
            Name = "Updatable Set",
            Procedures = new List<ProcedureRule>
            {
                new ProcedureRule(StopProcedureType.PassengerSignature, ProcedureAppliesTo.Any)
            },
            CreatedAt = DateTimeOffset.UtcNow
        };

        Context.ProcedureSets.Add(procSet);
        Context.SaveChanges();

        // Act
        using var updateContext = CreateNewContext();
        var loaded = updateContext.ProcedureSets.First(p => p.Id == "proc-set-update");
        loaded.Procedures = new List<ProcedureRule>
        {
            new ProcedureRule(StopProcedureType.CollectCopay, ProcedureAppliesTo.Pickup),
            new ProcedureRule(StopProcedureType.AssistDoorToDoor, ProcedureAppliesTo.Any)
        };
        updateContext.SaveChanges();

        // Assert
        using var verifyContext = CreateNewContext();
        var verified = verifyContext.ProcedureSets.First(p => p.Id == "proc-set-update");
        Assert.Equal(2, verified.Procedures.Count);
        Assert.Contains(verified.Procedures, p => p.ProcedureId == StopProcedureType.CollectCopay);
    }

    #endregion

    #region ViewConfiguration Tests

    [Fact]
    public void ViewConfiguration_CanPersist_WithMinimalPreferences()
    {
        // Arrange
        var viewConfig = new ViewConfiguration
        {
            Id = "view-config-1",
            TenantId = "tenant-1",
            UserId = "user-1",
            ViewId = "trips_list",
            Preferences = new ViewPreferences(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.ViewConfigurations.Add(viewConfig);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.ViewConfigurations.First(v => v.Id == "view-config-1");
        Assert.Equal("user-1", loaded.UserId);
        Assert.Equal("trips_list", loaded.ViewId);
        Assert.Null(loaded.EntityName);
        Assert.NotNull(loaded.Preferences);
    }

    [Fact]
    public void ViewConfiguration_CanPersist_WithFullPreferences()
    {
        // Arrange
        var viewConfig = new ViewConfiguration
        {
            Id = "view-config-2",
            TenantId = "tenant-1",
            UserId = "user-2",
            ViewId = "vehicles_table",
            EntityName = "Vehicle",
            Preferences = new ViewPreferences(
                columnOrder: new List<string> { "vin", "make", "model", "year", "status" },
                columnVisibility: new Dictionary<string, bool>
                {
                    { "vin", true },
                    { "make", true },
                    { "model", true },
                    { "year", false },
                    { "status", true }
                },
                columnSizes: new Dictionary<string, int>
                {
                    { "vin", 200 },
                    { "make", 150 },
                    { "model", 150 },
                    { "status", 100 }
                },
                sorting: new List<SortingState>
                {
                    new SortingState("vin", false),
                    new SortingState("status", true)
                }
            ),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.ViewConfigurations.Add(viewConfig);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.ViewConfigurations.First(v => v.Id == "view-config-2");
        Assert.Equal("Vehicle", loaded.EntityName);

        var prefs = loaded.Preferences;
        Assert.NotNull(prefs.ColumnOrder);
        Assert.Equal(5, prefs.ColumnOrder.Count);
        Assert.Equal("vin", prefs.ColumnOrder[0]);

        Assert.NotNull(prefs.ColumnVisibility);
        Assert.False(prefs.ColumnVisibility["year"]);
        Assert.True(prefs.ColumnVisibility["status"]);

        Assert.NotNull(prefs.ColumnSizes);
        Assert.Equal(200, prefs.ColumnSizes["vin"]);

        Assert.NotNull(prefs.Sorting);
        Assert.Equal(2, prefs.Sorting.Count);
        Assert.False(prefs.Sorting[0].Descending);
        Assert.True(prefs.Sorting[1].Descending);
    }

    [Fact]
    public void ViewConfiguration_EnforcesUniqueUserViewConstraint()
    {
        // Arrange
        var config1 = new ViewConfiguration
        {
            Id = "view-config-dup-1",
            TenantId = "tenant-1",
            UserId = "user-dup",
            ViewId = "same_view",
            Preferences = new ViewPreferences(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        var config2 = new ViewConfiguration
        {
            Id = "view-config-dup-2",
            TenantId = "tenant-1",
            UserId = "user-dup",
            ViewId = "same_view",
            Preferences = new ViewPreferences(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act & Assert
        Context.ViewConfigurations.Add(config1);
        Context.SaveChanges();

        Context.ViewConfigurations.Add(config2);
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => Context.SaveChanges());
    }

    [Fact]
    public void ViewConfiguration_CanUpdate_Preferences()
    {
        // Arrange
        var viewConfig = new ViewConfiguration
        {
            Id = "view-config-update",
            TenantId = "tenant-1",
            UserId = "user-update",
            ViewId = "drivers_grid",
            Preferences = new ViewPreferences(
                columnOrder: new List<string> { "name", "status" }
            ),
            CreatedAt = DateTimeOffset.UtcNow
        };

        Context.ViewConfigurations.Add(viewConfig);
        Context.SaveChanges();

        // Act
        using var updateContext = CreateNewContext();
        var loaded = updateContext.ViewConfigurations.First(v => v.Id == "view-config-update");
        loaded.Preferences = new ViewPreferences(
            columnOrder: new List<string> { "status", "name", "phone" },
            sorting: new List<SortingState> { new SortingState("status", false) }
        );
        updateContext.SaveChanges();

        // Assert
        using var verifyContext = CreateNewContext();
        var verified = verifyContext.ViewConfigurations.First(v => v.Id == "view-config-update");
        Assert.Equal(3, verified.Preferences.ColumnOrder!.Count);
        Assert.Equal("status", verified.Preferences.ColumnOrder[0]);
        Assert.NotNull(verified.Preferences.Sorting);
        Assert.Single(verified.Preferences.Sorting);
    }

    #endregion
}
