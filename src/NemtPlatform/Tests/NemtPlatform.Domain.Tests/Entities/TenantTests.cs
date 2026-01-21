namespace NemtPlatform.Domain.Tests.Entities;

using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Domain.Entities;

public class TenantTests
{
    [Fact]
    public void Tenant_NewInstance_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var tenant = new Tenant();

        // Assert
        Assert.Equal(string.Empty, tenant.Name);
        Assert.Equal(TenantStatus.Trial, tenant.Status);
        Assert.Null(tenant.PrimaryContact);
        Assert.Null(tenant.Address);
        Assert.Null(tenant.Settings);
    }

    [Fact]
    public void Tenant_SetBasicProperties_ShouldMaintainValues()
    {
        // Arrange & Act
        var tenant = new Tenant
        {
            Id = "tenant-001",
            Name = "Acme Transportation",
            Status = TenantStatus.Active
        };

        // Assert
        Assert.Equal("tenant-001", tenant.Id);
        Assert.Equal("Acme Transportation", tenant.Name);
        Assert.Equal(TenantStatus.Active, tenant.Status);
    }

    [Fact]
    public void Tenant_WithPrimaryContact_ShouldSetCorrectly()
    {
        // Arrange
        var contact = new TenantContact(
            Name: "John Smith",
            Email: "john@acme.com",
            Phone: "555-1234");

        // Act
        var tenant = new Tenant
        {
            PrimaryContact = contact
        };

        // Assert
        Assert.NotNull(tenant.PrimaryContact);
        Assert.Equal("John Smith", tenant.PrimaryContact.Name);
        Assert.Equal("john@acme.com", tenant.PrimaryContact.Email);
        Assert.Equal("555-1234", tenant.PrimaryContact.Phone);
    }

    [Fact]
    public void Tenant_WithAddress_ShouldSetCorrectly()
    {
        // Arrange
        var address = new Address(
            Street: "100 Business Park Drive",
            City: "Chicago",
            State: "IL",
            ZipCode: "60601");

        // Act
        var tenant = new Tenant
        {
            Address = address
        };

        // Assert
        Assert.NotNull(tenant.Address);
        Assert.Equal("Chicago", tenant.Address.City);
        Assert.Equal("IL", tenant.Address.State);
    }

    [Fact]
    public void Tenant_WithSettings_ShouldSetCorrectly()
    {
        // Arrange
        var settings = new TenantSettings
        {
            Regional = new RegionalSettings("America/Chicago", "USD"),
            Branding = new BrandingSettings(
                LogoUrl: "https://example.com/logo.png",
                PrimaryColor: "#007bff"),
            Inspections = new InspectionSettings(
                RequirePreShiftInspection: true,
                RequirePostShiftInspection: true)
        };

        // Act
        var tenant = new Tenant
        {
            Settings = settings
        };

        // Assert
        Assert.NotNull(tenant.Settings);
        Assert.NotNull(tenant.Settings.Regional);
        Assert.Equal("America/Chicago", tenant.Settings.Regional.Timezone);
        Assert.Equal("USD", tenant.Settings.Regional.Currency);
        Assert.NotNull(tenant.Settings.Branding);
        Assert.Equal("#007bff", tenant.Settings.Branding.PrimaryColor);
        Assert.NotNull(tenant.Settings.Inspections);
        Assert.True(tenant.Settings.Inspections.RequirePreShiftInspection);
    }

    [Theory]
    [InlineData(TenantStatus.Trial)]
    [InlineData(TenantStatus.Active)]
    [InlineData(TenantStatus.PastDue)]
    [InlineData(TenantStatus.Canceled)]
    public void Tenant_AllStatuses_ShouldBeValid(TenantStatus status)
    {
        // Arrange
        var tenant = new Tenant();

        // Act
        tenant.Status = status;

        // Assert
        Assert.Equal(status, tenant.Status);
    }
}
