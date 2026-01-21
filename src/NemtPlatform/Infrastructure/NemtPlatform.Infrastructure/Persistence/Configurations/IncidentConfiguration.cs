namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Execution;

/// <summary>
/// Entity configuration for the Incident entity.
/// </summary>
public class IncidentConfiguration : TenantEntityConfiguration<Incident>
{
    public override void Configure(EntityTypeBuilder<Incident> builder)
    {
        base.Configure(builder);

        builder.ToTable("Incidents");

        builder.Property(e => e.Type)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.ReportedAt)
            .IsRequired();

        // GpsLocation as owned type
        builder.OwnsOne(e => e.Location, loc =>
        {
            loc.Property(l => l.Latitude).IsRequired();
            loc.Property(l => l.Longitude).IsRequired();
        });

        builder.Property(e => e.ReportedBy)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.DriverId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.VehicleId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.RouteId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.ActiveStopId)
            .HasMaxLength(50);

        // List<string> as JSON
        builder.Property(e => e.PassengerIdsOnBoard)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        // List<string>? as JSON
        builder.Property(e => e.ActionsTaken)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.Type);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.DriverId);
        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.RouteId);
    }
}
