namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Compliance;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Compliance entities:
/// AttributeDefinition, Credential (system-wide), DriverAttribute, EmployeeCredential, InspectionTemplate.
/// </summary>
public class ComplianceOnlyDbContext : DbContext
{
    public ComplianceOnlyDbContext(DbContextOptions<ComplianceOnlyDbContext> options) : base(options) { }

    public DbSet<AttributeDefinition> AttributeDefinitions => Set<AttributeDefinition>();
    public DbSet<Credential> Credentials => Set<Credential>();
    public DbSet<DriverAttribute> DriverAttributes => Set<DriverAttribute>();
    public DbSet<EmployeeCredential> EmployeeCredentials => Set<EmployeeCredential>();
    public DbSet<InspectionTemplate> InspectionTemplates => Set<InspectionTemplate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // AttributeDefinition configuration (Entity, not TenantEntity - system-wide, no CreatedAt)
        modelBuilder.Entity<AttributeDefinition>(builder =>
        {
            builder.ToTable("AttributeDefinitions");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(1000);

            builder.HasIndex(e => e.Name).IsUnique();
        });

        // Credential configuration (Entity, not TenantEntity - system-wide, no CreatedAt)
        modelBuilder.Entity<Credential>(builder =>
        {
            builder.ToTable("Credentials");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).HasMaxLength(1000);
            builder.Property(e => e.IssuingBody).HasMaxLength(200);

            builder.HasIndex(e => e.Name).IsUnique();
        });

        // DriverAttribute configuration
        modelBuilder.Entity<DriverAttribute>(builder =>
        {
            builder.ToTable("DriverAttributes");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.DriverProfileId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.AttributeId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.DateAwarded).IsRequired();
            builder.Property(e => e.DocumentId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.DriverProfileId);
            builder.HasIndex(e => new { e.DriverProfileId, e.AttributeId }).IsUnique();
        });

        // EmployeeCredential configuration
        modelBuilder.Entity<EmployeeCredential>(builder =>
        {
            builder.ToTable("EmployeeCredentials");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.EmployeeId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.CredentialId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.LicenseNumber).HasMaxLength(100);
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.DocumentId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.EmployeeId);
            builder.HasIndex(e => e.ExpirationDate);
            builder.HasIndex(e => new { e.EmployeeId, e.CredentialId }).IsUnique();
        });

        // InspectionTemplate configuration
        modelBuilder.Entity<InspectionTemplate>(builder =>
        {
            builder.ToTable("InspectionTemplates");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<ChecklistItem> as JSON
            builder.Property(e => e.ChecklistItems)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<ChecklistItem>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Name);
        });
    }
}
