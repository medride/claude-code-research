namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Locations;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Place, AccessPoint, and Region entities.
/// </summary>
public class LocationsOnlyDbContext : DbContext
{
    public LocationsOnlyDbContext(DbContextOptions<LocationsOnlyDbContext> options) : base(options) { }

    public DbSet<Place> Places => Set<Place>();
    public DbSet<AccessPoint> AccessPoints => Set<AccessPoint>();
    public DbSet<Region> Regions => Set<Region>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Place configuration
        modelBuilder.Entity<Place>(builder =>
        {
            builder.ToTable("Places");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.CreatedAt).IsRequired();

            // Address - required owned type
            builder.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).IsRequired().HasMaxLength(200);
                address.Property(a => a.City).IsRequired().HasMaxLength(100);
                address.Property(a => a.State).IsRequired().HasMaxLength(50);
                address.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
            });

            // GpsLocation - optional owned type
            builder.OwnsOne(e => e.CenterGps, gps =>
            {
                gps.Property(g => g.Latitude);
                gps.Property(g => g.Longitude);
            });

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Name);
        });

        // AccessPoint configuration
        modelBuilder.Entity<AccessPoint>(builder =>
        {
            builder.ToTable("AccessPoints");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PlaceId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.OperatingHours).HasMaxLength(100);
            builder.Property(e => e.Instructions).HasMaxLength(1000);
            builder.Property(e => e.CreatedAt).IsRequired();

            // GpsLocation - required owned type
            builder.OwnsOne(e => e.Gps, gps =>
            {
                gps.Property(g => g.Latitude).IsRequired();
                gps.Property(g => g.Longitude).IsRequired();
            });

            // List<AccessPointTag> as JSON
            builder.Property(e => e.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<AccessPointTag>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.PlaceId);
        });

        // Region configuration
        modelBuilder.Entity<Region>(builder =>
        {
            builder.ToTable("Regions");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.CreatedAt).IsRequired();

            // GeoJsonPolygon - use value converter (nested collections not supported by ToJson)
            builder.Property(e => e.Boundary)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<GeoJsonPolygon>(v, (System.Text.Json.JsonSerializerOptions?)null));

            // List<string> as JSON
            builder.Property(e => e.ZipCodes)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Name);
        });
    }
}
