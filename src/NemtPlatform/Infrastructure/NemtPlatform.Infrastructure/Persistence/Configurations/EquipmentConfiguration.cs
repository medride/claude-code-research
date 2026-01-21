namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Fleet;

/// <summary>
/// Entity configuration for the Equipment entity.
/// </summary>
public class EquipmentConfiguration : TenantEntityConfiguration<Equipment>
{
    public override void Configure(EntityTypeBuilder<Equipment> builder)
    {
        base.Configure(builder);

        builder.ToTable("Equipment");

        builder.Property(e => e.EquipmentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.SerialNumber)
            .HasMaxLength(100);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.AssignedVehicleId)
            .HasMaxLength(50);

        builder.HasIndex(e => e.EquipmentType);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.AssignedVehicleId);
    }
}
