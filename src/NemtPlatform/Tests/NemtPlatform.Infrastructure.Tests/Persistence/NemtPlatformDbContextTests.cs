namespace NemtPlatform.Infrastructure.Tests.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NemtPlatform.Infrastructure.Persistence;
using Xunit;

/// <summary>
/// Integration tests for NemtPlatformDbContext to validate schema creation
/// and entity configurations work correctly with all domain entities.
/// </summary>
public class NemtPlatformDbContextTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly NemtPlatformDbContext _context;

    public NemtPlatformDbContextTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;

        _context = new NemtPlatformDbContext(options);
    }

    [Fact]
    public void CanCreateDatabaseSchema()
    {
        // This test will fail if any entity configuration is invalid
        // The error message will tell us exactly which entity/property has issues
        _context.Database.EnsureCreated();

        // If we get here, schema was created successfully
        Assert.True(true);
    }

    [Fact]
    public void AllDbSetsAreAccessible()
    {
        _context.Database.EnsureCreated();

        // Core
        Assert.NotNull(_context.Tenants);

        // Identity
        Assert.NotNull(_context.Users);
        Assert.NotNull(_context.Employees);
        Assert.NotNull(_context.DriverProfiles);
        Assert.NotNull(_context.Roles);
        Assert.NotNull(_context.Permissions);
        Assert.NotNull(_context.PartnerUsers);

        // Passengers
        Assert.NotNull(_context.Passengers);
        Assert.NotNull(_context.PatientProfiles);
        Assert.NotNull(_context.StudentProfiles);
        Assert.NotNull(_context.Guardians);
        Assert.NotNull(_context.GuardianPassengerLinks);
        Assert.NotNull(_context.EmergencyContacts);
        Assert.NotNull(_context.TripCompanions);

        // Locations
        Assert.NotNull(_context.Places);
        Assert.NotNull(_context.AccessPoints);
        Assert.NotNull(_context.Regions);

        // Operations
        Assert.NotNull(_context.Trips);
        Assert.NotNull(_context.Routes);
        Assert.NotNull(_context.RouteManifests);
        Assert.NotNull(_context.Shifts);
        Assert.NotNull(_context.ShiftSessions);
        Assert.NotNull(_context.Journeys);
        Assert.NotNull(_context.StandingOrders);

        // Fleet
        Assert.NotNull(_context.Vehicles);
        Assert.NotNull(_context.VehicleCredentials);
        Assert.NotNull(_context.Equipment);
        Assert.NotNull(_context.MaintenanceRecords);
        Assert.NotNull(_context.VehicleInspections);
        Assert.NotNull(_context.FuelLogs);
        Assert.NotNull(_context.VehicleTelemetry);

        // Billing
        Assert.NotNull(_context.FundingSources);
        Assert.NotNull(_context.Partners);
        Assert.NotNull(_context.PartnerContracts);
        Assert.NotNull(_context.Authorizations);
        Assert.NotNull(_context.EligibilityRecords);
        Assert.NotNull(_context.ServiceCodes);
        Assert.NotNull(_context.Claims);
        Assert.NotNull(_context.Remittances);

        // Execution
        Assert.NotNull(_context.TripExecutions);
        Assert.NotNull(_context.Incidents);
        Assert.NotNull(_context.AuditLogs);
        Assert.NotNull(_context.Documents);

        // Compliance
        Assert.NotNull(_context.Credentials);
        Assert.NotNull(_context.EmployeeCredentials);
        Assert.NotNull(_context.AttributeDefinitions);
        Assert.NotNull(_context.DriverAttributes);
        Assert.NotNull(_context.InspectionTemplates);

        // Configuration
        Assert.NotNull(_context.ProcedureDefinitions);
        Assert.NotNull(_context.ProcedureSets);
        Assert.NotNull(_context.FormConfigurations);
        Assert.NotNull(_context.ViewConfigurations);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
