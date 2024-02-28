namespace GreatCircle.Tests;

public class CoordinatesTests
{
    [Fact]
    public void Coordinates_Latitude_OutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Coordinates.Coordinates(100, 0));
    }

    [Fact]
    public void Coordinates_Longitude_ResetToRange()
    {
        double longitude = 50;
        Assert.Equal(longitude, new Coordinates.Coordinates(0, longitude+360).Longitude);
    }

    [Fact]
    public void Coordinates_PolarPoint_ResetsLongitude()
    {
        Assert.Equal(0, new Coordinates.Coordinates(90, 30).Longitude);
    }

    [Fact]
    public void Coordinates_DateLineAtNeg180()
    {
        Assert.Equal(-180, new Coordinates.Coordinates(0, 180).Longitude);
    }

    [Fact]
    public void IsAPole_PolarPoint()
    {
        Assert.True(Coordinates.Coordinates.NorthPole().IsAPole());
    }

    [Fact]
    public void IsAPole_NonPolarPoint()
    {
        Assert.False(Coordinates.Coordinates.Origin().IsAPole());
    }

    [Fact]
    public void IsAntipodalTo_PolarPoint()
    {
        Assert.True(
            Coordinates.Coordinates.NorthPole().IsAntipodalTo(
            Coordinates.Coordinates.SouthPole()));
    }

    [Fact]
    public void IsAntipodalTo_NonPolarPoint()
    {
        Assert.True(
            Coordinates.Coordinates.Origin().IsAntipodalTo(
            new Coordinates.Coordinates(0, 180)));
    }
}
