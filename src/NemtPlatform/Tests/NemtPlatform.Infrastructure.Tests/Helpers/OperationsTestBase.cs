namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Operations entities:
/// Trip, Route, RouteManifest, Shift, ShiftSession, Journey, StandingOrder.
/// </summary>
public abstract class OperationsTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly OperationsOnlyDbContext Context;

    protected OperationsTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<OperationsOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new OperationsOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected OperationsOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<OperationsOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new OperationsOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
