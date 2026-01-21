namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Billing;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Claim entity.
/// </summary>
public class ClaimConfiguration : TenantEntityConfiguration<Claim>
{
    public override void Configure(EntityTypeBuilder<Claim> builder)
    {
        base.Configure(builder);

        builder.ToTable("Claims");

        builder.Property(e => e.FundingSourceId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.PartnerId)
            .HasMaxLength(50);

        builder.Property(e => e.ContractId)
            .HasMaxLength(50);

        builder.Property(e => e.TotalBilledAmount)
            .HasPrecision(18, 2);

        // List<string> as JSON
        builder.Property(e => e.TripIds)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        // List<ClaimLineItem> as JSON
        builder.Property(e => e.LineItems)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<ClaimLineItem>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        // List<string> as JSON
        builder.Property(e => e.DiagnosisCodeIds)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasIndex(e => e.FundingSourceId);
        builder.HasIndex(e => e.Status);
    }
}
