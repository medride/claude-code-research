namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Passengers;

/// <summary>
/// Entity configuration for the Passenger entity.
/// Configures passenger identification, contact information, and constraint settings.
/// </summary>
public class PassengerConfiguration : TenantEntityConfiguration<Passenger>
{
    /// <summary>
    /// Configures the Passenger entity with table name, property constraints, owned types, and indexes.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public override void Configure(EntityTypeBuilder<Passenger> builder)
    {
        // Apply base configuration (TenantId, audit fields)
        base.Configure(builder);

        builder.ToTable("Passengers");

        // Foreign keys
        builder.Property(e => e.PartnerId)
            .HasMaxLength(50);

        builder.Property(e => e.UserId)
            .HasMaxLength(50);

        builder.Property(e => e.PatientProfileId)
            .HasMaxLength(50);

        builder.Property(e => e.StudentProfileId)
            .HasMaxLength(50);

        // Contact properties
        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.DateOfBirth);

        // Enum property stored as string
        builder.Property(e => e.Gender)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Owned type: Name (PersonName)
        builder.OwnsOne(e => e.Name, name =>
        {
            name.Property(n => n.First)
                .IsRequired()
                .HasMaxLength(100);

            name.Property(n => n.Last)
                .IsRequired()
                .HasMaxLength(100);

            // Note: FullName is a computed property, not stored
        });

        // Owned type: Constraints (TripConstraints)
        // TripConstraints is a complex nested structure that will be stored as JSON
        // EF Core will handle serialization automatically in modern versions

        // EmergencyContactIds as JSON collection
        // EF Core will handle List<string> serialization automatically in modern versions

        // Indexes
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.PartnerId);
        builder.HasIndex(e => e.PatientProfileId);
        builder.HasIndex(e => e.StudentProfileId);
        builder.HasIndex(e => e.PhoneNumber);
        builder.HasIndex(e => new { e.TenantId, e.UserId });
    }
}
