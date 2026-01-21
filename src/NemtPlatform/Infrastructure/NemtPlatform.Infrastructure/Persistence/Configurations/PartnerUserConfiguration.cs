namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Identity;

/// <summary>
/// Entity configuration for the PartnerUser entity.
/// </summary>
public class PartnerUserConfiguration : TenantEntityConfiguration<PartnerUser>
{
    public override void Configure(EntityTypeBuilder<PartnerUser> builder)
    {
        base.Configure(builder);

        builder.ToTable("PartnerUsers");

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PartnerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.JobTitle)
            .HasMaxLength(100);

        // List<string> as JSON
        builder.Property(e => e.RoleIds)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.PartnerId);
    }
}
