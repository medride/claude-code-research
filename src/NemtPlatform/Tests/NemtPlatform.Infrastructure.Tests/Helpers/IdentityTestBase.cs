namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing User, Role, and Permission entities in isolation.
/// Uses IdentityOnlyDbContext to avoid configuring all other entities.
/// </summary>
public abstract class IdentityTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly IdentityOnlyDbContext Context;

    protected IdentityTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<IdentityOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new IdentityOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected IdentityOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<IdentityOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new IdentityOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
