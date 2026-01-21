namespace NemtPlatform.Infrastructure.Tests.Helpers;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Domain.Entities.Billing;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Minimal DbContext for testing Billing entities:
/// Authorization, Claim, EligibilityRecord, FundingSource, Partner, PartnerContract, Remittance, ServiceCode.
/// </summary>
public class BillingOnlyDbContext : DbContext
{
    public BillingOnlyDbContext(DbContextOptions<BillingOnlyDbContext> options) : base(options) { }

    public DbSet<Authorization> Authorizations => Set<Authorization>();
    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<EligibilityRecord> EligibilityRecords => Set<EligibilityRecord>();
    public DbSet<FundingSource> FundingSources => Set<FundingSource>();
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<PartnerContract> PartnerContracts => Set<PartnerContract>();
    public DbSet<Remittance> Remittances => Set<Remittance>();
    public DbSet<ServiceCode> ServiceCodes => Set<ServiceCode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Authorization configuration
        modelBuilder.Entity<Authorization>(builder =>
        {
            builder.ToTable("Authorizations");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PassengerId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FundingSourceId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.AuthorizationCode).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CreatedAt).IsRequired();

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

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.PassengerId);
            builder.HasIndex(e => e.FundingSourceId);
            builder.HasIndex(e => e.AuthorizationCode);
        });

        // Claim configuration
        modelBuilder.Entity<Claim>(builder =>
        {
            builder.ToTable("Claims");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FundingSourceId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.PartnerId).HasMaxLength(50);
            builder.Property(e => e.ContractId).HasMaxLength(50);
            builder.Property(e => e.TotalBilledAmount).HasPrecision(18, 2);
            builder.Property(e => e.CreatedAt).IsRequired();

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

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.FundingSourceId);
            builder.HasIndex(e => e.Status);
        });

        // EligibilityRecord configuration
        modelBuilder.Entity<EligibilityRecord>(builder =>
        {
            builder.ToTable("EligibilityRecords");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PassengerId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FundingSourceId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.EffectiveStartDate).IsRequired();
            builder.Property(e => e.EffectiveEndDate).IsRequired();
            builder.Property(e => e.LastVerifiedAt).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.PassengerId);
            builder.HasIndex(e => e.FundingSourceId);
        });

        // FundingSource configuration
        modelBuilder.Entity<FundingSource>(builder =>
        {
            builder.ToTable("FundingSources");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.ProcedureSetId).HasMaxLength(50);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Name);
        });

        // Partner configuration
        modelBuilder.Entity<Partner>(builder =>
        {
            builder.ToTable("Partners");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.CreatedAt).IsRequired();

            // List<string> as JSON
            builder.Property(e => e.AuthorizedFundingSourceIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Name);
        });

        // PartnerContract configuration
        modelBuilder.Entity<PartnerContract>(builder =>
        {
            builder.ToTable("PartnerContracts");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PartnerId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.CreatedAt).IsRequired();

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

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.PartnerId);
            builder.HasIndex(e => e.Status);
        });

        // Remittance configuration
        modelBuilder.Entity<Remittance>(builder =>
        {
            builder.ToTable("Remittances");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FundingSourceId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PaymentDate).IsRequired();
            builder.Property(e => e.TotalPaidAmount).HasPrecision(18, 2);
            builder.Property(e => e.CheckOrReferenceNumber).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CreatedAt).IsRequired();

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

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.FundingSourceId);
            builder.HasIndex(e => e.PaymentDate);
        });

        // ServiceCode configuration
        modelBuilder.Entity<ServiceCode>(builder =>
        {
            builder.ToTable("ServiceCodes");
            builder.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.DefaultRate).HasPrecision(18, 2);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.Code);
            builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
        });
    }
}
