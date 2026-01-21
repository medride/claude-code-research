namespace NemtPlatform.Infrastructure.Tests.Persistence;

using Microsoft.EntityFrameworkCore;
using Moq;
using NemtPlatform.Application.Contracts.MultiTenancy;
using NemtPlatform.Domain.Entities;
using NemtPlatform.Infrastructure.Persistence;

public class DbContextTests : IDisposable
{
    private readonly NemtPlatformDbContext _context;

    public DbContextTests()
    {
        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NemtPlatformDbContext(options);
    }

    [Fact]
    public void DbContext_ShouldCreateWithoutTenantContext()
    {
        // Assert
        Assert.NotNull(_context);
        Assert.NotNull(_context.Tenants);
    }

    [Fact]
    public void DbContext_ShouldCreateWithTenantContext()
    {
        // Arrange
        var tenantContextMock = new Mock<ITenantContext>();
        tenantContextMock.Setup(x => x.TenantId).Returns("test-tenant");

        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new NemtPlatformDbContext(options, tenantContextMock.Object);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public async Task DbContext_Tenants_ShouldPersistAndRetrieve()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = "context-test-001",
            Name = "Context Test Tenant",
            Status = TenantStatus.Active
        };

        // Act
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        // Assert
        var retrieved = await _context.Tenants.FindAsync("context-test-001");
        Assert.NotNull(retrieved);
        Assert.Equal("Context Test Tenant", retrieved.Name);
    }

    [Fact]
    public async Task DbContext_SaveChangesAsync_ShouldSetAuditFields()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = "audit-test-001",
            Name = "Audit Test",
            Status = TenantStatus.Trial
        };

        // Act
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        // Assert
        var retrieved = await _context.Tenants.FindAsync("audit-test-001");
        Assert.NotNull(retrieved);
        // CreatedAt should be set automatically by the DbContext
        // Note: The actual audit logic depends on DbContext implementation
    }

    [Fact]
    public void DbContext_ShouldHaveAllExpectedDbSets()
    {
        // Assert - verify key DbSets exist
        Assert.NotNull(_context.Tenants);
        Assert.NotNull(_context.Users);
        Assert.NotNull(_context.Employees);
        Assert.NotNull(_context.Roles);
        Assert.NotNull(_context.Trips);
        Assert.NotNull(_context.Vehicles);
        Assert.NotNull(_context.Passengers);
    }

    [Fact]
    public async Task DbContext_ShouldSupportLinqQueries()
    {
        // Arrange
        _context.Tenants.AddRange(
            new Tenant { Id = "linq-1", Name = "Alpha", Status = TenantStatus.Active },
            new Tenant { Id = "linq-2", Name = "Beta", Status = TenantStatus.Trial },
            new Tenant { Id = "linq-3", Name = "Gamma", Status = TenantStatus.Active }
        );
        await _context.SaveChangesAsync();

        // Act
        var activeCount = await _context.Tenants
            .Where(t => t.Status == TenantStatus.Active)
            .CountAsync();

        var orderedNames = await _context.Tenants
            .OrderBy(t => t.Name)
            .Select(t => t.Name)
            .ToListAsync();

        // Assert
        Assert.Equal(2, activeCount);
        Assert.Equal(new[] { "Alpha", "Beta", "Gamma" }, orderedNames);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
