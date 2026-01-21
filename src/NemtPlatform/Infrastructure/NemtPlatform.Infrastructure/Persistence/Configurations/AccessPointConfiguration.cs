namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Locations;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Entity configuration for the AccessPoint entity.
/// </summary>
public class AccessPointConfiguration : TenantEntityConfiguration<AccessPoint>
{
    public override void Configure(EntityTypeBuilder<AccessPoint> builder)
    {
        base.Configure(builder);

        builder.ToTable("AccessPoints");

        builder.Property(e => e.PlaceId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        // GpsLocation as owned type
        builder.OwnsOne(e => e.Gps, gps =>
        {
            gps.Property(g => g.Latitude).IsRequired();
            gps.Property(g => g.Longitude).IsRequired();
        });

        // List<AccessPointTag> as JSON
        builder.Property(e => e.Tags)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<AccessPointTag>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.Property(e => e.OperatingHours)
            .HasMaxLength(200);

        builder.HasIndex(e => e.PlaceId);
        builder.HasIndex(e => e.Name);
    }
}
