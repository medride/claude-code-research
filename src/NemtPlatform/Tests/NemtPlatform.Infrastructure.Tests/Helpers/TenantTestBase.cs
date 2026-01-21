namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Tenant entity in isolation.
/// Uses TenantOnlyDbContext to avoid configuring all other entities.
/// </summary>
public abstract class TenantTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly TenantOnlyDbContext Context;

    protected TenantTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<TenantOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new TenantOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected TenantOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<TenantOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new TenantOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
