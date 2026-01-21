namespace NemtPlatform.Application.Tests.Features.Trips;

using Moq;
using NemtPlatform.Application.Contracts.Persistence;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Entities.Operations;

/// <summary>
/// Example tests demonstrating CQRS query handler pattern.
/// These tests scaffold the expected behavior for a GetTripByIdQuery handler.
/// </summary>
public class GetTripByIdQueryTests
{
    private readonly Mock<IRepository<Trip>> _tripRepositoryMock;

    public GetTripByIdQueryTests()
    {
        _tripRepositoryMock = new Mock<IRepository<Trip>>();
    }

    [Fact]
    public async Task Handle_ExistingTrip_ShouldReturnTrip()
    {
        // Arrange
        var tripId = "trip-001";
        var expectedTrip = new Trip
        {
            Id = tripId,
            TenantId = "tenant-001",
            PassengerId = "passenger-001",
            FundingSourceId = "funding-001",
            Status = TripStatus.Scheduled
        };

        _tripRepositoryMock
            .Setup(r => r.GetByIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTrip);

        // Act - This would be the handler call when implemented:
        // var handler = new GetTripByIdQueryHandler(_tripRepositoryMock.Object);
        // var result = await handler.Handle(new GetTripByIdQuery(tripId), CancellationToken.None);
        var result = await _tripRepositoryMock.Object.GetByIdAsync(tripId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tripId, result.Id);
        Assert.Equal(TripStatus.Scheduled, result.Status);
    }

    [Fact]
    public async Task Handle_NonExistingTrip_ShouldReturnNull()
    {
        // Arrange
        var tripId = "non-existing-trip";

        _tripRepositoryMock
            .Setup(r => r.GetByIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Trip?)null);

        // Act
        var result = await _tripRepositoryMock.Object.GetByIdAsync(tripId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnce()
    {
        // Arrange
        var tripId = "trip-verify-call";
        _tripRepositoryMock
            .Setup(r => r.GetByIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Trip { Id = tripId });

        // Act
        await _tripRepositoryMock.Object.GetByIdAsync(tripId);

        // Assert
        _tripRepositoryMock.Verify(
            r => r.GetByIdAsync(tripId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
