namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Passengers;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;

public class PassengersTests : PassengersTestBase
{
    #region Passenger Tests

    [Fact]
    public async Task Can_Create_Passenger_With_Required_Fields()
    {
        var passenger = new Passenger
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = new PersonName("John", "Doe")
        };

        Context.Passengers.Add(passenger);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Passengers.FindAsync(passenger.Id);

        Assert.NotNull(loaded);
        Assert.Equal("John", loaded.Name.First);
        Assert.Equal("Doe", loaded.Name.Last);
        Assert.Equal("John Doe", loaded.Name.FullName);
    }

    [Fact]
    public async Task Can_Create_Passenger_With_All_Fields()
    {
        var passenger = new Passenger
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PartnerId = "partner-001",
            UserId = "user-001",
            Name = new PersonName("Jane", "Smith"),
            PhoneNumber = "555-1234",
            DateOfBirth = new DateTimeOffset(1990, 5, 15, 0, 0, 0, TimeSpan.Zero),
            Gender = Gender.Female,
            PatientProfileId = "patient-001",
            StudentProfileId = "student-001",
            EmergencyContactIds = new List<string> { "contact-1", "contact-2" }
        };

        Context.Passengers.Add(passenger);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Passengers.FindAsync(passenger.Id);

        Assert.NotNull(loaded);
        Assert.Equal("partner-001", loaded.PartnerId);
        Assert.Equal("user-001", loaded.UserId);
        Assert.Equal("555-1234", loaded.PhoneNumber);
        Assert.Equal(Gender.Female, loaded.Gender);
        Assert.NotNull(loaded.EmergencyContactIds);
        Assert.Equal(2, loaded.EmergencyContactIds.Count);
    }

    [Fact]
    public async Task Can_Create_Passenger_With_TripConstraints()
    {
        var constraints = new TripConstraints
        {
            Preferences = new ConstraintSet
            {
                Driver = new DriverConstraints { Ids = new List<string> { "driver-1" } },
                Vehicle = new VehicleConstraints { Ids = new List<string> { "vehicle-1" } }
            }
        };

        var passenger = new Passenger
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = new PersonName("Test", "Passenger"),
            Constraints = constraints
        };

        Context.Passengers.Add(passenger);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Passengers.FindAsync(passenger.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Constraints);
        Assert.NotNull(loaded.Constraints.Preferences);
        Assert.NotNull(loaded.Constraints.Preferences.Driver);
        Assert.Contains("driver-1", loaded.Constraints.Preferences.Driver.Ids!);
    }

    #endregion

    #region Guardian Tests

    [Fact]
    public async Task Can_Create_Guardian()
    {
        var guardian = new Guardian
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            UserId = "user-guardian-001"
        };

        Context.Guardians.Add(guardian);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Guardians.FindAsync(guardian.Id);

        Assert.NotNull(loaded);
        Assert.Equal("tenant-001", loaded.TenantId);
        Assert.Equal("user-guardian-001", loaded.UserId);
    }

    #endregion

    #region GuardianPassengerLink Tests

    [Fact]
    public async Task Can_Create_GuardianPassengerLink_With_Required_Fields()
    {
        var link = new GuardianPassengerLink
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            GuardianId = "guardian-001",
            PassengerId = "passenger-001",
            Relationship = GuardianRelationship.Parent,
            Permissions = new GuardianPermissions(
                canManageSchedule: true,
                canManageBilling: false,
                canViewHistory: true,
                isPrimaryContact: true
            )
        };

        Context.GuardianPassengerLinks.Add(link);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.GuardianPassengerLinks.FindAsync(link.Id);

        Assert.NotNull(loaded);
        Assert.Equal(GuardianRelationship.Parent, loaded.Relationship);
        Assert.True(loaded.Permissions.CanManageSchedule);
        Assert.False(loaded.Permissions.CanManageBilling);
        Assert.True(loaded.Permissions.IsPrimaryContact);
    }

    [Fact]
    public async Task Can_Create_GuardianPassengerLink_With_NotificationPreferences()
    {
        var link = new GuardianPassengerLink
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            GuardianId = "guardian-001",
            PassengerId = "passenger-002",
            Relationship = GuardianRelationship.LegalGuardian,
            Permissions = new GuardianPermissions(),
            NotificationPreferences = new NotificationPreferences(
                channels: new List<NotificationChannel> { NotificationChannel.Sms, NotificationChannel.Email },
                quietHours: new QuietHours(
                    new TimeOnly(22, 0),
                    new TimeOnly(7, 0),
                    "America/New_York"
                )
            )
        };

        Context.GuardianPassengerLinks.Add(link);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.GuardianPassengerLinks.FindAsync(link.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.NotificationPreferences);
        Assert.Equal(2, loaded.NotificationPreferences.Channels.Count);
        Assert.Contains(NotificationChannel.Sms, loaded.NotificationPreferences.Channels);
        Assert.NotNull(loaded.NotificationPreferences.QuietHours);
    }

    [Fact]
    public async Task All_GuardianRelationships_Can_Be_Stored()
    {
        var relationships = new[] {
            GuardianRelationship.Parent,
            GuardianRelationship.LegalGuardian,
            GuardianRelationship.Caseworker,
            GuardianRelationship.Other
        };

        foreach (var relationship in relationships)
        {
            var link = new GuardianPassengerLink
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                GuardianId = $"guardian-{relationship}",
                PassengerId = $"passenger-{relationship}",
                Relationship = relationship,
                Permissions = new GuardianPermissions()
            };

            Context.GuardianPassengerLinks.Add(link);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.GuardianPassengerLinks.FindAsync(link.Id);
            Assert.Equal(relationship, loaded!.Relationship);
        }
    }

    [Fact]
    public async Task GuardianPassengerLink_Unique_Constraint()
    {
        var link1 = new GuardianPassengerLink
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            GuardianId = "guardian-unique",
            PassengerId = "passenger-unique",
            Relationship = GuardianRelationship.Parent,
            Permissions = new GuardianPermissions()
        };

        var link2 = new GuardianPassengerLink
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            GuardianId = "guardian-unique",
            PassengerId = "passenger-unique",
            Relationship = GuardianRelationship.Other,
            Permissions = new GuardianPermissions()
        };

        Context.GuardianPassengerLinks.Add(link1);
        await Context.SaveChangesAsync();

        Context.GuardianPassengerLinks.Add(link2);
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(
            () => Context.SaveChangesAsync());
    }

    #endregion
}
