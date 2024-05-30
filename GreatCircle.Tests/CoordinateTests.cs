using System.Globalization;

namespace GreatCircle.Tests;

public class CoordinateTests
{
    /// <summary>
    /// Check that a latitude above the range [-90, 90] is reset to this range.
    /// </summary>
    [Fact]
    public void Coordinates_Latitude_PositiveResetToRange()
    {
        double latitude = 90 + 20;
        double expected = 90 - 20;
        Assert.Equal(expected, new Coordinate(latitude, 0).Latitude);
    }

    /// <summary>
    /// Check that a latitude below the range [-90, 90] is reset to this range.
    /// </summary>
    [Fact]
    public void Coordinates_Latitude_NegativeResetToRange()
    {
        double latitude = -90 - 20;
        double expected = -90 + 20;
        Assert.Equal(expected, new Coordinate(latitude, 0).Latitude);
    }

    /// <summary>
    /// Check that a longitude above the range [-180, 180) is reset to this range.
    /// </summary>
    [Fact]
    public void Coordinates_Longitude_PositiveResetToRange()
    {
        double longitude = 50;
        Assert.Equal(longitude, new Coordinate(0, longitude+360).Longitude);
    }

    /// <summary>
    /// Check that a longitude below the range [-180, 180) is reset to this range.
    /// </summary>
    [Fact]
    public void Coordinates_Longitude_NegativeResetToRange()
    {
        double longitude = 50;
        Assert.Equal(longitude, new Coordinate(0, longitude - 360).Longitude);
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
        double latitude = 0;
        double longitude = 0;
        string expected =
            $"{latitude:F2}{AngleUtilities.Degree} N, {longitude:F2}{AngleUtilities.Degree} E";
        Assert.Equal(expected, Coordinate.Origin.ToString());
    }

    /// <summary>
    /// Check that a custom string formatting is correct for the origin.
    /// </summary>
    [Fact]
    public void String_Origin_CustomFormat()
    {
        double latitude = 0;
        double longitude = 0;
        string expected =
            $"{latitude:F0}{AngleUtilities.Degree} N, {longitude:F0}{AngleUtilities.Degree} E";
        Assert.Equal(expected, Coordinate.Origin.ToString("F0"));
    }

    /// <summary>
    /// Check that the string formatting of the north pole is correct.
    /// </summary>
    [Fact]
    public void String_NorthPole_DefaultFormat()
    {
        double latitude = 90;
        double longitude = 0;
        string expected =
            $"{latitude:F2}{AngleUtilities.Degree} N, {longitude:F2}{AngleUtilities.Degree} E";
        Assert.Equal(expected, Coordinate.NorthPole.ToString());
    }

    /// <summary>
    /// Check that the string formatting of the south pole is correct.
    /// </summary>
    [Fact]
    public void String_SouthPole_DefaultFormat()
    {
        double latitude = -90;
        double longitude = 0;
        string expected =
            $"{-latitude:F2}{AngleUtilities.Degree} S, {longitude:F2}{AngleUtilities.Degree} E";
        Assert.Equal(expected, Coordinate.SouthPole.ToString());
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
            $"{latitude:F2}{AngleUtilities.Degree} N, {longitude:F2}{AngleUtilities.Degree} E",
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
            $"{latitude:F2}{AngleUtilities.Degree} N, {-longitude:F2}{AngleUtilities.Degree} W",
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
            $"{-latitude:F2}{AngleUtilities.Degree} S, {longitude:F2}{AngleUtilities.Degree} E",
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
            $"{-latitude:F2}{AngleUtilities.Degree} S, {-longitude:F2}{AngleUtilities.Degree} W",
            new Coordinate(latitude, longitude).ToString());
    }
}
