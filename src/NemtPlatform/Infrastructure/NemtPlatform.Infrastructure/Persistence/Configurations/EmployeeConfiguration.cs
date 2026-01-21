namespace NemtPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NemtPlatform.Domain.Entities.Identity;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Entity configuration for the Employee entity.
/// </summary>
public class EmployeeConfiguration : TenantEntityConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);

        builder.ToTable("Employees");

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.HireDate)
            .IsRequired();

        builder.Property(e => e.DriverProfileId)
            .HasMaxLength(50);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(2000);

        // Address? as JSON
        builder.Property(e => e.Address)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<Address>(v, (System.Text.Json.JsonSerializerOptions?)null));

        // List<string> as JSON
        builder.Property(e => e.RoleIds)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        // List<string>? as JSON
        builder.Property(e => e.EmergencyContactIds)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.DriverProfileId);
    }
}
