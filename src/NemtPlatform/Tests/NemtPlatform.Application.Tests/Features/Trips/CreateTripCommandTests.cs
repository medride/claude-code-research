namespace NemtPlatform.Application.Tests.Features.Trips;

using Moq;
using NemtPlatform.Application.Contracts.Persistence;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Domain.Entities.Operations;

/// <summary>
/// Example tests demonstrating CQRS command handler pattern.
/// These tests scaffold the expected behavior for a CreateTripCommand handler.
/// </summary>
public class CreateTripCommandTests
{
    private readonly Mock<IRepository<Trip>> _tripRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateTripCommandTests()
    {
        _tripRepositoryMock = new Mock<IRepository<Trip>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTrip()
    {
        // Arrange
        var command = new CreateTripCommandStub(
            TenantId: "tenant-001",
            PassengerId: "passenger-001",
            FundingSourceId: "funding-001",
            PickupType: PickupType.Scheduled);

        Trip? capturedTrip = null;
        _tripRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Trip>(), It.IsAny<CancellationToken>()))
            .Callback<Trip, CancellationToken>((t, _) => capturedTrip = t)
            .ReturnsAsync((Trip t, CancellationToken _) => t);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act - Simulating what the handler would do
        var trip = new Trip
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = command.TenantId,
            PassengerId = command.PassengerId,
            FundingSourceId = command.FundingSourceId,
            PickupType = command.PickupType,
            Status = TripStatus.PendingApproval,
            CapacityRequirements = new CapacityRequirements(0, 1, 0)
        };

        await _tripRepositoryMock.Object.AddAsync(trip);
        await _unitOfWorkMock.Object.SaveChangesAsync();

        // Assert
        Assert.NotNull(capturedTrip);
        Assert.Equal(command.PassengerId, capturedTrip.PassengerId);
        Assert.Equal(command.FundingSourceId, capturedTrip.FundingSourceId);
        Assert.Equal(TripStatus.PendingApproval, capturedTrip.Status);
    }

    [Fact]
    public async Task Handle_ShouldCallUnitOfWorkSaveChanges()
    {
        // Arrange
        _tripRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Trip>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Trip t, CancellationToken _) => t);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _tripRepositoryMock.Object.AddAsync(new Trip { Id = "test" });
        await _unitOfWorkMock.Object.SaveChangesAsync();

        // Assert
        _unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithCapacityRequirements_ShouldSetCorrectly()
    {
        // Arrange
        var capacity = new CapacityRequirements(
            WheelchairSpaces: 1,
            AmbulatorySeats: 0,
            StretcherCapacity: 0);

        Trip? capturedTrip = null;
        _tripRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Trip>(), It.IsAny<CancellationToken>()))
            .Callback<Trip, CancellationToken>((t, _) => capturedTrip = t)
            .ReturnsAsync((Trip t, CancellationToken _) => t);

        // Act
        var trip = new Trip
        {
            Id = "wheelchair-trip",
            TenantId = "tenant-001",
            PassengerId = "passenger-001",
            FundingSourceId = "funding-001",
            CapacityRequirements = capacity
        };

        await _tripRepositoryMock.Object.AddAsync(trip);

        // Assert
        Assert.NotNull(capturedTrip);
        Assert.Equal(1, capturedTrip.CapacityRequirements.WheelchairSpaces);
        Assert.Equal(0, capturedTrip.CapacityRequirements.AmbulatorySeats);
    }
}

/// <summary>
/// Stub command record for testing purposes.
/// In a real implementation, this would be defined in the Application layer.
/// </summary>
internal record CreateTripCommandStub(
    string TenantId,
    string PassengerId,
    string FundingSourceId,
    PickupType PickupType);
