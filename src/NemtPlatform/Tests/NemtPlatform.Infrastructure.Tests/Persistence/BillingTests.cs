namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Billing;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;
using Xunit;

/// <summary>
/// Tests for Billing entities:
/// Authorization, Claim, EligibilityRecord, FundingSource, Partner, PartnerContract, Remittance, ServiceCode.
/// </summary>
public class BillingTests : BillingTestBase
{
    #region Authorization Tests

    [Fact]
    public void Authorization_CanPersist_WithRequiredFields()
    {
        // Arrange
        var auth = new Authorization
        {
            Id = "auth-1",
            TenantId = "tenant-1",
            PassengerId = "passenger-1",
            FundingSourceId = "fs-1",
            Status = AuthorizationStatus.Active,
            AuthorizationCode = "AUTH-2024-001",
            EffectiveDateRange = new DateRange(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddMonths(6)
            ),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Authorizations.Add(auth);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Authorizations.First(a => a.Id == "auth-1");
        Assert.Equal("tenant-1", loaded.TenantId);
        Assert.Equal("passenger-1", loaded.PassengerId);
        Assert.Equal(AuthorizationStatus.Active, loaded.Status);
        Assert.Equal("AUTH-2024-001", loaded.AuthorizationCode);
        Assert.NotNull(loaded.EffectiveDateRange);
        Assert.Null(loaded.AuthorizedDestinations);
        Assert.Null(loaded.Limits);
        Assert.Null(loaded.ApprovedServiceCodes);
    }

    [Fact]
    public void Authorization_CanPersist_WithAllFields()
    {
        // Arrange
        var auth = new Authorization
        {
            Id = "auth-2",
            TenantId = "tenant-1",
            PassengerId = "passenger-2",
            FundingSourceId = "fs-1",
            Status = AuthorizationStatus.Active,
            AuthorizationCode = "AUTH-2024-002",
            EffectiveDateRange = new DateRange(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(1)
            ),
            AuthorizedDestinations = new List<AuthorizedDestination>
            {
                new AuthorizedDestination("place-1", "https://docs.example.com/auth1", "Primary clinic"),
                new AuthorizedDestination("place-2", null, "Secondary facility")
            },
            Limits = new AuthorizationLimits(maxTrips: 50, maxMiles: 1000, maxCost: 5000m),
            ApprovedServiceCodes = new List<string> { "A0130", "A0140", "A0080" },
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Authorizations.Add(auth);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Authorizations.First(a => a.Id == "auth-2");

        Assert.NotNull(loaded.AuthorizedDestinations);
        Assert.Equal(2, loaded.AuthorizedDestinations.Count);
        Assert.Equal("place-1", loaded.AuthorizedDestinations[0].PlaceId);

        Assert.NotNull(loaded.Limits);
        Assert.Equal(50, loaded.Limits.MaxTrips);
        Assert.Equal(1000, loaded.Limits.MaxMiles);
        Assert.Equal(5000m, loaded.Limits.MaxCost);

        Assert.NotNull(loaded.ApprovedServiceCodes);
        Assert.Equal(3, loaded.ApprovedServiceCodes.Count);
    }

    [Fact]
    public void Authorization_CanPersist_AllStatuses()
    {
        var statuses = Enum.GetValues<AuthorizationStatus>();
        var idx = 0;

        foreach (var status in statuses)
        {
            var auth = new Authorization
            {
                Id = $"auth-status-{idx++}",
                TenantId = "tenant-1",
                PassengerId = $"passenger-{idx}",
                FundingSourceId = "fs-1",
                Status = status,
                AuthorizationCode = $"AUTH-{idx}",
                EffectiveDateRange = new DateRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(30)),
                CreatedAt = DateTimeOffset.UtcNow
            };

            Context.Authorizations.Add(auth);
        }

        Context.SaveChanges();

        using var newContext = CreateNewContext();
        Assert.Equal(statuses.Length, newContext.Authorizations.Count());
    }

    #endregion

    #region Claim Tests

    [Fact]
    public void Claim_CanPersist_WithRequiredFields()
    {
        // Arrange
        var claim = new Claim
        {
            Id = "claim-1",
            TenantId = "tenant-1",
            TripIds = new List<string> { "trip-1", "trip-2" },
            FundingSourceId = "fs-1",
            Status = ClaimStatus.Draft,
            LineItems = new List<ClaimLineItem>
            {
                new ClaimLineItem("li-1", "sc-1", 150.00m, 1)
            },
            DiagnosisCodeIds = new List<string> { "icd-1" },
            TotalBilledAmount = 150.00m,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Claims.Add(claim);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Claims.First(c => c.Id == "claim-1");
        Assert.Equal(2, loaded.TripIds.Count);
        Assert.Equal(ClaimStatus.Draft, loaded.Status);
        Assert.Single(loaded.LineItems);
        Assert.Equal(150.00m, loaded.TotalBilledAmount);
        Assert.Null(loaded.PartnerId);
        Assert.Null(loaded.DateSubmitted);
    }

    [Fact]
    public void Claim_CanPersist_WithAllFields()
    {
        // Arrange
        var claim = new Claim
        {
            Id = "claim-2",
            TenantId = "tenant-1",
            TripIds = new List<string> { "trip-3" },
            FundingSourceId = "fs-1",
            Status = ClaimStatus.Submitted,
            PartnerId = "partner-1",
            ContractId = "contract-1",
            LineItems = new List<ClaimLineItem>
            {
                new ClaimLineItem("li-2", "sc-1", 100.00m, 2, new List<string> { "mod-1", "mod-2" }),
                new ClaimLineItem("li-3", "sc-2", 75.50m, 1)
            },
            DiagnosisCodeIds = new List<string> { "icd-1", "icd-2" },
            TotalBilledAmount = 275.50m,
            DateSubmitted = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Claims.Add(claim);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Claims.First(c => c.Id == "claim-2");
        Assert.Equal("partner-1", loaded.PartnerId);
        Assert.Equal("contract-1", loaded.ContractId);
        Assert.Equal(2, loaded.LineItems.Count);
        Assert.NotNull(loaded.LineItems[0].Modifiers);
        Assert.Equal(2, loaded.LineItems[0].Modifiers.Count);
        Assert.NotNull(loaded.DateSubmitted);
    }

    [Fact]
    public void Claim_CanPersist_AllStatuses()
    {
        var statuses = Enum.GetValues<ClaimStatus>();
        var idx = 0;

        foreach (var status in statuses)
        {
            var claim = new Claim
            {
                Id = $"claim-status-{idx++}",
                TenantId = "tenant-1",
                TripIds = new List<string> { $"trip-{idx}" },
                FundingSourceId = "fs-1",
                Status = status,
                LineItems = new List<ClaimLineItem>(),
                DiagnosisCodeIds = new List<string>(),
                TotalBilledAmount = 0,
                CreatedAt = DateTimeOffset.UtcNow
            };

            Context.Claims.Add(claim);
        }

        Context.SaveChanges();

        using var newContext = CreateNewContext();
        Assert.Equal(statuses.Length, newContext.Claims.Count());
    }

    #endregion

    #region EligibilityRecord Tests

    [Fact]
    public void EligibilityRecord_CanPersist_WithRequiredFields()
    {
        // Arrange
        var record = new EligibilityRecord
        {
            Id = "elig-1",
            TenantId = "tenant-1",
            PassengerId = "passenger-1",
            FundingSourceId = "fs-1",
            Status = EligibilityStatus.Active,
            EffectiveStartDate = DateTimeOffset.UtcNow.AddMonths(-1),
            EffectiveEndDate = DateTimeOffset.UtcNow.AddMonths(11),
            LastVerifiedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.EligibilityRecords.Add(record);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.EligibilityRecords.First(e => e.Id == "elig-1");
        Assert.Equal("tenant-1", loaded.TenantId);
        Assert.Equal("passenger-1", loaded.PassengerId);
        Assert.Equal(EligibilityStatus.Active, loaded.Status);
    }

    [Fact]
    public void EligibilityRecord_CanPersist_AllStatuses()
    {
        var statuses = Enum.GetValues<EligibilityStatus>();
        var idx = 0;

        foreach (var status in statuses)
        {
            var record = new EligibilityRecord
            {
                Id = $"elig-status-{idx++}",
                TenantId = "tenant-1",
                PassengerId = $"passenger-{idx}",
                FundingSourceId = "fs-1",
                Status = status,
                EffectiveStartDate = DateTimeOffset.UtcNow,
                EffectiveEndDate = DateTimeOffset.UtcNow.AddYears(1),
                LastVerifiedAt = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow
            };

            Context.EligibilityRecords.Add(record);
        }

        Context.SaveChanges();

        using var newContext = CreateNewContext();
        Assert.Equal(statuses.Length, newContext.EligibilityRecords.Count());
    }

    #endregion

    #region FundingSource Tests

    [Fact]
    public void FundingSource_CanPersist_WithRequiredFields()
    {
        // Arrange
        var fundingSource = new FundingSource
        {
            Id = "fs-1",
            TenantId = "tenant-1",
            Name = "State Medicaid",
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.FundingSources.Add(fundingSource);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.FundingSources.First(f => f.Id == "fs-1");
        Assert.Equal("State Medicaid", loaded.Name);
        Assert.Null(loaded.ProcedureSetId);
    }

    [Fact]
    public void FundingSource_CanPersist_WithProcedureSet()
    {
        // Arrange
        var fundingSource = new FundingSource
        {
            Id = "fs-2",
            TenantId = "tenant-1",
            Name = "Medicare",
            ProcedureSetId = "proc-set-1",
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.FundingSources.Add(fundingSource);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.FundingSources.First(f => f.Id == "fs-2");
        Assert.Equal("proc-set-1", loaded.ProcedureSetId);
    }

    #endregion

    #region Partner Tests

    [Fact]
    public void Partner_CanPersist_WithEmptyFundingSources()
    {
        // Arrange
        var partner = new Partner
        {
            Id = "partner-1",
            TenantId = "tenant-1",
            Name = "City Hospital",
            AuthorizedFundingSourceIds = new List<string>(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Partners.Add(partner);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Partners.First(p => p.Id == "partner-1");
        Assert.Equal("City Hospital", loaded.Name);
        Assert.Empty(loaded.AuthorizedFundingSourceIds);
    }

    [Fact]
    public void Partner_CanPersist_WithAuthorizedFundingSources()
    {
        // Arrange
        var partner = new Partner
        {
            Id = "partner-2",
            TenantId = "tenant-1",
            Name = "Regional Clinic",
            AuthorizedFundingSourceIds = new List<string> { "fs-1", "fs-2", "fs-3" },
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Partners.Add(partner);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Partners.First(p => p.Id == "partner-2");
        Assert.Equal(3, loaded.AuthorizedFundingSourceIds.Count);
        Assert.Contains("fs-2", loaded.AuthorizedFundingSourceIds);
    }

    #endregion

    #region PartnerContract Tests

    [Fact]
    public void PartnerContract_CanPersist_WithRequiredFields()
    {
        // Arrange
        var contract = new PartnerContract
        {
            Id = "contract-1",
            TenantId = "tenant-1",
            PartnerId = "partner-1",
            Name = "2024 Transportation Agreement",
            Status = PartnerContractStatus.Active,
            EffectiveDateRange = new DateRange(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(1)
            ),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.PartnerContracts.Add(contract);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.PartnerContracts.First(c => c.Id == "contract-1");
        Assert.Equal("partner-1", loaded.PartnerId);
        Assert.Equal(PartnerContractStatus.Active, loaded.Status);
        Assert.NotNull(loaded.EffectiveDateRange);
        Assert.Null(loaded.Sla);
    }

    [Fact]
    public void PartnerContract_CanPersist_WithSla()
    {
        // Arrange
        var contract = new PartnerContract
        {
            Id = "contract-2",
            TenantId = "tenant-1",
            PartnerId = "partner-1",
            Name = "Premium Service Contract",
            Status = PartnerContractStatus.Active,
            EffectiveDateRange = new DateRange(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(2)
            ),
            Sla = new ServiceLevelAgreement(0.95m, 15),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.PartnerContracts.Add(contract);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.PartnerContracts.First(c => c.Id == "contract-2");
        Assert.NotNull(loaded.Sla);
        Assert.Equal(0.95m, loaded.Sla.OnTimePerformanceTarget);
        Assert.Equal(15, loaded.Sla.MaxWaitTimeMinutes);
    }

    [Fact]
    public void PartnerContract_CanPersist_AllStatuses()
    {
        var statuses = Enum.GetValues<PartnerContractStatus>();
        var idx = 0;

        foreach (var status in statuses)
        {
            var contract = new PartnerContract
            {
                Id = $"contract-status-{idx++}",
                TenantId = "tenant-1",
                PartnerId = "partner-1",
                Name = $"Contract {idx}",
                Status = status,
                EffectiveDateRange = new DateRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(30)),
                CreatedAt = DateTimeOffset.UtcNow
            };

            Context.PartnerContracts.Add(contract);
        }

        Context.SaveChanges();

        using var newContext = CreateNewContext();
        Assert.Equal(statuses.Length, newContext.PartnerContracts.Count());
    }

    #endregion

    #region Remittance Tests

    [Fact]
    public void Remittance_CanPersist_WithRequiredFields()
    {
        // Arrange
        var remittance = new Remittance
        {
            Id = "remit-1",
            TenantId = "tenant-1",
            FundingSourceId = "fs-1",
            PaymentDate = DateTimeOffset.UtcNow,
            TotalPaidAmount = 1500.00m,
            CheckOrReferenceNumber = "CHK-12345",
            ClaimIds = new List<string> { "claim-1", "claim-2" },
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Remittances.Add(remittance);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Remittances.First(r => r.Id == "remit-1");
        Assert.Equal(1500.00m, loaded.TotalPaidAmount);
        Assert.Equal("CHK-12345", loaded.CheckOrReferenceNumber);
        Assert.Equal(2, loaded.ClaimIds.Count);
        Assert.Null(loaded.Adjustments);
    }

    [Fact]
    public void Remittance_CanPersist_WithAdjustments()
    {
        // Arrange
        var remittance = new Remittance
        {
            Id = "remit-2",
            TenantId = "tenant-1",
            FundingSourceId = "fs-1",
            PaymentDate = DateTimeOffset.UtcNow,
            TotalPaidAmount = 1200.00m,
            CheckOrReferenceNumber = "EFT-98765",
            ClaimIds = new List<string> { "claim-3" },
            Adjustments = new List<RemittanceAdjustment>
            {
                new RemittanceAdjustment("li-1", "CO-45", -50.00m, "Contractual adjustment"),
                new RemittanceAdjustment("li-2", "PR-1", -25.00m, "Patient responsibility")
            },
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.Remittances.Add(remittance);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Remittances.First(r => r.Id == "remit-2");
        Assert.NotNull(loaded.Adjustments);
        Assert.Equal(2, loaded.Adjustments.Count);
        Assert.Equal("CO-45", loaded.Adjustments[0].ReasonCode);
        Assert.Equal(-50.00m, loaded.Adjustments[0].Amount);
    }

    #endregion

    #region ServiceCode Tests

    [Fact]
    public void ServiceCode_CanPersist_WithRequiredFields()
    {
        // Arrange
        var serviceCode = new ServiceCode
        {
            Id = "sc-1",
            TenantId = "tenant-1",
            Code = "A0130",
            Description = "Wheelchair van transport, non-emergency",
            Type = ServiceCodeType.Hcpcs,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.ServiceCodes.Add(serviceCode);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.ServiceCodes.First(s => s.Id == "sc-1");
        Assert.Equal("A0130", loaded.Code);
        Assert.Equal(ServiceCodeType.Hcpcs, loaded.Type);
        Assert.Null(loaded.DefaultRate);
    }

    [Fact]
    public void ServiceCode_CanPersist_WithDefaultRate()
    {
        // Arrange
        var serviceCode = new ServiceCode
        {
            Id = "sc-2",
            TenantId = "tenant-1",
            Code = "A0140",
            Description = "Ambulance service, air transport",
            Type = ServiceCodeType.Hcpcs,
            DefaultRate = 250.00m,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.ServiceCodes.Add(serviceCode);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.ServiceCodes.First(s => s.Id == "sc-2");
        Assert.Equal(250.00m, loaded.DefaultRate);
    }

    [Fact]
    public void ServiceCode_CanPersist_AllTypes()
    {
        var types = Enum.GetValues<ServiceCodeType>();
        var idx = 0;

        foreach (var type in types)
        {
            var serviceCode = new ServiceCode
            {
                Id = $"sc-type-{idx}",
                TenantId = "tenant-1",
                Code = $"CODE-{idx++}",
                Description = $"Service code type {type}",
                Type = type,
                CreatedAt = DateTimeOffset.UtcNow
            };

            Context.ServiceCodes.Add(serviceCode);
        }

        Context.SaveChanges();

        using var newContext = CreateNewContext();
        Assert.Equal(types.Length, newContext.ServiceCodes.Count());
    }

    [Fact]
    public void ServiceCode_EnforcesUniqueTenantCodeConstraint()
    {
        // Arrange
        var sc1 = new ServiceCode
        {
            Id = "sc-dup-1",
            TenantId = "tenant-1",
            Code = "A0999",
            Description = "First code",
            Type = ServiceCodeType.Hcpcs,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var sc2 = new ServiceCode
        {
            Id = "sc-dup-2",
            TenantId = "tenant-1",
            Code = "A0999",
            Description = "Duplicate code",
            Type = ServiceCodeType.Hcpcs,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act & Assert
        Context.ServiceCodes.Add(sc1);
        Context.SaveChanges();

        Context.ServiceCodes.Add(sc2);
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => Context.SaveChanges());
    }

    #endregion
}
