namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities;
using NemtPlatform.Infrastructure.Tests.Helpers;

public class TenantTests : TenantTestBase
{
    [Fact]
    public async Task Can_Create_Tenant_With_Minimal_Data()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Company",
            Status = TenantStatus.Trial
        };

        // Act
        Context.Tenants.Add(tenant);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Tenants.FindAsync(tenant.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Test Company", loaded.Name);
        Assert.Equal(TenantStatus.Trial, loaded.Status);
    }

    [Fact]
    public async Task Can_Create_Tenant_With_PrimaryContact()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Company",
            Status = TenantStatus.Active,
            PrimaryContact = new TenantContact("John Doe", "john@test.com", "555-1234")
        };

        // Act
        Context.Tenants.Add(tenant);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Tenants.FindAsync(tenant.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.PrimaryContact);
        Assert.Equal("John Doe", loaded.PrimaryContact.Name);
        Assert.Equal("john@test.com", loaded.PrimaryContact.Email);
        Assert.Equal("555-1234", loaded.PrimaryContact.Phone);
    }

    [Fact]
    public async Task Can_Create_Tenant_With_Address()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Company",
            Status = TenantStatus.Active,
            Address = new NemtPlatform.Domain.Common.ValueObjects.Address(
                "123 Main St",
                "Springfield",
                "IL",
                "62701"
            )
        };

        // Act
        Context.Tenants.Add(tenant);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Tenants.FindAsync(tenant.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Address);
        Assert.Equal("123 Main St", loaded.Address.Street);
        Assert.Equal("Springfield", loaded.Address.City);
        Assert.Equal("IL", loaded.Address.State);
        Assert.Equal("62701", loaded.Address.ZipCode);
    }

    [Fact]
    public async Task Can_Create_Tenant_With_Settings()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Company",
            Status = TenantStatus.Active,
            Settings = new TenantSettings
            {
                Regional = new RegionalSettings("America/New_York", "USD"),
                Branding = new BrandingSettings("https://example.com/logo.png", "#005A9C"),
                Inspections = new InspectionSettings(
                    requirePreShiftInspection: true,
                    requirePostShiftInspection: false,
                    defaultPreShiftTemplateId: "template-1",
                    defaultPostShiftTemplateId: null
                )
            }
        };

        // Act
        Context.Tenants.Add(tenant);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Tenants.FindAsync(tenant.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Settings);

        // Regional settings
        Assert.NotNull(loaded.Settings.Regional);
        Assert.Equal("America/New_York", loaded.Settings.Regional.Timezone);
        Assert.Equal("USD", loaded.Settings.Regional.Currency);

        // Branding settings
        Assert.NotNull(loaded.Settings.Branding);
        Assert.Equal("https://example.com/logo.png", loaded.Settings.Branding.LogoUrl);
        Assert.Equal("#005A9C", loaded.Settings.Branding.PrimaryColor);

        // Inspection settings
        Assert.NotNull(loaded.Settings.Inspections);
        Assert.True(loaded.Settings.Inspections.RequirePreShiftInspection);
        Assert.False(loaded.Settings.Inspections.RequirePostShiftInspection);
        Assert.Equal("template-1", loaded.Settings.Inspections.DefaultPreShiftTemplateId);
        Assert.Null(loaded.Settings.Inspections.DefaultPostShiftTemplateId);
    }

    [Fact]
    public async Task Can_Create_Full_Tenant()
    {
        // Arrange - Full Tenant with all properties as per entities.ts
        var tenant = new Tenant
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Your NEMT Company",
            Status = TenantStatus.Active,
            PrimaryContact = new TenantContact("Jane Smith", "jane@nemt.com", "555-9876"),
            Address = new NemtPlatform.Domain.Common.ValueObjects.Address(
                "456 Business Ave",
                "Chicago",
                "IL",
                "60601"
            ),
            Settings = new TenantSettings
            {
                Regional = new RegionalSettings("America/Chicago", "USD"),
                Branding = new BrandingSettings("https://nemt.com/logo.png", "#FF5733"),
                Inspections = new InspectionSettings(
                    requirePreShiftInspection: true,
                    requirePostShiftInspection: true,
                    defaultPreShiftTemplateId: "pre-template",
                    defaultPostShiftTemplateId: "post-template"
                )
            }
        };

        // Act
        Context.Tenants.Add(tenant);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Tenants.FindAsync(tenant.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Your NEMT Company", loaded.Name);
        Assert.Equal(TenantStatus.Active, loaded.Status);

        // Contact
        Assert.NotNull(loaded.PrimaryContact);
        Assert.Equal("Jane Smith", loaded.PrimaryContact.Name);

        // Address
        Assert.NotNull(loaded.Address);
        Assert.Equal("456 Business Ave", loaded.Address.Street);

        // Settings
        Assert.NotNull(loaded.Settings);
        Assert.NotNull(loaded.Settings.Regional);
        Assert.NotNull(loaded.Settings.Branding);
        Assert.NotNull(loaded.Settings.Inspections);
    }

    [Fact]
    public async Task Can_Update_Tenant_Status()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Company",
            Status = TenantStatus.Trial
        };

        Context.Tenants.Add(tenant);
        await Context.SaveChangesAsync();

        // Act
        using var updateContext = CreateNewContext();
        var toUpdate = await updateContext.Tenants.FindAsync(tenant.Id);
        Assert.NotNull(toUpdate);

        toUpdate.Status = TenantStatus.Active;
        await updateContext.SaveChangesAsync();

        // Assert
        using var verifyContext = CreateNewContext();
        var loaded = await verifyContext.Tenants.FindAsync(tenant.Id);

        Assert.NotNull(loaded);
        Assert.Equal(TenantStatus.Active, loaded.Status);
    }

    [Fact]
    public async Task All_TenantStatus_Values_Can_Be_Stored()
    {
        // Test all status values from entities.ts: TRIAL, ACTIVE, PAST_DUE, CANCELED
        var statuses = new[] { TenantStatus.Trial, TenantStatus.Active, TenantStatus.PastDue, TenantStatus.Canceled };

        foreach (var status in statuses)
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"Tenant with {status}",
                Status = status
            };

            Context.Tenants.Add(tenant);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.Tenants.FindAsync(tenant.Id);

            Assert.NotNull(loaded);
            Assert.Equal(status, loaded.Status);
        }
    }
}
