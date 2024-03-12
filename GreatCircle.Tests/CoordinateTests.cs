namespace GreatCircle.Tests;

public class CoordinateTests
{
    private readonly string degreeSymbol = "\u00B0";

    /// <summary>
    /// Check for <c>ArgumentOutOfRangeException</c> when the latitude is outside of [-90, 90].
    /// </summary>
    [Fact]
    public void Coordinates_Latitude_OutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Coordinate(100, 0));
    }

    /// <summary>
    /// Check that a longitude outside the range [-180, 180) is reset to this range.
    /// </summary>
    [Fact]
    public void Coordinates_Longitude_ResetToRange()
    {
        double longitude = 50;
        Assert.Equal(longitude, new Coordinate(0, longitude+360).Longitude);
    }

    /// <summary>
    /// Check that the dateline is given as -180 (180 W).
    /// </summary>
    [Fact]
    public void Coordinates_DateLineAtNeg180()
    {
        Assert.Equal(-180, new Coordinate(0, 180).Longitude);
    }

    /// <summary>
    /// Check that IsAPole returns True for the North Pole.
    /// </summary>
    [Fact]
    public void IsAPole_PolarPoint()
    {
        Assert.True(Coordinate.NorthPole.IsAPole());
    }

    /// <summary>
    /// Check that IsAPole returns True for the North Pole even if the longitude is set.
    /// </summary>
    [Fact]
    public void IsAPole_PolarPoint_NonstandardLongitude()
    {
        Assert.True(new Coordinate(90, 15).IsAPole());
    }

    /// <summary>
    /// Check that IsAPole returns False for a non-polar point.
    /// </summary>
    [Fact]
    public void IsAPole_NonPolarPoint()
    {
        Assert.False(Coordinate.Origin.IsAPole());
    }

    /// <summary>
    /// Check that the antipode of a polar point is calculated correctly.
    /// </summary>
    [Fact]
    public void IsAntipodalTo_PolarPoint()
    {
        Assert.True(
            Coordinate.NorthPole
            .IsAntipodalTo(Coordinate.SouthPole));
    }

    /// <summary>
    /// Check that the antipode of a polar point with arbitrary longitude is correct.
    /// </summary>
    [Fact]
    public void IsAntipodalTo_PolarPoint_NonstandardLongitude()
    {
        Assert.True(
            new Coordinate(90, 15)
            .IsAntipodalTo(Coordinate.SouthPole));
    }

    /// <summary>
    /// Check that the antipode of a non-polar point is correct.
    /// </summary>
    [Fact]
    public void IsAntipodalTo_NonPolarPoint()
    {
        Assert.True(
            Coordinate.Origin.IsAntipodalTo(
            new Coordinate(0, 180)));
    }

    /// <summary>
    /// Check that the default string formatting is correct for the origin.
    /// </summary>
    [Fact]
    public void String_Origin_DefaultFormat()
    {
        Assert.Equal(
            $"0.00{degreeSymbol} N, 0.00{degreeSymbol} E",
            Coordinate.Origin.ToString());
    }

    /// <summary>
    /// Check that a custom string formatting is correct for the origin.
    /// </summary>
    [Fact]
    public void String_Origin_CustomFormat()
    {
        Assert.Equal(
            $"0{degreeSymbol} N, 0{degreeSymbol} E",
            Coordinate.Origin.ToString("F0"));
    }

    /// <summary>
    /// Check that the string formatting of the north pole is correct.
    /// </summary>
    [Fact]
    public void String_NorthPole_DefaultFormat()
    {
        Assert.Equal(
            $"90.00{degreeSymbol} N, 0.00{degreeSymbol} E",
            Coordinate.NorthPole.ToString());
    }

    /// <summary>
    /// Check that the string formatting of the south pole is correct.
    /// </summary>
    [Fact]
    public void String_SouthPole_DefaultFormat()
    {
        Assert.Equal(
            $"90.00{degreeSymbol} S, 0.00{degreeSymbol} E",
            Coordinate.SouthPole.ToString());
    }

    /// <summary>
    /// Check that the string formatting of a point in the northeast quadrant is correct.
    /// </summary>
    [Fact]
    public void String_NorthEast_DefaultFormat()
    {
        double latitude = 10;
        double longitude = 70;
        Assert.Equal(
            $"{latitude:F2}{degreeSymbol} N, {longitude:F2}{degreeSymbol} E",
            new Coordinate(latitude, longitude).ToString());
    }

    /// <summary>
    /// Check that the string formatting of a point in the northwest quadrant is correct.
    /// </summary>
    [Fact]
    public void String_NorthWest_DefaultFormat()
    {
        double latitude = 10;
        double longitude = -70;
        Assert.Equal(
            $"{latitude:F2}{degreeSymbol} N, {-longitude:F2}{degreeSymbol} W",
            new Coordinate(latitude, longitude).ToString());
    }

    /// <summary>
    /// Check that the string formatting of a point in the southeast quadrant is correct.
    /// </summary>
    [Fact]
    public void String_SouthEast_DefaultFormat()
    {
        double latitude = -10;
        double longitude = 70;
        Assert.Equal(
            $"{-latitude:F2}{degreeSymbol} S, {longitude:F2}{degreeSymbol} E",
            new Coordinate(latitude, longitude).ToString());
    }

    /// <summary>
    /// Check that the string formatting of a point in the southwest quadrant is correct.
    /// </summary>
    [Fact]
    public void String_SouthWest_DefaultFormat()
    {
        double latitude = -10;
        double longitude = -70;
        Assert.Equal(
            $"{-latitude:F2}{degreeSymbol} S, {-longitude:F2}{degreeSymbol} W",
            new Coordinate(latitude, longitude).ToString());
    }
}
