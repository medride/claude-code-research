namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Operations;

/// <summary>
/// Entity configuration for the RouteManifest entity.
/// </summary>
public class RouteManifestConfiguration : TenantEntityConfiguration<RouteManifest>
{
    public override void Configure(EntityTypeBuilder<RouteManifest> builder)
    {
        base.Configure(builder);

        builder.ToTable("RouteManifests");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.OptimizationJobId)
            .HasMaxLength(100);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.ShiftId)
            .HasMaxLength(50);

        // List<string> as JSON
        builder.Property(e => e.TripIds)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.ShiftId);
    }
}
