namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Operations;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Shift entity.
/// </summary>
public class ShiftConfiguration : TenantEntityConfiguration<Shift>
{
    public override void Configure(EntityTypeBuilder<Shift> builder)
    {
        base.Configure(builder);

        builder.ToTable("Shifts");

        builder.Property(e => e.VehicleId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.EndTime)
            .IsRequired();

        // List<ShiftPersonnel> as JSON
        builder.Property(e => e.Personnel)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<ShiftPersonnel>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        // GpsLocation as owned type
        builder.OwnsOne(e => e.StartLocation, loc =>
        {
            loc.Property(l => l.Latitude).HasColumnName("StartLocation_Latitude").IsRequired();
            loc.Property(l => l.Longitude).HasColumnName("StartLocation_Longitude").IsRequired();
        });

        builder.OwnsOne(e => e.EndLocation, loc =>
        {
            loc.Property(l => l.Latitude).HasColumnName("EndLocation_Latitude").IsRequired();
            loc.Property(l => l.Longitude).HasColumnName("EndLocation_Longitude").IsRequired();
        });

        // List<DriverServiceStop>? as JSON
        builder.Property(e => e.RequiredStops)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<DriverServiceStop>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.StartTime);
    }
}
