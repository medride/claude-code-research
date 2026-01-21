namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Passengers;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Passenger, Guardian, and GuardianPassengerLink entities.
/// </summary>
public class PassengersOnlyDbContext : DbContext
{
    public PassengersOnlyDbContext(DbContextOptions<PassengersOnlyDbContext> options) : base(options) { }

    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<Guardian> Guardians => Set<Guardian>();
    public DbSet<GuardianPassengerLink> GuardianPassengerLinks => Set<GuardianPassengerLink>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Passenger configuration
        modelBuilder.Entity<Passenger>(builder =>
        {
            builder.ToTable("Passengers");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PartnerId).HasMaxLength(50);
            builder.Property(e => e.UserId).HasMaxLength(50);
            builder.Property(e => e.PhoneNumber).HasMaxLength(20);
            builder.Property(e => e.Gender).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.PatientProfileId).HasMaxLength(50);
            builder.Property(e => e.StudentProfileId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            // PersonName - simple owned type
            builder.OwnsOne(e => e.Name, name =>
            {
                name.Property(n => n.First).IsRequired().HasMaxLength(100);
                name.Property(n => n.Last).IsRequired().HasMaxLength(100);
            });

            // TripConstraints - complex nested, use JSON value converter
            builder.Property(e => e.Constraints)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<TripConstraints>(v, (System.Text.Json.JsonSerializerOptions?)null));

            // List<string> as JSON
            builder.Property(e => e.EmergencyContactIds)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.UserId);
        });

        // Guardian configuration - very simple
        modelBuilder.Entity<Guardian>(builder =>
        {
            builder.ToTable("Guardians");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.UserId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.UserId);
        });

        // GuardianPassengerLink configuration
        modelBuilder.Entity<GuardianPassengerLink>(builder =>
        {
            builder.ToTable("GuardianPassengerLinks");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.GuardianId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PassengerId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Relationship).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.CreatedAt).IsRequired();

            // GuardianPermissions - has nested NotificationSettings, use JSON
            builder.Property(e => e.Permissions)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<GuardianPermissions>(v, (System.Text.Json.JsonSerializerOptions?)null)!);

            // NotificationPreferences - has nested QuietHours and List<enum>, use JSON
            builder.Property(e => e.NotificationPreferences)
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<NotificationPreferences>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.GuardianId);
            builder.HasIndex(e => e.PassengerId);
            builder.HasIndex(e => new { e.GuardianId, e.PassengerId }).IsUnique();
        });
    }
}
