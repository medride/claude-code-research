namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Fleet;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the MaintenanceRecord entity.
/// </summary>
public class MaintenanceRecordConfiguration : TenantEntityConfiguration<MaintenanceRecord>
{
    public override void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
        base.Configure(builder);

        builder.ToTable("MaintenanceRecords");

        builder.Property(e => e.VehicleId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.Type)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.Description)
            .IsRequired();

        builder.Property(e => e.ExternalVendorId)
            .HasMaxLength(50);

        builder.Property(e => e.SourceInspectionId)
            .HasMaxLength(50);

        builder.Property(e => e.SourceIncidentId)
            .HasMaxLength(50);

        // MaintenanceCosts? as JSON
        builder.Property(e => e.Costs)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<MaintenanceCosts>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // List<string>? as JSON
        builder.Property(e => e.InternalMechanicIds)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.Status);
    }
}
