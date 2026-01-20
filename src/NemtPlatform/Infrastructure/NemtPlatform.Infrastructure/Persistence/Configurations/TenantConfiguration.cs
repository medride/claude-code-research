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

        // Owned type: PrimaryContact
        builder.OwnsOne(e => e.PrimaryContact, contact =>
        {
            contact.Property(c => c.Name)
                .HasMaxLength(200);

            contact.Property(c => c.Email)
                .HasMaxLength(100);

            contact.Property(c => c.Phone)
                .HasMaxLength(20);
        });

        // Owned type: Address
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

        // Owned type: Settings with nested owned types
        builder.OwnsOne(e => e.Settings, settings =>
        {
            // Regional settings
            settings.OwnsOne(s => s.Regional, regional =>
            {
                regional.Property(r => r.Timezone)
                    .HasMaxLength(50);

                regional.Property(r => r.Currency)
                    .HasMaxLength(10);
            });

            // Branding settings
            settings.OwnsOne(s => s.Branding, branding =>
            {
                branding.Property(b => b.LogoUrl)
                    .HasMaxLength(500);

                branding.Property(b => b.PrimaryColor)
                    .HasMaxLength(20);
            });

            // Inspection settings
            settings.OwnsOne(s => s.Inspections, inspections =>
            {
                inspections.Property(i => i.RequirePreShiftInspection);

                inspections.Property(i => i.RequirePostShiftInspection);

                inspections.Property(i => i.DefaultPreShiftTemplateId)
                    .HasMaxLength(50);

                inspections.Property(i => i.DefaultPostShiftTemplateId)
                    .HasMaxLength(50);
            });
        });

        // Indexes
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Status);
    }
}
