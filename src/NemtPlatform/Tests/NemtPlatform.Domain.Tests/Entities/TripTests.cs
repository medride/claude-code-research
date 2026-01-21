namespace NemtPlatform.Domain.Tests.Entities;

using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Domain.Entities.Operations;

public class TripTests
{
    [Fact]
    public void Trip_NewInstance_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var trip = new Trip();

        // Assert
        Assert.Equal(string.Empty, trip.PassengerId);
        Assert.Equal(string.Empty, trip.FundingSourceId);
        Assert.NotNull(trip.CapacityRequirements);
        Assert.NotNull(trip.Stops);
        Assert.Empty(trip.Stops);
    }

    [Fact]
    public void Trip_SetRequiredFields_ShouldMaintainValues()
    {
        // Arrange
        var trip = new Trip
        {
            Id = "trip-001",
            TenantId = "tenant-001",
            PassengerId = "passenger-001",
            FundingSourceId = "funding-001",
            Status = TripStatus.Scheduled
        };

        // Assert
        Assert.Equal("trip-001", trip.Id);
        Assert.Equal("tenant-001", trip.TenantId);
        Assert.Equal("passenger-001", trip.PassengerId);
        Assert.Equal("funding-001", trip.FundingSourceId);
        Assert.Equal(TripStatus.Scheduled, trip.Status);
    }

    [Fact]
    public void Trip_WithCapacityRequirements_ShouldSetCorrectly()
    {
        // Arrange
        var capacity = new CapacityRequirements(
            WheelchairSpaces: 1,
            AmbulatorySeats: 0,
            StretcherCapacity: 0);

        // Act
        var trip = new Trip
        {
            CapacityRequirements = capacity
        };

        // Assert
        Assert.Equal(1, trip.CapacityRequirements.WheelchairSpaces);
        Assert.Equal(0, trip.CapacityRequirements.AmbulatorySeats);
    }

    [Fact]
    public void Trip_WithStops_ShouldAddToCollection()
    {
        // Arrange
        var trip = new Trip();
        var pickupStop = new PassengerStop
        {
            Id = "stop-001",
            Type = StopType.Pickup
        };
        var dropoffStop = new PassengerStop
        {
            Id = "stop-002",
            Type = StopType.Dropoff
        };

        // Act
        trip.Stops.Add(pickupStop);
        trip.Stops.Add(dropoffStop);

        // Assert
        Assert.Equal(2, trip.Stops.Count);
        Assert.Equal(StopType.Pickup, trip.Stops[0].Type);
        Assert.Equal(StopType.Dropoff, trip.Stops[1].Type);
    }

    [Fact]
    public void Trip_OptionalFields_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var trip = new Trip();

        // Assert
        Assert.Null(trip.PartnerId);
        Assert.Null(trip.JourneyId);
        Assert.Null(trip.RouteManifestId);
        Assert.Null(trip.AuthorizationId);
        Assert.Null(trip.ExternalIds);
        Assert.Null(trip.Constraints);
        Assert.Null(trip.PlannedRoute);
        Assert.Null(trip.InternalNotes);
        Assert.Null(trip.RejectionReason);
        Assert.Null(trip.CancellationReason);
    }

    [Theory]
    [InlineData(TripStatus.PendingApproval)]
    [InlineData(TripStatus.Approved)]
    [InlineData(TripStatus.Scheduled)]
    [InlineData(TripStatus.InProgress)]
    [InlineData(TripStatus.Completed)]
    [InlineData(TripStatus.Canceled)]
    public void Trip_StatusTransitions_ShouldAcceptAllValidStatuses(TripStatus status)
    {
        // Arrange
        var trip = new Trip();

        // Act
        trip.Status = status;

        // Assert
        Assert.Equal(status, trip.Status);
    }
}
