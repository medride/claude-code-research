namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Locations;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Place entity.
/// </summary>
public class PlaceConfiguration : TenantEntityConfiguration<Place>
{
    public override void Configure(EntityTypeBuilder<Place> builder)
    {
        base.Configure(builder);

        builder.ToTable("Places");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(e => e.Type)
            .HasConversion<string>()
            .HasMaxLength(50);

        // Address as owned type
        builder.OwnsOne(e => e.Address, addr =>
        {
            addr.Property(a => a.Street).IsRequired().HasMaxLength(500);
            addr.Property(a => a.City).IsRequired().HasMaxLength(100);
            addr.Property(a => a.State).IsRequired().HasMaxLength(5);
            addr.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
        });

        // GpsLocation? as owned type
        builder.OwnsOne(e => e.CenterGps, gps =>
        {
            gps.Property(g => g.Latitude);
            gps.Property(g => g.Longitude);
        });

        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Type);
    }
}
