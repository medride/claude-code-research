namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Common;

/// <summary>
/// Abstract base configuration for entities that inherit from TenantEntity.
/// Provides common configuration for TenantId, audit fields, and global query filters.
/// All tenant entity configurations should inherit from this class.
/// </summary>
/// <typeparam name="T">The entity type that inherits from TenantEntity.</typeparam>
public abstract class TenantEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : TenantEntity
{
    /// <summary>
    /// Configures the entity with common TenantEntity properties.
    /// Override this method in derived classes to add entity-specific configuration.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // TenantId is required and indexed
        builder.Property(e => e.TenantId)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.TenantId);

        // Audit fields - CreatedAt is required, others are optional
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(50);

        // Global tenant filter - uncomment when implementing tenant context service
        // This ensures queries automatically filter by the current tenant
        // builder.HasQueryFilter(e => EF.Property<string>(e, "TenantId") == _tenantId);
    }
}
