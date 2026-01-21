namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Execution entities:
/// AuditLog, Document, Incident, TripExecution.
/// </summary>
public abstract class ExecutionTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly ExecutionOnlyDbContext Context;

    protected ExecutionTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ExecutionOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new ExecutionOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected ExecutionOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<ExecutionOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new ExecutionOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
