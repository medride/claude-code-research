namespace NemtPlatform.Domain.Tests.ValueObjects;

using NemtPlatform.Domain.Common.ValueObjects;

public class TimeWindowTests
{
    [Fact]
    public void TimeWindow_WithAllValues_ShouldCreate()
    {
        // Arrange
        var minStart = new TimeOnly(8, 0);
        var maxStart = new TimeOnly(9, 0);
        var minEnd = new TimeOnly(17, 0);
        var maxEnd = new TimeOnly(18, 0);

        // Act
        var window = new TimeWindow(minStart, maxStart, minEnd, maxEnd);

        // Assert
        Assert.Equal(minStart, window.MinStartTime);
        Assert.Equal(maxStart, window.MaxStartTime);
        Assert.Equal(minEnd, window.MinEndTime);
        Assert.Equal(maxEnd, window.MaxEndTime);
    }

    [Fact]
    public void TimeWindow_WithNullValues_ShouldCreate()
    {
        // Arrange & Act
        var window = new TimeWindow(null, null, null, null);

        // Assert
        Assert.Null(window.MinStartTime);
        Assert.Null(window.MaxStartTime);
        Assert.Null(window.MinEndTime);
        Assert.Null(window.MaxEndTime);
    }

    [Fact]
    public void TimeWindow_WithPartialValues_ShouldCreate()
    {
        // Arrange
        var minStart = new TimeOnly(8, 0);
        var maxEnd = new TimeOnly(18, 0);

        // Act
        var window = new TimeWindow(minStart, null, null, maxEnd);

        // Assert
        Assert.Equal(minStart, window.MinStartTime);
        Assert.Null(window.MaxStartTime);
        Assert.Null(window.MinEndTime);
        Assert.Equal(maxEnd, window.MaxEndTime);
    }

    [Fact]
    public void TimeWindow_EqualValues_ShouldBeEqual()
    {
        // Arrange
        var window1 = new TimeWindow(
            new TimeOnly(8, 0),
            new TimeOnly(9, 0),
            new TimeOnly(17, 0),
            new TimeOnly(18, 0));
        var window2 = new TimeWindow(
            new TimeOnly(8, 0),
            new TimeOnly(9, 0),
            new TimeOnly(17, 0),
            new TimeOnly(18, 0));

        // Assert
        Assert.Equal(window1, window2);
    }

    [Fact]
    public void TimeWindow_DifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var window1 = new TimeWindow(new TimeOnly(8, 0), null, null, null);
        var window2 = new TimeWindow(new TimeOnly(9, 0), null, null, null);

        // Assert
        Assert.NotEqual(window1, window2);
    }
}
