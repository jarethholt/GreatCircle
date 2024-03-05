namespace GreatCircle.Tests;

public class GreatCirclePathTests
{
    private readonly string degreeSymbol = "\u00B0";

    [Fact]
    public void Path_InitialPoint_Polar()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinates.Coordinate.NorthPole, 10));
    }

    [Fact]
    public void Path_Azimuth_Northward()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinates.Coordinate.Origin, 0));
    }

    [Fact]
    public void Path_Azimuth_Southward()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinates.Coordinate.Origin, 180));
    }

    [Fact]
    public void Path_Azimuth_NorthwardWrapped()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinates.Coordinate.Origin, 360 - 1e-8));
    }

    [Fact]
    public void String_DefaultFormat()
    {
        double latitude = 10;
        double longitude = 70;
        double azimuth = 30;
        Assert.Equal(
            $"{latitude:F2}{degreeSymbol} N, {longitude:F2}{degreeSymbol} E; "
            + $"heading {azimuth:F2}{degreeSymbol}",
            new GreatCirclePath(latitude, longitude, azimuth).ToString());
    }

    [Fact]
    public void String_CustomFormat()
    {
        double latitude = 10;
        double longitude = 70;
        double azimuth = 30;
        Assert.Equal(
            $"{latitude:F0}{degreeSymbol} N, {longitude:F0}{degreeSymbol} E; "
            + $"heading {azimuth:F0}{degreeSymbol}",
            new GreatCirclePath(latitude, longitude, azimuth).ToString("F0"));
    }
}
