namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Fleet;

/// <summary>
/// Entity configuration for the VehicleCredential entity.
/// </summary>
public class VehicleCredentialConfiguration : TenantEntityConfiguration<VehicleCredential>
{
    public override void Configure(EntityTypeBuilder<VehicleCredential> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleCredentials");

        builder.Property(e => e.VehicleId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.CredentialId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.PolicyOrDocumentNumber)
            .HasMaxLength(100);

        builder.Property(e => e.ExpirationDate)
            .IsRequired();

        builder.Property(e => e.DocumentId)
            .HasMaxLength(50);

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.CredentialId);
        builder.HasIndex(e => e.ExpirationDate);
    }
}
