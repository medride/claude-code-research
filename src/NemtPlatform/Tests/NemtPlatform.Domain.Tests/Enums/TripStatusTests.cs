namespace NemtPlatform.Domain.Tests.Enums;

using NemtPlatform.Domain.Common.Enums;

public class TripStatusTests
{
    [Fact]
    public void TripStatus_ShouldHaveExpectedValues()
    {
        // Assert - verify all expected statuses exist
        Assert.True(Enum.IsDefined(typeof(TripStatus), TripStatus.PendingApproval));
        Assert.True(Enum.IsDefined(typeof(TripStatus), TripStatus.Approved));
        Assert.True(Enum.IsDefined(typeof(TripStatus), TripStatus.Scheduled));
        Assert.True(Enum.IsDefined(typeof(TripStatus), TripStatus.InProgress));
        Assert.True(Enum.IsDefined(typeof(TripStatus), TripStatus.Completed));
        Assert.True(Enum.IsDefined(typeof(TripStatus), TripStatus.Canceled));
    }

    [Fact]
    public void TripStatus_Count_ShouldMatchExpected()
    {
        // Arrange
        var values = Enum.GetValues<TripStatus>();

        // Assert - TripStatus has 8 values
        Assert.Equal(8, values.Length);
    }

    [Theory]
    [InlineData("PendingApproval", TripStatus.PendingApproval)]
    [InlineData("Approved", TripStatus.Approved)]
    [InlineData("Scheduled", TripStatus.Scheduled)]
    [InlineData("InProgress", TripStatus.InProgress)]
    [InlineData("Completed", TripStatus.Completed)]
    [InlineData("Canceled", TripStatus.Canceled)]
    public void TripStatus_Parse_ShouldReturnCorrectValue(string name, TripStatus expected)
    {
        // Act
        var result = Enum.Parse<TripStatus>(name);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TripStatus_ToString_ShouldReturnName()
    {
        // Arrange & Act
        var pendingApproval = TripStatus.PendingApproval.ToString();
        var completed = TripStatus.Completed.ToString();

        // Assert
        Assert.Equal("PendingApproval", pendingApproval);
        Assert.Equal("Completed", completed);
    }
}
