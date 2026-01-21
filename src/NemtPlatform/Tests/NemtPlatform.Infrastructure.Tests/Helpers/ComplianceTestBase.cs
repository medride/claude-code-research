namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Compliance entities:
/// AttributeDefinition, Credential (system-wide), DriverAttribute, EmployeeCredential, InspectionTemplate.
/// </summary>
public abstract class ComplianceTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly ComplianceOnlyDbContext Context;

    protected ComplianceTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ComplianceOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new ComplianceOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected ComplianceOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<ComplianceOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new ComplianceOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
