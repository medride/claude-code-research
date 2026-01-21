namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Operations;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Operations entities:
/// Trip, Route, RouteManifest, Shift, ShiftSession, Journey, StandingOrder.
/// Note: PassengerStop and DriverServiceStop are stored as JSON within their parent entities.
/// </summary>
public class OperationsOnlyDbContext : DbContext
{
    public OperationsOnlyDbContext(DbContextOptions<OperationsOnlyDbContext> options) : base(options) { }

    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<RouteManifest> RouteManifests => Set<RouteManifest>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<ShiftSession> ShiftSessions => Set<ShiftSession>();
    public DbSet<Journey> Journeys => Set<Journey>();
    public DbSet<StandingOrder> StandingOrders => Set<StandingOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Trip configuration
        modelBuilder.Entity<Trip>(builder =>
        {
            builder.ToTable("Trips");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PartnerId).HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.PassengerId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FundingSourceId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.JourneyId).HasMaxLength(50);
            builder.Property(e => e.RouteManifestId).HasMaxLength(50);
            builder.Property(e => e.AuthorizationId).HasMaxLength(50);
            builder.Property(e => e.PickupType).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.RegionId).HasMaxLength(50);
            builder.Property(e => e.ProcedureSetId).HasMaxLength(50);
            builder.Property(e => e.InternalNotes).HasMaxLength(2000);
            builder.Property(e => e.RejectionReason).HasMaxLength(500);
            builder.Property(e => e.CancellationReason).HasMaxLength(500);
            builder.Property(e => e.CreatedAt).IsRequired();

            // CapacityRequirements - simple owned type
            builder.OwnsOne(e => e.CapacityRequirements, cap =>
            {
                cap.Property(c => c.WheelchairSpaces);
                cap.Property(c => c.AmbulatorySeats);
                cap.Property(c => c.StretcherCapacity);
            });

            // Complex nested types as JSON
            builder.Property(e => e.ExternalIds)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<TripExternalIds>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.Property(e => e.CompanionIds)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.Property(e => e.Constraints)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<TripConstraints>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.Property(e => e.PlannedRoute)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<DirectionsData>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.Property(e => e.Stops)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<PassengerStop>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.Property(e => e.PostTripDirective)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<PostTripDirective>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.PassengerId);
            builder.HasIndex(e => e.Status);
        });

        // Route configuration
        modelBuilder.Entity<Route>(builder =>
        {
            builder.ToTable("Routes");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ShiftId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.SourceRouteManifestId).HasMaxLength(50);
            builder.Property(e => e.SpawningIncidentId).HasMaxLength(50);
            builder.Property(e => e.EstimatedStartTime).IsRequired();
            builder.Property(e => e.EstimatedEndTime).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<BaseStop> as JSON (polymorphic)
            builder.Property(e => e.Stops)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, GetPolymorphicOptions()),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<BaseStop>>(v, GetPolymorphicOptions()) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.ShiftId);
        });

        // RouteManifest configuration
        modelBuilder.Entity<RouteManifest>(builder =>
        {
            builder.ToTable("RouteManifests");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.OptimizationJobId).HasMaxLength(100);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.ShiftId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<string> as JSON
            builder.Property(e => e.TripIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Status);
        });

        // Shift configuration
        modelBuilder.Entity<Shift>(builder =>
        {
            builder.ToTable("Shifts");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.VehicleId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.StartTime).IsRequired();
            builder.Property(e => e.EndTime).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            // GpsLocation - owned types
            builder.OwnsOne(e => e.StartLocation, gps =>
            {
                gps.Property(g => g.Latitude).IsRequired();
                gps.Property(g => g.Longitude).IsRequired();
            });

            builder.OwnsOne(e => e.EndLocation, gps =>
            {
                gps.Property(g => g.Latitude).IsRequired();
                gps.Property(g => g.Longitude).IsRequired();
            });

            // List<ShiftPersonnel> as JSON
            builder.Property(e => e.Personnel)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<ShiftPersonnel>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            // List<DriverServiceStop> as JSON
            builder.Property(e => e.RequiredStops)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<DriverServiceStop>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.VehicleId);
        });

        // ShiftSession configuration
        modelBuilder.Entity<ShiftSession>(builder =>
        {
            builder.ToTable("ShiftSessions");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ShiftId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.StartTime).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.ShiftId);
        });

        // Journey configuration
        modelBuilder.Entity<Journey>(builder =>
        {
            builder.ToTable("Journeys");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PartnerId).HasMaxLength(50);
            builder.Property(e => e.PassengerId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.BookingDate).IsRequired();
            builder.Property(e => e.SourceStandingOrderId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<JourneyLeg> as JSON
            builder.Property(e => e.Legs)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<JourneyLeg>>(v, (System.Text.Json.JsonSerializerOptions?)null)!);

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.PassengerId);
        });

        // StandingOrder configuration
        modelBuilder.Entity<StandingOrder>(builder =>
        {
            builder.ToTable("StandingOrders");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PartnerId).HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.PassengerId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.RecurrenceRule).IsRequired().HasMaxLength(500);
            builder.Property(e => e.CreatedAt).IsRequired();

            // DateRange - simple owned type
            builder.OwnsOne(e => e.EffectiveDateRange, range =>
            {
                range.Property(r => r.Start).IsRequired();
                range.Property(r => r.End).IsRequired();
            });

            // List<string> as JSON
            builder.Property(e => e.ExclusionDates)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            // JourneyTemplate - complex nested, use JSON
            builder.Property(e => e.JourneyTemplate)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<JourneyTemplate>(v, (System.Text.Json.JsonSerializerOptions?)null)!);

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.PassengerId);
            builder.HasIndex(e => e.Status);
        });
    }

    private static System.Text.Json.JsonSerializerOptions GetPolymorphicOptions()
    {
        var options = new System.Text.Json.JsonSerializerOptions();
        options.Converters.Add(new BaseStopJsonConverter());
        return options;
    }
}

/// <summary>
/// JSON converter for polymorphic BaseStop serialization/deserialization.
/// </summary>
public class BaseStopJsonConverter : System.Text.Json.Serialization.JsonConverter<BaseStop>
{
    public override BaseStop? Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        using var doc = System.Text.Json.JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        // Determine the type based on discriminator or presence of specific properties
        if (root.TryGetProperty("PassengerId", out _) || root.TryGetProperty("AccessPointId", out _))
        {
            return System.Text.Json.JsonSerializer.Deserialize<PassengerStop>(root.GetRawText(), options);
        }
        else
        {
            return System.Text.Json.JsonSerializer.Deserialize<DriverServiceStop>(root.GetRawText(), options);
        }
    }

    public override void Write(System.Text.Json.Utf8JsonWriter writer, BaseStop value, System.Text.Json.JsonSerializerOptions options)
    {
        // Create a new options instance without the converter to avoid recursion
        var newOptions = new System.Text.Json.JsonSerializerOptions(options);
        newOptions.Converters.Clear();

        switch (value)
        {
            case PassengerStop ps:
                System.Text.Json.JsonSerializer.Serialize(writer, ps, newOptions);
                break;
            case DriverServiceStop ds:
                System.Text.Json.JsonSerializer.Serialize(writer, ds, newOptions);
                break;
            default:
                throw new NotSupportedException($"Unknown stop type: {value.GetType().Name}");
        }
    }
}
