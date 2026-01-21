namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NemtPlatform.Infrastructure.Persistence;

/// <summary>
/// Base class for tests that need a SQLite in-memory database.
/// Manages connection lifetime to ensure database persists for test duration.
/// </summary>
public abstract class SqliteTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly NemtPlatformDbContext Context;

    protected SqliteTestBase()
    {
        // SQLite in-memory databases are destroyed when the connection closes.
        // We must keep the connection open for the entire test lifetime.
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseSqlite(_connection)
            .ConfigureWarnings(warnings =>
            {
                // Ignore model validation warnings for complex types that are stored as JSON
                warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
                warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning);
            })
            .EnableDetailedErrors()
            .Options;

        Context = new NemtPlatformDbContext(options);

        // Create the database schema
        Context.Database.EnsureCreated();
    }

    /// <summary>
    /// Creates a new DbContext instance sharing the same connection.
    /// Useful for testing scenarios that require multiple context instances.
    /// </summary>
    protected NemtPlatformDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new NemtPlatformDbContext(options);
        return context;
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
