namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Operations;

/// <summary>
/// Entity configuration for the Route entity.
/// </summary>
public class RouteConfiguration : TenantEntityConfiguration<Route>
{
    public override void Configure(EntityTypeBuilder<Route> builder)
    {
        base.Configure(builder);

        builder.ToTable("Routes");

        builder.Property(e => e.ShiftId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.SourceRouteManifestId)
            .HasMaxLength(50);

        builder.Property(e => e.SpawningIncidentId)
            .HasMaxLength(50);

        builder.Property(e => e.EstimatedStartTime)
            .IsRequired();

        builder.Property(e => e.EstimatedEndTime)
            .IsRequired();

        // List<BaseStop> as JSON - complex polymorphic collection
        // Must explicitly configure nested owned types within the JSON structure
        builder.OwnsMany(e => e.Stops, stop =>
        {
            stop.ToJson();
            stop.OwnsMany(s => s.TimeWindows);
        });

        builder.HasIndex(e => e.ShiftId);
        builder.HasIndex(e => e.EstimatedStartTime);
    }
}
