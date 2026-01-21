namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Execution;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the TripExecution entity.
/// </summary>
public class TripExecutionConfiguration : TenantEntityConfiguration<TripExecution>
{
    public override void Configure(EntityTypeBuilder<TripExecution> builder)
    {
        base.Configure(builder);

        builder.ToTable("TripExecutions");

        builder.Property(e => e.TripId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.RouteId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LiveStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.OnTimeStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.EPcrReferenceId)
            .HasMaxLength(100);

        // DirectionsData? as JSON
        builder.Property(e => e.ApproachRoute)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<DirectionsData>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // DirectionsData? as JSON
        builder.Property(e => e.LiveRoute)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<DirectionsData>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // List<StopReconciliation> as JSON
        builder.Property(e => e.Reconciliations)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<StopReconciliation>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasIndex(e => e.TripId);
        builder.HasIndex(e => e.RouteId);
        builder.HasIndex(e => e.LiveStatus);
    }
}
