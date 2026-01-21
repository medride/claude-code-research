namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities;

/// <summary>
/// A minimal DbContext for testing only the Tenant entity.
/// Allows us to test Tenant in isolation without configuring all other entities.
/// </summary>
public class TenantOnlyDbContext : DbContext
{
    public TenantOnlyDbContext(DbContextOptions<TenantOnlyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(builder =>
        {
            builder.ToTable("Tenants");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.CreatedBy).HasMaxLength(50);
            builder.Property(e => e.UpdatedBy).HasMaxLength(50);

            // Owned type: PrimaryContact - simple value object
            builder.OwnsOne(e => e.PrimaryContact, contact =>
            {
                contact.Property(c => c.Name).HasMaxLength(200);
                contact.Property(c => c.Email).HasMaxLength(100);
                contact.Property(c => c.Phone).HasMaxLength(20);
            });

            // Owned type: Address - simple value object
            builder.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.State).HasMaxLength(50);
                address.Property(a => a.ZipCode).HasMaxLength(20);
            });

            // Owned type: Settings - complex nested structure stored as JSON
            // Must explicitly configure all nested owned types within the JSON
            builder.OwnsOne(e => e.Settings, settings =>
            {
                settings.ToJson();
                settings.OwnsOne(s => s.Regional);
                settings.OwnsOne(s => s.Branding);
                settings.OwnsOne(s => s.Inspections);
            });

            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.Status);
        });
    }
}
