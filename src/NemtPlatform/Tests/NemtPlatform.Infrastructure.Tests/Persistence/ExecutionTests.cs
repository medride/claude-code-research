namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Execution;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;

public class ExecutionTests : ExecutionTestBase
{
    #region AuditLog Tests

    [Fact]
    public async Task Can_Create_AuditLog_With_Required_Fields()
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            EntityType = "Passenger",
            EntityId = "passenger-001",
            Action = AuditAction.Create,
            Timestamp = DateTimeOffset.UtcNow
        };

        Context.AuditLogs.Add(auditLog);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.AuditLogs.FindAsync(auditLog.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Passenger", loaded.EntityType);
        Assert.Equal("passenger-001", loaded.EntityId);
        Assert.Equal(AuditAction.Create, loaded.Action);
    }

    [Fact]
    public async Task Can_Create_AuditLog_With_All_Fields()
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            EntityType = "Trip",
            EntityId = "trip-001",
            Action = AuditAction.Update,
            Changes = "{\"status\": {\"before\": \"Pending\", \"after\": \"Approved\"}}",
            Timestamp = DateTimeOffset.UtcNow,
            UserId = "user-001",
            UserRole = "Dispatcher",
            IpAddress = "192.168.1.100",
            Notes = "Approved for scheduling"
        };

        Context.AuditLogs.Add(auditLog);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.AuditLogs.FindAsync(auditLog.Id);

        Assert.NotNull(loaded);
        Assert.Equal("user-001", loaded.UserId);
        Assert.Equal("Dispatcher", loaded.UserRole);
        Assert.Equal("192.168.1.100", loaded.IpAddress);
        Assert.Contains("status", loaded.Changes!);
    }

    [Fact]
    public async Task All_AuditActions_Can_Be_Stored()
    {
        var actions = new[] { AuditAction.Create, AuditAction.Update, AuditAction.Delete,
                              AuditAction.Access, AuditAction.Login, AuditAction.Logout,
                              AuditAction.PasswordChange, AuditAction.RoleChange,
                              AuditAction.DataExport, AuditAction.DataImport,
                              AuditAction.PermissionChange, AuditAction.Other };

        foreach (var action in actions)
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                EntityType = "User",
                EntityId = $"entity-{action}",
                Action = action,
                Timestamp = DateTimeOffset.UtcNow
            };

            Context.AuditLogs.Add(auditLog);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.AuditLogs.FindAsync(auditLog.Id);
            Assert.Equal(action, loaded!.Action);
        }
    }

    #endregion

    #region Document Tests

    [Fact]
    public async Task Can_Create_Document()
    {
        var document = new Document
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            FileName = "vehicle_registration.pdf",
            FileUrl = "https://storage.example.com/docs/vehicle_registration.pdf",
            DocumentType = DocumentType.Registration,
            AssociatedEntityId = "vehicle-001",
            AssociatedEntityType = AssociatedEntityType.Vehicle,
            UploadDate = DateTimeOffset.UtcNow,
            UploadedByUserId = "user-001",
            Notes = "Annual registration renewal"
        };

        Context.Documents.Add(document);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Documents.FindAsync(document.Id);

        Assert.NotNull(loaded);
        Assert.Equal("vehicle_registration.pdf", loaded.FileName);
        Assert.Equal(DocumentType.Registration, loaded.DocumentType);
        Assert.Equal(AssociatedEntityType.Vehicle, loaded.AssociatedEntityType);
    }

    [Fact]
    public async Task All_DocumentTypes_Can_Be_Stored()
    {
        var types = new[] { DocumentType.Registration, DocumentType.Insurance,
                           DocumentType.Invoice, DocumentType.Title,
                           DocumentType.Photo, DocumentType.Other };

        foreach (var type in types)
        {
            var document = new Document
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                FileName = $"document_{type}.pdf",
                FileUrl = $"https://storage.example.com/docs/{type}.pdf",
                DocumentType = type,
                AssociatedEntityId = "entity-001",
                AssociatedEntityType = AssociatedEntityType.Vehicle,
                UploadDate = DateTimeOffset.UtcNow
            };

            Context.Documents.Add(document);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.Documents.FindAsync(document.Id);
            Assert.Equal(type, loaded!.DocumentType);
        }
    }

    [Fact]
    public async Task All_AssociatedEntityTypes_Can_Be_Stored()
    {
        var types = new[] { AssociatedEntityType.Vehicle, AssociatedEntityType.MaintenanceRecord,
                           AssociatedEntityType.Employee, AssociatedEntityType.Trip };

        foreach (var type in types)
        {
            var document = new Document
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                FileName = $"document_for_{type}.pdf",
                FileUrl = $"https://storage.example.com/docs/{type}.pdf",
                DocumentType = DocumentType.Other,
                AssociatedEntityId = $"entity-{type}",
                AssociatedEntityType = type,
                UploadDate = DateTimeOffset.UtcNow
            };

            Context.Documents.Add(document);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.Documents.FindAsync(document.Id);
            Assert.Equal(type, loaded!.AssociatedEntityType);
        }
    }

    #endregion

    #region Incident Tests

    [Fact]
    public async Task Can_Create_Incident_With_Required_Fields()
    {
        var incident = new Incident
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Type = IncidentType.VehicleBreakdown,
            Status = IncidentStatus.Reported,
            ReportedAt = DateTimeOffset.UtcNow,
            Location = new GpsLocation(41.8781, -87.6298),
            ReportedBy = "driver-001",
            DriverId = "driver-001",
            VehicleId = "vehicle-001",
            RouteId = "route-001",
            PassengerIdsOnBoard = new List<string> { "passenger-001", "passenger-002" },
            Description = "Vehicle experienced flat tire on I-90"
        };

        Context.Incidents.Add(incident);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Incidents.FindAsync(incident.Id);

        Assert.NotNull(loaded);
        Assert.Equal(IncidentType.VehicleBreakdown, loaded.Type);
        Assert.Equal(IncidentStatus.Reported, loaded.Status);
        Assert.Equal(41.8781, loaded.Location.Latitude);
        Assert.Equal(2, loaded.PassengerIdsOnBoard.Count);
    }

    [Fact]
    public async Task Can_Create_Resolved_Incident()
    {
        var incident = new Incident
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Type = IncidentType.ServiceDelay,
            Status = IncidentStatus.Resolved,
            ReportedAt = DateTimeOffset.UtcNow.AddHours(-2),
            Location = new GpsLocation(40.7128, -74.0060),
            ReportedBy = "driver-002",
            DriverId = "driver-002",
            VehicleId = "vehicle-002",
            RouteId = "route-002",
            PassengerIdsOnBoard = new List<string>(),
            Description = "Traffic delay due to accident on highway",
            ActionsTaken = new List<string> { "Notified dispatch", "Rerouted to alternate path", "Contacted affected passengers" },
            ResolutionNotes = "Successfully completed all pickups with 15-minute delay"
        };

        Context.Incidents.Add(incident);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Incidents.FindAsync(incident.Id);

        Assert.NotNull(loaded);
        Assert.Equal(IncidentStatus.Resolved, loaded.Status);
        Assert.NotNull(loaded.ActionsTaken);
        Assert.Equal(3, loaded.ActionsTaken.Count);
        Assert.NotNull(loaded.ResolutionNotes);
    }

    [Fact]
    public async Task All_IncidentTypes_Can_Be_Stored()
    {
        var types = new[] { IncidentType.VehicleAccident, IncidentType.VehicleBreakdown,
                           IncidentType.PassengerMedicalEmergency, IncidentType.DriverMedicalEmergency,
                           IncidentType.SafetyConcern, IncidentType.ServiceDelay };

        foreach (var type in types)
        {
            var incident = new Incident
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Type = type,
                Status = IncidentStatus.Reported,
                ReportedAt = DateTimeOffset.UtcNow,
                Location = new GpsLocation(0, 0),
                ReportedBy = "reporter-001",
                DriverId = "driver-001",
                VehicleId = "vehicle-001",
                RouteId = "route-001",
                PassengerIdsOnBoard = new List<string>(),
                Description = $"Incident of type {type}"
            };

            Context.Incidents.Add(incident);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.Incidents.FindAsync(incident.Id);
            Assert.Equal(type, loaded!.Type);
        }
    }

    #endregion

    #region TripExecution Tests

    [Fact]
    public async Task Can_Create_TripExecution_With_Required_Fields()
    {
        var execution = new TripExecution
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            TripId = "trip-001",
            RouteId = "route-001",
            LiveStatus = LiveStatus.Dispatched,
            Reconciliations = new List<StopReconciliation>()
        };

        Context.TripExecutions.Add(execution);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.TripExecutions.FindAsync(execution.Id);

        Assert.NotNull(loaded);
        Assert.Equal("trip-001", loaded.TripId);
        Assert.Equal(LiveStatus.Dispatched, loaded.LiveStatus);
    }

    [Fact]
    public async Task Can_Create_TripExecution_With_Route_Data()
    {
        var execution = new TripExecution
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            TripId = "trip-002",
            RouteId = "route-001",
            LiveStatus = LiveStatus.EnRouteToPickup,
            ApproachRoute = new DirectionsData(
                "approachPolyline",
                new Distance("2.5 mi", 4023),
                new Duration("8 mins", 480)
            ),
            OnTimeStatus = OnTimeStatus.OnTime,
            Reconciliations = new List<StopReconciliation>()
        };

        Context.TripExecutions.Add(execution);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.TripExecutions.FindAsync(execution.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.ApproachRoute);
        Assert.Equal("approachPolyline", loaded.ApproachRoute.EncodedPolyline);
        Assert.Equal(OnTimeStatus.OnTime, loaded.OnTimeStatus);
    }

    [Fact]
    public async Task Can_Create_TripExecution_With_Reconciliations()
    {
        var reconciliations = new List<StopReconciliation>
        {
            new StopReconciliation(
                stopId: "stop-pickup",
                outcome: StopOutcome.CompletedAsPlanned,
                actualCapacityDelta: new CapacityRequirements(0, 1, 0),
                timestamp: DateTimeOffset.UtcNow.AddMinutes(-30),
                verifiedBy: "driver-001",
                verificationMethod: ReconciliationMethod.Signature,
                signatureData: "base64SignatureData"
            ),
            new StopReconciliation(
                stopId: "stop-dropoff",
                outcome: StopOutcome.CompletedAsPlanned,
                actualCapacityDelta: new CapacityRequirements(0, -1, 0),
                timestamp: DateTimeOffset.UtcNow,
                verifiedBy: "driver-001",
                verificationMethod: ReconciliationMethod.Photo,
                photoUrl: "https://storage.example.com/photos/dropoff-001.jpg",
                handOffRecipient: new HandOffRecipient(HandOffRecipientType.Staff, "Nurse Johnson")
            )
        };

        var execution = new TripExecution
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            TripId = "trip-003",
            RouteId = "route-001",
            LiveStatus = LiveStatus.WaitingAtDropoff,
            ActualStartTime = DateTimeOffset.UtcNow.AddMinutes(-45),
            ActualEndTime = DateTimeOffset.UtcNow,
            Reconciliations = reconciliations
        };

        Context.TripExecutions.Add(execution);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.TripExecutions.FindAsync(execution.Id);

        Assert.NotNull(loaded);
        Assert.Equal(2, loaded.Reconciliations.Count);

        var pickup = loaded.Reconciliations.First(r => r.StopId == "stop-pickup");
        Assert.Equal(StopOutcome.CompletedAsPlanned, pickup.Outcome);
        Assert.Equal(ReconciliationMethod.Signature, pickup.VerificationMethod);

        var dropoff = loaded.Reconciliations.First(r => r.StopId == "stop-dropoff");
        Assert.NotNull(dropoff.HandOffRecipient);
        Assert.Equal(HandOffRecipientType.Staff, dropoff.HandOffRecipient.Type);
        Assert.Equal("Nurse Johnson", dropoff.HandOffRecipient.Name);
    }

    [Fact]
    public async Task Can_Create_TripExecution_With_ScannedData()
    {
        var reconciliation = new StopReconciliation(
            stopId: "stop-001",
            outcome: StopOutcome.CompletedAsPlanned,
            actualCapacityDelta: new CapacityRequirements(0, 1, 0),
            timestamp: DateTimeOffset.UtcNow,
            verifiedBy: "driver-001",
            verificationMethod: ReconciliationMethod.Scan,
            scannedData: new ScannedData(ScanType.QrCode, "PASSENGER-001-VERIFIED")
        );

        var execution = new TripExecution
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            TripId = "trip-004",
            RouteId = "route-001",
            LiveStatus = LiveStatus.Transporting,
            Reconciliations = new List<StopReconciliation> { reconciliation }
        };

        Context.TripExecutions.Add(execution);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.TripExecutions.FindAsync(execution.Id);

        Assert.NotNull(loaded);
        Assert.Single(loaded.Reconciliations);
        Assert.NotNull(loaded.Reconciliations[0].ScannedData);
        Assert.Equal(ScanType.QrCode, loaded.Reconciliations[0].ScannedData.Type);
        Assert.Equal("PASSENGER-001-VERIFIED", loaded.Reconciliations[0].ScannedData.Value);
    }

    [Fact]
    public async Task All_LiveStatuses_Can_Be_Stored()
    {
        var statuses = new[] { LiveStatus.Dispatched, LiveStatus.EnRouteToPickup,
                               LiveStatus.WaitingAtPickup, LiveStatus.Transporting,
                               LiveStatus.WaitingAtDropoff };

        foreach (var status in statuses)
        {
            var execution = new TripExecution
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                TripId = $"trip-{status}",
                RouteId = "route-001",
                LiveStatus = status,
                Reconciliations = new List<StopReconciliation>()
            };

            Context.TripExecutions.Add(execution);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.TripExecutions.FindAsync(execution.Id);
            Assert.Equal(status, loaded!.LiveStatus);
        }
    }

    [Fact]
    public async Task TripExecution_TripId_Is_Unique()
    {
        var execution1 = new TripExecution
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            TripId = "trip-unique",
            RouteId = "route-001",
            LiveStatus = LiveStatus.Dispatched,
            Reconciliations = new List<StopReconciliation>()
        };

        var execution2 = new TripExecution
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            TripId = "trip-unique",
            RouteId = "route-002",
            LiveStatus = LiveStatus.EnRouteToPickup,
            Reconciliations = new List<StopReconciliation>()
        };

        Context.TripExecutions.Add(execution1);
        await Context.SaveChangesAsync();

        Context.TripExecutions.Add(execution2);
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(
            () => Context.SaveChangesAsync());
    }

    #endregion
}
