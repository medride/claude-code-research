namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Identity;

/// <summary>
/// A minimal DbContext for testing User, Role, and Permission entities.
/// Allows testing these entities in isolation without configuring all other entities.
/// </summary>
public class IdentityOnlyDbContext : DbContext
{
    public IdentityOnlyDbContext(DbContextOptions<IdentityOnlyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration - extends AuditableEntity (no TenantId)
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.PhoneNumber).HasMaxLength(20);
            builder.Property(e => e.DisplayName).HasMaxLength(200);
            builder.Property(e => e.PhotoUrl).HasMaxLength(500);

            // FK references (stored as strings, not navigation properties)
            builder.Property(e => e.EmployeeId).HasMaxLength(50);
            builder.Property(e => e.PassengerId).HasMaxLength(50);
            builder.Property(e => e.GuardianId).HasMaxLength(50);
            builder.Property(e => e.PartnerUserId).HasMaxLength(50);

            // Audit fields from AuditableEntity
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.CreatedBy).HasMaxLength(50);
            builder.Property(e => e.UpdatedBy).HasMaxLength(50);

            builder.HasIndex(e => e.Email).IsUnique();
        });

        // Role configuration - extends AuditableEntity (optional TenantId)
        modelBuilder.Entity<Role>(builder =>
        {
            builder.ToTable("Roles");

            builder.Property(e => e.TenantId).HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);

            // List<string> stored as JSON
            builder.Property(e => e.PermissionIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            // Audit fields
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.CreatedBy).HasMaxLength(50);
            builder.Property(e => e.UpdatedBy).HasMaxLength(50);

            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.TenantId);
        });

        // Permission configuration - extends Entity (minimal base)
        modelBuilder.Entity<Permission>(builder =>
        {
            builder.ToTable("Permissions");

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);

            builder.HasIndex(e => e.Name).IsUnique();
        });
    }
}
