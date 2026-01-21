namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Locations;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Region entity.
/// </summary>
public class RegionConfiguration : TenantEntityConfiguration<Region>
{
    public override void Configure(EntityTypeBuilder<Region> builder)
    {
        base.Configure(builder);

        builder.ToTable("Regions");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        // GeoJsonPolygon? as JSON
        builder.Property(e => e.Boundary)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<GeoJsonPolygon>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // List<string>? as JSON
        builder.Property(e => e.ZipCodes)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.Name);
    }
}
