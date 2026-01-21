namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Fleet entities:
/// Vehicle, Equipment, FuelLog, MaintenanceRecord, VehicleCredential, VehicleInspection, VehicleTelemetry.
/// </summary>
public abstract class FleetTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly FleetOnlyDbContext Context;

    protected FleetTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<FleetOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new FleetOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected FleetOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<FleetOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new FleetOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
