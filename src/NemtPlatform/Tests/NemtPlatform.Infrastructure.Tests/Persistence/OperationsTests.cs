namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Operations;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;

public class OperationsTests : OperationsTestBase
{
    #region Trip Tests

    [Fact]
    public async Task Can_Create_Trip_With_Required_Fields()
    {
        var trip = new Trip
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PassengerId = "passenger-001",
            FundingSourceId = "funding-001",
            Status = TripStatus.PendingApproval,
            PickupType = PickupType.Scheduled,
            CapacityRequirements = new CapacityRequirements(0, 1, 0),
            Stops = new List<PassengerStop>()
        };

        Context.Trips.Add(trip);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Trips.FindAsync(trip.Id);

        Assert.NotNull(loaded);
        Assert.Equal("passenger-001", loaded.PassengerId);
        Assert.Equal(TripStatus.PendingApproval, loaded.Status);
        Assert.Equal(PickupType.Scheduled, loaded.PickupType);
    }

    [Fact]
    public async Task Can_Create_Trip_With_Stops()
    {
        var stops = new List<PassengerStop>
        {
            new PassengerStop
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Type = StopType.Pickup,
                Status = StopStatus.Pending,
                PassengerId = "passenger-001",
                AccessPointId = "ap-001",
                PlaceId = "place-001",
                Duration = TimeSpan.FromMinutes(5),
                CapacityDelta = new CapacityRequirements(0, 1, 0),
                TimeWindows = new List<TimeWindow>
                {
                    new TimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 30), null, null)
                }
            },
            new PassengerStop
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Type = StopType.Dropoff,
                Status = StopStatus.Pending,
                PassengerId = "passenger-001",
                AccessPointId = "ap-002",
                PlaceId = "place-002",
                Duration = TimeSpan.FromMinutes(5),
                CapacityDelta = new CapacityRequirements(0, -1, 0),
                TimeWindows = new List<TimeWindow>()
            }
        };

        var trip = new Trip
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PassengerId = "passenger-001",
            FundingSourceId = "funding-001",
            Status = TripStatus.Approved,
            PickupType = PickupType.Scheduled,
            CapacityRequirements = new CapacityRequirements(0, 1, 0),
            Stops = stops
        };

        Context.Trips.Add(trip);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Trips.FindAsync(trip.Id);

        Assert.NotNull(loaded);
        Assert.Equal(2, loaded.Stops.Count);
        Assert.Contains(loaded.Stops, s => s.Type == StopType.Pickup);
        Assert.Contains(loaded.Stops, s => s.Type == StopType.Dropoff);
    }

    [Fact]
    public async Task Can_Create_Trip_With_PlannedRoute()
    {
        var trip = new Trip
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PassengerId = "passenger-001",
            FundingSourceId = "funding-001",
            Status = TripStatus.Scheduled,
            PickupType = PickupType.Scheduled,
            CapacityRequirements = new CapacityRequirements(0, 1, 0),
            Stops = new List<PassengerStop>(),
            PlannedRoute = new DirectionsData(
                "encodedPolylineString",
                new Distance("5.2 mi", 8369),
                new Duration("15 mins", 900)
            )
        };

        Context.Trips.Add(trip);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Trips.FindAsync(trip.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.PlannedRoute);
        Assert.Equal("encodedPolylineString", loaded.PlannedRoute.EncodedPolyline);
        Assert.Equal(8369, loaded.PlannedRoute.Distance!.ValueInMeters);
    }

    [Fact]
    public async Task All_TripStatuses_Can_Be_Stored()
    {
        var statuses = new[] { TripStatus.PendingApproval, TripStatus.Rejected, TripStatus.Approved,
                               TripStatus.Scheduled, TripStatus.InProgress, TripStatus.Completed,
                               TripStatus.Incomplete, TripStatus.Canceled };

        foreach (var status in statuses)
        {
            var trip = new Trip
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                PassengerId = $"passenger-{status}",
                FundingSourceId = "funding-001",
                Status = status,
                PickupType = PickupType.Scheduled,
                CapacityRequirements = new CapacityRequirements(),
                Stops = new List<PassengerStop>()
            };

            Context.Trips.Add(trip);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.Trips.FindAsync(trip.Id);
            Assert.Equal(status, loaded!.Status);
        }
    }

    #endregion

    #region Route Tests

    [Fact]
    public async Task Can_Create_Route_With_Required_Fields()
    {
        var route = new Route
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            ShiftId = "shift-001",
            Stops = new List<BaseStop>(),
            EstimatedStartTime = DateTimeOffset.UtcNow,
            EstimatedEndTime = DateTimeOffset.UtcNow.AddHours(4),
            EstimatedTotalDistance = 50000,
            EstimatedDuration = TimeSpan.FromHours(4)
        };

        Context.Routes.Add(route);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Routes.FindAsync(route.Id);

        Assert.NotNull(loaded);
        Assert.Equal("shift-001", loaded.ShiftId);
        Assert.Equal(50000, loaded.EstimatedTotalDistance);
    }

    [Fact]
    public async Task Can_Create_Route_With_Mixed_Stop_Types()
    {
        var stops = new List<BaseStop>
        {
            new PassengerStop
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Type = StopType.Pickup,
                Status = StopStatus.Pending,
                AccessPointId = "ap-001",
                PlaceId = "place-001",
                Duration = TimeSpan.FromMinutes(5),
                CapacityDelta = new CapacityRequirements(0, 1, 0),
                TimeWindows = new List<TimeWindow>()
            },
            new DriverServiceStop
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Type = StopType.Break,
                Status = StopStatus.Pending,
                Duration = TimeSpan.FromMinutes(30),
                Location = new GpsLocation(41.8781, -87.6298),
                TimeWindows = new List<TimeWindow>()
            },
            new PassengerStop
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Type = StopType.Dropoff,
                Status = StopStatus.Pending,
                AccessPointId = "ap-002",
                PlaceId = "place-002",
                Duration = TimeSpan.FromMinutes(5),
                CapacityDelta = new CapacityRequirements(0, -1, 0),
                TimeWindows = new List<TimeWindow>()
            }
        };

        var route = new Route
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            ShiftId = "shift-001",
            Stops = stops,
            EstimatedStartTime = DateTimeOffset.UtcNow,
            EstimatedEndTime = DateTimeOffset.UtcNow.AddHours(4),
            EstimatedTotalDistance = 50000,
            EstimatedDuration = TimeSpan.FromHours(4)
        };

        Context.Routes.Add(route);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Routes.FindAsync(route.Id);

        Assert.NotNull(loaded);
        Assert.Equal(3, loaded.Stops.Count);
        Assert.Equal(2, loaded.Stops.OfType<PassengerStop>().Count());
        Assert.Single(loaded.Stops.OfType<DriverServiceStop>());
    }

    #endregion

    #region RouteManifest Tests

    [Fact]
    public async Task Can_Create_RouteManifest()
    {
        var manifest = new RouteManifest
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Route 101 AM",
            TripIds = new List<string> { "trip-001", "trip-002", "trip-003" },
            Status = RouteManifestStatus.Planning
        };

        Context.RouteManifests.Add(manifest);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.RouteManifests.FindAsync(manifest.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Route 101 AM", loaded.Name);
        Assert.Equal(3, loaded.TripIds.Count);
        Assert.Equal(RouteManifestStatus.Planning, loaded.Status);
    }

    [Fact]
    public async Task All_RouteManifestStatuses_Can_Be_Stored()
    {
        var statuses = new[] { RouteManifestStatus.Planning, RouteManifestStatus.Dispatched,
                               RouteManifestStatus.Completed };

        foreach (var status in statuses)
        {
            var manifest = new RouteManifest
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Name = $"Route for {status}",
                TripIds = new List<string>(),
                Status = status
            };

            Context.RouteManifests.Add(manifest);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.RouteManifests.FindAsync(manifest.Id);
            Assert.Equal(status, loaded!.Status);
        }
    }

    #endregion

    #region Shift Tests

    [Fact]
    public async Task Can_Create_Shift_With_Required_Fields()
    {
        var shift = new Shift
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "vehicle-001",
            Personnel = new List<ShiftPersonnel>
            {
                new ShiftPersonnel("employee-001", CrewRole.Driver)
            },
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow.AddHours(8),
            StartLocation = new GpsLocation(41.8781, -87.6298),
            EndLocation = new GpsLocation(41.8900, -87.6500)
        };

        Context.Shifts.Add(shift);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Shifts.FindAsync(shift.Id);

        Assert.NotNull(loaded);
        Assert.Equal("vehicle-001", loaded.VehicleId);
        Assert.Single(loaded.Personnel);
        Assert.Equal(CrewRole.Driver, loaded.Personnel[0].Role);
        Assert.Equal(41.8781, loaded.StartLocation.Latitude);
    }

    [Fact]
    public async Task Can_Create_Shift_With_Multi_Person_Crew()
    {
        var shift = new Shift
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            VehicleId = "ambulance-001",
            Personnel = new List<ShiftPersonnel>
            {
                new ShiftPersonnel("employee-001", CrewRole.Driver),
                new ShiftPersonnel("employee-002", CrewRole.Emt),
                new ShiftPersonnel("employee-003", CrewRole.Paramedic)
            },
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow.AddHours(12),
            StartLocation = new GpsLocation(41.8781, -87.6298),
            EndLocation = new GpsLocation(41.8781, -87.6298)
        };

        Context.Shifts.Add(shift);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Shifts.FindAsync(shift.Id);

        Assert.NotNull(loaded);
        Assert.Equal(3, loaded.Personnel.Count);
        Assert.Contains(loaded.Personnel, p => p.Role == CrewRole.Driver);
        Assert.Contains(loaded.Personnel, p => p.Role == CrewRole.Emt);
        Assert.Contains(loaded.Personnel, p => p.Role == CrewRole.Paramedic);
    }

    #endregion

    #region ShiftSession Tests

    [Fact]
    public async Task Can_Create_ShiftSession()
    {
        var session = new ShiftSession
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            ShiftId = "shift-001",
            StartTime = DateTimeOffset.UtcNow
        };

        Context.ShiftSessions.Add(session);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.ShiftSessions.FindAsync(session.Id);

        Assert.NotNull(loaded);
        Assert.Equal("shift-001", loaded.ShiftId);
        Assert.Null(loaded.EndTime);
    }

    [Fact]
    public async Task Can_Create_Completed_ShiftSession()
    {
        var session = new ShiftSession
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            ShiftId = "shift-001",
            StartTime = DateTimeOffset.UtcNow.AddHours(-8),
            EndTime = DateTimeOffset.UtcNow
        };

        Context.ShiftSessions.Add(session);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.ShiftSessions.FindAsync(session.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.EndTime);
    }

    #endregion

    #region Journey Tests

    [Fact]
    public async Task Can_Create_Journey_With_Legs()
    {
        var journey = new Journey
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PassengerId = "passenger-001",
            Name = "Weekly Dialysis - Monday",
            BookingDate = DateTimeOffset.UtcNow,
            Legs = new List<JourneyLeg>
            {
                new JourneyLeg("trip-001", new LegTransition("WaitAndReturn", TimeSpan.FromHours(3))),
                new JourneyLeg("trip-002", null)
            }
        };

        Context.Journeys.Add(journey);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Journeys.FindAsync(journey.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Weekly Dialysis - Monday", loaded.Name);
        Assert.Equal(2, loaded.Legs.Count);
        Assert.Equal("trip-001", loaded.Legs[0].TripId);
        Assert.NotNull(loaded.Legs[0].TransitionToNext);
        Assert.Equal("WaitAndReturn", loaded.Legs[0].TransitionToNext.Type);
    }

    #endregion

    #region StandingOrder Tests

    [Fact]
    public async Task Can_Create_StandingOrder_With_Required_Fields()
    {
        var standingOrder = new StandingOrder
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "John's Weekly Dialysis",
            PassengerId = "passenger-001",
            Status = StandingOrderStatus.Active,
            RecurrenceRule = "FREQ=WEEKLY;BYDAY=MO,WE,FR",
            EffectiveDateRange = new DateRange(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddMonths(6)
            ),
            JourneyTemplate = new JourneyTemplate(
                fundingSourceId: "funding-001",
                capacityRequirements: new CapacityRequirements(0, 1, 0),
                legs: new List<JourneyLegTemplate>
                {
                    new JourneyLegTemplate(
                        stops: new List<StopTemplate>
                        {
                            new StopTemplate(StopType.Pickup, "ap-001", "place-001", TimeSpan.FromMinutes(5), new List<TimeWindow>()),
                            new StopTemplate(StopType.Dropoff, "ap-002", "place-002", TimeSpan.FromMinutes(5), new List<TimeWindow>())
                        },
                        transitionToNext: new LegTransition("WaitAndReturn", TimeSpan.FromHours(3))
                    )
                }
            )
        };

        Context.StandingOrders.Add(standingOrder);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.StandingOrders.FindAsync(standingOrder.Id);

        Assert.NotNull(loaded);
        Assert.Equal("John's Weekly Dialysis", loaded.Name);
        Assert.Equal(StandingOrderStatus.Active, loaded.Status);
        Assert.Equal("FREQ=WEEKLY;BYDAY=MO,WE,FR", loaded.RecurrenceRule);
        Assert.NotNull(loaded.EffectiveDateRange);
        Assert.NotNull(loaded.JourneyTemplate);
        Assert.Equal("funding-001", loaded.JourneyTemplate.FundingSourceId);
    }

    [Fact]
    public async Task Can_Create_StandingOrder_With_ExclusionDates()
    {
        var standingOrder = new StandingOrder
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Daily Transport with Holidays",
            PassengerId = "passenger-001",
            Status = StandingOrderStatus.Active,
            RecurrenceRule = "FREQ=DAILY",
            EffectiveDateRange = new DateRange(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(1)
            ),
            ExclusionDates = new List<string> { "2024-12-25", "2024-12-26", "2025-01-01" },
            JourneyTemplate = new JourneyTemplate(
                fundingSourceId: "funding-001",
                capacityRequirements: new CapacityRequirements(),
                legs: new List<JourneyLegTemplate>()
            )
        };

        Context.StandingOrders.Add(standingOrder);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.StandingOrders.FindAsync(standingOrder.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.ExclusionDates);
        Assert.Equal(3, loaded.ExclusionDates.Count);
        Assert.Contains("2024-12-25", loaded.ExclusionDates);
    }

    [Fact]
    public async Task All_StandingOrderStatuses_Can_Be_Stored()
    {
        var statuses = new[] { StandingOrderStatus.Active, StandingOrderStatus.Paused,
                               StandingOrderStatus.Ended };

        foreach (var status in statuses)
        {
            var standingOrder = new StandingOrder
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Name = $"Order with status {status}",
                PassengerId = "passenger-001",
                Status = status,
                RecurrenceRule = "FREQ=WEEKLY",
                EffectiveDateRange = new DateRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMonths(1)),
                JourneyTemplate = new JourneyTemplate("funding-001", new CapacityRequirements(), new List<JourneyLegTemplate>())
            };

            Context.StandingOrders.Add(standingOrder);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.StandingOrders.FindAsync(standingOrder.Id);
            Assert.Equal(status, loaded!.Status);
        }
    }

    #endregion
}
