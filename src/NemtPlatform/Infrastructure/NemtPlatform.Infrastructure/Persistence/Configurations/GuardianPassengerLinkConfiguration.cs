namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Passengers;

/// <summary>
/// Entity configuration for the GuardianPassengerLink entity.
/// Configures the many-to-many relationship between guardians and passengers,
/// with permissions and notification settings stored as JSON.
/// </summary>
public class GuardianPassengerLinkConfiguration : TenantEntityConfiguration<GuardianPassengerLink>
{
    /// <summary>
    /// Configures the GuardianPassengerLink entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public override void Configure(EntityTypeBuilder<GuardianPassengerLink> builder)
    {
        // Apply base configuration (TenantId, audit fields)
        base.Configure(builder);

        builder.ToTable("GuardianPassengerLinks");

        // Foreign keys
        builder.Property(e => e.GuardianId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PassengerId)
            .IsRequired()
            .HasMaxLength(50);

        // Enum property stored as string
        builder.Property(e => e.Relationship)
            .HasConversion<string>()
            .HasMaxLength(30);

        // Owned type: Permissions - complex nested structure stored as JSON
        // GuardianPermissions contains NotificationSettings as a nested type
        builder.OwnsOne(e => e.Permissions, permissions =>
        {
            permissions.ToJson();
        });

        // Owned type: NotificationPreferences - complex structure stored as JSON
        builder.OwnsOne(e => e.NotificationPreferences, prefs =>
        {
            prefs.ToJson();
        });

        // Indexes
        builder.HasIndex(e => e.GuardianId);
        builder.HasIndex(e => e.PassengerId);
        builder.HasIndex(e => new { e.GuardianId, e.PassengerId }).IsUnique();
        builder.HasIndex(e => new { e.TenantId, e.GuardianId });
        builder.HasIndex(e => new { e.TenantId, e.PassengerId });
    }
}
