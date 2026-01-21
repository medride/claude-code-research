namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Compliance;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;
using Xunit;

/// <summary>
/// Tests for Compliance entities:
/// AttributeDefinition, Credential (system-wide), DriverAttribute, EmployeeCredential, InspectionTemplate.
/// </summary>
public class ComplianceTests : ComplianceTestBase
{
    #region AttributeDefinition Tests

    [Fact]
    public void AttributeDefinition_CanPersist_WithRequiredFields()
    {
        // Arrange
        var attributeDef = new AttributeDefinition
        {
            Id = "attr-def-1",
            Name = "Background Check",
            Description = "Criminal background check verification"
        };

        // Act
        Context.AttributeDefinitions.Add(attributeDef);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.AttributeDefinitions.First(a => a.Id == "attr-def-1");
        Assert.Equal("Background Check", loaded.Name);
        Assert.Equal("Criminal background check verification", loaded.Description);
        Assert.Null(loaded.RenewalPeriodInMonths);
    }

    [Fact]
    public void AttributeDefinition_CanPersist_WithRenewalPeriod()
    {
        // Arrange
        var attributeDef = new AttributeDefinition
        {
            Id = "attr-def-2",
            Name = "Drug Test",
            Description = "DOT-mandated drug screening",
            RenewalPeriodInMonths = 12
        };

        // Act
        Context.AttributeDefinitions.Add(attributeDef);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.AttributeDefinitions.First(a => a.Id == "attr-def-2");
        Assert.Equal(12, loaded.RenewalPeriodInMonths);
    }

