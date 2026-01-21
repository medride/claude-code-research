namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Passenger, Guardian, and GuardianPassengerLink entities.
/// </summary>
public abstract class PassengersTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly PassengersOnlyDbContext Context;

    protected PassengersTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<PassengersOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new PassengersOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected PassengersOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<PassengersOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new PassengersOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
