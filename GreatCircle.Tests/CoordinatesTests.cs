namespace GreatCircle.Tests;

public class CoordinatesTests
{
    [Fact]
    public void Coordinates_Latitude_OutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Coordinates.Coordinate(100, 0));
    }

    [Fact]
    public void Coordinates_Longitude_ResetToRange()
    {
        double longitude = 50;
        Assert.Equal(longitude, new Coordinates.Coordinate(0, longitude+360).Longitude);
    }

    [Fact]
    public void Coordinates_PolarPoint_ResetsLongitude()
    {
        Assert.Equal(0, new Coordinates.Coordinate(90, 30).Longitude);
    }

    [Fact]
    public void Coordinates_DateLineAtNeg180()
    {
        Assert.Equal(-180, new Coordinates.Coordinate(0, 180).Longitude);
    }

    [Fact]
    public void IsAPole_PolarPoint()
    {
        Assert.True(Coordinates.Coordinate.NorthPole().IsAPole());
    }

    [Fact]
    public void IsAPole_NonPolarPoint()
    {
        Assert.False(Coordinates.Coordinate.Origin().IsAPole());
    }

    [Fact]
    public void IsAntipodalTo_PolarPoint()
    {
        Assert.True(
            Coordinates.Coordinate.NorthPole().IsAntipodalTo(
            Coordinates.Coordinate.SouthPole()));
    }

    [Fact]
    public void IsAntipodalTo_NonPolarPoint()
    {
        Assert.True(
            Coordinates.Coordinate.Origin().IsAntipodalTo(
            new Coordinates.Coordinate(0, 180)));
    }
}
