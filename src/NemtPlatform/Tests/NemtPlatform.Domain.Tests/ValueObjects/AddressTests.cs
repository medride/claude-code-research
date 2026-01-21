namespace NemtPlatform.Domain.Tests.ValueObjects;

using NemtPlatform.Domain.Common.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Address_WithAllFields_ShouldCreate()
    {
        // Arrange & Act
        var address = new Address(
            Street: "123 Main St",
            City: "New York",
            State: "NY",
            ZipCode: "10001");

        // Assert
        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("New York", address.City);
        Assert.Equal("NY", address.State);
        Assert.Equal("10001", address.ZipCode);
    }

    [Fact]
    public void Address_EqualAddresses_ShouldBeEqual()
    {
        // Arrange
        var address1 = new Address("123 Main St", "NYC", "NY", "10001");
        var address2 = new Address("123 Main St", "NYC", "NY", "10001");

        // Assert
        Assert.Equal(address1, address2);
        Assert.True(address1 == address2);
    }

    [Fact]
    public void Address_DifferentStreet_ShouldNotBeEqual()
    {
        // Arrange
        var address1 = new Address("123 Main St", "NYC", "NY", "10001");
        var address2 = new Address("456 Oak Ave", "NYC", "NY", "10001");

        // Assert
        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void Address_DifferentCity_ShouldNotBeEqual()
    {
        // Arrange
        var address1 = new Address("123 Main St", "NYC", "NY", "10001");
        var address2 = new Address("123 Main St", "Los Angeles", "CA", "90001");

        // Assert
        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void Address_With_ShouldCreateModifiedCopy()
    {
        // Arrange
        var original = new Address("123 Main St", "NYC", "NY", "10001");

        // Act
        var modified = original with { City = "Los Angeles", State = "CA", ZipCode = "90001" };

        // Assert
        Assert.Equal("123 Main St", modified.Street);
        Assert.Equal("Los Angeles", modified.City);
        Assert.Equal("CA", modified.State);
        Assert.Equal("90001", modified.ZipCode);
    }
}
