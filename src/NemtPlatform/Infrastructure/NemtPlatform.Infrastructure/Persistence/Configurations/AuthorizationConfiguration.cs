namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Billing;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Authorization entity.
/// Configures owned types for DateRange, AuthorizationLimits, and JSON storage for collections.
/// </summary>
public class AuthorizationConfiguration : TenantEntityConfiguration<Authorization>
{
    public override void Configure(EntityTypeBuilder<Authorization> builder)
    {
        base.Configure(builder);

        builder.ToTable("Authorizations");

        builder.Property(e => e.PassengerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FundingSourceId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.AuthorizationCode)
            .IsRequired()
            .HasMaxLength(100);

        // DateRange as owned type
        builder.OwnsOne(e => e.EffectiveDateRange, dr =>
        {
            dr.Property(d => d.Start).IsRequired();
            dr.Property(d => d.End).IsRequired();
        });

        // List<AuthorizedDestination>? as JSON
        builder.Property(e => e.AuthorizedDestinations)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<AuthorizedDestination>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // AuthorizationLimits? as JSON
        builder.Property(e => e.Limits)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<AuthorizationLimits>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // List<string>? as JSON
        builder.Property(e => e.ApprovedServiceCodes)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.PassengerId);
        builder.HasIndex(e => e.FundingSourceId);
        builder.HasIndex(e => e.AuthorizationCode);
    }
}
