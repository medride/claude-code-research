namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Billing;

/// <summary>
/// Entity configuration for the Partner entity.
/// </summary>
public class PartnerBillingConfiguration : TenantEntityConfiguration<Partner>
{
    public override void Configure(EntityTypeBuilder<Partner> builder)
    {
        base.Configure(builder);

        builder.ToTable("Partners");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        // List<string> as JSON
        builder.Property(e => e.AuthorizedFundingSourceIds)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasIndex(e => e.Name);
    }
}
