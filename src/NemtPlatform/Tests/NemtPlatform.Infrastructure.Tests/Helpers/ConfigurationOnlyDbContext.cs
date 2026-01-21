namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Configuration;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Configuration entities:
/// FormConfiguration, ProcedureDefinition (system-wide), ProcedureSet, ViewConfiguration.
/// </summary>
public class ConfigurationOnlyDbContext : DbContext
{
    public ConfigurationOnlyDbContext(DbContextOptions<ConfigurationOnlyDbContext> options) : base(options) { }

    public DbSet<FormConfiguration> FormConfigurations => Set<FormConfiguration>();
    public DbSet<ProcedureDefinition> ProcedureDefinitions => Set<ProcedureDefinition>();
    public DbSet<ProcedureSet> ProcedureSets => Set<ProcedureSet>();
    public DbSet<ViewConfiguration> ViewConfigurations => Set<ViewConfiguration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // FormConfiguration configuration
        modelBuilder.Entity<FormConfiguration>(builder =>
        {
            builder.ToTable("FormConfigurations");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.EntityName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Context).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.Config).IsRequired();
            builder.Property(e => e.Version).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.IsSystemDefault).IsRequired();
            builder.Property(e => e.DerivedFromConfigId).HasMaxLength(50);
            builder.Property(e => e.Notes).HasMaxLength(2000);
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<string>? as JSON
            builder.Property(e => e.Tags)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.EntityName);
            builder.HasIndex(e => new { e.TenantId, e.EntityName, e.Context });
        });

        // ProcedureDefinition configuration (Entity, not TenantEntity - system-wide, no CreatedAt)
        modelBuilder.Entity<ProcedureDefinition>(builder =>
        {
            builder.ToTable("ProcedureDefinitions");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(1000);

            builder.HasIndex(e => e.Name).IsUnique();
        });

        // ProcedureSet configuration
        modelBuilder.Entity<ProcedureSet>(builder =>
        {
            builder.ToTable("ProcedureSets");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<ProcedureRule> as JSON
            builder.Property(e => e.Procedures)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<ProcedureRule>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Name);
        });

        // ViewConfiguration configuration
        modelBuilder.Entity<ViewConfiguration>(builder =>
        {
            builder.ToTable("ViewConfigurations");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.UserId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ViewId).IsRequired().HasMaxLength(100);
            builder.Property(e => e.EntityName).HasMaxLength(100);
            builder.Property(e => e.CreatedAt).IsRequired();

            // ViewPreferences as JSON (complex type with nested collections)
            builder.Property(e => e.Preferences)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<ViewPreferences>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => new { e.UserId, e.ViewId }).IsUnique();
        });
    }
}
