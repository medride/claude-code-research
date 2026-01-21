namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Identity;

/// <summary>
/// Entity configuration for the User entity.
/// User is an AuditableEntity (not TenantEntity) as users span multiple tenants.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.DisplayName)
            .HasMaxLength(200);

        builder.Property(e => e.PhotoUrl)
            .HasMaxLength(2000);

        builder.Property(e => e.EmployeeId)
            .HasMaxLength(50);

        builder.Property(e => e.PassengerId)
            .HasMaxLength(50);

        builder.Property(e => e.GuardianId)
            .HasMaxLength(50);

        builder.Property(e => e.PartnerUserId)
            .HasMaxLength(50);

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasIndex(e => e.EmployeeId);
        builder.HasIndex(e => e.PassengerId);
    }
}
