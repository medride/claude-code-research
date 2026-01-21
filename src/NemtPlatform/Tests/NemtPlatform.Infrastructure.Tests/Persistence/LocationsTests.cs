namespace NemtPlatform.Infrastructure.Tests.Persistence;

using NemtPlatform.Domain.Entities.Locations;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;
using NemtPlatform.Infrastructure.Tests.Helpers;

public class LocationsTests : LocationsTestBase
{
    #region Place Tests

    [Fact]
    public async Task Can_Create_Place_With_Required_Fields()
    {
        var place = new Place
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Mercy General Hospital",
            Type = PlaceType.Hospital,
            Address = new Address("123 Medical Dr", "Springfield", "IL", "62701")
        };

        Context.Places.Add(place);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Places.FindAsync(place.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Mercy General Hospital", loaded.Name);
        Assert.Equal(PlaceType.Hospital, loaded.Type);
        Assert.NotNull(loaded.Address);
        Assert.Equal("123 Medical Dr", loaded.Address.Street);
    }

    [Fact]
    public async Task Can_Create_Place_With_CenterGps()
    {
        var place = new Place
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "City Clinic",
            Type = PlaceType.Clinic,
            Address = new Address("456 Health Ave", "Chicago", "IL", "60601"),
            CenterGps = new GpsLocation(41.8781, -87.6298)
        };

        Context.Places.Add(place);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Places.FindAsync(place.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.CenterGps);
        Assert.Equal(41.8781, loaded.CenterGps.Latitude);
        Assert.Equal(-87.6298, loaded.CenterGps.Longitude);
    }

    [Fact]
    public async Task All_PlaceTypes_Can_Be_Stored()
    {
        var types = new[] { PlaceType.Hospital, PlaceType.Clinic, PlaceType.School,
                           PlaceType.Residence, PlaceType.Business };

        foreach (var type in types)
        {
            var place = new Place
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = "tenant-001",
                Name = $"Place of type {type}",
                Type = type,
                Address = new Address("1 Test St", "City", "ST", "12345")
            };

            Context.Places.Add(place);
            await Context.SaveChangesAsync();

            using var newContext = CreateNewContext();
            var loaded = await newContext.Places.FindAsync(place.Id);
            Assert.Equal(type, loaded!.Type);
        }
    }

    #endregion

    #region AccessPoint Tests

    [Fact]
    public async Task Can_Create_AccessPoint_With_Required_Fields()
    {
        var accessPoint = new AccessPoint
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PlaceId = "place-001",
            Name = "Emergency Room Entrance",
            Gps = new GpsLocation(41.8781, -87.6298),
            Tags = new List<AccessPointTag> { AccessPointTag.Entrance, AccessPointTag.WheelchairAccessible }
        };

        Context.AccessPoints.Add(accessPoint);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.AccessPoints.FindAsync(accessPoint.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Emergency Room Entrance", loaded.Name);
        Assert.Equal(41.8781, loaded.Gps.Latitude);
        Assert.Equal(2, loaded.Tags.Count);
        Assert.Contains(AccessPointTag.Entrance, loaded.Tags);
        Assert.Contains(AccessPointTag.WheelchairAccessible, loaded.Tags);
    }

    [Fact]
    public async Task Can_Create_AccessPoint_With_All_Fields()
    {
        var accessPoint = new AccessPoint
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PlaceId = "place-001",
            Name = "Main Lobby",
            Gps = new GpsLocation(40.7128, -74.0060),
            Tags = new List<AccessPointTag> { AccessPointTag.Entrance, AccessPointTag.Exit },
            OperatingHours = "Mon-Fri 07:00-18:00",
            Instructions = "Check in at front desk. Parking in lot B."
        };

        Context.AccessPoints.Add(accessPoint);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.AccessPoints.FindAsync(accessPoint.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Mon-Fri 07:00-18:00", loaded.OperatingHours);
        Assert.Equal("Check in at front desk. Parking in lot B.", loaded.Instructions);
    }

    [Fact]
    public async Task AccessPoint_Tags_Persists_As_Json()
    {
        var tags = new List<AccessPointTag>
        {
            AccessPointTag.Entrance,
            AccessPointTag.DropOff,
            AccessPointTag.WheelchairAccessible,
            AccessPointTag.StretcherAccessible
        };

        var accessPoint = new AccessPoint
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            PlaceId = "place-001",
            Name = "Accessible Entrance",
            Gps = new GpsLocation(0, 0),
            Tags = tags
        };

        Context.AccessPoints.Add(accessPoint);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.AccessPoints.FindAsync(accessPoint.Id);

        Assert.NotNull(loaded);
        Assert.Equal(4, loaded.Tags.Count);
        Assert.Equal(tags, loaded.Tags);
    }

    #endregion

    #region Region Tests

    [Fact]
    public async Task Can_Create_Region_With_ZipCodes()
    {
        var region = new Region
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "North County",
            ZipCodes = new List<string> { "62701", "62702", "62703", "62704" }
        };

        Context.Regions.Add(region);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Regions.FindAsync(region.Id);

        Assert.NotNull(loaded);
        Assert.Equal("North County", loaded.Name);
        Assert.NotNull(loaded.ZipCodes);
        Assert.Equal(4, loaded.ZipCodes.Count);
        Assert.Contains("62701", loaded.ZipCodes);
    }

    [Fact]
    public async Task Can_Create_Region_With_GeoJsonBoundary()
    {
        // Simple square polygon
        var coordinates = new List<List<double[]>>
        {
            new List<double[]>
            {
                new double[] { -87.7, 41.8 },
                new double[] { -87.6, 41.8 },
                new double[] { -87.6, 41.9 },
                new double[] { -87.7, 41.9 },
                new double[] { -87.7, 41.8 } // Closed ring
            }
        };

        var region = new Region
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Downtown District",
            Boundary = new GeoJsonPolygon("Polygon", coordinates)
        };

        Context.Regions.Add(region);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Regions.FindAsync(region.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Boundary);
        Assert.Equal("Polygon", loaded.Boundary.Type);
        Assert.Single(loaded.Boundary.Coordinates);
        Assert.Equal(5, loaded.Boundary.Coordinates[0].Count);
    }

    [Fact]
    public async Task Can_Create_Region_With_Both_Boundary_And_ZipCodes()
    {
        var region = new Region
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Mixed Definition Region",
            Boundary = new GeoJsonPolygon("Polygon", new List<List<double[]>>
            {
                new List<double[]> { new[] { 0.0, 0.0 }, new[] { 1.0, 0.0 }, new[] { 1.0, 1.0 }, new[] { 0.0, 0.0 } }
            }),
            ZipCodes = new List<string> { "12345", "67890" }
        };

        Context.Regions.Add(region);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Regions.FindAsync(region.Id);

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Boundary);
        Assert.NotNull(loaded.ZipCodes);
        Assert.Equal(2, loaded.ZipCodes.Count);
    }

    [Fact]
    public async Task Can_Create_Region_With_Minimal_Data()
    {
        var region = new Region
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-001",
            Name = "Simple Region"
            // No boundary, no zip codes
        };

        Context.Regions.Add(region);
        await Context.SaveChangesAsync();

        using var newContext = CreateNewContext();
        var loaded = await newContext.Regions.FindAsync(region.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Simple Region", loaded.Name);
        Assert.Null(loaded.Boundary);
        Assert.Null(loaded.ZipCodes);
    }

    #endregion
}
