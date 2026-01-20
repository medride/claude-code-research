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

        // Owned type: CapacityRequirements
        builder.OwnsOne(e => e.CapacityRequirements, capacity =>
        {
            capacity.Property(c => c.WheelchairSpaces);
            capacity.Property(c => c.AmbulatorySeats);
            capacity.Property(c => c.StretcherCapacity);
        });

        // Owned type: PlannedRoute (DirectionsData)
        builder.OwnsOne(e => e.PlannedRoute, route =>
        {
            route.Property(r => r.EncodedPolyline)
                .HasMaxLength(5000);

            route.OwnsOne(r => r.Distance, distance =>
            {
                distance.Property(d => d.Text)
                    .HasMaxLength(50);

                distance.Property(d => d.ValueInMeters);
            });

            route.OwnsOne(r => r.Duration, duration =>
            {
                duration.Property(d => d.Text)
                    .HasMaxLength(50);

                duration.Property(d => d.ValueInSeconds);
            });
        });

        // Owned type: Constraints (TripConstraints)
        // TripConstraints is a complex nested structure that will be stored as JSON
        // EF Core will handle serialization automatically in modern versions

        // Owned type: ExternalIds
        builder.OwnsOne(e => e.ExternalIds, externalIds =>
        {
            externalIds.Property(ext => ext.BrokerTripId)
                .HasMaxLength(100);
        });

        // Owned type: PostTripDirective
        builder.OwnsOne(e => e.PostTripDirective, directive =>
        {
            directive.Property(d => d.Type)
                .HasMaxLength(50);

            directive.Property(d => d.Duration);

            directive.Property(d => d.NextTripId)
                .HasMaxLength(50);
        });

        // Owned many: Stops (PassengerStop collection)
        builder.OwnsMany(e => e.Stops, stop =>
        {
            stop.ToTable("TripStops");

            // Foreign key back to Trip
            stop.WithOwner()
                .HasForeignKey("TripId");

            // Primary key for the stop
            stop.HasKey("Id");

            stop.Property("Id")
                .HasMaxLength(50);

            // Stop type and status as strings
            stop.Property(s => s.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            stop.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(30);

            // Foreign keys
            stop.Property(s => s.PassengerId)
                .HasMaxLength(50);

            stop.Property(s => s.AccessPointId)
                .IsRequired()
                .HasMaxLength(50);

            stop.Property(s => s.PlaceId)
                .IsRequired()
                .HasMaxLength(50);

            // Foreign keys from BaseStop
            stop.Property(s => s.RegionId)
                .HasMaxLength(50);

            // BaseStop properties
            stop.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(30);

            stop.Property(s => s.Duration);

            stop.Property(s => s.OperationalNotes)
                .HasMaxLength(1000);

            stop.Property(s => s.ActualArrivalTime);

            stop.Property(s => s.ActualDepartureTime);

            // TimeWindows will be stored as JSON collection
            // EF Core will handle serialization automatically

            // Owned type within stop: CapacityDelta
            stop.OwnsOne(s => s.CapacityDelta, capacity =>
            {
                capacity.Property(c => c.WheelchairSpaces);
                capacity.Property(c => c.AmbulatorySeats);
                capacity.Property(c => c.StretcherCapacity);
            });

            // Owned type within stop: ProcedureOverrides
            stop.OwnsOne(s => s.ProcedureOverrides, overrides =>
            {
                // Assuming ProcedureOverrides has lists of procedure IDs
                // EF Core will serialize lists as JSON by convention in modern versions
                // If explicit configuration is needed, add it here
            });

            // Indexes on stops
            stop.HasIndex(s => s.Type);
            stop.HasIndex(s => s.Status);
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
