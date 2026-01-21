namespace NemtPlatform.Domain.Tests.ValueObjects;

using NemtPlatform.Domain.Common.ValueObjects;

public class GpsLocationTests
{
    [Fact]
    public void GpsLocation_WithValidCoordinates_ShouldCreate()
    {
        // Arrange & Act
        var location = new GpsLocation(40.7128, -74.0060);

        // Assert
        Assert.Equal(40.7128, location.Latitude);
        Assert.Equal(-74.0060, location.Longitude);
    }

    [Fact]
    public void GpsLocation_EqualCoordinates_ShouldBeEqual()
    {
        // Arrange
        var location1 = new GpsLocation(40.7128, -74.0060);
        var location2 = new GpsLocation(40.7128, -74.0060);

        // Assert
        Assert.Equal(location1, location2);
        Assert.True(location1 == location2);
    }

    [Fact]
    public void GpsLocation_DifferentCoordinates_ShouldNotBeEqual()
    {
        // Arrange
        var location1 = new GpsLocation(40.7128, -74.0060);
        var location2 = new GpsLocation(34.0522, -118.2437);

        // Assert
        Assert.NotEqual(location1, location2);
        Assert.True(location1 != location2);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(90, 180)]
    [InlineData(-90, -180)]
    [InlineData(45.5, -122.7)]
    public void GpsLocation_ValidBoundaryCoordinates_ShouldCreate(double lat, double lon)
    {
        // Arrange & Act
        var location = new GpsLocation(lat, lon);

        // Assert
        Assert.Equal(lat, location.Latitude);
        Assert.Equal(lon, location.Longitude);
    }

    [Fact]
    public void GpsLocation_With_ShouldCreateNewInstanceWithModifiedValue()
    {
        // Arrange
        var original = new GpsLocation(40.7128, -74.0060);

        // Act
        var modified = original with { Latitude = 34.0522 };

        // Assert
        Assert.Equal(34.0522, modified.Latitude);
        Assert.Equal(-74.0060, modified.Longitude);
        Assert.NotEqual(original, modified);
    }
}
