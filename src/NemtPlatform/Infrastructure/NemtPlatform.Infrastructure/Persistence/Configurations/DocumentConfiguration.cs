namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Execution;

/// <summary>
/// Entity configuration for the Document entity.
/// </summary>
public class DocumentConfiguration : TenantEntityConfiguration<Document>
{
    public override void Configure(EntityTypeBuilder<Document> builder)
    {
        base.Configure(builder);

        builder.ToTable("Documents");

        builder.Property(e => e.FileName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.FileUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(e => e.DocumentType)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.AssociatedEntityId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.AssociatedEntityType)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.UploadDate)
            .IsRequired();

        builder.Property(e => e.UploadedByUserId)
            .HasMaxLength(50);

        builder.HasIndex(e => e.AssociatedEntityId);
        builder.HasIndex(e => e.AssociatedEntityType);
        builder.HasIndex(e => e.DocumentType);
    }
}
