namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Execution;

/// <summary>
/// Entity configuration for the AuditLog entity.
/// </summary>
public class AuditLogConfiguration : TenantEntityConfiguration<AuditLog>
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        base.Configure(builder);

        builder.ToTable("AuditLogs");

        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.EntityId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Action)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.Timestamp)
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasMaxLength(50);

        builder.Property(e => e.UserRole)
            .HasMaxLength(50);

        builder.Property(e => e.IpAddress)
            .HasMaxLength(50);

        builder.HasIndex(e => e.EntityType);
        builder.HasIndex(e => e.EntityId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Timestamp);
    }
}
