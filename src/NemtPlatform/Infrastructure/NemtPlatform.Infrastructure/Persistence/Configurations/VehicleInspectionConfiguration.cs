namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Fleet;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the VehicleInspection entity.
/// </summary>
public class VehicleInspectionConfiguration : TenantEntityConfiguration<VehicleInspection>
{
    public override void Configure(EntityTypeBuilder<VehicleInspection> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleInspections");

        builder.Property(e => e.VehicleId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.DriverId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.ShiftId)
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.InspectionDate)
            .IsRequired();

        builder.Property(e => e.DriverSignatureUrl)
            .HasMaxLength(2000);

        // List<InspectionDefect>? as JSON
        builder.Property(e => e.DefectsFound)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<InspectionDefect>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.DriverId);
        builder.HasIndex(e => e.InspectionDate);
    }
}
