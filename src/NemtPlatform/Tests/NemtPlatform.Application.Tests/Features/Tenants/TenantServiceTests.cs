namespace NemtPlatform.Application.Tests.Features.Tenants;

using Moq;
using NemtPlatform.Application.Contracts.Persistence;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Domain.Entities;

/// <summary>
/// Example tests demonstrating service layer testing with mocked repositories.
/// </summary>
public class TenantServiceTests
{
    private readonly Mock<IRepository<Tenant>> _tenantRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public TenantServiceTests()
    {
        _tenantRepositoryMock = new Mock<IRepository<Tenant>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task GetTenantById_ExistingTenant_ShouldReturnTenant()
    {
        // Arrange
        var tenantId = "tenant-001";
        var tenant = new Tenant
        {
            Id = tenantId,
            Name = "Test Company",
            Status = TenantStatus.Active
        };

        _tenantRepositoryMock
            .Setup(r => r.GetByIdAsync(tenantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenant);

        // Act
        var result = await _tenantRepositoryMock.Object.GetByIdAsync(tenantId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Company", result.Name);
        Assert.Equal(TenantStatus.Active, result.Status);
    }

    [Fact]
    public async Task GetAllTenants_ShouldReturnAllTenants()
    {
        // Arrange
        var tenants = new List<Tenant>
        {
            new() { Id = "t1", Name = "Company A", Status = TenantStatus.Active },
            new() { Id = "t2", Name = "Company B", Status = TenantStatus.Trial },
            new() { Id = "t3", Name = "Company C", Status = TenantStatus.Active }
        };

        _tenantRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenants);

        // Act
        var result = await _tenantRepositoryMock.Object.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task CreateTenant_ShouldAddAndSave()
    {
        // Arrange
        Tenant? capturedTenant = null;
        _tenantRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Tenant>(), It.IsAny<CancellationToken>()))
            .Callback<Tenant, CancellationToken>((t, _) => capturedTenant = t)
            .ReturnsAsync((Tenant t, CancellationToken _) => t);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var newTenant = new Tenant
        {
            Id = "new-tenant",
            Name = "New Company",
            Status = TenantStatus.Trial,
            PrimaryContact = new TenantContact("John Doe", "john@company.com", "555-1234")
        };

        await _tenantRepositoryMock.Object.AddAsync(newTenant);
        await _unitOfWorkMock.Object.SaveChangesAsync();

        // Assert
        Assert.NotNull(capturedTenant);
        Assert.Equal("New Company", capturedTenant.Name);
        Assert.Equal(TenantStatus.Trial, capturedTenant.Status);
        Assert.NotNull(capturedTenant.PrimaryContact);
        Assert.Equal("john@company.com", capturedTenant.PrimaryContact.Email);
    }

    [Fact]
    public async Task UpdateTenantStatus_ShouldModifyAndSave()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = "tenant-upgrade",
            Name = "Upgrading Company",
            Status = TenantStatus.Trial
        };

        _tenantRepositoryMock
            .Setup(r => r.GetByIdAsync("tenant-upgrade", It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenant);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act - Simulating status upgrade
        var fetchedTenant = await _tenantRepositoryMock.Object.GetByIdAsync("tenant-upgrade");
        fetchedTenant!.Status = TenantStatus.Active;
        await _tenantRepositoryMock.Object.UpdateAsync(fetchedTenant);
        await _unitOfWorkMock.Object.SaveChangesAsync();

        // Assert
        Assert.Equal(TenantStatus.Active, tenant.Status);
        _tenantRepositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Tenant>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task FindTenantsByStatus_ShouldReturnFilteredResults()
    {
        // Arrange
        var activeTenants = new List<Tenant>
        {
            new() { Id = "t1", Name = "Active 1", Status = TenantStatus.Active },
            new() { Id = "t2", Name = "Active 2", Status = TenantStatus.Active }
        };

        _tenantRepositoryMock
            .Setup(r => r.FindAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Tenant, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(activeTenants);

        // Act
        var result = await _tenantRepositoryMock.Object.FindAsync(t => t.Status == TenantStatus.Active);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(TenantStatus.Active, t.Status));
    }
}
