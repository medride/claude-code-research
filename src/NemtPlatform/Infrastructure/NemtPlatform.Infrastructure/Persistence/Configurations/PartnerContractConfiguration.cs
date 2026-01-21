namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Billing;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the PartnerContract entity.
/// </summary>
public class PartnerContractConfiguration : TenantEntityConfiguration<PartnerContract>
{
    public override void Configure(EntityTypeBuilder<PartnerContract> builder)
    {
        base.Configure(builder);

        builder.ToTable("PartnerContracts");

        builder.Property(e => e.PartnerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // DateRange as owned type
        builder.OwnsOne(e => e.EffectiveDateRange, dr =>
        {
            dr.Property(d => d.Start).IsRequired();
            dr.Property(d => d.End).IsRequired();
        });

        // ServiceLevelAgreement? as JSON
        builder.Property(e => e.Sla)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<ServiceLevelAgreement>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.PartnerId);
        builder.HasIndex(e => e.Status);
    }
}
