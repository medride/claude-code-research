namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Fleet;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Vehicle entity.
/// Configures vehicle identification properties, capacity profile, and medical capabilities.
/// </summary>
public class VehicleConfiguration : TenantEntityConfiguration<Vehicle>
{
    /// <summary>
    /// Configures the Vehicle entity with table name, property constraints, owned types, and indexes.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public override void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        // Apply base configuration (TenantId, audit fields)
        base.Configure(builder);

        builder.ToTable("Vehicles");

        // Name is required
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        // License plate is required and unique within tenant
        builder.Property(e => e.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        // Enum properties stored as strings
        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.VehicleType)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.CurrentComplianceStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        // Vehicle identification properties
        builder.Property(e => e.Vin)
            .HasMaxLength(17); // Standard VIN length

        builder.Property(e => e.Make)
            .HasMaxLength(50);

        builder.Property(e => e.Model)
            .HasMaxLength(50);

        builder.Property(e => e.Year);

        // Foreign keys
        builder.Property(e => e.HomeRegionId)
            .HasMaxLength(50);

        // Owned type: CapacityProfile (CapacityRequirements)
        builder.OwnsOne(e => e.CapacityProfile, capacity =>
        {
            capacity.Property(c => c.WheelchairSpaces)
                .IsRequired();

            capacity.Property(c => c.AmbulatorySeats)
                .IsRequired();

            capacity.Property(c => c.StretcherCapacity)
                .IsRequired();
        });

        // MedicalCapabilities? as JSON
        builder.Property(e => e.MedicalCapabilities)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<MedicalCapabilities>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // List<string>? as JSON
        builder.Property(e => e.VehicleAttributes)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // Indexes
        builder.HasIndex(e => e.LicensePlate)
            .IsUnique(); // License plate must be unique globally

        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.VehicleType);
        builder.HasIndex(e => e.HomeRegionId);
        builder.HasIndex(e => new { e.TenantId, e.Status });
        builder.HasIndex(e => new { e.TenantId, e.LicensePlate });
    }
}
