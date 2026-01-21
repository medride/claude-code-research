namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Execution;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Execution entities:
/// AuditLog, Document, Incident, TripExecution.
/// Note: StopReconciliation is a value object stored as JSON within TripExecution.
/// </summary>
public class ExecutionOnlyDbContext : DbContext
{
    public ExecutionOnlyDbContext(DbContextOptions<ExecutionOnlyDbContext> options) : base(options) { }

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<TripExecution> TripExecutions => Set<TripExecution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>(builder =>
        {
            builder.ToTable("AuditLogs");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.EntityType).IsRequired().HasMaxLength(100);
            builder.Property(e => e.EntityId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Action).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.Changes).HasMaxLength(10000);
            builder.Property(e => e.Timestamp).IsRequired();
            builder.Property(e => e.UserId).HasMaxLength(50);
            builder.Property(e => e.UserRole).HasMaxLength(50);
            builder.Property(e => e.IpAddress).HasMaxLength(50);
            builder.Property(e => e.Notes).HasMaxLength(1000);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.EntityType);
            builder.HasIndex(e => e.Timestamp);
        });

        // Document configuration
        modelBuilder.Entity<Document>(builder =>
        {
            builder.ToTable("Documents");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FileName).IsRequired().HasMaxLength(500);
            builder.Property(e => e.FileUrl).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.DocumentType).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.AssociatedEntityId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.AssociatedEntityType).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.UploadDate).IsRequired();
            builder.Property(e => e.UploadedByUserId).HasMaxLength(50);
            builder.Property(e => e.Notes).HasMaxLength(1000);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.AssociatedEntityId);
            builder.HasIndex(e => e.DocumentType);
        });

        // Incident configuration
        modelBuilder.Entity<Incident>(builder =>
        {
            builder.ToTable("Incidents");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.ReportedAt).IsRequired();
            builder.Property(e => e.ReportedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.DriverId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.VehicleId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.RouteId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ActiveStopId).HasMaxLength(50);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(5000);
            builder.Property(e => e.ResolutionNotes).HasMaxLength(5000);
            builder.Property(e => e.CreatedAt).IsRequired();

            // GpsLocation - owned type
            builder.OwnsOne(e => e.Location, gps =>
            {
                gps.Property(g => g.Latitude).IsRequired();
                gps.Property(g => g.Longitude).IsRequired();
            });

            // List<string> as JSON
            builder.Property(e => e.PassengerIdsOnBoard)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.Property(e => e.ActionsTaken)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.DriverId);
            builder.HasIndex(e => e.VehicleId);
        });

        // TripExecution configuration
        modelBuilder.Entity<TripExecution>(builder =>
        {
            builder.ToTable("TripExecutions");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.TripId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.RouteId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LiveStatus).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.OnTimeStatus).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.EPcrReferenceId).HasMaxLength(100);
            builder.Property(e => e.CreatedAt).IsRequired();

            // DirectionsData as JSON
            builder.Property(e => e.ApproachRoute)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<DirectionsData>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.Property(e => e.LiveRoute)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<DirectionsData>(v, (System.Text.Json.JsonSerializerOptions?)null));

            // List<StopReconciliation> as JSON
            builder.Property(e => e.Reconciliations)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<StopReconciliation>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.TripId).IsUnique();
            builder.HasIndex(e => e.RouteId);
            builder.HasIndex(e => e.LiveStatus);
        });
    }
}
