namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities;

/// <summary>
/// Entity configuration for the Tenant entity.
/// Note: Tenant extends AuditableEntity (not TenantEntity) because it IS the tenant.
/// </summary>
public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    /// <summary>
    /// Configures the Tenant entity with table name, property constraints, and owned types.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        // Primary key (Id) is configured by convention from AuditableEntity

        // Name is required
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Status is stored as string in database
        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Audit fields from AuditableEntity
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(50);

        // Owned type: PrimaryContact - simple value object, no nesting
        builder.OwnsOne(e => e.PrimaryContact, contact =>
        {
            contact.Property(c => c.Name)
                .HasMaxLength(200);

            contact.Property(c => c.Email)
                .HasMaxLength(100);

            contact.Property(c => c.Phone)
                .HasMaxLength(20);
        });

        // Owned type: Address - simple value object, no nesting
        builder.OwnsOne(e => e.Address, address =>
        {
            address.Property(a => a.Street)
                .HasMaxLength(200);

            address.Property(a => a.City)
                .HasMaxLength(100);

            address.Property(a => a.State)
                .HasMaxLength(50);

            address.Property(a => a.ZipCode)
                .HasMaxLength(20);
        });

        // Owned type: Settings - complex nested structure stored as JSON
        // Must explicitly configure all nested owned types within the JSON
        builder.OwnsOne(e => e.Settings, settings =>
        {
            settings.ToJson();
            settings.OwnsOne(s => s.Regional);
            settings.OwnsOne(s => s.Branding);
            settings.OwnsOne(s => s.Inspections);
        });

        // Indexes
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Status);
    }
}
