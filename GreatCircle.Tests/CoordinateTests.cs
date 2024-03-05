namespace GreatCircle.Tests;

public class CoordinateTests
{
    private readonly string degreeSymbol = "\u00B0";
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
    public void Coordinates_DateLineAtNeg180()
    {
        Assert.Equal(-180, new Coordinates.Coordinate(0, 180).Longitude);
    }

    [Fact]
    public void IsAPole_PolarPoint()
    {
        Assert.True(Coordinates.Coordinate.NorthPole.IsAPole);
    }

    [Fact]
    public void IsAPole_PolarPoint_NonstandardLongitude()
    {
        Assert.True(new Coordinates.Coordinate(90, 15).IsAPole);
    }

    [Fact]
    public void IsAPole_NonPolarPoint()
    {
        Assert.False(Coordinates.Coordinate.Origin.IsAPole);
    }

    [Fact]
    public void IsAntipodalTo_PolarPoint()
    {
        Assert.True(
            Coordinates.Coordinate.NorthPole
            .IsAntipodalTo(Coordinates.Coordinate.SouthPole));
    }

    [Fact]
    public void IsAntipodalTo_PolarPoint_NonstandardLongitude()
    {
        Assert.True(
            new Coordinates.Coordinate(90, 15)
            .IsAntipodalTo(Coordinates.Coordinate.SouthPole));
    }

    [Fact]
    public void IsAntipodalTo_NonPolarPoint()
    {
        Assert.True(
            Coordinates.Coordinate.Origin.IsAntipodalTo(
            new Coordinates.Coordinate(0, 180)));
    }

    [Fact]
    public void String_Origin_DefaultFormat()
    {
        Assert.Equal(
            $"0.00{degreeSymbol} N, 0.00{degreeSymbol} E",
            Coordinates.Coordinate.Origin.ToString());
    }

    [Fact]
    public void String_Origin_CustomFormat()
    {
        Assert.Equal(
            $"0{degreeSymbol} N, 0{degreeSymbol} E",
            Coordinates.Coordinate.Origin.ToString("F0"));
    }

    [Fact]
    public void String_NorthPole_DefaultFormat()
    {
        Assert.Equal(
            $"90.00{degreeSymbol} N, 0.00{degreeSymbol} E",
            Coordinates.Coordinate.NorthPole.ToString());
    }

    [Fact]
    public void String_SouthPole_DefaultFormat()
    {
        Assert.Equal(
            $"90.00{degreeSymbol} S, 0.00{degreeSymbol} E",
            Coordinates.Coordinate.SouthPole.ToString());
    }

    [Fact]
    public void String_NorthEast_DefaultFormat()
    {
        double latitude = 10;
        double longitude = 70;
        Assert.Equal(
            $"{latitude:F2}{degreeSymbol} N, {longitude:F2}{degreeSymbol} E",
            new Coordinates.Coordinate(latitude, longitude).ToString());
    }

    [Fact]
    public void String_NorthWest_DefaultFormat()
    {
        double latitude = 10;
        double longitude = -70;
        Assert.Equal(
            $"{latitude:F2}{degreeSymbol} N, {-longitude:F2}{degreeSymbol} W",
            new Coordinates.Coordinate(latitude, longitude).ToString());
    }

    [Fact]
    public void String_SouthEast_DefaultFormat()
    {
        double latitude = -10;
        double longitude = 70;
        Assert.Equal(
            $"{-latitude:F2}{degreeSymbol} S, {longitude:F2}{degreeSymbol} E",
            new Coordinates.Coordinate(latitude, longitude).ToString());
    }

    [Fact]
    public void String_SouthWest_DefaultFormat()
    {
        double latitude = -10;
        double longitude = -70;
        Assert.Equal(
            $"{-latitude:F2}{degreeSymbol} S, {-longitude:F2}{degreeSymbol} W",
            new Coordinates.Coordinate(latitude, longitude).ToString());
    }
}
