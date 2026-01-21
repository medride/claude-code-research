namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Billing;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Remittance entity.
/// </summary>
public class RemittanceConfiguration : TenantEntityConfiguration<Remittance>
{
    public override void Configure(EntityTypeBuilder<Remittance> builder)
    {
        base.Configure(builder);

        builder.ToTable("Remittances");

        builder.Property(e => e.FundingSourceId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PaymentDate)
            .IsRequired();

        builder.Property(e => e.TotalPaidAmount)
            .HasPrecision(18, 2);

        builder.Property(e => e.CheckOrReferenceNumber)
            .IsRequired()
            .HasMaxLength(100);

        // List<string> as JSON
        builder.Property(e => e.ClaimIds)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        // List<RemittanceAdjustment>? as JSON
        builder.Property(e => e.Adjustments)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<RemittanceAdjustment>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.FundingSourceId);
        builder.HasIndex(e => e.PaymentDate);
    }
}
