namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base class for testing Billing entities:
/// Authorization, Claim, EligibilityRecord, FundingSource, Partner, PartnerContract, Remittance, ServiceCode.
/// </summary>
public abstract class BillingTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly BillingOnlyDbContext Context;

    protected BillingTestBase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<BillingOnlyDbContext>()
            .UseSqlite(_connection)
            .EnableDetailedErrors()
            .Options;

        Context = new BillingOnlyDbContext(options);
        Context.Database.EnsureCreated();
    }

    protected BillingOnlyDbContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<BillingOnlyDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new BillingOnlyDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
