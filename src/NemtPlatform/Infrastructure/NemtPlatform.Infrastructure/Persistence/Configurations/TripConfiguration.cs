namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Operations;

/// <summary>
/// Entity configuration for the Trip entity.
/// Configures complex owned types for capacity requirements, route data, constraints,
/// and collection of passenger stops.
/// </summary>
public class TripConfiguration : TenantEntityConfiguration<Trip>
{
    /// <summary>
    /// Configures the Trip entity with table name, property constraints, owned types, and indexes.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public override void Configure(EntityTypeBuilder<Trip> builder)
    {
        // Apply base configuration (TenantId, audit fields)
        base.Configure(builder);

        builder.ToTable("Trips");

        // Foreign keys
        builder.Property(e => e.PartnerId)
            .HasMaxLength(50);

        builder.Property(e => e.PassengerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FundingSourceId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.JourneyId)
            .HasMaxLength(50);

        builder.Property(e => e.RouteManifestId)
            .HasMaxLength(50);

        builder.Property(e => e.AuthorizationId)
            .HasMaxLength(50);

        builder.Property(e => e.RegionId)
            .HasMaxLength(50);

        builder.Property(e => e.ProcedureSetId)
            .HasMaxLength(50);

        // Enum properties stored as strings
        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.PickupType)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Text properties
        builder.Property(e => e.InternalNotes)
            .HasMaxLength(2000);

        builder.Property(e => e.RejectionReason)
            .HasMaxLength(500);

        builder.Property(e => e.CancellationReason)
            .HasMaxLength(500);

        // Owned type: CapacityRequirements - simple flat structure
        builder.OwnsOne(e => e.CapacityRequirements, capacity =>
        {
            capacity.Property(c => c.WheelchairSpaces);
            capacity.Property(c => c.AmbulatorySeats);
            capacity.Property(c => c.StretcherCapacity);
        });

        // Owned type: PlannedRoute (DirectionsData) - complex nested structure stored as JSON
        builder.OwnsOne(e => e.PlannedRoute, route =>
        {
            route.ToJson();
        });

        // Owned type: Constraints (TripConstraints) - complex nested structure stored as JSON
        // Must explicitly configure nested owned types within the JSON structure
        builder.OwnsOne(e => e.Constraints, constraints =>
        {
            constraints.ToJson();
            constraints.OwnsOne(c => c.Preferences, pref =>
            {
                pref.OwnsOne(p => p.Driver);
                pref.OwnsOne(p => p.Vehicle);
            });
            constraints.OwnsOne(c => c.Requirements, req =>
            {
                req.OwnsOne(r => r.Driver);
                req.OwnsOne(r => r.Vehicle);
            });
            constraints.OwnsOne(c => c.Prohibitions, proh =>
            {
                proh.OwnsOne(p => p.Driver);
                proh.OwnsOne(p => p.Vehicle);
            });
        });

        // Owned type: ExternalIds - simple flat structure
        builder.OwnsOne(e => e.ExternalIds, externalIds =>
        {
            externalIds.Property(ext => ext.BrokerTripId)
                .HasMaxLength(100);
        });

        // Owned type: PostTripDirective - simple flat structure
        builder.OwnsOne(e => e.PostTripDirective, directive =>
        {
            directive.Property(d => d.Type)
                .HasMaxLength(50);

            directive.Property(d => d.Duration);

            directive.Property(d => d.NextTripId)
                .HasMaxLength(50);
        });

        // Owned many: Stops - stored as JSON array to avoid complex nested mapping
        // Must explicitly configure nested owned types within the JSON structure
        builder.OwnsMany(e => e.Stops, stop =>
        {
            stop.ToJson();
            stop.OwnsMany(s => s.TimeWindows);
            stop.OwnsOne(s => s.CapacityDelta);
            stop.OwnsOne(s => s.ProcedureOverrides);
        });

        // Indexes on Trip
        builder.HasIndex(e => e.PassengerId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.PartnerId);
        builder.HasIndex(e => e.FundingSourceId);
        builder.HasIndex(e => e.JourneyId);
        builder.HasIndex(e => e.RouteManifestId);
        builder.HasIndex(e => new { e.TenantId, e.Status });
        builder.HasIndex(e => new { e.TenantId, e.PassengerId });
    }
}
