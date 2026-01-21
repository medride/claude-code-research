namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Fleet;

/// <summary>
/// Entity configuration for the VehicleTelemetry entity.
/// </summary>
public class VehicleTelemetryConfiguration : TenantEntityConfiguration<VehicleTelemetry>
{
    public override void Configure(EntityTypeBuilder<VehicleTelemetry> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleTelemetries");

        builder.Property(e => e.VehicleId)
            .IsRequired()
            .HasMaxLength(50);

        // GpsLocation as owned type
        builder.OwnsOne(e => e.Gps, gps =>
        {
            gps.Property(g => g.Latitude).IsRequired();
            gps.Property(g => g.Longitude).IsRequired();
        });

        builder.Property(e => e.LastUpdatedAt)
            .IsRequired();

        builder.HasIndex(e => e.VehicleId);
    }
}
