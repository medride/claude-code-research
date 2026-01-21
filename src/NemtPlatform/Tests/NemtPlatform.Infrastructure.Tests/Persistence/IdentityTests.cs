namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Identity;
using NemtPlatform.Infrastructure.Tests.Helpers;

public class IdentityTests : IdentityTestBase
{
    #region User Tests

    [Fact]
    public async Task Can_Create_User_With_Minimal_Data()
    {
        // Arrange - only required fields per entities.ts
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@example.com"
        };

        // Act
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Users.FindAsync(user.Id);

        Assert.NotNull(loaded);
        Assert.Equal("test@example.com", loaded.Email);
    }

    [Fact]
    public async Task Can_Create_User_With_All_Optional_Fields()
    {
        // Arrange - all fields per entities.ts
        var user = new User
        {
            Id = "firebase-uid-123",
            Email = "john@example.com",
            PhoneNumber = "555-1234",
            DisplayName = "John Doe",
            PhotoUrl = "https://example.com/photo.jpg",
            EmployeeId = "emp-001",
            PassengerId = "pass-001",
            GuardianId = "guard-001",
            PartnerUserId = "partner-001"
        };

        // Act
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Users.FindAsync(user.Id);

        Assert.NotNull(loaded);
        Assert.Equal("john@example.com", loaded.Email);
        Assert.Equal("555-1234", loaded.PhoneNumber);
        Assert.Equal("John Doe", loaded.DisplayName);
        Assert.Equal("https://example.com/photo.jpg", loaded.PhotoUrl);
        Assert.Equal("emp-001", loaded.EmployeeId);
        Assert.Equal("pass-001", loaded.PassengerId);
        Assert.Equal("guard-001", loaded.GuardianId);
        Assert.Equal("partner-001", loaded.PartnerUserId);
    }

    [Fact]
    public async Task User_Email_Must_Be_Unique()
    {
        // Arrange
        var user1 = new User { Id = Guid.NewGuid().ToString(), Email = "duplicate@example.com" };
        var user2 = new User { Id = Guid.NewGuid().ToString(), Email = "duplicate@example.com" };

        Context.Users.Add(user1);
        await Context.SaveChangesAsync();

        // Act & Assert
        Context.Users.Add(user2);
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(
            () => Context.SaveChangesAsync());
    }

    #endregion

    #region Permission Tests

    [Fact]
    public async Task Can_Create_Permission_With_Minimal_Data()
    {
        // Arrange - per entities.ts: id and name are required
        var permission = new Permission
        {
            Id = "trip:create",
            Name = "Create Trips"
        };

        // Act
        Context.Permissions.Add(permission);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Permissions.FindAsync(permission.Id);

        Assert.NotNull(loaded);
        Assert.Equal("trip:create", loaded.Id);
        Assert.Equal("Create Trips", loaded.Name);
    }

    [Fact]
    public async Task Can_Create_Permission_With_Description()
    {
        // Arrange
        var permission = new Permission
        {
            Id = "billing:view",
            Name = "View Billing Reports",
            Description = "Allows viewing of billing reports and financial data"
        };

        // Act
        Context.Permissions.Add(permission);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Permissions.FindAsync(permission.Id);

        Assert.NotNull(loaded);
        Assert.Equal("View Billing Reports", loaded.Name);
        Assert.Equal("Allows viewing of billing reports and financial data", loaded.Description);
    }

    [Fact]
    public async Task Permission_Name_Must_Be_Unique()
    {
        // Arrange
        var perm1 = new Permission { Id = "perm:1", Name = "Same Name" };
        var perm2 = new Permission { Id = "perm:2", Name = "Same Name" };

        Context.Permissions.Add(perm1);
        await Context.SaveChangesAsync();

        // Act & Assert
        Context.Permissions.Add(perm2);
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(
            () => Context.SaveChangesAsync());
    }

    #endregion

    #region Role Tests

    [Fact]
    public async Task Can_Create_SystemWide_Role()
    {
        // Arrange - TenantId is null for system-wide roles per entities.ts
        var role = new Role
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = null, // System-wide
            Name = "SuperAdmin",
            Description = "Has full access to all tenants",
            PermissionIds = new List<string> { "admin:*", "tenant:*" }
        };

        // Act
        Context.Roles.Add(role);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Roles.FindAsync(role.Id);

        Assert.NotNull(loaded);
        Assert.Null(loaded.TenantId);
        Assert.Equal("SuperAdmin", loaded.Name);
        Assert.Equal(2, loaded.PermissionIds.Count);
        Assert.Contains("admin:*", loaded.PermissionIds);
    }

    [Fact]
    public async Task Can_Create_TenantSpecific_Role()
    {
        // Arrange - TenantId set for tenant-specific roles per entities.ts
        var role = new Role
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Dispatcher",
            Description = "Can manage trips and routes",
            PermissionIds = new List<string> { "trip:create", "trip:update", "route:manage" }
        };

        // Act
        Context.Roles.Add(role);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Roles.FindAsync(role.Id);

        Assert.NotNull(loaded);
        Assert.Equal("tenant-001", loaded.TenantId);
        Assert.Equal("Dispatcher", loaded.Name);
        Assert.Equal(3, loaded.PermissionIds.Count);
    }

    [Fact]
    public async Task Role_PermissionIds_Persists_As_Json()
    {
        // Arrange - test JSON serialization of List<string>
        var permissions = new List<string>
        {
            "trip:create",
            "trip:read",
            "trip:update",
            "trip:delete",
            "billing:view"
        };

        var role = new Role
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-002",
            Name = "Trip Manager",
            PermissionIds = permissions
        };

        // Act
        Context.Roles.Add(role);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Roles.FindAsync(role.Id);

        Assert.NotNull(loaded);
        Assert.Equal(5, loaded.PermissionIds.Count);
        Assert.Equal(permissions, loaded.PermissionIds);
    }

    [Fact]
    public async Task Role_With_Empty_PermissionIds_Works()
    {
        // Arrange
        var role = new Role
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Empty Role",
            PermissionIds = new List<string>()
        };

        // Act
        Context.Roles.Add(role);
        await Context.SaveChangesAsync();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = await newContext.Roles.FindAsync(role.Id);

        Assert.NotNull(loaded);
        Assert.Empty(loaded.PermissionIds);
    }

    [Fact]
    public async Task Can_Update_Role_PermissionIds()
    {
        // Arrange
        var role = new Role
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Evolving Role",
            PermissionIds = new List<string> { "trip:read" }
        };

        Context.Roles.Add(role);
        await Context.SaveChangesAsync();

        // Act - replace the entire collection (required for JSON-backed properties)
        // EF Core's value converter compares JSON strings, so in-place modifications
        // don't trigger change detection. Must assign a new list.
        using var updateContext = CreateNewContext();
        var toUpdate = await updateContext.Roles.FindAsync(role.Id);
        Assert.NotNull(toUpdate);

        toUpdate.PermissionIds = new List<string> { "trip:read", "trip:create", "trip:update" };
        await updateContext.SaveChangesAsync();

        // Assert
        using var verifyContext = CreateNewContext();
        var loaded = await verifyContext.Roles.FindAsync(role.Id);

        Assert.NotNull(loaded);
        Assert.Equal(3, loaded.PermissionIds.Count);
        Assert.Contains("trip:read", loaded.PermissionIds);
        Assert.Contains("trip:create", loaded.PermissionIds);
        Assert.Contains("trip:update", loaded.PermissionIds);
    }

    #endregion
}
