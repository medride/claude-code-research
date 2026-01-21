namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Fleet;

/// <summary>
/// Entity configuration for the FuelLog entity.
/// </summary>
public class FuelLogConfiguration : TenantEntityConfiguration<FuelLog>
{
    public override void Configure(EntityTypeBuilder<FuelLog> builder)
    {
        base.Configure(builder);

        builder.ToTable("FuelLogs");

        builder.Property(e => e.VehicleId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.DriverId)
            .HasMaxLength(50);

        builder.Property(e => e.TransactionDate)
            .IsRequired();

        builder.Property(e => e.Gallons)
            .HasPrecision(10, 3);

        builder.Property(e => e.CostPerGallon)
            .HasPrecision(10, 4);

        builder.Property(e => e.TotalCost)
            .HasPrecision(18, 2);

        builder.Property(e => e.VendorName)
            .HasMaxLength(200);

        builder.Property(e => e.FuelCardId)
            .HasMaxLength(50);

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.TransactionDate);
    }
}
