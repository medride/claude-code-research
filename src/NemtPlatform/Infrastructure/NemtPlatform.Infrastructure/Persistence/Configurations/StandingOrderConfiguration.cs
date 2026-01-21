namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Operations;

/// <summary>
/// Entity configuration for the StandingOrder entity.
/// Configures the JourneyTemplate and EffectiveDateRange owned types.
/// </summary>
public class StandingOrderConfiguration : TenantEntityConfiguration<StandingOrder>
{
    /// <summary>
    /// Configures the StandingOrder entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public override void Configure(EntityTypeBuilder<StandingOrder> builder)
    {
        // Apply base configuration (TenantId, audit fields)
        base.Configure(builder);

        builder.ToTable("StandingOrders");

        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.PassengerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PartnerId)
            .HasMaxLength(50);

        builder.Property(e => e.RecurrenceRule)
            .IsRequired()
            .HasMaxLength(500);

        // Enum property stored as string
        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        // Owned type: EffectiveDateRange - simple flat structure
        builder.OwnsOne(e => e.EffectiveDateRange, range =>
        {
            range.Property(r => r.Start).IsRequired();
            range.Property(r => r.End);
        });

        // Owned type: JourneyTemplate - complex nested structure stored as JSON
        // Must explicitly configure nested owned types within JSON to prevent ownership conflicts
        builder.OwnsOne(e => e.JourneyTemplate, template =>
        {
            template.ToJson();

            // Explicitly configure nested types within the JSON structure
            template.OwnsOne(t => t.CapacityRequirements);
            template.OwnsOne(t => t.Constraints);
            template.OwnsMany(t => t.Legs, leg =>
            {
                leg.OwnsOne(l => l.TransitionToNext);
                leg.OwnsMany(l => l.Stops, stop =>
                {
                    stop.OwnsMany(s => s.TimeWindows);
                    stop.OwnsOne(s => s.ProcedureOverrides);
                });
            });
        });

        // Indexes
        builder.HasIndex(e => e.PassengerId);
        builder.HasIndex(e => e.PartnerId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => new { e.TenantId, e.PassengerId });
        builder.HasIndex(e => new { e.TenantId, e.Status });
    }
}
