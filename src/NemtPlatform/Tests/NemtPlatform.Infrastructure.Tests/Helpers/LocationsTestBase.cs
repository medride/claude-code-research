namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Place, AccessPoint, and Region entities.
/// </summary>
public abstract class LocationsTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly LocationsOnlyDbContext Context;

    protected LocationsTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<LocationsOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new LocationsOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected LocationsOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<LocationsOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new LocationsOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
