namespace NemtPlatform.Infrastructure.Tests.Persistence;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities;
using NemtPlatform.Infrastructure.Persistence;

public class RepositoryTests : IDisposable
{
    private readonly NemtPlatformDbContext _context;
    private readonly Repository<Tenant> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<NemtPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NemtPlatformDbContext(options);
        _repository = new Repository<Tenant>(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityToContext()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = "tenant-001",
            Name = "Test Tenant",
            Status = TenantStatus.Active
        };

        // Act
        var result = await _repository.AddAsync(tenant);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("tenant-001", result.Id);
        Assert.Equal(1, await _context.Tenants.CountAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingEntity_ShouldReturnEntity()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = "tenant-002",
            Name = "Test Tenant",
            Status = TenantStatus.Active
        };
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync("tenant-002");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Tenant", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingEntity_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync("non-existing-id");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        _context.Tenants.AddRange(
            new Tenant { Id = "t1", Name = "Tenant 1", Status = TenantStatus.Active },
            new Tenant { Id = "t2", Name = "Tenant 2", Status = TenantStatus.Trial },
            new Tenant { Id = "t3", Name = "Tenant 3", Status = TenantStatus.Active }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task FindAsync_WithPredicate_ShouldReturnMatchingEntities()
    {
        // Arrange
        _context.Tenants.AddRange(
            new Tenant { Id = "t1", Name = "Tenant 1", Status = TenantStatus.Active },
            new Tenant { Id = "t2", Name = "Tenant 2", Status = TenantStatus.Trial },
            new Tenant { Id = "t3", Name = "Tenant 3", Status = TenantStatus.Active }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.FindAsync(t => t.Status == TenantStatus.Active);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(TenantStatus.Active, t.Status));
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyEntity()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = "tenant-update",
            Name = "Original Name",
            Status = TenantStatus.Trial
        };
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        _context.Entry(tenant).State = EntityState.Detached;

        // Act
        var entityToUpdate = await _repository.GetByIdAsync("tenant-update");
        entityToUpdate!.Name = "Updated Name";
        entityToUpdate.Status = TenantStatus.Active;
        await _repository.UpdateAsync(entityToUpdate);
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _context.Tenants.FindAsync("tenant-update");
        Assert.Equal("Updated Name", updated!.Name);
        Assert.Equal(TenantStatus.Active, updated.Status);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        var tenant = new Tenant
        {
            Id = "tenant-delete",
            Name = "To Delete",
            Status = TenantStatus.Active
        };
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(tenant);
        await _context.SaveChangesAsync();

        // Assert
        Assert.Equal(0, await _context.Tenants.CountAsync());
    }

    [Fact]
    public async Task ExistsAsync_ExistingEntity_ShouldReturnTrue()
    {
        // Arrange
        var tenant = new Tenant { Id = "exists-001", Name = "Existing", Status = TenantStatus.Active };
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync("exists-001");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_NonExistingEntity_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsAsync("non-existing-id");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        _context.Tenants.AddRange(
            new Tenant { Id = "c1", Name = "Tenant 1", Status = TenantStatus.Active },
            new Tenant { Id = "c2", Name = "Tenant 2", Status = TenantStatus.Active }
        );
        await _context.SaveChangesAsync();

        // Act
        var count = await _repository.CountAsync();

        // Assert
        Assert.Equal(2, count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
