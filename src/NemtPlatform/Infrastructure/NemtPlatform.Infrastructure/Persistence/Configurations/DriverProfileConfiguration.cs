namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Identity;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the DriverProfile entity.
/// </summary>
public class DriverProfileConfiguration : TenantEntityConfiguration<DriverProfile>
{
    public override void Configure(EntityTypeBuilder<DriverProfile> builder)
    {
        base.Configure(builder);

        builder.ToTable("DriverProfiles");

        builder.Property(e => e.EmployeeId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.DriversLicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LicenseClass)
            .HasMaxLength(20);

        builder.Property(e => e.LicenseState)
            .HasMaxLength(5);

        builder.Property(e => e.CurrentComplianceStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        // List<string>? as JSON
        builder.Property(e => e.LicenseEndorsements)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // List<DriverSkill>? as JSON
        builder.Property(e => e.Skills)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<DriverSkill>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // DriverPerformanceMetrics? as JSON
        builder.Property(e => e.PerformanceMetrics)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<DriverPerformanceMetrics>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.EmployeeId);
        builder.HasIndex(e => e.DriversLicenseNumber);
    }
}
