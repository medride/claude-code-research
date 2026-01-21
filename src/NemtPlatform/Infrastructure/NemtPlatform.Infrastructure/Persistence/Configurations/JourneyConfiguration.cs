namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Operations;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Journey entity.
/// </summary>
public class JourneyConfiguration : TenantEntityConfiguration<Journey>
{
    public override void Configure(EntityTypeBuilder<Journey> builder)
    {
        base.Configure(builder);

        builder.ToTable("Journeys");

        builder.Property(e => e.PartnerId)
            .HasMaxLength(50);

        builder.Property(e => e.PassengerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Name)
            .HasMaxLength(200);

        builder.Property(e => e.BookingDate)
            .IsRequired();

        builder.Property(e => e.SourceStandingOrderId)
            .HasMaxLength(50);

        // List<JourneyLeg> as JSON
        builder.Property(e => e.Legs)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<JourneyLeg>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasIndex(e => e.PassengerId);
        builder.HasIndex(e => e.PartnerId);
        builder.HasIndex(e => e.BookingDate);
        builder.HasIndex(e => e.SourceStandingOrderId);
    }
}
