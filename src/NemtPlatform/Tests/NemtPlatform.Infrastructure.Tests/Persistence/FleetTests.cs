namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Fleet;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;

public class FleetTests : FleetTestBase
{
    #region Vehicle Tests

    [Fact]
    public async Task Can_Create_Vehicle_With_Required_Fields()
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Van 12",
            LicensePlate = "ABC-1234",
            VehicleType = VehicleType.Van,
            CapacityProfile = new CapacityRequirements(0, 4, 0)
        };

        Context.Vehicles.Add(vehicle);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Vehicles.FindAsync(vehicle.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Van 12", loaded.Name);
        Assert.Equal("ABC-1234", loaded.LicensePlate);
        Assert.Equal(VehicleType.Van, loaded.VehicleType);
        Assert.Equal(VehicleStatus.Active, loaded.Status);
        Assert.Equal(ComplianceStatus.Clear, loaded.CurrentComplianceStatus);
    }

    [Fact]
    public async Task Can_Create_Vehicle_With_All_Fields()
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Wheelchair Van 5",
            Status = VehicleStatus.Active,
            CurrentComplianceStatus = ComplianceStatus.Clear,
            Vin = "1HGBH41JXMN109186",
            LicensePlate = "XYZ-5678",
            Make = "Ford",
            Model = "Transit",
            Year = 2023,
            HomeRegionId = "region-001",
            VehicleType = VehicleType.WheelchairVan,
            CapacityProfile = new CapacityRequirements(2, 3, 0),
            MedicalCapabilities = new MedicalCapabilities(MedicalServiceLevel.Bls, new List<string> { "equip-001", "equip-002" }),
            VehicleAttributes = new List<string> { "GPS_TRACKING", "POWER_INVERTER" }
        };

        Context.Vehicles.Add(vehicle);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Vehicles.FindAsync(vehicle.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Ford", loaded.Make);
        Assert.Equal("Transit", loaded.Model);
        Assert.Equal(2023, loaded.Year);
        Assert.NotNull(loaded.CapacityProfile);
        Assert.Equal(2, loaded.CapacityProfile.WheelchairSpaces);
        Assert.Equal(3, loaded.CapacityProfile.AmbulatorySeats);
        Assert.NotNull(loaded.MedicalCapabilities);
        Assert.Equal(MedicalServiceLevel.Bls, loaded.MedicalCapabilities.LevelOfService);
        Assert.Equal(2, loaded.MedicalCapabilities.OnboardEquipmentIds!.Count);
        Assert.NotNull(loaded.VehicleAttributes);
        Assert.Equal(2, loaded.VehicleAttributes.Count);
    }

    [Fact]
    public async Task All_VehicleStatuses_Can_Be_Stored()
    {
        var statuses = new[] { VehicleStatus.Active, VehicleStatus.Inactive,
                               VehicleStatus.InMaintenance, VehicleStatus.Decommissioned };

        foreach (var status in statuses)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Name = $"Vehicle with status {status}",
                LicensePlate = $"PLT-{status}",
                VehicleType = VehicleType.Sedan,
                Status = status,
                CapacityProfile = new CapacityRequirements(0, 3, 0)
            };

            Context.Vehicles.Add(vehicle);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.Vehicles.FindAsync(vehicle.Id);
            Assert.Equal(status, loaded!.Status);
        }
    }

    #endregion

    #region Equipment Tests

    [Fact]
    public async Task Can_Create_Equipment_With_Required_Fields()
    {
        var equipment = new Equipment
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            EquipmentType = "Wheelchair"
        };

        Context.Equipment.Add(equipment);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Equipment.FindAsync(equipment.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Wheelchair", loaded.EquipmentType);
        Assert.Equal(EquipmentStatus.Available, loaded.Status);
    }

    [Fact]
    public async Task Can_Create_Equipment_With_All_Fields()
    {
        var equipment = new Equipment
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            EquipmentType = "Oxygen Tank",
            SerialNumber = "OX-2024-001",
            Status = EquipmentStatus.InUse,
            AssignedVehicleId = "vehicle-001",
            LastServiceDate = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero),
            NextServiceDate = new DateTimeOffset(2024, 7, 15, 0, 0, 0, TimeSpan.Zero)
        };

        Context.Equipment.Add(equipment);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Equipment.FindAsync(equipment.Id);

        Assert.NotNull(loaded);
        Assert.Equal("OX-2024-001", loaded.SerialNumber);
        Assert.Equal(EquipmentStatus.InUse, loaded.Status);
        Assert.Equal("vehicle-001", loaded.AssignedVehicleId);
    }

    #endregion

    #region FuelLog Tests

    [Fact]
    public async Task Can_Create_FuelLog()
    {
        var fuelLog = new FuelLog
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            DriverId = "driver-001",
            TransactionDate = DateTimeOffset.UtcNow,
            OdometerReading = 45000,
            Gallons = 15.5m,
            CostPerGallon = 3.45m,
            TotalCost = 53.48m,
            VendorName = "Shell Gas Station",
            FuelCardId = "card-001"
        };

        Context.FuelLogs.Add(fuelLog);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.FuelLogs.FindAsync(fuelLog.Id);

        Assert.NotNull(loaded);
        Assert.Equal("vehicle-001", loaded.VehicleId);
        Assert.Equal(45000, loaded.OdometerReading);
        Assert.Equal(15.5m, loaded.Gallons);
        Assert.Equal(3.45m, loaded.CostPerGallon);
        Assert.Equal(53.48m, loaded.TotalCost);
    }

    #endregion

    #region MaintenanceRecord Tests

    [Fact]
    public async Task Can_Create_MaintenanceRecord_With_Required_Fields()
    {
        var record = new MaintenanceRecord
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            Type = MaintenanceType.Preventative,
            Description = "Oil change and tire rotation",
            OdometerReading = 50000
        };

        Context.MaintenanceRecords.Add(record);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.MaintenanceRecords.FindAsync(record.Id);

        Assert.NotNull(loaded);
        Assert.Equal(WorkOrderStatus.Requested, loaded.Status);
        Assert.Equal(MaintenanceType.Preventative, loaded.Type);
        Assert.Equal("Oil change and tire rotation", loaded.Description);
    }

    [Fact]
    public async Task Can_Create_MaintenanceRecord_With_Costs()
    {
        var record = new MaintenanceRecord
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            Status = WorkOrderStatus.Completed,
            Type = MaintenanceType.Repair,
            Description = "Brake pad replacement",
            OdometerReading = 55000,
            Costs = new MaintenanceCosts(150.00m, 200.00m, 28.00m, 378.00m),
            DateRequested = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            ScheduledDate = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero),
            CompletionDate = new DateTimeOffset(2024, 1, 15, 14, 30, 0, TimeSpan.Zero),
            InternalMechanicIds = new List<string> { "mechanic-001", "mechanic-002" },
            Notes = "Front brakes only, rear still good."
        };

        Context.MaintenanceRecords.Add(record);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.MaintenanceRecords.FindAsync(record.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Costs);
        Assert.Equal(150.00m, loaded.Costs.Parts);
        Assert.Equal(200.00m, loaded.Costs.Labor);
        Assert.Equal(28.00m, loaded.Costs.Tax);
        Assert.Equal(378.00m, loaded.Costs.Total);
        Assert.NotNull(loaded.InternalMechanicIds);
        Assert.Equal(2, loaded.InternalMechanicIds.Count);
    }

    #endregion

    #region VehicleCredential Tests

    [Fact]
    public async Task Can_Create_VehicleCredential()
    {
        var credential = new VehicleCredential
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            CredentialId = "cred-registration",
            Status = CredentialStatus.Active,
            PolicyOrDocumentNumber = "REG-2024-12345",
            IssueDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            ExpirationDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DocumentId = "doc-001"
        };

        Context.VehicleCredentials.Add(credential);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.VehicleCredentials.FindAsync(credential.Id);

        Assert.NotNull(loaded);
        Assert.Equal(CredentialStatus.Active, loaded.Status);
        Assert.Equal("REG-2024-12345", loaded.PolicyOrDocumentNumber);
    }

    [Fact]
    public async Task All_CredentialStatuses_Can_Be_Stored()
    {
        var statuses = new[] { CredentialStatus.PendingVerification, CredentialStatus.Active,
                               CredentialStatus.Expired, CredentialStatus.Revoked };

        foreach (var status in statuses)
        {
            var credential = new VehicleCredential
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                VehicleId = $"vehicle-{status}",
                CredentialId = "cred-001",
                Status = status,
                ExpirationDate = DateTimeOffset.UtcNow.AddYears(1)
            };

            Context.VehicleCredentials.Add(credential);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.VehicleCredentials.FindAsync(credential.Id);
            Assert.Equal(status, loaded!.Status);
        }
    }

    #endregion

    #region VehicleInspection Tests

    [Fact]
    public async Task Can_Create_VehicleInspection_Passed()
    {
        var inspection = new VehicleInspection
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            DriverId = "driver-001",
            ShiftId = "shift-001",
            Status = InspectionStatus.Pass,
            OdometerReading = 60000,
            InspectionDate = DateTimeOffset.UtcNow,
            DriverSignatureUrl = "https://storage.example.com/signatures/sig-001.png"
        };

        Context.VehicleInspections.Add(inspection);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.VehicleInspections.FindAsync(inspection.Id);

        Assert.NotNull(loaded);
        Assert.Equal(InspectionStatus.Pass, loaded.Status);
        Assert.Equal(60000, loaded.OdometerReading);
        Assert.Null(loaded.DefectsFound);
    }

    [Fact]
    public async Task Can_Create_VehicleInspection_With_Defects()
    {
        var defects = new List<InspectionDefect>
        {
            new InspectionDefect("Lights", "Left rear turn signal not working", true),
            new InspectionDefect("Tires", "Right front tire tread low", false)
        };

        var inspection = new VehicleInspection
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            DriverId = "driver-001",
            Status = InspectionStatus.Fail,
            OdometerReading = 61000,
            InspectionDate = DateTimeOffset.UtcNow,
            DefectsFound = defects,
            Notes = "Vehicle should not be used until lights are fixed."
        };

        Context.VehicleInspections.Add(inspection);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.VehicleInspections.FindAsync(inspection.Id);

        Assert.NotNull(loaded);
        Assert.Equal(InspectionStatus.Fail, loaded.Status);
        Assert.NotNull(loaded.DefectsFound);
        Assert.Equal(2, loaded.DefectsFound.Count);
        Assert.Contains(loaded.DefectsFound, d => d.Category == "Lights" && d.IsCritical);
        Assert.Contains(loaded.DefectsFound, d => d.Category == "Tires" && !d.IsCritical);
    }

    #endregion

    #region VehicleTelemetry Tests

    [Fact]
    public async Task Can_Create_VehicleTelemetry()
    {
        var telemetry = new VehicleTelemetry
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            Gps = new GpsLocation(41.8781, -87.6298),
            LastUpdatedAt = DateTimeOffset.UtcNow
        };

        Context.VehicleTelemetry.Add(telemetry);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.VehicleTelemetry.FindAsync(telemetry.Id);

        Assert.NotNull(loaded);
        Assert.Equal("vehicle-001", loaded.VehicleId);
        Assert.NotNull(loaded.Gps);
        Assert.Equal(41.8781, loaded.Gps.Latitude);
        Assert.Equal(-87.6298, loaded.Gps.Longitude);
    }

    [Fact]
    public async Task VehicleTelemetry_VehicleId_Is_Unique()
    {
        var telemetry1 = new VehicleTelemetry
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-unique",
            Gps = new GpsLocation(41.8781, -87.6298),
            LastUpdatedAt = DateTimeOffset.UtcNow
        };

        var telemetry2 = new VehicleTelemetry
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-unique",
            Gps = new GpsLocation(40.7128, -74.0060),
            LastUpdatedAt = DateTimeOffset.UtcNow
        };

        Context.VehicleTelemetry.Add(telemetry1);
        await Context.SaveChangesAsync();

        Context.VehicleTelemetry.Add(telemetry2);
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(
            () => Context.SaveChangesAsync());
    }

    #endregion
}
