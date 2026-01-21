namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Fleet;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Fleet entities:
/// Vehicle, Equipment, FuelLog, MaintenanceRecord, VehicleCredential, VehicleInspection, VehicleTelemetry.
/// </summary>
public class FleetOnlyDbContext : DbContext
{
    public FleetOnlyDbContext(DbContextOptions<FleetOnlyDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<FuelLog> FuelLogs => Set<FuelLog>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
    public DbSet<VehicleCredential> VehicleCredentials => Set<VehicleCredential>();
    public DbSet<VehicleInspection> VehicleInspections => Set<VehicleInspection>();
    public DbSet<VehicleTelemetry> VehicleTelemetry => Set<VehicleTelemetry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Vehicle configuration
        modelBuilder.Entity<Vehicle>(builder =>
        {
            builder.ToTable("Vehicles");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.CurrentComplianceStatus).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.Vin).HasMaxLength(50);
            builder.Property(e => e.LicensePlate).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Make).HasMaxLength(50);
            builder.Property(e => e.Model).HasMaxLength(50);
            builder.Property(e => e.HomeRegionId).HasMaxLength(50);
            builder.Property(e => e.VehicleType).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.CreatedAt).IsRequired();

            // CapacityRequirements - simple value object, use OwnsOne
            builder.OwnsOne(e => e.CapacityProfile, capacity =>
            {
                capacity.Property(c => c.WheelchairSpaces);
                capacity.Property(c => c.AmbulatorySeats);
                capacity.Property(c => c.StretcherCapacity);
            });

            // MedicalCapabilities - has nested List<string>, use JSON value converter
            builder.Property(e => e.MedicalCapabilities)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<MedicalCapabilities>(v, (System.Text.Json.JsonSerializerOptions?)null));

            // List<string> as JSON
            builder.Property(e => e.VehicleAttributes)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.LicensePlate);
        });

        // Equipment configuration
        modelBuilder.Entity<Equipment>(builder =>
        {
            builder.ToTable("Equipment");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.EquipmentType).IsRequired().HasMaxLength(100);
            builder.Property(e => e.SerialNumber).HasMaxLength(100);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.AssignedVehicleId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.AssignedVehicleId);
        });

        // FuelLog configuration
        modelBuilder.Entity<FuelLog>(builder =>
        {
            builder.ToTable("FuelLogs");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.VehicleId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.DriverId).HasMaxLength(50);
            builder.Property(e => e.TransactionDate).IsRequired();
            builder.Property(e => e.Gallons).HasPrecision(10, 3);
            builder.Property(e => e.CostPerGallon).HasPrecision(10, 3);
            builder.Property(e => e.TotalCost).HasPrecision(10, 2);
            builder.Property(e => e.VendorName).HasMaxLength(200);
            builder.Property(e => e.FuelCardId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.VehicleId);
        });

        // MaintenanceRecord configuration
        modelBuilder.Entity<MaintenanceRecord>(builder =>
        {
            builder.ToTable("MaintenanceRecords");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.VehicleId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.ExternalVendorId).HasMaxLength(50);
            builder.Property(e => e.SourceInspectionId).HasMaxLength(50);
            builder.Property(e => e.SourceIncidentId).HasMaxLength(50);
            builder.Property(e => e.Notes).HasMaxLength(2000);
            builder.Property(e => e.CreatedAt).IsRequired();

            // MaintenanceCosts - simple value object, use OwnsOne
            builder.OwnsOne(e => e.Costs, costs =>
            {
                costs.Property(c => c.Parts).HasPrecision(10, 2);
                costs.Property(c => c.Labor).HasPrecision(10, 2);
                costs.Property(c => c.Tax).HasPrecision(10, 2);
                costs.Property(c => c.Total).HasPrecision(10, 2);
            });

            // List<string> as JSON
            builder.Property(e => e.InternalMechanicIds)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.VehicleId);
        });

        // VehicleCredential configuration
        modelBuilder.Entity<VehicleCredential>(builder =>
        {
            builder.ToTable("VehicleCredentials");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.VehicleId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.CredentialId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.PolicyOrDocumentNumber).HasMaxLength(100);
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.DocumentId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.VehicleId);
            builder.HasIndex(e => e.ExpirationDate);
        });

        // VehicleInspection configuration
        modelBuilder.Entity<VehicleInspection>(builder =>
        {
            builder.ToTable("VehicleInspections");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.VehicleId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.DriverId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ShiftId).HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.InspectionDate).IsRequired();
            builder.Property(e => e.DriverSignatureUrl).HasMaxLength(500);
            builder.Property(e => e.Notes).HasMaxLength(2000);
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<InspectionDefect> as JSON
            builder.Property(e => e.DefectsFound)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<InspectionDefect>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.VehicleId);
            builder.HasIndex(e => e.DriverId);
        });

        // VehicleTelemetry configuration
        modelBuilder.Entity<VehicleTelemetry>(builder =>
        {
            builder.ToTable("VehicleTelemetry");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.VehicleId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastUpdatedAt).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            // GpsLocation - simple owned type
            builder.OwnsOne(e => e.Gps, gps =>
            {
                gps.Property(g => g.Latitude).IsRequired();
                gps.Property(g => g.Longitude).IsRequired();
            });

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.VehicleId).IsUnique();
        });
    }
}