    [Fact]
    public void AttributeDefinition_EnforcesUniqueNameConstraint()
    {
        // Arrange
        var attr1 = new AttributeDefinition
        {
            Id = "attr-def-u1",
            Name = "Unique Attribute Name",
            Description = "First attribute"
        };

        var attr2 = new AttributeDefinition
        {
            Id = "attr-def-u2",
            Name = "Unique Attribute Name",
            Description = "Duplicate name"
        };

        // Act & Assert
        Context.AttributeDefinitions.Add(attr1);
        Context.SaveChanges();

        Context.AttributeDefinitions.Add(attr2);
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => Context.SaveChanges());
    }

    #endregion

    #region Credential Tests

    [Fact]
    public void Credential_CanPersist_WithRequiredFields()
    {
        // Arrange
        var credential = new Credential
        {
            Id = "cred-1",
            Name = "Commercial Driver's License"
        };

        // Act
        Context.Credentials.Add(credential);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Credentials.First(c => c.Id == "cred-1");
        Assert.Equal("Commercial Driver's License", loaded.Name);
        Assert.Null(loaded.Description);
        Assert.Null(loaded.IssuingBody);
    }

    [Fact]
    public void Credential_CanPersist_WithAllFields()
    {
        // Arrange
        var credential = new Credential
        {
            Id = "cred-2",
            Name = "CPR Certification",
            Description = "Basic life support certification",
            IssuingBody = "American Red Cross"
        };

        // Act
        Context.Credentials.Add(credential);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.Credentials.First(c => c.Id == "cred-2");
        Assert.Equal("CPR Certification", loaded.Name);
        Assert.Equal("Basic life support certification", loaded.Description);
        Assert.Equal("American Red Cross", loaded.IssuingBody);
    }

    [Fact]
    public void Credential_EnforcesUniqueNameConstraint()
    {
        // Arrange
        var cred1 = new Credential
        {
            Id = "cred-u1",
            Name = "Unique Credential Name"
        };

        var cred2 = new Credential
        {
            Id = "cred-u2",
            Name = "Unique Credential Name"
        };

        // Act & Assert
        Context.Credentials.Add(cred1);
        Context.SaveChanges();

        Context.Credentials.Add(cred2);
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => Context.SaveChanges());
    }

    #endregion

    #region DriverAttribute Tests

    [Fact]
    public void DriverAttribute_CanPersist_WithRequiredFields()
    {
        // Arrange
        var driverAttr = new DriverAttribute
        {
            Id = "driver-attr-1",
            TenantId = "tenant-1",
            DriverProfileId = "driver-1",
            AttributeId = "attr-def-1",
            Status = CredentialStatus.Active,
            DateAwarded = DateTimeOffset.UtcNow.AddMonths(-6),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.DriverAttributes.Add(driverAttr);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.DriverAttributes.First(d => d.Id == "driver-attr-1");
        Assert.Equal("tenant-1", loaded.TenantId);
        Assert.Equal("driver-1", loaded.DriverProfileId);
        Assert.Equal("attr-def-1", loaded.AttributeId);
        Assert.Equal(CredentialStatus.Active, loaded.Status);
        Assert.Null(loaded.ExpirationDate);
        Assert.Null(loaded.DocumentId);
    }

    [Fact]
    public void DriverAttribute_CanPersist_WithAllFields()
    {
        // Arrange
        var driverAttr = new DriverAttribute
        {
            Id = "driver-attr-2",
            TenantId = "tenant-1",
            DriverProfileId = "driver-2",
            AttributeId = "attr-def-2",
            Status = CredentialStatus.Active,
            DateAwarded = DateTimeOffset.UtcNow.AddMonths(-3),
            ExpirationDate = DateTimeOffset.UtcNow.AddMonths(9),
            DocumentId = "doc-123",
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.DriverAttributes.Add(driverAttr);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.DriverAttributes.First(d => d.Id == "driver-attr-2");
        Assert.NotNull(loaded.ExpirationDate);
        Assert.Equal("doc-123", loaded.DocumentId);
    }

    [Fact]
    public void DriverAttribute_CanPersist_AllCredentialStatuses()
    {
        var statuses = Enum.GetValues<CredentialStatus>();
        var idx = 0;

        foreach (var status in statuses)
        {
            var driverAttr = new DriverAttribute
            {
                Id = $"driver-attr-status-{idx++}",
                TenantId = "tenant-1",
                DriverProfileId = $"driver-{idx}",
                AttributeId = $"attr-{idx}",
                Status = status,
                DateAwarded = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow
            };

            Context.DriverAttributes.Add(driverAttr);
        }

        Context.SaveChanges();

        using var newContext = CreateNewContext();
        Assert.Equal(statuses.Length, newContext.DriverAttributes.Count());
    }

    [Fact]
    public void DriverAttribute_EnforcesUniqueDriverAttributeConstraint()
    {
        // Arrange
        var attr1 = new DriverAttribute
        {
            Id = "driver-attr-dup-1",
            TenantId = "tenant-1",
            DriverProfileId = "driver-dup",
            AttributeId = "attr-dup",
            Status = CredentialStatus.Active,
            DateAwarded = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var attr2 = new DriverAttribute
        {
            Id = "driver-attr-dup-2",
            TenantId = "tenant-1",
            DriverProfileId = "driver-dup",
            AttributeId = "attr-dup",
            Status = CredentialStatus.PendingVerification,
            DateAwarded = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act & Assert
        Context.DriverAttributes.Add(attr1);
        Context.SaveChanges();

        Context.DriverAttributes.Add(attr2);
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => Context.SaveChanges());
    }

    #endregion

    #region EmployeeCredential Tests

    [Fact]
    public void EmployeeCredential_CanPersist_WithRequiredFields()
    {
        // Arrange
        var empCred = new EmployeeCredential
        {
            Id = "emp-cred-1",
            TenantId = "tenant-1",
            EmployeeId = "employee-1",
            CredentialId = "cred-1",
            Status = CredentialStatus.Active,
            ExpirationDate = DateTimeOffset.UtcNow.AddYears(2),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.EmployeeCredentials.Add(empCred);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.EmployeeCredentials.First(e => e.Id == "emp-cred-1");
        Assert.Equal("tenant-1", loaded.TenantId);
        Assert.Equal("employee-1", loaded.EmployeeId);
        Assert.Equal("cred-1", loaded.CredentialId);
        Assert.Equal(CredentialStatus.Active, loaded.Status);
        Assert.Null(loaded.LicenseNumber);
        Assert.Null(loaded.IssueDate);
        Assert.Null(loaded.DocumentId);
    }

    [Fact]
    public void EmployeeCredential_CanPersist_WithAllFields()
    {
        // Arrange
        var empCred = new EmployeeCredential
        {
            Id = "emp-cred-2",
            TenantId = "tenant-1",
            EmployeeId = "employee-2",
            CredentialId = "cred-2",
            Status = CredentialStatus.Active,
            LicenseNumber = "CDL-123456",
            IssueDate = DateTimeOffset.UtcNow.AddYears(-1),
            ExpirationDate = DateTimeOffset.UtcNow.AddYears(4),
            DocumentId = "doc-456",
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.EmployeeCredentials.Add(empCred);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.EmployeeCredentials.First(e => e.Id == "emp-cred-2");
        Assert.Equal("CDL-123456", loaded.LicenseNumber);
        Assert.NotNull(loaded.IssueDate);
        Assert.Equal("doc-456", loaded.DocumentId);
    }

    [Fact]
    public void EmployeeCredential_EnforcesUniqueEmployeeCredentialConstraint()
    {
        // Arrange
        var cred1 = new EmployeeCredential
        {
            Id = "emp-cred-dup-1",
            TenantId = "tenant-1",
            EmployeeId = "employee-dup",
            CredentialId = "cred-dup",
            Status = CredentialStatus.Active,
            ExpirationDate = DateTimeOffset.UtcNow.AddYears(1),
            CreatedAt = DateTimeOffset.UtcNow
        };

        var cred2 = new EmployeeCredential
        {
            Id = "emp-cred-dup-2",
            TenantId = "tenant-1",
            EmployeeId = "employee-dup",
            CredentialId = "cred-dup",
            Status = CredentialStatus.Expired,
            ExpirationDate = DateTimeOffset.UtcNow.AddYears(2),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act & Assert
        Context.EmployeeCredentials.Add(cred1);
        Context.SaveChanges();

        Context.EmployeeCredentials.Add(cred2);
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => Context.SaveChanges());
    }

    #endregion

    #region InspectionTemplate Tests

    [Fact]
    public void InspectionTemplate_CanPersist_WithEmptyChecklist()
    {
        // Arrange
        var template = new InspectionTemplate
        {
            Id = "template-1",
            TenantId = "tenant-1",
            Name = "Empty Template",
            Type = InspectionTemplateType.PreShift,
            ChecklistItems = new List<ChecklistItem>(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.InspectionTemplates.Add(template);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.InspectionTemplates.First(t => t.Id == "template-1");
        Assert.Equal("Empty Template", loaded.Name);
        Assert.Equal(InspectionTemplateType.PreShift, loaded.Type);
        Assert.Empty(loaded.ChecklistItems);
    }

    [Fact]
    public void InspectionTemplate_CanPersist_WithChecklistItems()
    {
        // Arrange
        var template = new InspectionTemplate
        {
            Id = "template-2",
            TenantId = "tenant-1",
            Name = "Pre-Shift Inspection",
            Type = InspectionTemplateType.PreShift,
            ChecklistItems = new List<ChecklistItem>
            {
                new ChecklistItem("Brakes", "Check brake pedal resistance"),
                new ChecklistItem("Brakes", "Verify parking brake holds"),
                new ChecklistItem("Lights", "Test headlights"),
                new ChecklistItem("Lights", "Test brake lights"),
                new ChecklistItem("Safety Equipment", "Verify fire extinguisher is present"),
                new ChecklistItem("Safety Equipment", "Check first aid kit contents")
            },
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.InspectionTemplates.Add(template);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.InspectionTemplates.First(t => t.Id == "template-2");
        Assert.Equal(6, loaded.ChecklistItems.Count);

        var brakeItems = loaded.ChecklistItems.Where(c => c.Category == "Brakes").ToList();
        Assert.Equal(2, brakeItems.Count);
        Assert.Contains(brakeItems, c => c.Prompt == "Check brake pedal resistance");
    }

    [Fact]
    public void InspectionTemplate_CanPersist_PostShiftType()
    {
        // Arrange
        var template = new InspectionTemplate
        {
            Id = "template-3",
            TenantId = "tenant-1",
            Name = "Post-Shift Inspection",
            Type = InspectionTemplateType.PostShift,
            ChecklistItems = new List<ChecklistItem>
            {
                new ChecklistItem("Cleanliness", "Interior cleaned"),
                new ChecklistItem("Fuel", "Fuel level reported")
            },
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        Context.InspectionTemplates.Add(template);
        Context.SaveChanges();

        // Assert
        using var newContext = CreateNewContext();
        var loaded = newContext.InspectionTemplates.First(t => t.Id == "template-3");
        Assert.Equal(InspectionTemplateType.PostShift, loaded.Type);
    }

    [Fact]
    public void InspectionTemplate_CanUpdate_ChecklistItems()
    {
        // Arrange
        var template = new InspectionTemplate
        {
            Id = "template-4",
            TenantId = "tenant-1",
            Name = "Updatable Template",
            Type = InspectionTemplateType.PreShift,
            ChecklistItems = new List<ChecklistItem>
            {
                new ChecklistItem("Initial", "Initial item")
            },
            CreatedAt = DateTimeOffset.UtcNow
        };

        Context.InspectionTemplates.Add(template);
        Context.SaveChanges();

        // Act
        using var updateContext = CreateNewContext();
        var loadedTemplate = updateContext.InspectionTemplates.First(t => t.Id == "template-4");
        loadedTemplate.ChecklistItems = new List<ChecklistItem>
        {
            new ChecklistItem("Updated", "Updated item 1"),
            new ChecklistItem("Updated", "Updated item 2")
        };
        updateContext.SaveChanges();

        // Assert
        using var verifyContext = CreateNewContext();
        var verified = verifyContext.InspectionTemplates.First(t => t.Id == "template-4");
        Assert.Equal(2, verified.ChecklistItems.Count);
        Assert.All(verified.ChecklistItems, c => Assert.Equal("Updated", c.Category));
    }

    #endregion
}
