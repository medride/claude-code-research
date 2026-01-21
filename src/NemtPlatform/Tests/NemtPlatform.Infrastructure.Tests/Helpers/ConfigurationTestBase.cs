namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Configuration entities:
/// FormConfiguration, ProcedureDefinition (system-wide), ProcedureSet, ViewConfiguration.
/// </summary>
public abstract class ConfigurationTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly ConfigurationOnlyDbContext Context;

    protected ConfigurationTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ConfigurationOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new ConfigurationOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected ConfigurationOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<ConfigurationOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new ConfigurationOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
