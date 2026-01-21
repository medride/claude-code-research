namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Operations;

/// <summary>
/// Entity configuration for the ShiftSession entity.
/// </summary>
public class ShiftSessionConfiguration : TenantEntityConfiguration<ShiftSession>
{
    public override void Configure(EntityTypeBuilder<ShiftSession> builder)
    {
        base.Configure(builder);

        builder.ToTable("ShiftSessions");

        builder.Property(e => e.ShiftId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.HasIndex(e => e.ShiftId);
        builder.HasIndex(e => e.StartTime);
    }
}
